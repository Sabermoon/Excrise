using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Excrise {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private FolderBrowserDialog contents = null;                //指定数据成员
        private class CheckOut {
            public string name;
            public long NUM = 1;
        }

        private void Button1_Click(object sender, EventArgs e) {        //目录选定
            contents = new FolderBrowserDialog();
            if (contents.ShowDialog() == DialogResult.OK) {
                //label1.Text = @"选择的目录为：" + contents.SelectedPath;
                textBox1.Text = contents.SelectedPath;
            }contents.SelectedPath = textBox1.Text;
            if (!Directory.Exists(contents.SelectedPath)) {
                MessageBox.Show(@"目录无效！");
                return;
            }DirectoryInfo folder = new DirectoryInfo(contents.SelectedPath);
            listView1.Items.Clear();
            FileInfo[] info = folder.GetFiles();
            foreach (FileInfo item in info)
                if (item.Extension != "")
                    listView1.Items.Add(new ListViewItem(new string[]
                        {item.Name.Replace(item.Extension, ""), null, item.Extension, null}));
                else
                    listView1.Items.Add(new ListViewItem(new string[] {item.Name, null, null, null}));
        }

        private void Button2_Click(object sender, EventArgs e) {                 //完成越界审查
            int begin = Convert.ToInt32(textBox2.Text) - 1;
            int num = Convert.ToInt32(textBox3.Text);
            if (begin < 0) begin = 0;
            long number = 1;
            foreach (ListViewItem item in listView1.Items) {
                if (begin + num > item.SubItems[0].Text.Length)
                    if (begin <= item.SubItems[0].Text.Length)
                        item.SubItems[1].Text = item.SubItems[0].Text.Remove(begin);
                    else {
                        item.SubItems[1].Text = item.SubItems[0].Text;
                        item.SubItems[3].Text = @"超出索引范围";
                    }
                else item.SubItems[1].Text = item.SubItems[0].Text.Remove(begin, num);
                List<CheckOut> check = new List<CheckOut>();                     //完成重名修正
                if (File.Exists(contents.SelectedPath + "\\" + item.SubItems[1].Text + item.SubItems[2].Text)) {
                    item.SubItems[3].Text = @"文件名重复，已修改";
                    do {
                        check.Add(new CheckOut { name = item.SubItems[0].Text });
                        check[0].NUM++;
                        item.SubItems[1].Text = item.SubItems[0].Text + @"(" +   @")";
                    } while (File.Exists(contents.SelectedPath + "\\" + item.SubItems[1].Text + item.SubItems[2].Text));
                }listView1.Refresh();
            }
        }

        private void button3_Click(object sender, EventArgs e) {
            foreach (ListViewItem item in listView1.Items){
                FileInfo temp =
                    new FileInfo(contents.SelectedPath + "\\" + item.SubItems[0].Text + item.SubItems[2].Text);
                temp.MoveTo(contents.SelectedPath+"\\"+ item.SubItems[1].Text+item.SubItems[2].Text);
                if (item.SubItems[3].Text != null)
                    item.SubItems[3].Text = @"重命名完成";
            }
        }
    }
}
