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
    public partial class Form1 : Form
    {
        static Form1 form1;

        public Form1()
        {
            InitializeComponent();
        }

        public void ShowBox(string message, string label)
        {
            form1 = new Form1();
            form1.textBox1.Text = message;
            
            form1.button1.Text = label;
            form1.ShowDialog();
           // form1.label2.Text = label;
            
           // MessageBox.Show(label);
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            this.Dispose();


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
