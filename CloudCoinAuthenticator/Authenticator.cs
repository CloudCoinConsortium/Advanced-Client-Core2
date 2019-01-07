using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCoinCore;
using Newtonsoft.Json;

namespace CloudCoinAuthenticator
{
    public class Authenticator
    {
        public string BasePath = "";
        public static FileSystem FS;
        public Authenticator(string BasePath)
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
            // watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                Console.WriteLine("\n" + Path.GetFileName(e.FullPath));
                if (Path.GetFileName(e.FullPath).Contains("erase"))
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

                       if(command == "pown")
                        {

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
