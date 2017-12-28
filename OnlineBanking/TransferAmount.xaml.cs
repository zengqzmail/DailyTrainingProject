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
    /// Interaction logic for TransferAmount.xaml
    /// </summary>
    public partial class TransferAmount : UserControl
    {
        int errors;
        public TransferAmount()
        {
            InitializeComponent();
            errors = 0;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 0)
            {
                string amount = textBox.Text;
                double convert_amount = 0;
                try
                {
                    convert_amount = Convert.ToDouble(amount);
                }
                catch
                {
                    Console.WriteLine("Unable to convert.");
                }

                if (From.SelectionBoxItem.ToString() == "Saving Account --- $325.00 available" && To.SelectionBoxItem.ToString() == "Checking Account --- $45.00 available" && convert_amount == 15)
                {
                    new DialogWindow("           Verification Page \n\nYou have successfully transfered $15.00 from your saving account to your checking account.", "Confirm").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "TransferAmountConfirm");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Correct");
                    MainWindow.taskDataList.Add(dictionary);

                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "SubTaskCompleted");
                    dictionary1.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary1.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary1);

                    new DialogWindow("You have completed this subtask! Please proceed to the next subtask, thank you!", "Continue").ShowDialog();

                    
                    MainWindow.subtask++;
                    errors = 0;

                    this.Content = new SelectTask1();

                    Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                    dictionary2.Add("time", DateTime.Now.ToString());
                    dictionary2.Add("eventType", "SubTaskStart");
                    dictionary2.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary2.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary2);

                    new DialogWindow("Subtask 2 description:\nNow you need to set up three accounts in the online bill pay function so that you can pay your bills online. You will set up accounts to pay three bills: Bank of America Visa Credit Card, AT&T, Florida Power and Light.", "Continue").ShowDialog();

                }
                else
                {
                    if (errors == 2)
                    {
                        errors = 0;
                        new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "TransferAmountConfirm");
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

                        new DialogWindow("Subtask 2 description:\nNow you need to set up three accounts in the online bill pay function so that you can pay your bills online. You will set up accounts to pay three bills: Bank of America Visa Credit Card, AT&T, Florida Power and Light.", "Continue").ShowDialog();

                    }
                    else
                    {
                        new DialogWindow("You need $5.00 for metro and $55.00 for groceries. You have $45.00 in your checking account. If you do not have enough money, you need to transfer money from your saving account to your checking account.", "OK").ShowDialog();
                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "TransferAmountConfirm");
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
