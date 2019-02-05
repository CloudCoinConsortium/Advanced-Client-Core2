using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCoinCore;

namespace CloudCoinVaulter
{
    public class Vaulter
    {
        public string BasePath = "";
        public static FileSystem FS;


        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public Vaulter(string BasePath)
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
            watcher.Renamed += new RenamedEventHandler(OnChanged);

            watcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                Console.WriteLine("\n" + Path.GetFileName(e.FullPath));
                if (Path.GetFileName(e.FullPath).Contains("vault"))
                {
                    //File.WriteAllText(lostFileName, "");
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
                        string passphrase = jo.GetValue("passphrase").ToString();
                        string cloudcoin = jo.GetValue("cloudcoin").ToString();
                        FileSystem fs = new FileSystem("");
                        FS = new FileSystem(FolderManager.FolderManager.RootPath + Path.DirectorySeparatorChar + Config.TAG_DETECTED);
                        var bankCoins = FS.LoadFolderCoins(FolderManager.FolderManager.RootPath + Path.DirectorySeparatorChar + Config.TAG_BANK);
                        var frackedCoins = FS.LoadFolderCoins(FolderManager.FolderManager.RootPath + Path.DirectorySeparatorChar + Config.TAG_FRACKED);
                         bankCoins.AddRange(frackedCoins.ToList());
                        string BankPath = FolderManager.FolderManager.RootPath + Path.DirectorySeparatorChar + Config.TAG_BANK; ;
                        string VaultPath = FolderManager.FolderManager.RootPath + Path.DirectorySeparatorChar + Config.TAG_VAULT;
                        if (command == "toVault")
                        {
                            string md5 = CreateMD5(passphrase);
                            int intAgain = int.Parse("A436BF8C", System.Globalization.NumberStyles.HexNumber);
                            int coinCount = 0;
                            foreach(var coin in bankCoins)
                            {
                                int anCount = 0;
                                List<string> newANS = new List<string>(new string[Config.NodeCount]);
                                
                                foreach(var an in coin.an)
                                {
                                    //Console.WriteLine("AN is " + an);
                                    string secondOctect = an.Substring(8, 8);
                                    string thirdOctect = an.Substring(15, 8);
                                    string firstOctect = an.Substring(0, 8);
                                    string fourthOctect = an.Substring(23, 8);

                                    int firstOctectInt = int.Parse(secondOctect, System.Globalization.NumberStyles.HexNumber);
                                    int secondOctectInt = int.Parse(thirdOctect, System.Globalization.NumberStyles.HexNumber);

                                    Console.WriteLine("Second Octect -" + secondOctect);
                                    Console.WriteLine("Third Octect -" + thirdOctect);

                                    string passFirstOctect = md5.Substring(0, 8);
                                    string passSecondOctect = md5.Substring(8, 8);

                                    int thirdOctectInt = int.Parse(passFirstOctect, System.Globalization.NumberStyles.HexNumber);
                                    int fourthOctectInt = int.Parse(passSecondOctect, System.Globalization.NumberStyles.HexNumber);

                                    Console.WriteLine("Pass First Octect -" + secondOctect);
                                    Console.WriteLine("Pass Second Octect -" + thirdOctect);
                                    int firstDiff = firstOctectInt - thirdOctectInt;
                                    int secondDiff = secondOctectInt - fourthOctectInt;

                                    Console.WriteLine("first diff-" + firstDiff);
                                    Console.WriteLine("second diff-" + secondDiff);

                                    string firstDiffHex = firstDiff.ToString("X");
                                    string secondDiffHex = secondDiff.ToString("X");

                                    Console.WriteLine("First Diff hex - " + firstDiffHex);
                                    Console.WriteLine("Second Diff hex - " + secondDiffHex);
                                    string newAN = firstOctect + firstDiffHex + secondDiffHex + fourthOctect;
                                    Console.WriteLine("New AN -" + newAN);
                                    newANS[anCount] = newAN;
                                    anCount++;
                                    //if (anCount == 1) break;


                                }
                                coin.an = newANS;
                                coinCount++;
                                FS.WriteCoin(coin, VaultPath,true);
                                //Console.WriteLine(coin.ExistingFileName);

                              //  File.Delete(FolderManager.FolderManager.RootPath + Path.PathSeparator + Config.TAG_BANK + Path.DirectorySeparatorChar + coin.ExistingFileName);
                                File.Delete(coin.ExistingFileName );


                                // if (coinCount ==1) break;
                            }
                            // Console.WriteLine("md5 hash - "+md5 + "-" + intAgain+ ". Coin Counted-"+ coinCount);

                        }
                        if(command == "fromVault")
                        {
                            var vaultCoins = FS.LoadFolderCoins(FolderManager.FolderManager.RootPath + Path.DirectorySeparatorChar + Config.TAG_VAULT);
                            string md5 = CreateMD5(passphrase);
                            string passFirstOctect = md5.Substring(0, 8);
                            string passSecondOctect = md5.Substring(8, 8);

                            int firstOctectInt = int.Parse(passFirstOctect, System.Globalization.NumberStyles.HexNumber);
                            int secondOctectInt = int.Parse(passSecondOctect, System.Globalization.NumberStyles.HexNumber);

                            Console.WriteLine(passFirstOctect);
                            Console.WriteLine(passSecondOctect);
                            Console.WriteLine(firstOctectInt);
                            Console.WriteLine(secondOctectInt);

                            foreach(var coin in vaultCoins)
                            {
                                var ans = coin.an;
                                List<string> newANS = new List<string>(new string[Config.NodeCount]);
                                int anCount = 0;
                                foreach (var an in ans)
                                {
                                    string secondOctect = an.Substring(8, 8);
                                    string thirdOctect = an.Substring(15, 8);
                                    string firstOctect = an.Substring(0, 8);
                                    string fourthOctect = an.Substring(23, 8);

                                    int thirdOctectInt = int.Parse(thirdOctect, System.Globalization.NumberStyles.HexNumber);
                                    int scondOctectInt = int.Parse(secondOctect, System.Globalization.NumberStyles.HexNumber);

                                    Console.WriteLine(thirdOctectInt);
                                    Console.WriteLine(scondOctectInt);

                                    int van1 = firstOctectInt + scondOctectInt ;
                                    int van2 = secondOctectInt + thirdOctectInt;

                                    string firstAN = van1.ToString("X");
                                    string secondAN = van2.ToString("X");

                                    Console.WriteLine(firstAN + "-" + secondAN);

                                    string newAN = firstOctect + firstAN + secondAN + fourthOctect;
                                    newANS[anCount] = newAN;
                                    anCount++;


                                }
                                coin.an = newANS;
                                //coinCount++;
                                FS.WriteCoin(coin, BankPath, true);
                                //Console.WriteLine(coin.ExistingFileName);

                                //  File.Delete(FolderManager.FolderManager.RootPath + Path.PathSeparator + Config.TAG_BANK + Path.DirectorySeparatorChar + coin.ExistingFileName);
                                File.Delete(coin.ExistingFileName);

                            }
                        }
                        File.Delete(e.FullPath);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

    }
}
