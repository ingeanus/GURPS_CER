namespace GURPS_CER
{
    partial class LoadForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadFileButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.newFile = new System.Windows.Forms.Button();
            this.selectLoadFile = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // loadFileButton
            // 
            this.loadFileButton.Location = new System.Drawing.Point(260, 41);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(75, 23);
            this.loadFileButton.TabIndex = 0;
            this.loadFileButton.Text = "Load As";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.loadFileButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Character File Here";
            // 
            // newFile
            // 
            this.newFile.Location = new System.Drawing.Point(260, 12);
            this.newFile.Name = "newFile";
            this.newFile.Size = new System.Drawing.Size(75, 23);
            this.newFile.TabIndex = 1;
            this.newFile.Text = "New";
            this.newFile.UseVisualStyleBackColor = true;
            this.newFile.Click += new System.EventHandler(this.newFile_Click);
            // 
            // selectLoadFile
            // 
            this.selectLoadFile.FormattingEnabled = true;
            this.selectLoadFile.Location = new System.Drawing.Point(5, 4);
            this.selectLoadFile.Name = "selectLoadFile";
            this.selectLoadFile.Size = new System.Drawing.Size(240, 355);
            this.selectLoadFile.TabIndex = 2;
            this.selectLoadFile.SelectedIndexChanged += new System.EventHandler(this.selectLoadFile_SelectedIndexChanged);
            // 
            // LoadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 371);
            this.Controls.Add(this.selectLoadFile);
            this.Controls.Add(this.newFile);
            this.Controls.Add(this.loadFileButton);
            this.Name = "LoadForm";
            this.Text = "Load / Create a new Character";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button newFile;
        private System.Windows.Forms.ListBox selectLoadFile;
    }
}

