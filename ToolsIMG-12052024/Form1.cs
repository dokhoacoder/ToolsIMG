
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolsIMG
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private int progressValue = 0;
        DriveInfo[] drives = DriveInfo.GetDrives();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double stepProgress = 0;
            resetFilter();
            var rsPickFolder = folderBrowserDialog1.ShowDialog();
            if(rsPickFolder == DialogResult.OK)
            {
                GlobalVar.pathFolder = folderBrowserDialog1.SelectedPath;

                if (IsDrivePath(GlobalVar.pathFolder))
                {
                    MessageBox.Show("Vui lòng chọn folder trong ổ đĩa");
                }
                else
                {
                    this.filterByType(GlobalVar.TypeFilter);
                    textBox1.Text = GlobalVar.pathFolder;
                    int totalFilesOne = GlobalVar.listFilesConditionOne.Count;
                    int totalFilesTWo = GlobalVar.listFilesConditionTwo.Count;
                    label2.Text = GlobalVar.listFilesConditionOne.Count.ToString();
                    label3.Text = GlobalVar.listFilesConditionTwo.Count.ToString();
                    int progressBarMax = 100; // Đặt giá trị tối đa của ProgressBar là 1000
                    double percentage = (double)GlobalVar.listFilesInfoOne.Count / totalFilesOne * 100;
                    int progressBarValue = (int)(percentage / 100 * progressBarMax);
                    if (GlobalVar.listFilesConditionOne.Count > 0 && GlobalVar.listFilesConditionTwo.Count > 0)
                    {
                        GlobalVar.listFilesConditionOne.ForEach(item =>
                        {
                            GlobalVar.listFilesInfoOne.Add(new FileInfo(Path.GetFileNameWithoutExtension(item), Path.GetFullPath(item), Path.GetExtension(item)));
                        });
                        GlobalVar.listFilesConditionTwo.ForEach(item =>
                        {
                            GlobalVar.listFilesInfoTwo.Add(new FileInfo(Path.GetFileNameWithoutExtension(item), Path.GetFullPath(item), Path.GetExtension(item)));
                        });
                        //progressBar1.Value = progressBarValue;

                        // Nếu bạn muốn làm cho ProgressBar trông trơn tru hơn, 
                        // bạn có thể gọi Application.DoEvents() để làm cho giao diện người dùng được cập nhật.
                        Application.DoEvents();
                        MessageBox.Show("Đếm ảnh thành công, Vui lòng click 'Phân loại' để chia ảnh theo Folder");
                        progressBar1.Value = 0;
                        dataGridView1.DataSource = GlobalVar.listFilesInfoOne;
                        dataGridView2.DataSource = GlobalVar.listFilesInfoTwo;
                        dataGridView1.Columns[0].HeaderText = "Tên ảnh";
                        dataGridView1.Columns[1].HeaderText = "Loại ảnh";
                        dataGridView1.Columns[1].Width = 120;
                        dataGridView1.Columns[2].HeaderText = "Đường dẫn (Click để copy)";
                        dataGridView2.Columns[0].HeaderText = "Tên ảnh";
                        dataGridView2.Columns[1].HeaderText = "Loại ảnh";
                        dataGridView2.Columns[1].Width = 120;
                        dataGridView2.Columns[2].HeaderText = "Đường dẫn (Click để copy)";
                    }
                    else
                    {
                        MessageBox.Show("Folder chỉ có một loại ảnh hoăc không có ảnh , không cần phân loại");
                        progressBar1.Value = progressBar1.Maximum;
                    }
                }
            }    
           


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Clipboard.SetText(dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString());
            MessageBox.Show("Copy to clipboard");
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Clipboard.SetText(dataGridView2[e.ColumnIndex, e.RowIndex].Value.ToString());
            MessageBox.Show("Copy to clipboard");
        }

        // Kiểm tra xem đường dẫn có là một ổ đĩa và không phải là thư mục con của nó
        public bool IsDrivePath(string path)
        {
            // Lấy danh sách các ổ đĩa trên hệ thống
            DriveInfo[] drives = DriveInfo.GetDrives();

            // Kiểm tra xem đường dẫn có là một trong số các ổ đĩa hay không
            foreach (DriveInfo drive in drives)
            {
                if (path.StartsWith(drive.Name) && path.Length == drive.Name.Length)
                {
                    // Kiểm tra xem đường dẫn có phải là thư mục con của ổ đĩa hay không
                    string[] directories = Directory.GetDirectories(drive.Name);
                    foreach (string directory in directories)
                    {
                        if (path == directory)
                        {
                            // Đường dẫn là một thư mục con của ổ đĩa
                            return false;
                        }
                    }
                    // Đường dẫn là ổ đĩa
                    return true;
                }
            }
            // Không phải là ổ đĩa
            return false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            GlobalVar.TypeFilter = 1;
            label1.Text = "Số lượng ảnh JPG :";
            label4.Text = "Số lượng ảnh RAW :";
        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            GlobalVar.TypeFilter = 2;
            label1.Text = "Số lượng ảnh PNG :";
            label4.Text = "Số lượng ảnh RAW :";
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            GlobalVar.TypeFilter = 3;
            label1.Text = "Số lượng ảnh JPG :";
            label4.Text = "Số lượng ảnh PNG :";
        }

        private void filterByType(int type)
        {
            var rawExtensions = new List<string> { ".cr2", ".nef", ".arw", ".raf", ".orf", ".rw2", ".pef", ".dng" };
            var jpgExtensions = new List<string> { ".jpeg", ".jpg" };
            var pngExtensions = new List<string> { ".png" };

            switch (type) {
                case 2:
                    GlobalVar.listFilesConditionOne = Directory.EnumerateFiles(GlobalVar.pathFolder, "*.*", SearchOption.AllDirectories)
                                .Where(s => jpgExtensions.Contains(Path.GetExtension(s).ToLower()))
                                .ToList(); ;
                    GlobalVar.listFilesConditionTwo = Directory.EnumerateFiles(GlobalVar.pathFolder, "*.*", SearchOption.AllDirectories)
                                .Where(s => rawExtensions.Contains(Path.GetExtension(s).ToLower()))
                                .ToList();
                    break;
                case 3:
                    GlobalVar.listFilesConditionOne = Directory.EnumerateFiles(GlobalVar.pathFolder, "*.*", SearchOption.AllDirectories)
                                .Where(s => jpgExtensions.Contains(Path.GetExtension(s).ToLower()))
                                .ToList(); ;
                    GlobalVar.listFilesConditionTwo = Directory.EnumerateFiles(GlobalVar.pathFolder, "*.*", SearchOption.AllDirectories)
                                .Where(s => pngExtensions.Contains(Path.GetExtension(s).ToLower()))
                                .ToList(); ;
                    break;
                default:
                    GlobalVar.listFilesConditionOne = Directory.EnumerateFiles(GlobalVar.pathFolder, "*.*", SearchOption.AllDirectories)
                                .Where(s => jpgExtensions.Contains(Path.GetExtension(s).ToLower()))
                                .ToList();
                    GlobalVar.listFilesConditionTwo = Directory.EnumerateFiles(GlobalVar.pathFolder, "*.*", SearchOption.AllDirectories)
                                .Where(s => rawExtensions.Contains(Path.GetExtension(s).ToLower()))
                                .ToList();
                    break;
            }
        }

        private void resetFilter()
        {
            GlobalVar.pathFolder = "";
            GlobalVar.listFilesConditionOne = new List<string> { };
            GlobalVar.listFilesConditionTwo = new List<string> { };
            GlobalVar.listFilesInfoOne = new List<FileInfo>{};
            GlobalVar.listFilesInfoTwo = new List<FileInfo>{};
        }

        static async Task<string> GetAPIForPick(string driveUrl, string albumSecretKey)
        {
            try
            {
                // Tạo HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Tạo dữ liệu form-urlencoded
                    var formData = new List<KeyValuePair<string, string>>();
                    formData.Add(new KeyValuePair<string, string>("driveUrl", driveUrl));
                    formData.Add(new KeyValuePair<string, string>("albumSecretKey", albumSecretKey));

                    // Tạo nội dung của yêu cầu HTTP dưới dạng form-urlencoded
                    var content = new FormUrlEncodedContent(formData);

                    // Gửi yêu cầu POST đến API
                    HttpResponseMessage response = await client.PostAsync("https://s2.io.vn/create?_data=routes%2Fcreate", content);

                    // Đọc phản hồi từ API
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(responseBody);

                    // Lấy giá trị pageId từ đối tượng phản hồi
                    string pageId = responseObject.response.pageId;

                    // Cộng chuỗi pageId vào sau "https://s2.io.vn/albums/"
                    string albumUrl = $"https://s2.io.vn/albums/{pageId}";

                    // In URL của album
                    return albumUrl;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return "";
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            string driveUrl = textBox2.Text;
            //string driveUrl = "https://drive.google.com/drive/folders/1S9noaP-ZOI9hcLY3KObbRMnyfi3_yEE9?usp=sharing";
            string albumSecretKey = ""; // Thay thế bằng album secret key thực tế của bạn
            string urlAlbulm = await GetAPIForPick(driveUrl, albumSecretKey);
            textBox4.Text = urlAlbulm;
        }

        private void textBox4_Click(object sender, EventArgs e)
        {
            // Sao chép nội dung của TextBox vào Clipboard
            Clipboard.SetText(textBox4.Text);
            MessageBox.Show("Đã sao chép nội dung.");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (var myDialogForm = new MyDialogFormCustom())
            {
                var result = myDialogForm.ShowDialog(); // Hiển thị form dialog và chờ đợi cho đến khi form đóng lại
                                                        // Xử lý kết quả trả về nếu cần thiết
                if (result == DialogResult.OK)
                {
                    // Thực hiện hành động khi người dùng chọn OK
                }
                else if (result == DialogResult.Cancel)
                {
                    // Thực hiện hành động khi người dùng chọn Cancel hoặc đóng form
                }
            }
        }
    }
}