using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCoinCore;

namespace CloudCoin_ShowCoins
{
    public class ShowCoins
    {
        public string BasePath = "";
        public static FileSystem FS;
        public ShowCoins(string BasePath)
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

        public void showCoins()
        {

        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                Console.WriteLine("\n" + Path.GetFileName(e.FullPath));
                if (Path.GetFileName(e.FullPath).Contains("showcoins"))
                {
                    Console.WriteLine("caught show coins");
                    var bankCoins = FS.LoadFolderCoins(FolderManager.FolderManager.BankFolder);


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
                    File.WriteAllText(bankFileName, "");

                    var frackedCoins = FS.LoadFolderCoins(FolderManager.FolderManager.FrackedFolder);


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

                    String frackedFileName = FolderManager.FolderManager.ShowCoinsLogsFolder + Path.DirectorySeparatorChar + "fracked." + frackedonesCount + "." + frackedfivesCount + "." + frackedqtrCount + "." + frackedhundredsCount + "." + frackedtwoFiftiesCount + ".txt";
                    File.WriteAllText(frackedFileName, "");

                    var lostCoins = FS.LoadFolderCoins(FolderManager.FolderManager.LostFolder);


                    int lostonesCount = (from x in lostCoins
                                     where x.denomination == 1
                                     select x).Count();
                    int lostfivesCount = (from x in lostCoins
                                      where x.denomination == 5
                                      select x).Count();
                    int lostqtrCount = (from x in lostCoins
                                    where x.denomination == 25
                                    select x).Count();
                    int losthundredsCount = (from x in lostCoins
                                         where x.denomination == 100
                                         select x).Count();
                    int losttwoFiftiesCount = (from x in lostCoins
                                           where x.denomination == 250
                                           select x).Count();

                    String lostFileName = FolderManager.FolderManager.ShowCoinsLogsFolder + Path.DirectorySeparatorChar + "lost." + lostonesCount + "." + lostfivesCount + "." + lostqtrCount + "." + losthundredsCount + "." + losttwoFiftiesCount + ".txt";
                    File.WriteAllText(lostFileName, "");
                }
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        
        private static String GetLogFileName(int num)
        {
            Node node = RAIDA.GetInstance().nodes[num];
            return (num) + "_" + node.RAIDANodeStatus + "_" +
                    node.echoresponses.milliseconds + "_" + node.internalExecutionTime + ".txt";
        }
    }
}
