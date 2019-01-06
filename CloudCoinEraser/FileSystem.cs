using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCoinCore;

namespace CloudCoinEraser
{
    public class FileSystem : IFileSystem
    {
        public FileSystem(string BasePath)
        {
            RootPath = BasePath;
        }
        public override void ClearCoins(string FolderName)
        {
            throw new NotImplementedException();
        }

        public override bool CreateFolderStructure()
        {
            throw new NotImplementedException();
        }

        public override void DetectPreProcessing()
        {
            throw new NotImplementedException();
        }

        public override void LoadFileSystem()
        {
            throw new NotImplementedException();
        }

        public override void MoveImportedFiles()
        {
            throw new NotImplementedException();
        }

        public override void ProcessCoins(IEnumerable<CloudCoin> coins)
        {
            throw new NotImplementedException();
        }

        public override bool WriteCoinToBARCode(CloudCoin cloudCoin, string OutputFile, string tag)
        {
            throw new NotImplementedException();
        }

        public override bool WriteCoinToJpeg(CloudCoin cloudCoin, string TemplateFile, string OutputFile, string tag)
        {
            throw new NotImplementedException();
        }

        public override bool WriteCoinToQRCode(CloudCoin cloudCoin, string OutputFile, string tag)
        {
            throw new NotImplementedException();
        }
    }
}
