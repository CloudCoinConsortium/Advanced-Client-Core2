using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FolderManager
{
    public class FolderManager
    {
        public static  String BasePath = "C:" + Path.DirectorySeparatorChar + "CloudCoinServer";
        public static  String RootPath = BasePath + Path.DirectorySeparatorChar + "accounts" + Path.DirectorySeparatorChar + "DefaultUser" + Path.DirectorySeparatorChar;

        public static String DetectedFolder = RootPath + Config.TAG_DETECTED + Path.DirectorySeparatorChar ;

        public static String BankFolder = RootPath + Config.TAG_BANK + Path.DirectorySeparatorChar ;
        public static String ImportFolder = RootPath + Config.TAG_IMPORT + Path.DirectorySeparatorChar;
        public static String SuspectFolder = RootPath + Config.TAG_SUSPECT + Path.DirectorySeparatorChar;

        public static String FrackedFolder = RootPath + Config.TAG_FRACKED + Path.DirectorySeparatorChar ;
        public static String CounterfeitFolder = RootPath + Config.TAG_COUNTERFEIT + Path.DirectorySeparatorChar ;
        public static String LostFolder = RootPath + Config.TAG_LOST + Path.DirectorySeparatorChar ;
        public static String GalleryFolder = RootPath + Config.TAG_GALLERY + Path.DirectorySeparatorChar;
        public static String MindFolder = RootPath + Config.TAG_MIND + Path.DirectorySeparatorChar;
        public static String VaultFolder = RootPath + Config.TAG_VAULT + Path.DirectorySeparatorChar;


        public static String LogsFolder = RootPath + Config.TAG_LOGS + Path.DirectorySeparatorChar ;
        public static String ReceiptsFolder = RootPath + Config.TAG_RECEIPTS + Path.DirectorySeparatorChar ;
        public static String CommandFolder = BasePath + Path.DirectorySeparatorChar  + Config.TAG_COMMAND;
        public static String MainLogsFolder = BasePath + Path.DirectorySeparatorChar  + Config.TAG_LOGS;
        public static String EchoerLogsFolder = BasePath + Path.DirectorySeparatorChar  + Config.TAG_LOGS + Path.DirectorySeparatorChar  + Config.TAG_ECHOER;
        public static String BackupperLogsFolder = BasePath + Path.DirectorySeparatorChar + Config.TAG_LOGS + Path.DirectorySeparatorChar + Config.TAG_BACKUPPER;

        public static String ShowCoinsLogsFolder = BasePath + Path.DirectorySeparatorChar + Config.TAG_LOGS + Path.DirectorySeparatorChar + Config.TAG_SHOWCOINS;

        public static void CreateDirectories()
        {
            Directory.CreateDirectory(BasePath);
            Directory.CreateDirectory(RootPath);
            Directory.CreateDirectory(DetectedFolder);
            Directory.CreateDirectory(ImportFolder);
            Directory.CreateDirectory(BankFolder);
            Directory.CreateDirectory(SuspectFolder);
            Directory.CreateDirectory(FrackedFolder);
            Directory.CreateDirectory(CounterfeitFolder);
            Directory.CreateDirectory(MindFolder);
            Directory.CreateDirectory(VaultFolder);
            Directory.CreateDirectory(LostFolder);
            Directory.CreateDirectory(LogsFolder);
            Directory.CreateDirectory(ReceiptsFolder);
            Directory.CreateDirectory(GalleryFolder);
            Directory.CreateDirectory(CommandFolder);
            Directory.CreateDirectory(MainLogsFolder);
            Directory.CreateDirectory(EchoerLogsFolder);
            Directory.CreateDirectory(ShowCoinsLogsFolder);
            Directory.CreateDirectory(BackupperLogsFolder);


        }
    }
}
