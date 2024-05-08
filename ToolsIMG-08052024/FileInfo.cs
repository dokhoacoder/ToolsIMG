using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ToolsIMG
{
    internal class FileInfo
    {
        private string name;
        private string path;
        private string type;
        public FileInfo(string name, string path, string type)
        {
            this.name = name;
            this.type = type;
            this.path = path;
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
    }
}