using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ATM;
using System.Windows.Forms;

namespace MetroTickets
{
    /// <summary>
    /// Interaction logic for BuyTicket.xaml
    /// </summary>
    public partial class BuyTicket : System.Windows.Controls.UserControl
    {
        public BuyTicket()
        {
            InitializeComponent();
            errors = 0;
        }
        
        int errors;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 1)
            {
                MainWindow.makeMessage("Your task is:\n" + MainWindow.subtasks[MainWindow.subtask], "Continue");
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "BuyCardButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Incorrect");
                MainWindow.taskDataList.Add(dictionary);
                errors++;
            }
            else
            {
                this.Content = new BuyCardOptions();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 0)
            {
                MainWindow.makeMessage("Your task is:\n" + MainWindow.subtasks[MainWindow.subtask], "Continue");
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "BuyTicketButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Incorrect");
                MainWindow.taskDataList.Add(dictionary);
                errors++;
            }
            else
            {
                this.Content = new BuyTicketOptions();
            }
        }
    }
}
