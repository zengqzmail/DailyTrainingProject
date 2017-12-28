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
    /// Interaction logic for AddPayee3.xaml
    /// </summary>
    public partial class AddPayee3 : UserControl
    {
        int errors;
        public AddPayee3()
        {
            InitializeComponent();
            errors = 0;
        }

        private void Other_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 1)
            {
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "OtherClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Correct");
                MainWindow.taskDataList.Add(dictionary);

                this.Content = new PayeeInfo3();
            }
        }

        private void Credit_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 1)
            {
                if (errors == 2)
                {
                    errors = 0;
                    new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "CreditClick");
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
                    new DialogWindow("You will need to add your Florida Power and Light account as a payee.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "CreditClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
        }
    }
}
