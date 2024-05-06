using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolsIMG
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            GlobalVar.pathFolder = folderBrowserDialog1.SelectedPath;
            GlobalVar.listFiles = Directory.GetFiles(GlobalVar.pathFolder,"*.jpg",SearchOption.AllDirectories).ToList();
            textBox1.Text = GlobalVar.pathFolder;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = GlobalVar.listFiles;
        }
    }
}
