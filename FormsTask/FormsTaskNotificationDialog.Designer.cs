namespace FormsTask
{
    partial class FormsTaskNotificationDialog
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
            this.notificationTextLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // notificationTextLabel
            // 
            this.notificationTextLabel.AutoSize = true;
            this.notificationTextLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notificationTextLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.notificationTextLabel.Location = new System.Drawing.Point(0, 0);
            this.notificationTextLabel.Margin = new System.Windows.Forms.Padding(10, 20, 3, 0);
            this.notificationTextLabel.MaximumSize = new System.Drawing.Size(500, 150);
            this.notificationTextLabel.Name = "notificationTextLabel";
            this.notificationTextLabel.Size = new System.Drawing.Size(283, 31);
            this.notificationTextLabel.TabIndex = 0;
            this.notificationTextLabel.Text = "<message goes here>";
            // 
            // okButton
            // 
            this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.okButton.Location = new System.Drawing.Point(250, 140);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 75);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // FormsTaskNotificationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 222);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.notificationTextLabel);
            this.Name = "FormsTaskNotificationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Forms Task";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label notificationTextLabel;
        private System.Windows.Forms.Button okButton;
    }
}