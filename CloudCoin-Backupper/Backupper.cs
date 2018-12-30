using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCoinCore;
using Newtonsoft.Json.Linq;
using Json.Net;

namespace CloudCoin_Backupper
{
    public class Backupper
    {
        public string BasePath = "";
        public static FileSystem FS;


        public Backupper(string BasePath)
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
                if (Path.GetFileName(e.FullPath).Contains("backup"))
                {
                    Console.WriteLine("caught backup command");
                    string jsonText = File.ReadAllText(e.FullPath);
                    //JObject jObject = new JObject(jsonText);
                   
                    var jo = JObject.Parse(@jsonText);
                    string account = jo.GetValue("account").ToString();
                    string targetPath = jo.GetValue("toPath").ToString();

                    string backupSourceLocation = FolderManager.FolderManager.BasePath + System.IO.Path.DirectorySeparatorChar +"accounts"+ Path.DirectorySeparatorChar + account;
                    string bankPath = backupSourceLocation + Path.DirectorySeparatorChar + Config.TAG_BANK;
                    string frackedPath = backupSourceLocation + Path.DirectorySeparatorChar + Config.TAG_FRACKED;
                    string lostPath = backupSourceLocation + Path.DirectorySeparatorChar + Config.TAG_LOST;
                    string mindPath = backupSourceLocation + Path.DirectorySeparatorChar + Config.TAG_MIND;
                    string vaultPath = backupSourceLocation + Path.DirectorySeparatorChar + Config.TAG_VAULT;
                    string galleryPath = backupSourceLocation + Path.DirectorySeparatorChar + Config.TAG_GALLERY;

                    var bankCoins = FS.LoadFolderCoins(bankPath);
                    var frackedCoins = FS.LoadFolderCoins(frackedPath);
                    var lostCoins = FS.LoadFolderCoins(lostPath);
                    var mindCoins = FS.LoadFolderCoins(mindPath);
                    var vaultCoins = FS.LoadFolderCoins(vaultPath);
                    var galleryCoins = FS.LoadFolderCoins(galleryPath);


                    FS.WriteCoinsToFile(bankCoins,targetPath + Path.DirectorySeparatorChar + "bank.backup"+ string.Format("{0:yyyyMMddhhmmssfff}", DateTime.Now) + ".stack");
                    FS.WriteCoinsToFile(frackedCoins, targetPath+  Path.DirectorySeparatorChar +  "fracked.backup"+ string.Format("{0:yyyyMMddhhmmssfff}", DateTime.Now) + ".stack");

                    FS.WriteCoinsToFile(lostCoins, targetPath + Path.DirectorySeparatorChar + "lost.backup" + string.Format("{0:yyyyMMddhhmmssfff}", DateTime.Now) + ".stack");
                    FS.WriteCoinsToFile(mindCoins, targetPath + Path.DirectorySeparatorChar + "mind.backup" + string.Format("{0:yyyyMMddhhmmssfff}", DateTime.Now) + ".stack");
                    FS.WriteCoinsToFile(vaultCoins, targetPath + Path.DirectorySeparatorChar + "vault.backup" + string.Format("{0:yyyyMMddhhmmssfff}", DateTime.Now) + ".stack");
                    FS.WriteCoinsToFile(galleryCoins, targetPath + Path.DirectorySeparatorChar + "gallery.backup" + string.Format("{0:yyyyMMddhhmmssfff}", DateTime.Now) + ".stack");
                    Console.WriteLine(account);
                    //File.WriteAllText(lostFileName, "");
                    File.Delete(e.FullPath);
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
