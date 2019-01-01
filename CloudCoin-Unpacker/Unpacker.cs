using CloudCoinCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCoinUnpacker
{
    public class Unpacker
    {
        public string BasePath = "";
        public static FileSystem FS;


        public Unpacker(string BasePath)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            //String CommandFolder = BasePath + File.separator + Config.TAG_COMMAND;
            this.BasePath = BasePath;
            watcher.Path = BasePath + Path.DirectorySeparatorChar + Config.TAG_IMPORT;
            Console.WriteLine(watcher.Path + " Watching");
            RAIDA.ActiveRAIDA = RAIDA.GetInstance();

            FS = new FileSystem(FolderManager.FolderManager.RootPath + Path.DirectorySeparatorChar + Config.TAG_IMPORT);
            RAIDA.FileSystem = FS;
            RAIDA.ActiveRAIDA.FS = FS;

            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.stack";

            // Add event handlers.
            // watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);
            // watcher.Deleted += new FileSystemEventHandler(OnChanged);
            //watcher.Renamed += new RenamedEventHandler(OnRenamed);

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
                //if (Path.GetFileName(e.FullPath).Contains("backup"))
                {
                    Console.WriteLine("caught grader action");
                    try
                    {

                       // string jsonText = File.ReadAllText(e.FullPath);
                        //JObject jObject = new JObject(jsonText);

                        //                    var jo = JObject.Parse(@jsonText);
                        string account = "DefaultUser";// jo.GetValue("account").ToString();
                                                       //string targetPath = ""; // jo.GetValue("toPath").ToString();

                        string graderSourceLocation = FolderManager.FolderManager.BasePath + Path.DirectorySeparatorChar + "accounts" + Path.DirectorySeparatorChar + account;
                        //Console.WriteLine("Started Watching " + graderSourceLocation);

                        string importPath = graderSourceLocation + Path.DirectorySeparatorChar + Config.TAG_IMPORT;
                        string suspectPath = graderSourceLocation + Path.DirectorySeparatorChar + Config.TAG_SUSPECT;


                        var cloudCoins = FS.LoadCoinsFromFile(e.FullPath);
                        
                        foreach(var coin in cloudCoins)
                        {
                            FS.WriteCoin(coin, suspectPath);
                        }

                        File.Delete(e.FullPath);

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
