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

namespace GURPS_CER
{
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();
            PopulateListBox();
        }

        private void PopulateListBox()
        {
            DirectoryInfo dir = new DirectoryInfo(@".\Characters\");
            FileInfo[] files = dir.GetFiles("*");
            foreach(FileInfo file in files)
            {
                selectLoadFile.Items.Add(file.Name);
            }

        }

        private void loadFileButton_Click(object sender, EventArgs e)
        {
            if (selectLoadFile.SelectedIndex != -1)
            {
                string file = @".\Characters\" + selectLoadFile.SelectedItem.ToString();

                try
                {
                    this.Hide();
                    MainForm form1 = new MainForm();

                    form1.Load_File(file);
                    form1.FormClosing += new FormClosingEventHandler(this.MainForm_Closing);

                    form1.Show();
                }
                catch (IOException)
                {
                }
            }
            else
            {
                int size = -1;
                openFileDialog1.InitialDirectory = @".\Characters";
                DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
                if (result == DialogResult.OK) // Test result.
                {
                    string file = openFileDialog1.FileName;
                    try
                    {
                        this.Hide();
                        MainForm form1 = new MainForm();

                        form1.Load_File(file);
                        form1.FormClosing += new FormClosingEventHandler(this.MainForm_Closing);

                        form1.Show();
                    }
                    catch (IOException)
                    {
                    }
                }
                Console.WriteLine(size); // <-- Shows file size in debugging mode.
                Console.WriteLine(result);
            }
        }


        public void MainForm_Closing(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newFile_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm form1 = new MainForm();

            form1.Show();
        }
    }
}
