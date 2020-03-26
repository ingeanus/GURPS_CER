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

// Author : ingeanus https://github.com/ingeanus/GURPS_CER
// Date : 26-Mar-20
// Can you use this code yourself? I dunno why you would but sure, just credit me I guess.
namespace GURPS_CER
{
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();
            PopulateListBox();
        }

        // Just looks for every file in the Characters folder using DirectoryInfo and FileInfo.
        private void PopulateListBox()
        {
            DirectoryInfo dir = new DirectoryInfo(@".\Characters\");
            FileInfo[] files = dir.GetFiles("*");
            foreach(FileInfo file in files)
            {
                selectLoadFile.Items.Add(file.Name);
            }

        }

        // Handles the two forms of loading in a character.
        private void loadFileButton_Click(object sender, EventArgs e)
        {
            if (selectLoadFile.SelectedIndex != -1)                             // If nothing is selected, it opens up dialog to browse and select a file.
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
            else                                                              // If a file is selected on the right, open that instead.
            {
                openFileDialog1.InitialDirectory = @".\Characters";
                DialogResult result = openFileDialog1.ShowDialog();
                if (result == DialogResult.OK) 
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
            }
        }

        // Makes sure that when the form closes, the Application quits.
        public void MainForm_Closing(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // If you click new, it just starts a basic Form.
        private void newFile_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm form1 = new MainForm();
            form1.FormClosing += new FormClosingEventHandler(this.MainForm_Closing);

            form1.Show();
        }

        // If you select anything on the left, it changes the Load button from Load As to Load.
        private void selectLoadFile_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.loadFileButton.Text = "Load";
        }
    }
}
