using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UserValidationAPI.FileOperations
{
    public static class FileOparation { 
        private static readonly FileInfo _file;

        static FileOparation()
        {
            _file = new FileInfo();
            Create();
        }
        public static void Create()
        {
            if (!File.Exists(_file.fileName + "." + _file.extension))
                File.Create(_file.fileName + "." + _file.extension);
        }

        public static void WriteText(string errorText)
        {
            try
            {
                StreamWriter logFile = new StreamWriter(_file.fileName + "." + _file.extension);
                logFile.WriteLine(errorText);
                logFile.Close();
            }
            catch (Exception)
            {

            }
        }
    }
}
