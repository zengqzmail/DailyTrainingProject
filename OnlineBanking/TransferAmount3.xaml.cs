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
    /// Interaction logic for TransferAmount3.xaml
    /// </summary>
    public partial class TransferAmount3 : UserControl
    {
        int errors;
        public TransferAmount3()
        {
            InitializeComponent();
            errors = 0;
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 3)
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

                if (From.SelectionBoxItem.ToString() == "Saving Account --- $135.00 available" && To.SelectionBoxItem.ToString() == "Checking Account --- $0.00 available" && convert_amount == 70)
                {
                    new DialogWindow("           Verification Page \n\nYou have successfully transfered $70.00 from your saving account to your checking account.", "Confirm").ShowDialog();
                    new DialogWindow("After returning automatically to the home page, please log out your account.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "TransferAmountConfirm");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Correct");
                    MainWindow.taskDataList.Add(dictionary);

                    this.Content = new SignOff();
                }
                else
                {
                    if (errors == 2)
                    {
                        errors = 0;

                        new DialogWindow("You have tried four times. Please proceed to exit this subtask.", "Continue").ShowDialog();

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


                        Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                        dictionary3.Add("time", DateTime.Now.ToString());
                        dictionary3.Add("eventType", "TaskClose");
                        dictionary3.Add("eventData", (MainWindow.subtask + 1).ToString());
                        dictionary3.Add("eventSummary", "");
                        MainWindow.taskDataList.Add(dictionary3);

                        new DialogWindow("You have completed all the tasks. Thank you very much!", "Continue").ShowDialog();
                        Window parent = Window.GetWindow(this);
                        parent.Close();

                    }
                    else
                    {
                        new DialogWindow("Your task is to transfer $70.00 from your saving account to your checking account.", "OK").ShowDialog();
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
