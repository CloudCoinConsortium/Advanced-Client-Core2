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

        private static void OnChanged(object source, FileSystemEventArgs e)
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


                    var bankCoins = FS.LoadFolderCoins(FolderManager.FolderManager.BankFolder);
                    var frackedCoins = FS.LoadFolderCoins(FolderManager.FolderManager.FrackedFolder);

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
