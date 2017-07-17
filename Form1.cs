using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace SaveFormExample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void OnClickSave(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            string filename = sfd.FileName;
            if (string.IsNullOrEmpty(filename)) return;
            FileInfo dirInfo = new FileInfo(filename);
            string directory = dirInfo.Directory.ToString(); ;
            if (!filename.Contains(".zip"))
                filename += ".zip";
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            //make a temporary directory
            string temp_path = @"C:\TestZip\";
            if (!Directory.Exists(temp_path))
                Directory.CreateDirectory(temp_path);

            //create the progress form
            Form progress_form = new Form();
            Label pg_label = new Label();
            progress_form.ShowInTaskbar = false;
            progress_form.Controls.Add(pg_label);
            progress_form.Show();

            //export the files
            for (int i = 0; i < 10; i++)
            {
                using (StreamWriter sw = new StreamWriter(temp_path + "file" + i + ".txt"))
                {
                    string line = "first\n" + sw + "\n" + sw + "\n" + "last";
                    //string[] lines = line.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    string[] lines = line.Split('\n');
                    line = string.Join(Environment.NewLine, lines.Take(lines.Length - 1).Skip(1));
                    sw.WriteLine(line);
                }
                pg_label.Text = "File: " + (i+1) + "/" + 10;
                pg_label.Update();
                System.Threading.Thread.Sleep(1000);
            }
            //make the zip archive
            ZipFile.CreateFromDirectory(temp_path, filename);
            pg_label.Text = "Export Completed";

            //delete the temporary files
            Directory.Delete(temp_path, true);
            System.Threading.Thread.Sleep(2000);
            //progress_form.Focus();
            progress_form.Close();

            //open to the exported file location
            System.Diagnostics.Process.Start(directory);
        }
    }
}
