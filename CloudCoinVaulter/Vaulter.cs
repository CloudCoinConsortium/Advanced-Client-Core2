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
                if (Path.GetFileName(e.FullPath).Contains("vault"))
                {
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

    }
}
