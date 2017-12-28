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
    
    public partial class Assessor : Form
    {
        Form citiBankForm;
        private String args;
        public Assessor(String args)
        {
            InitializeComponent();
            this.args = args;
        }
        
        private void btnCitiBank_Click(object sender, EventArgs e)
        {
            citiBankForm = new CitiBankForm(this.args,"");
            this.Visible = false;
            citiBankForm.ShowDialog();
        }

        private void btnBankOfAmerica_Click(object sender, EventArgs e)
        {
            Form bankOfAmericaForm = new BankOfAmericaForm();
            this.Visible = false;
            bankOfAmericaForm.ShowDialog();
        }
    }
}
