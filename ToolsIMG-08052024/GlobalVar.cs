using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ToolsIMG
{
    internal class GlobalVar
    {
        public static string pathFolder = "";
        public static List<string> listFilesConditionOne;
        public static List<string> listFilesConditionTwo;
        public static List<FileInfo> listFilesInfoOne = new List<FileInfo>();
        public static List<FileInfo> listFilesInfoTwo = new List<FileInfo>();
        public static int TypeFilter = 1;
    }
}