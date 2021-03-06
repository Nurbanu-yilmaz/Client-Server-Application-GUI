namespace _408_proje_grup6
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
            this.components = new System.ComponentModel.Container();
            this.ip_label = new System.Windows.Forms.Label();
            this.port_label = new System.Windows.Forms.Label();
            this.name_label = new System.Windows.Forms.Label();
            this.ip_text = new System.Windows.Forms.TextBox();
            this.port_text = new System.Windows.Forms.TextBox();
            this.name_text = new System.Windows.Forms.TextBox();
            this.rich_text = new System.Windows.Forms.RichTextBox();
            this.con_button = new System.Windows.Forms.Button();
            this.dis_button = new System.Windows.Forms.Button();
            this.bro_button = new System.Windows.Forms.Button();
            this.upload_button = new System.Windows.Forms.Button();
            this.list_button = new System.Windows.Forms.Button();
            this.download_button = new System.Windows.Forms.Button();
            this.copy_button = new System.Windows.Forms.Button();
            this.delete_button = new System.Windows.Forms.Button();
            this.textBox_download = new System.Windows.Forms.TextBox();
            this.textBox_copy = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_public = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox_public = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_owner = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ip_label
            // 
            this.ip_label.AutoSize = true;
            this.ip_label.Location = new System.Drawing.Point(20, 13);
            this.ip_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ip_label.Name = "ip_label";
            this.ip_label.Size = new System.Drawing.Size(20, 13);
            this.ip_label.TabIndex = 0;
            this.ip_label.Text = "IP:";
            // 
            // port_label
            // 
            this.port_label.AutoSize = true;
            this.port_label.Location = new System.Drawing.Point(20, 49);
            this.port_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.port_label.Name = "port_label";
            this.port_label.Size = new System.Drawing.Size(29, 13);
            this.port_label.TabIndex = 1;
            this.port_label.Text = "Port:";
            // 
            // name_label
            // 
            this.name_label.AutoSize = true;
            this.name_label.Location = new System.Drawing.Point(20, 82);
            this.name_label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.name_label.Name = "name_label";
            this.name_label.Size = new System.Drawing.Size(60, 13);
            this.name_label.TabIndex = 2;
            this.name_label.Text = "User Name";
            // 
            // ip_text
            // 
            this.ip_text.Location = new System.Drawing.Point(105, 8);
            this.ip_text.Margin = new System.Windows.Forms.Padding(2);
            this.ip_text.Name = "ip_text";
            this.ip_text.Size = new System.Drawing.Size(76, 20);
            this.ip_text.TabIndex = 3;
            // 
            // port_text
            // 
            this.port_text.Location = new System.Drawing.Point(105, 49);
            this.port_text.Margin = new System.Windows.Forms.Padding(2);
            this.port_text.Name = "port_text";
            this.port_text.Size = new System.Drawing.Size(76, 20);
            this.port_text.TabIndex = 4;
            // 
            // name_text
            // 
            this.name_text.Location = new System.Drawing.Point(105, 82);
            this.name_text.Margin = new System.Windows.Forms.Padding(2);
            this.name_text.Name = "name_text";
            this.name_text.Size = new System.Drawing.Size(76, 20);
            this.name_text.TabIndex = 5;
            // 
            // rich_text
            // 
            this.rich_text.Location = new System.Drawing.Point(199, 11);
            this.rich_text.Margin = new System.Windows.Forms.Padding(2);
            this.rich_text.Name = "rich_text";
            this.rich_text.ReadOnly = true;
            this.rich_text.Size = new System.Drawing.Size(301, 368);
            this.rich_text.TabIndex = 6;
            this.rich_text.Text = "";
            // 
            // con_button
            // 
            this.con_button.Location = new System.Drawing.Point(5, 121);
            this.con_button.Margin = new System.Windows.Forms.Padding(2);
            this.con_button.Name = "con_button";
            this.con_button.Size = new System.Drawing.Size(83, 19);
            this.con_button.TabIndex = 7;
            this.con_button.Text = "Connect";
            this.con_button.UseVisualStyleBackColor = true;
            this.con_button.Click += new System.EventHandler(this.con_button_Click);
            // 
            // dis_button
            // 
            this.dis_button.Enabled = false;
            this.dis_button.Location = new System.Drawing.Point(5, 154);
            this.dis_button.Margin = new System.Windows.Forms.Padding(2);
            this.dis_button.Name = "dis_button";
            this.dis_button.Size = new System.Drawing.Size(83, 19);
            this.dis_button.TabIndex = 8;
            this.dis_button.Text = "Disconnect";
            this.dis_button.UseVisualStyleBackColor = true;
            this.dis_button.Click += new System.EventHandler(this.dis_button_Click);
            // 
            // bro_button
            // 
            this.bro_button.Enabled = false;
            this.bro_button.Location = new System.Drawing.Point(105, 121);
            this.bro_button.Margin = new System.Windows.Forms.Padding(2);
            this.bro_button.Name = "bro_button";
            this.bro_button.Size = new System.Drawing.Size(83, 19);
            this.bro_button.TabIndex = 9;
            this.bro_button.Text = "Browse";
            this.bro_button.UseVisualStyleBackColor = true;
            this.bro_button.Click += new System.EventHandler(this.bro_button_Click);
            // 
            // upload_button
            // 
            this.upload_button.Enabled = false;
            this.upload_button.Location = new System.Drawing.Point(105, 154);
            this.upload_button.Margin = new System.Windows.Forms.Padding(2);
            this.upload_button.Name = "upload_button";
            this.upload_button.Size = new System.Drawing.Size(83, 19);
            this.upload_button.TabIndex = 10;
            this.upload_button.Text = "Upload";
            this.upload_button.UseVisualStyleBackColor = true;
            this.upload_button.Click += new System.EventHandler(this.upload_button_Click);
            // 
            // list_button
            // 
            this.list_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.list_button.Enabled = false;
            this.list_button.Location = new System.Drawing.Point(5, 185);
            this.list_button.Name = "list_button";
            this.list_button.Size = new System.Drawing.Size(86, 23);
            this.list_button.TabIndex = 11;
            this.list_button.Text = "List My Files";
            this.list_button.UseVisualStyleBackColor = true;
            this.list_button.Click += new System.EventHandler(this.list_button_Click);
            // 
            // download_button
            // 
            this.download_button.Enabled = false;
            this.download_button.Location = new System.Drawing.Point(100, 276);
            this.download_button.Name = "download_button";
            this.download_button.Size = new System.Drawing.Size(75, 23);
            this.download_button.TabIndex = 12;
            this.download_button.Text = "Download";
            this.download_button.UseVisualStyleBackColor = true;
            this.download_button.Click += new System.EventHandler(this.download_button_Click);
            // 
            // copy_button
            // 
            this.copy_button.Enabled = false;
            this.copy_button.Location = new System.Drawing.Point(106, 313);
            this.copy_button.Name = "copy_button";
            this.copy_button.Size = new System.Drawing.Size(75, 23);
            this.copy_button.TabIndex = 13;
            this.copy_button.Text = "Copy";
            this.copy_button.UseVisualStyleBackColor = true;
            this.copy_button.Click += new System.EventHandler(this.copy_button_Click);
            // 
            // delete_button
            // 
            this.delete_button.Enabled = false;
            this.delete_button.Location = new System.Drawing.Point(105, 356);
            this.delete_button.Name = "delete_button";
            this.delete_button.Size = new System.Drawing.Size(75, 23);
            this.delete_button.TabIndex = 14;
            this.delete_button.Text = "Delete";
            this.delete_button.UseVisualStyleBackColor = true;
            this.delete_button.Click += new System.EventHandler(this.delete_button_Click);
            // 
            // textBox_download
            // 
            this.textBox_download.Location = new System.Drawing.Point(99, 220);
            this.textBox_download.Name = "textBox_download";
            this.textBox_download.Size = new System.Drawing.Size(81, 20);
            this.textBox_download.TabIndex = 15;
            // 
            // textBox_copy
            // 
            this.textBox_copy.Location = new System.Drawing.Point(7, 313);
            this.textBox_copy.Name = "textBox_copy";
            this.textBox_copy.Size = new System.Drawing.Size(81, 20);
            this.textBox_copy.TabIndex = 16;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 356);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(81, 20);
            this.textBox1.TabIndex = 17;
            // 
            // button_public
            // 
            this.button_public.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button_public.Enabled = false;
            this.button_public.Location = new System.Drawing.Point(106, 394);
            this.button_public.Name = "button_public";
            this.button_public.Size = new System.Drawing.Size(75, 23);
            this.button_public.TabIndex = 18;
            this.button_public.Text = "Make Public";
            this.button_public.UseVisualStyleBackColor = true;
            this.button_public.Click += new System.EventHandler(this.button_public_Click);
            // 
            // button2
            // 
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(99, 185);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(89, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "List Public Files";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox_public
            // 
            this.textBox_public.Location = new System.Drawing.Point(7, 397);
            this.textBox_public.Name = "textBox_public";
            this.textBox_public.Size = new System.Drawing.Size(81, 20);
            this.textBox_public.TabIndex = 20;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 223);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "File name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 252);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Owner name:";
            // 
            // textBox_owner
            // 
            this.textBox_owner.Location = new System.Drawing.Point(99, 249);
            this.textBox_owner.Name = "textBox_owner";
            this.textBox_owner.Size = new System.Drawing.Size(81, 20);
            this.textBox_owner.TabIndex = 25;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 429);
            this.Controls.Add(this.textBox_owner);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_public);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_public);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBox_copy);
            this.Controls.Add(this.textBox_download);
            this.Controls.Add(this.delete_button);
            this.Controls.Add(this.copy_button);
            this.Controls.Add(this.download_button);
            this.Controls.Add(this.list_button);
            this.Controls.Add(this.upload_button);
            this.Controls.Add(this.bro_button);
            this.Controls.Add(this.dis_button);
            this.Controls.Add(this.con_button);
            this.Controls.Add(this.rich_text);
            this.Controls.Add(this.name_text);
            this.Controls.Add(this.port_text);
            this.Controls.Add(this.ip_text);
            this.Controls.Add(this.name_label);
            this.Controls.Add(this.port_label);
            this.Controls.Add(this.ip_label);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ip_label;
        private System.Windows.Forms.Label port_label;
        private System.Windows.Forms.Label name_label;
        private System.Windows.Forms.TextBox ip_text;
        private System.Windows.Forms.TextBox port_text;
        private System.Windows.Forms.TextBox name_text;
        private System.Windows.Forms.RichTextBox rich_text;
        private System.Windows.Forms.Button con_button;
        private System.Windows.Forms.Button dis_button;
        private System.Windows.Forms.Button bro_button;
        private System.Windows.Forms.Button upload_button;
        private System.Windows.Forms.Button list_button;
        private System.Windows.Forms.Button download_button;
        private System.Windows.Forms.Button copy_button;
        private System.Windows.Forms.Button delete_button;
        private System.Windows.Forms.TextBox textBox_download;
        private System.Windows.Forms.TextBox textBox_copy;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_public;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_public;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_owner;
    }
}

