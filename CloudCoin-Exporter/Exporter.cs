using CloudCoinCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderManager;

namespace CloudCoin_Exporter
{
    public class Exporter
    {
        public string BasePath = "";
        public static FileSystem FS;
        int note100, return100, note50, return50, note25, return25, note10, return10,
        note5, return5, note1, return1, coin1, returncoins;
        int billamount = 0, recvamount = 0, differ = 0, change = 0;
        int exportJpegStack = 1;
        public Exporter(string BasePath)
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
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.Deleted += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;
        }
        List<CloudCoin> bankCoins;
        List<CloudCoin> frackedCoins;
        private  void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                Console.WriteLine("\n" + Path.GetFileName(e.FullPath));
                if (Path.GetFileName(e.FullPath).Contains("export"))
                {
                    Console.WriteLine("caught export command");
                    string jsonText = File.ReadAllText(e.FullPath);
                    var jo = JObject.Parse(@jsonText);
                    string account = jo.GetValue("account").ToString();
                    string amount = jo.GetValue("amount").ToString();
                    string type = jo.GetValue("type").ToString();
                    string tag = jo.GetValue("tag").ToString();
                    string ones = jo.GetValue("1s").ToString();
                    string fives = jo.GetValue("5s").ToString();
                    string qtrs = jo.GetValue("25s").ToString();
                    string hundreds = jo.GetValue("100s").ToString();
                    string twofifties = jo.GetValue("250s").ToString();


                    bankCoins = FS.LoadFolderCoins(FolderManager.FolderManager.BankFolder);
                    frackedCoins = FS.LoadFolderCoins(FolderManager.FolderManager.FrackedFolder);

                    int onesCount = (from x in bankCoins
                                     where x.denomination == 1
                                     select x).Count();
                    int fivesCount = (from x in bankCoins
                                      where x.denomination == 5
                                      select x).Count();
                    int qtrCount = (from x in bankCoins
                                    where x.denomination == 25
                                    select x).Count();
                    int hundredsCount = (from x in bankCoins
                                         where x.denomination == 100
                                         select x).Count();
                    int twoFiftiesCount = (from x in bankCoins
                                           where x.denomination == 250
                                           select x).Count();

                    String bankFileName = FolderManager.FolderManager.ShowCoinsLogsFolder + Path.DirectorySeparatorChar + "bank." + onesCount + "." + fivesCount + "." + qtrCount + "." + hundredsCount + "." + twoFiftiesCount + ".txt";
                    //File.WriteAllText(bankFileName, "");

                    


                    int frackedonesCount = (from x in frackedCoins
                                            where x.denomination == 1
                                            select x).Count();
                    int frackedfivesCount = (from x in frackedCoins
                                             where x.denomination == 5
                                             select x).Count();
                    int frackedqtrCount = (from x in frackedCoins
                                           where x.denomination == 25
                                           select x).Count();
                    int frackedhundredsCount = (from x in frackedCoins
                                                where x.denomination == 100
                                                select x).Count();
                    int frackedtwoFiftiesCount = (from x in frackedCoins
                                                  where x.denomination == 250
                                                  select x).Count();

                    
                   
                }
            }
        }

        public void export(int expOnes,int expFives,int expQtrs,int expHundreds,int expTwoFifties,string tag)
        {
            List<CloudCoin> exportedCoins = new List<CloudCoin>();
            
            
           
            int exp_1 = Convert.ToInt16(expOnes);
            int exp_5 = Convert.ToInt16(expFives);
            int exp_25 = Convert.ToInt16(expQtrs);
            int exp_100 = Convert.ToInt16(expHundreds);
            int exp_250 = Convert.ToInt16(expTwoFifties);
            //Warn if too many coins

            if (exp_1 + exp_5 + exp_25 + exp_100 + exp_250 == 0)
            {
                return;
            }

            int file_type = 0;
            file_type = exportJpegStack;

            //String tag = txtTag.Text;// reader.readString();
            int totalSaved = exp_1 + (exp_5 * 5) + (exp_25 * 25) + (exp_100 * 100) + (exp_250 * 250);
            List<CloudCoin> totalCoins = bankCoins.ToList();
            totalCoins.AddRange(frackedCoins);


            var onesToExport = (from x in totalCoins
                                where x.denomination == 1
                                select x).Take(exp_1);
            var fivesToExport = (from x in totalCoins
                                 where x.denomination == 5
                                 select x).Take(exp_5);
            var qtrToExport = (from x in totalCoins
                               where x.denomination == 25
                               select x).Take(exp_25);
            var hundredsToExport = (from x in totalCoins
                                    where x.denomination == 100
                                    select x).Take(exp_100);
            var twoFiftiesToExport = (from x in totalCoins
                                      where x.denomination == 250
                                      select x).Take(exp_250);
            List<CloudCoin> exportCoins = onesToExport.ToList();
            exportCoins.AddRange(fivesToExport);
            exportCoins.AddRange(qtrToExport);
            exportCoins.AddRange(hundredsToExport);
            exportCoins.AddRange(twoFiftiesToExport);

            exportCoins.ForEach(x => x.pan = null);

            if (file_type == 1)
            {
                String filename = (FS.ExportFolder + System.IO.Path.DirectorySeparatorChar + totalSaved + ".CloudCoins." + tag + "");


                foreach (var coin in exportCoins)
                {
                    string OutputFile = FS.ExportFolder + coin.FileName + tag + ".jpg";
                    if (File.Exists(OutputFile))
                    {
                        // tack on a random number if a file already exists with the same tag
                        Random rnd = new Random();
                        int tagrand = rnd.Next(999);
                        OutputFile = FS.ExportFolder + coin.FileName + tag + tagrand + ".jpg";
                    }//end if file exists
                    string templatePath = FS.GetCoinTemplate(coin);
                    if (!File.Exists(templatePath))
                    {
                    

                        //exportCoins.Remove(coin);
                        continue;
                    }
                    bool fileGenerated = FS.WriteCoinToJpeg(coin, FS.GetCoinTemplate(coin), OutputFile, "");
                    if (fileGenerated)
                    {
                        exportedCoins.Add(coin);
                        Console.WriteLine("CloudCoin exported as Jpeg to " + OutputFile);
                    
                    }
                }

                if (exportedCoins.Count > 0)
                {
                    FS.RemoveCoins(exportedCoins, FS.BankFolder);
                    FS.RemoveCoins(exportedCoins, FS.FrackedFolder);
                }
                if (exportedCoins.Count == 0)
                {
       

                }
            }

            // Export Coins as Stack
            if (file_type == 2)
            {
                int stack_type = 1;
                if (stack_type == 1)
                {
                    String filename = (FS.ExportFolder + System.IO.Path.DirectorySeparatorChar + totalSaved + ".CloudCoins." + tag + "");
                    if (File.Exists(filename + ".stack"))
                    {
                        // tack on a random number if a file already exists with the same tag
                        Random rnd = new Random();
                        int tagrand = rnd.Next(999);
                        filename = (FS.ExportFolder + System.IO.Path.DirectorySeparatorChar + totalSaved + ".CloudCoins." + tag + tagrand + "");
                    }//end if file exists

                    FS.WriteCoinsToFile(exportCoins, filename, ".stack");
                    FS.RemoveCoins(exportCoins, FS.BankFolder);
                    FS.RemoveCoins(exportCoins, FS.FrackedFolder);
                }
                else
                {
                    foreach (var coin in exportCoins)
                    {
                        string OutputFile = FS.ExportFolder + coin.FileName + tag + ".stack";
                        FS.WriteCoinToFile(coin, OutputFile);

                        FS.RemoveCoins(exportCoins, FS.BankFolder);
                        FS.RemoveCoins(exportCoins, FS.FrackedFolder);
                    }

                }
            }
            // end if type jpge or stack

            //RefreshCoins?.Invoke(this, new EventArgs());
            //updateLog("Exporting CloudCoins Completed.");
        
            //MessageBox.Show("Export completed.", "Cloudcoins", MessageBoxButtons.OK);
        }// end export One

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        private void calcNumberNotes()
        {
            int differ = 0;

            differ = recvamount - billamount;


            return100 = (int)((differ / 100  ));
            if (return100 > note100)
            {
                differ = differ - (note100 * 100);
                return100 = note100;
            }
            else
            {
                differ = differ - (return100 * 100);

            }


            return50 = (int)((differ / 50));
            if (return50 > note50)
            {
                differ = differ - (note50 * 50);
                return50 = note50;
            }
            else
            {
                differ = differ - (return50 * 50);

            }


            return25 = (int)((differ / 25 ));
            if (return25 > note25)
            {
                differ = differ - (note25 * 20);
                return25 = note25;
            }
            else
            {
                differ = differ - (return25 * 25);

            }



            return5 = (int)((differ / 5 ));
            if (return5 > note5)
            {
                differ = differ - (note5 * 5);
                return5 = note5;
            }
            else
            {
                differ = differ - (return5 * 5);

            }


            return1 = (int)((differ / 1));
            if (return1 > note1)
            {
                differ = differ - (note1 * 1);
                return1 = note1;
            }
            else
            {
                differ = differ - (return1 * 1);

            }

            if (differ <= coin1)
            {

                returncoins = differ;

            }
            else
            {

                returncoins = -1;

            }

        }

    }
}
