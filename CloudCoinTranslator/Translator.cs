using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CloudCoinCore;

namespace CloudCoinTranslator
{
    public class Translator
    {
        public string BasePath = "";
        public static FileSystem FS;
        string LogsFolder;
        string EchoerLogsFolder;
        string AuthenticatorLogsFolder;
        string LossFixerLogsFolder;
        string ShowCoinsLogsFolder;



        public Translator(string BasePath)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            FileSystemWatcher echoerwatcher = new FileSystemWatcher();
            FileSystemWatcher authenticatorwatcher = new FileSystemWatcher();
            FileSystemWatcher lossfixerwatcher = new FileSystemWatcher();
            FileSystemWatcher showcoinswatcher = new FileSystemWatcher();

            //String CommandFolder = BasePath + File.separator + Config.TAG_COMMAND;
            this.BasePath = BasePath;
            LogsFolder = BasePath + Path.DirectorySeparatorChar + Config.TAG_LOGS ;
            EchoerLogsFolder = BasePath + Path.DirectorySeparatorChar + Config.TAG_LOGS + Path.DirectorySeparatorChar + "Echoer";
            AuthenticatorLogsFolder = BasePath + Path.DirectorySeparatorChar + Config.TAG_LOGS + Path.DirectorySeparatorChar + "authenticator";
            LossFixerLogsFolder = BasePath + Path.DirectorySeparatorChar + Config.TAG_LOGS + Path.DirectorySeparatorChar + "LossFixer";
            ShowCoinsLogsFolder = BasePath + Path.DirectorySeparatorChar + Config.TAG_LOGS + Path.DirectorySeparatorChar + "ShowCoins";

            watcher.Path = BasePath + Path.DirectorySeparatorChar + Config.TAG_LOGS;
            echoerwatcher.Path = EchoerLogsFolder;
            authenticatorwatcher.Path = AuthenticatorLogsFolder;
            lossfixerwatcher.Path = LossFixerLogsFolder;
            showcoinswatcher.Path = ShowCoinsLogsFolder;

            Console.WriteLine(watcher.Path + " Watching");
            RAIDA.ActiveRAIDA = RAIDA.GetInstance();

            FS = new FileSystem(FolderManager.FolderManager.RootPath + Path.DirectorySeparatorChar + Config.TAG_LOGS);
            RAIDA.FileSystem = FS;
            RAIDA.ActiveRAIDA.FS = FS;

            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.stack";

            echoerwatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            echoerwatcher.Filter = "*.txt";

            authenticatorwatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            authenticatorwatcher.Filter = "*.txt";

            lossfixerwatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            lossfixerwatcher.Filter = "*.txt";

            showcoinswatcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            showcoinswatcher.Filter = "*.txt";

            // Add event handlers.
            // watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            // watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;

            echoerwatcher.Created += new FileSystemEventHandler(EchoerOnChanged);
            authenticatorwatcher.Created += new FileSystemEventHandler(EchoerOnChanged);
            showcoinswatcher.Created += new FileSystemEventHandler(EchoerOnChanged);
            lossfixerwatcher.Created += new FileSystemEventHandler(EchoerOnChanged);

        }

        public void showCoins()
        {

        }

        private static void EchoerOnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {

            }

        }

        private static void AuthenticatorOnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {

            }

        }

        private static void ShowCoinsOnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {

            }

        }

        private static void LossFixerOnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {

            }

        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                Console.WriteLine("\n" + Path.GetFileName(e.FullPath));
                //if (Path.GetFileName(e.FullPath).Contains("backup"))
                {
                    Console.WriteLine("caught grader action");
                    try
                    {

                        string jsonText = File.ReadAllText(e.FullPath);
                        //JObject jObject = new JObject(jsonText);

                        //                    var jo = JObject.Parse(@jsonText);
                        string account = "DefaultUser";// jo.GetValue("account").ToString();
                                                       //string targetPath = ""; // jo.GetValue("toPath").ToString();

                        string graderSourceLocation = FolderManager.FolderManager.BasePath + System.IO.Path.DirectorySeparatorChar + "accounts" + Path.DirectorySeparatorChar + account;
                        //Console.WriteLine("Started Watching " + graderSourceLocation);

                        string detectedPath = graderSourceLocation + Path.DirectorySeparatorChar + Config.TAG_DETECTED;
                        string bankPath = graderSourceLocation + Path.DirectorySeparatorChar + Config.TAG_BANK;
                        string frackedPath = graderSourceLocation + Path.DirectorySeparatorChar + Config.TAG_FRACKED;
                        string lostPath = graderSourceLocation + Path.DirectorySeparatorChar + Config.TAG_LOST;
                        string mindPath = graderSourceLocation + Path.DirectorySeparatorChar + Config.TAG_MIND;
                        string vaultPath = graderSourceLocation + Path.DirectorySeparatorChar + Config.TAG_VAULT;
                        string galleryPath = graderSourceLocation + Path.DirectorySeparatorChar + Config.TAG_GALLERY;

                        CloudCoin cloudCoin = FS.LoadCoin(e.FullPath);
                        if (cloudCoin != null)
                        {
                            cloudCoin.SortToFolder();
                        }

                        if (cloudCoin.folder == "Bank")
                        {
                            FS.MoveFile(e.FullPath, bankPath + Path.DirectorySeparatorChar + Path.GetFileName(e.FullPath), IFileSystem.FileMoveOptions.Replace);

                        }

                        if (cloudCoin.folder == "Fracked")
                        {
                            FS.MoveFile(e.FullPath, frackedPath + Path.DirectorySeparatorChar + Path.GetFileName(e.FullPath), IFileSystem.FileMoveOptions.Replace);

                        }

                        //  Console.WriteLine(account);
                        //File.WriteAllText(lostFileName, "");
                        //File.Delete(e.FullPath);
                    }
                    catch (Exception ex)
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
