using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OnlineBanking
{
    /// <summary>
    /// Interaction logic for PayeeInfo1.xaml
    /// </summary>
    public partial class PayeeInfo1 : UserControl
    {
        int errors;
        public PayeeInfo1()
        {
            InitializeComponent();
            errors = 0;
        }

        private void AddPayee_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 1)
            {
                if (CardType.SelectionBoxItem.ToString() == "VISA" && textBox.Text == "boacreditcard" && textBox1.Text == "123456789012")
                {
                    new DialogWindow("You have successfully added boa credit card account \n\n123456789012\n\nas a payee!", "Confirm").ShowDialog();
                    new DialogWindow("Next, please proceed to add your AT&T account as a payee.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AddPayeeConfirm");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Correct");
                    MainWindow.taskDataList.Add(dictionary);

                    this.Content = new SelectTask1dup();

                }
                else
                {
                    if (errors == 2)
                    {
                        errors = 0;
                        new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "AddPayeeConfirm");
                        dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                        dictionary.Add("eventSummary", "Incorrect");
                        MainWindow.taskDataList.Add(dictionary);

                        Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                        dictionary1.Add("time", DateTime.Now.ToString());
                        dictionary1.Add("eventType", "MaxErrorsReached");
                        dictionary1.Add("eventData", (MainWindow.subtask + 1).ToString());
                        dictionary1.Add("eventSummary", "");
                        MainWindow.taskDataList.Add(dictionary1);

                        Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                        dictionary2.Add("time", DateTime.Now.ToString());
                        dictionary2.Add("eventType", "SubTaskCompleted");
                        dictionary2.Add("eventData", (MainWindow.subtask + 1).ToString());
                        dictionary2.Add("eventSummary", "");
                        MainWindow.taskDataList.Add(dictionary2);

                        MainWindow.subtask++;
                        this.Content = new SelectTask1();

                        Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                        dictionary3.Add("time", DateTime.Now.ToString());
                        dictionary3.Add("eventType", "SubTaskStart");
                        dictionary3.Add("eventData", (MainWindow.subtask + 1).ToString());
                        dictionary3.Add("eventSummary", "");
                        MainWindow.taskDataList.Add(dictionary3);

                        new DialogWindow("Subtask 3 description:\nYour task is to use your account to pay some bills online. You have to pay three bills using your checking account. The bills are:\n\nAT&T:$60.00\n\nFlorida Power and Light:$65.00\n\nBank of America:$50.00", "Continue").ShowDialog();
                        new DialogWindow("If you do not have enough money you need to transfer money from your savings account to your checking account", "Continue").ShowDialog();

                    }
                    else
                    {
                        new DialogWindow("Use the given information to add BOA Visa Credit Card as a payee, please try again.", "OK").ShowDialog();
                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "AddPayeeConfirm");
                        dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                        dictionary.Add("eventSummary", "Incorrect");
                        MainWindow.taskDataList.Add(dictionary);
                        errors++;
                    }

                    
                }
            }
        }
    }
}
