using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CloudCoinCore;


namespace CloudCoinServants
{
    public class Echoer
    {
        public static string EchoerLogsFolder = "";
        public Echoer(string BasePath)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            //String CommandFolder = BasePath + File.separator + Config.TAG_COMMAND;
            EchoerLogsFolder = BasePath + Path.DirectorySeparatorChar + Config.TAG_LOGS + Path.DirectorySeparatorChar + "Echoer";
            Directory.CreateDirectory(EchoerLogsFolder);
            watcher.Path = BasePath;
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

        public void echo()
        {

        }
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
            if(e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                Console.WriteLine("\n"+Path.GetFileName(e.FullPath));
                if(Path.GetFileName(e.FullPath).Contains("echoer"))
                {
                    //Console.WriteLine("caught");
                   EchoRaidas().Wait();
                    for(int i=0;i < RAIDA.GetInstance().nodes.Length;i++)
                    {
                        string fileName = EchoerLogsFolder + Path.DirectorySeparatorChar + GetLogFileName(i);
                        File.WriteAllText(fileName, "{\n" +
                        "    \"url\":\"" + RAIDA.GetInstance().nodes[i].FullUrl  + "\"\n" +
                        "}" );
                    }
                }
            }
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine("File: {0} renamed to {1}", e.OldFullPath, e.FullPath);
        }

        public static async Task EchoRaidas(bool scanAll = false, bool silent = false)
        {
           // IsEchoRunning = true;
            var networks = (from x in RAIDA.networks
                            select x).Distinct().ToList();
            var echos = RAIDA.GetInstance().GetEchoTasks();
            RAIDA raida = RAIDA.GetInstance();

            await Task.WhenAll(echos.AsParallel().Select(async task => await task()));
            Console.WriteLine("Ready Count - " + raida.ReadyCount);
            Console.WriteLine("Not Ready Count - " + raida.NotReadyCount);



        }
        private static String GetLogFileName(int num)
        {
            Node node = RAIDA.GetInstance().nodes[num];
            return (num) + "_" + node.RAIDANodeStatus + "_" +
                    node.echoresponses.milliseconds + "_" + node.internalExecutionTime + ".txt";
        }

    }
}
