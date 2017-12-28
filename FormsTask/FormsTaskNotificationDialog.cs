using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FormsTask
{
    public partial class FormsTaskNotificationDialog : Form
    {
       
        public FormsTaskNotificationDialog(string _notificationText)
        {
            InitializeComponent();
            this.notificationTextLabel.Text = _notificationText;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public partial class FormsTaskNotifier
    {
        public static void Notify(string notificationMessage)
        {
            FormsTaskNotificationDialog f = new FormsTaskNotificationDialog(notificationMessage);
            f.ShowDialog();
        }
    }
}
