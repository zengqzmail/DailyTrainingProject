namespace PrescriptionRefill
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.player = new AxWMPLib.AxWindowsMediaPlayer();
            this.buttonCall = new System.Windows.Forms.Button();
            this.panelPicture = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lableWelcomeEn = new System.Windows.Forms.Label();
            this.lableWelcomeSp = new System.Windows.Forms.Label();
            this.lablePhoneScreen = new System.Windows.Forms.Label();
            this.panelPhonePicture = new System.Windows.Forms.Panel();
            this.buttonHangUp = new System.Windows.Forms.Button();
            this.buttonPound = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.buttonStar = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.player)).BeginInit();
            this.panelPicture.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panelPhonePicture.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.Controls.Add(this.player, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonCall, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.panelPicture, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lableWelcomeEn, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lableWelcomeSp, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(-1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(782, 732);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // player
            // 
            this.player.Enabled = true;
            this.player.Location = new System.Drawing.Point(42, 3);
            this.player.Name = "player";
            this.player.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("player.OcxState")));
            this.player.Size = new System.Drawing.Size(33, 27);
            this.player.TabIndex = 0;
            // 
            // buttonCall
            // 
            this.buttonCall.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonCall.BackColor = System.Drawing.Color.SpringGreen;
            this.buttonCall.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCall.Location = new System.Drawing.Point(177, 592);
            this.buttonCall.Name = "buttonCall";
            this.buttonCall.Padding = new System.Windows.Forms.Padding(3);
            this.buttonCall.Size = new System.Drawing.Size(426, 83);
            this.buttonCall.TabIndex = 1;
            this.buttonCall.Text = "Call Miami Jacks Pharmacy\nLlamar Miami Jacks Pharmacy";
            this.buttonCall.UseVisualStyleBackColor = false;
            this.buttonCall.Click += new System.EventHandler(this.buttonCall_Click);
            // 
            // panelPicture
            // 
            this.panelPicture.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panelPicture.Controls.Add(this.pictureBox1);
            this.panelPicture.Controls.Add(this.pictureBox2);
            this.panelPicture.Location = new System.Drawing.Point(70, 238);
            this.panelPicture.Name = "panelPicture";
            this.panelPicture.Size = new System.Drawing.Size(641, 331);
            this.panelPicture.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::PrescriptionRefill.Properties.Resources.Ampliex;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(220, 331);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackgroundImage = global::PrescriptionRefill.Properties.Resources.Megalith;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(421, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(220, 331);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // lableWelcomeEn
            // 
            this.lableWelcomeEn.AccessibleDescription = "";
            this.lableWelcomeEn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lableWelcomeEn.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lableWelcomeEn, 2);
            this.lableWelcomeEn.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lableWelcomeEn.Location = new System.Drawing.Point(68, 33);
            this.lableWelcomeEn.Name = "lableWelcomeEn";
            this.lableWelcomeEn.Size = new System.Drawing.Size(685, 101);
            this.lableWelcomeEn.TabIndex = 3;
            this.lableWelcomeEn.Text = "Please call the Miami Jacks pharmacy to refill two of your prescriptions.\nYour prescriptions are displayed below.\nTo begin, touch the button on the computer screen that says “Call Miami Jacks Pharmacy”.";
            this.lableWelcomeEn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lableWelcomeSp
            // 
            this.lableWelcomeSp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lableWelcomeSp.AutoSize = true;
            this.lableWelcomeSp.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lableWelcomeSp.Location = new System.Drawing.Point(71, 134);
            this.lableWelcomeSp.Name = "lableWelcomeSp";
            this.lableWelcomeSp.Size = new System.Drawing.Size(639, 101);
            this.lableWelcomeSp.TabIndex = 4;
            this.lableWelcomeSp.Text = resources.GetString("lableWelcomeSp.Text");
            this.lableWelcomeSp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lablePhoneScreen
            // 
            this.lablePhoneScreen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lablePhoneScreen.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.lablePhoneScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lablePhoneScreen.ForeColor = System.Drawing.Color.White;
            this.lablePhoneScreen.Location = new System.Drawing.Point(77, 123);
            this.lablePhoneScreen.Name = "lablePhoneScreen";
            this.lablePhoneScreen.Size = new System.Drawing.Size(260, 60);
            this.lablePhoneScreen.TabIndex = 4;
            this.lablePhoneScreen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelPhonePicture
            // 
            this.panelPhonePicture.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelPhonePicture.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panelPhonePicture.BackgroundImage = global::PrescriptionRefill.Properties.Resources.phone;
            this.panelPhonePicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelPhonePicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPhonePicture.Controls.Add(this.lablePhoneScreen);
            this.panelPhonePicture.Controls.Add(this.buttonHangUp);
            this.panelPhonePicture.Controls.Add(this.buttonPound);
            this.panelPhonePicture.Controls.Add(this.button10);
            this.panelPhonePicture.Controls.Add(this.buttonStar);
            this.panelPhonePicture.Controls.Add(this.button9);
            this.panelPhonePicture.Controls.Add(this.button8);
            this.panelPhonePicture.Controls.Add(this.button7);
            this.panelPhonePicture.Controls.Add(this.button6);
            this.panelPhonePicture.Controls.Add(this.button5);
            this.panelPhonePicture.Controls.Add(this.button4);
            this.panelPhonePicture.Controls.Add(this.button3);
            this.panelPhonePicture.Controls.Add(this.button2);
            this.panelPhonePicture.Controls.Add(this.button1);
            this.panelPhonePicture.Location = new System.Drawing.Point(155, 24);
            this.panelPhonePicture.Name = "panelPhonePicture";
            this.panelPhonePicture.Size = new System.Drawing.Size(420, 660);
            this.panelPhonePicture.TabIndex = 0;
            // 
            // buttonHangUp
            // 
            this.buttonHangUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonHangUp.BackColor = System.Drawing.Color.Red;
            this.buttonHangUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHangUp.Location = new System.Drawing.Point(230, 200);
            this.buttonHangUp.Name = "buttonHangUp";
            this.buttonHangUp.Size = new System.Drawing.Size(110, 65);
            this.buttonHangUp.TabIndex = 3;
            this.buttonHangUp.Text = "Hang Up\n Colgar";
            this.buttonHangUp.UseVisualStyleBackColor = false;
            this.buttonHangUp.Click += new System.EventHandler(this.buttonHangUp_Click);
            // 
            // buttonPound
            // 
            this.buttonPound.BackColor = System.Drawing.Color.MediumBlue;
            this.buttonPound.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPound.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonPound.Location = new System.Drawing.Point(251, 517);
            this.buttonPound.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPound.Name = "buttonPound";
            this.buttonPound.Size = new System.Drawing.Size(80, 80);
            this.buttonPound.TabIndex = 10;
            this.buttonPound.Text = "#";
            this.buttonPound.UseVisualStyleBackColor = false;
            this.buttonPound.Click += new System.EventHandler(this.buttonPound_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.Color.MediumBlue;
            this.button10.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button10.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button10.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button10.Location = new System.Drawing.Point(168, 517);
            this.button10.Margin = new System.Windows.Forms.Padding(0);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(80, 80);
            this.button10.TabIndex = 11;
            this.button10.Text = "0";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // buttonStar
            // 
            this.buttonStar.AutoEllipsis = true;
            this.buttonStar.BackColor = System.Drawing.Color.MediumBlue;
            this.buttonStar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonStar.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonStar.FlatAppearance.BorderSize = 0;
            this.buttonStar.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStar.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.buttonStar.Location = new System.Drawing.Point(83, 517);
            this.buttonStar.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStar.Name = "buttonStar";
            this.buttonStar.Size = new System.Drawing.Size(80, 80);
            this.buttonStar.TabIndex = 9;
            this.buttonStar.Text = "*";
            this.buttonStar.UseVisualStyleBackColor = false;
            this.buttonStar.Click += new System.EventHandler(this.buttonStar_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.MediumBlue;
            this.button9.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button9.Location = new System.Drawing.Point(251, 435);
            this.button9.Margin = new System.Windows.Forms.Padding(0);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(80, 80);
            this.button9.TabIndex = 7;
            this.button9.Text = "9";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button8
            // 
            this.button8.BackColor = System.Drawing.Color.MediumBlue;
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button8.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button8.Location = new System.Drawing.Point(168, 435);
            this.button8.Margin = new System.Windows.Forms.Padding(0);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(80, 80);
            this.button8.TabIndex = 8;
            this.button8.Text = "8";
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button7
            // 
            this.button7.AutoEllipsis = true;
            this.button7.BackColor = System.Drawing.Color.MediumBlue;
            this.button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button7.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button7.Location = new System.Drawing.Point(83, 435);
            this.button7.Margin = new System.Windows.Forms.Padding(0);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(80, 80);
            this.button7.TabIndex = 6;
            this.button7.Text = "7";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.MediumBlue;
            this.button6.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button6.Location = new System.Drawing.Point(251, 355);
            this.button6.Margin = new System.Windows.Forms.Padding(0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(80, 80);
            this.button6.TabIndex = 4;
            this.button6.Text = "6";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.MediumBlue;
            this.button5.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button5.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button5.Location = new System.Drawing.Point(168, 355);
            this.button5.Margin = new System.Windows.Forms.Padding(0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(80, 80);
            this.button5.TabIndex = 5;
            this.button5.Text = "5";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.AutoEllipsis = true;
            this.button4.BackColor = System.Drawing.Color.MediumBlue;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button4.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button4.Location = new System.Drawing.Point(83, 355);
            this.button4.Margin = new System.Windows.Forms.Padding(0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 80);
            this.button4.TabIndex = 3;
            this.button4.Text = "4";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.MediumBlue;
            this.button3.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button3.Location = new System.Drawing.Point(251, 275);
            this.button3.Margin = new System.Windows.Forms.Padding(0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 80);
            this.button3.TabIndex = 2;
            this.button3.Text = "3";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.MediumBlue;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button2.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.Location = new System.Drawing.Point(168, 275);
            this.button2.Margin = new System.Windows.Forms.Padding(0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 80);
            this.button2.TabIndex = 2;
            this.button2.Text = "2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.AutoEllipsis = true;
            this.button1.BackColor = System.Drawing.Color.MediumBlue;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.Font = new System.Drawing.Font("Times New Roman", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button1.Location = new System.Drawing.Point(83, 275);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 80);
            this.button1.TabIndex = 0;
            this.button1.Text = "1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 731);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.player)).EndInit();
            this.panelPicture.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panelPhonePicture.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AxWMPLib.AxWindowsMediaPlayer player;
        private System.Windows.Forms.Button buttonCall;
        private System.Windows.Forms.Panel panelPicture;
        private System.Windows.Forms.Label lableWelcomeEn;
        private System.Windows.Forms.Label lableWelcomeSp;
        private System.Windows.Forms.Label lablePhoneScreen;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;

        //panel phone

        private System.Windows.Forms.Panel panelPhonePicture;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button buttonPound;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button buttonStar;
        private System.Windows.Forms.Button buttonHangUp;
    }
}

