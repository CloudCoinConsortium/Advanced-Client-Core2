using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CloudCoinCore;
using Newtonsoft.Json;

namespace CloudCoinTranslator
{
    public class Translator
    {
        public string BasePath = "";
        public static FileSystem FS;
        static string LogsFolder;
        string EchoerLogsFolder;
        string AuthenticatorLogsFolder;
        string LossFixerLogsFolder;
        string ShowCoinsLogsFolder;


        static StatusReport statusReport = new StatusReport();
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
            //echoerwatcher.Filter = "*.txt";

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

            echoerwatcher.EnableRaisingEvents = true;
            watcher.EnableRaisingEvents = true;
            watcher.EnableRaisingEvents = true;
            watcher.EnableRaisingEvents = true;
            Console.WriteLine("Echoer Watcher field - " + echoerwatcher.Path);

        }

        public void showCoins()
        {

        }

        private static void EchoerOnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                //Console.WriteLine(e.FullPath);
                try
                {
                    
                    var filenames = Path.GetFileName(e.FullPath).Split('.');
                    string nodenum = filenames[0];
                    string status = filenames[1];
                    string milliseconds = filenames[2];
                    string internaltime = filenames[3];
                    Console.WriteLine(nodenum + "-" + status + "-" + milliseconds + "-" + internaltime);
                    if(nodenum == "0")
                    {
                        statusReport.echoer.raida0ready = status;
                    }
                    if (nodenum == "1")
                    {
                        statusReport.echoer.raida1ready = status;
                    }
                    if (nodenum == "2")
                    {
                        statusReport.echoer.raida2ready = status;
                    }
                    if (nodenum == "3")
                    {
                        statusReport.echoer.raida3ready = status;
                    }
                    if (nodenum == "4")
                    {
                        statusReport.echoer.raida4ready = status;
                    }
                    if (nodenum == "5")
                    {
                        statusReport.echoer.raida5ready = status;
                    }
                    if (nodenum == "6")
                    {
                        statusReport.echoer.raida6ready = status;
                    }
                    if (nodenum == "7")
                    {
                        statusReport.echoer.raida7ready = status;
                    }
                    if (nodenum == "8")
                    {
                        statusReport.echoer.raida8ready = status;
                    }
                    if (nodenum == "9")
                    {
                        statusReport.echoer.raida9ready = status;
                    }
                    if (nodenum == "10")
                    {
                        statusReport.echoer.raida10ready = status;
                    }
                    if (nodenum == "11")
                    {
                        statusReport.echoer.raida11ready = status;
                    }
                    if (nodenum == "12")
                    {
                        statusReport.echoer.raida12ready = status;
                    }
                    if (nodenum == "13")
                    {
                        statusReport.echoer.raida13ready = status;
                    }
                    if (nodenum == "14")
                    {
                        statusReport.echoer.raida14ready = status;
                    }
                    if (nodenum == "15")
                    {
                        statusReport.echoer.raida15ready = status;
                    }
                    if (nodenum == "16")
                    {
                        statusReport.echoer.raida16ready = status;
                    }
                    if (nodenum == "17")
                    {
                        statusReport.echoer.raida17ready = status;
                    }
                    if (nodenum == "18")
                    {
                        statusReport.echoer.raida18ready = status;
                    }
                    if (nodenum == "19")
                    {
                        statusReport.echoer.raida19ready = status;
                    }
                    if (nodenum == "20")
                    {
                        statusReport.echoer.raida20ready = status;
                    }
                    if (nodenum == "21")
                    {
                        statusReport.echoer.raida21ready = status;
                    }
                    if (nodenum == "22")
                    {
                        statusReport.echoer.raida22ready = status;
                    }
                    if (nodenum == "23")
                    {
                        statusReport.echoer.raida23ready = status;
                    }
                    if (nodenum == "24")
                    {
                        statusReport.echoer.raida24ready = status;
                    }
                    var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                    var text = JsonConvert.SerializeObject(statusReport, settings);

                    using (StreamWriter outputFile = new StreamWriter(Path.Combine( LogsFolder, "StatusReport.txt")))
                    {
                            outputFile.WriteLine(text);
                    }

                    Console.WriteLine(text);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
