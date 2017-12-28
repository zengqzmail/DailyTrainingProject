namespace ATM
{
    partial class Assessor
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCitiBank = new System.Windows.Forms.Button();
            this.btnBankOfAmerica = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(60, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Welcome";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(60, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(295, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Please select one of the following banks.";
            // 
            // btnCitiBank
            // 
            this.btnCitiBank.BackgroundImage = global::ATM.Properties.Resources.CitiBankLogo;
            this.btnCitiBank.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCitiBank.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCitiBank.Location = new System.Drawing.Point(60, 230);
            this.btnCitiBank.Name = "btnCitiBank";
            this.btnCitiBank.Size = new System.Drawing.Size(160, 60);
            this.btnCitiBank.TabIndex = 1;
            this.btnCitiBank.UseVisualStyleBackColor = true;
            this.btnCitiBank.Click += new System.EventHandler(this.btnCitiBank_Click);
            // 
            // btnBankOfAmerica
            // 
            this.btnBankOfAmerica.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnBankOfAmerica.BackgroundImage = global::ATM.Properties.Resources.BankOfAmericaLogo;
            this.btnBankOfAmerica.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBankOfAmerica.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBankOfAmerica.Location = new System.Drawing.Point(60, 151);
            this.btnBankOfAmerica.Name = "btnBankOfAmerica";
            this.btnBankOfAmerica.Size = new System.Drawing.Size(160, 60);
            this.btnBankOfAmerica.TabIndex = 0;
            this.btnBankOfAmerica.UseVisualStyleBackColor = false;
            this.btnBankOfAmerica.Click += new System.EventHandler(this.btnBankOfAmerica_Click);
            // 
            // Assessor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 352);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCitiBank);
            this.Controls.Add(this.btnBankOfAmerica);
            this.Name = "Assessor";
            this.Text = "Welcome";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBankOfAmerica;
        private System.Windows.Forms.Button btnCitiBank;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}