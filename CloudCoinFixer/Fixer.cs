using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCoinFixer
{
    public class Fixer
    {
        public string BasePath = "";
        public static FileSystem FS;
        public Fixer(string BasePath)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            //String CommandFolder = BasePath + File.separator + Config.TAG_COMMAND;
            this.BasePath = BasePath;
            watcher.Path = BasePath;
            FS = new FileSystem(FolderManager.FolderManager.RootPath);
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.txt";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChangedAsync);
            watcher.Created += new FileSystemEventHandler(OnChangedAsync);
            watcher.Deleted += new FileSystemEventHandler(OnChangedAsync);
            // watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;
        }

        private static async void OnChangedAsync(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                Console.WriteLine("\n" + Path.GetFileName(e.FullPath));
                //if (Path.GetFileName(e.FullPath).Contains("erase"))
                {
                    Console.WriteLine("caught erase command");
                    string jsonText = "";
                    try
                    {
                        FileStream reader = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        using (var sr = new StreamReader(reader, Encoding.Default))
                        {
                            jsonText = sr.ReadToEnd();
                            // read the stream
                            //...
                        }
                        var jo = Newtonsoft.Json.Linq.JObject.Parse(@jsonText);
                        string account = jo.GetValue("account").ToString();
                        string command = jo.GetValue("command").ToString();

                        string sourceLocation = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "accounts" + Path.DirectorySeparatorChar + account;

                        string commandPath = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "Commands";
                        string recieptsPath = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "Receipts";
                        string logsPath = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "Logs";
                        string userLogsLocation = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "accounts" + Path.DirectorySeparatorChar + account + Path.DirectorySeparatorChar + Config.TAG_LOGS;
                        string userRecieptsLocation = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "accounts" + Path.DirectorySeparatorChar + account + Path.DirectorySeparatorChar + "Receipts";
                        string suspectLocation = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "accounts" + Path.DirectorySeparatorChar + account + Path.DirectorySeparatorChar + "Suspect";
                        string detectedLocation = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "accounts" + Path.DirectorySeparatorChar + account + Path.DirectorySeparatorChar + "Detected";
                        string predetectLocation = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "accounts" + Path.DirectorySeparatorChar + account + Path.DirectorySeparatorChar + "Predetect";


                        if (command == "pown")
                        {
                            var suspectcoins = FS.LoadFolderCoins(suspectLocation);
                            Array.ForEach(Directory.GetFiles(predetectLocation),
                              delegate (string path) { File.Delete(path); });
                            foreach (var coin in suspectcoins)
                            {
                                FS.WriteCoin(coin, predetectLocation);
                            }
                            var predetectCoins = FS.LoadFolderCoins(predetectLocation);

                            int LotCount = predetectCoins.Count() / Config.MultiDetectLoad;
                            if (predetectCoins.Count() % Config.MultiDetectLoad > 0) LotCount++;
                            ProgressChangedEventArgs pge = new ProgressChangedEventArgs();

                            int CoinCount = 0;
                            int totalCoinCount = predetectCoins.Count();
                            RAIDA raida = RAIDA.GetInstance();
                            RAIDA.ActiveRAIDA = raida;

                            for (int i = 0; i < LotCount; i++)
                            {
                                //Pick up 200 Coins and send them to RAIDA
                                var coins = predetectCoins.Skip(i * Config.MultiDetectLoad).Take(Config.MultiDetectLoad);

                                try
                                {
                                    raida.coins = coins;
                                    var tasks = raida.GetMultiDetectTasks(coins.ToArray(), Config.milliSecondsToTimeOut, true);

                                    try
                                    {
                                        string requestFileName = Utils.RandomString(16).ToLower() + DateTime.Now.ToString("yyyyMMddHHmmss") + ".stack";
                                        // Write Request To file before detect
                                        FS.WriteCoinsToFile(coins, FS.RequestsFolder + requestFileName);
                                        await Task.WhenAll(tasks.AsParallel().Select(async task => await task()));
                                        int j = 0;
                                        foreach (var coin in coins)
                                        {
                                            coin.pown = "";
                                            for (int k = 0; k < CloudCoinCore.Config.NodeCount; k++)
                                            {
                                                coin.response[k] = raida.nodes[k].MultiResponse.responses[j];
                                                coin.pown += coin.response[k].outcome.Substring(0, 1);
                                            }
                                            int countp = coin.response.Where(x => x.outcome == "pass").Count();
                                            int countf = coin.response.Where(x => x.outcome == "fail").Count();
                                            coin.PassCount = countp;
                                            coin.FailCount = countf;
                                            CoinCount++;




                                            pge.MinorProgress = (CoinCount) * 100 / totalCoinCount;
                                            // Debug.WriteLine("Minor Progress- " + pge.MinorProgress);
                                            raida.OnProgressChanged(pge);
                                            coin.doPostProcessing();
                                            j++;
                                        }
                                        pge.MinorProgress = (CoinCount - 1) * 100 / totalCoinCount;
                                        // Debug.WriteLine("Minor Progress- " + pge.MinorProgress);
                                        raida.OnProgressChanged(pge);
                                        FS.WriteCoin(coins, detectedLocation, true);
                                        FS.RemoveCoinsByFileName(coins, predetectLocation);
                                        FS.RemoveCoinsByFileName(coins, suspectLocation);



                                        //FS.WriteCoin(coins, FS.DetectedFolder);

                                    }
                                    catch (Exception ex)
                                    {
                                        // Debug.WriteLine(ex.Message);
                                    }


                                }
                                catch (Exception exx)
                                {
                                    Console.WriteLine(exx.Message);
                                }


                            }

                        }


                    }
                    catch (Exception ex)
                    {


                        Console.WriteLine(ex.Message);
                    }


                    //File.WriteAllText(lostFileName, "");
                    File.Delete(e.FullPath);
                }
            }
        }
    }
}
