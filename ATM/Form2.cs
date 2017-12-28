using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ATM
{
    public partial class Form2 : Form
    {
       // public static Form2 form2;
        public Boolean yesClick;
        public Form2()
        {
            InitializeComponent();
        }

        public void ShowBox(string message, string label, string label2)
        {
            //form2 = new Form2();
            textBox1.Text = message;
            
            button2.Text = label;
            button1.Text = label2;
            ShowDialog();
            // form1.label2.Text = label;

            // MessageBox.Show(label);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            yesClick = true;
            this.Close();
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            yesClick = false;
            this.Close();
           
        }
    }
}
