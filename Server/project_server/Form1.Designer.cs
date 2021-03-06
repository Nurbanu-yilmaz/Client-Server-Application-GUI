namespace project_server
{
    partial class Form1
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
            this.listen_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // listen_button
            // 
            this.listen_button.Enabled = false;
            this.listen_button.Location = new System.Drawing.Point(228, 31);
            this.listen_button.Name = "listen_button";
            this.listen_button.Size = new System.Drawing.Size(75, 23);
            this.listen_button.TabIndex = 0;
            this.listen_button.Text = "Listen";
            this.listen_button.UseVisualStyleBackColor = true;
            this.listen_button.Click += new System.EventHandler(this.listen_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port";
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(71, 31);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(100, 20);
            this.textBox_port.TabIndex = 2;
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(228, 71);
            this.logs.Name = "logs";
            this.logs.ReadOnly = true;
            this.logs.Size = new System.Drawing.Size(213, 210);
            this.logs.TabIndex = 3;
            this.logs.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(31, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(129, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Choose Path";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 311);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.textBox_port);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listen_button);
            this.Name = "Form1";
            this.Text = "Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button listen_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

