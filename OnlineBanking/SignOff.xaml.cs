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
    /// Interaction logic for SignOff.xaml
    /// </summary>
    public partial class SignOff : UserControl
    {
        int errors;
        public SignOff()
        {
            InitializeComponent();
            errors = 0;
        }

        private void Acounts_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 2)
            {
                if (errors == 2)
                {
                    errors = 0;
                    new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountsClick");
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

                    this.Content = new SignOn();

                    Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "SubTaskStart");
                    dictionary3.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary3.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary3);

                    new DialogWindow("Subtask 4 description:\nNow that you are back from the grocery store and paid your bills. You need to transfer some money into your checking account to make sure you have some for the future. You want to transfer $70.00 from you saving account to your checking account.", "Continue").ShowDialog();

                }
                else
                {
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountsClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
            else if (MainWindow.subtask == 3)
            {
                if (errors == 2)
                {
                    errors = 0;

                    new DialogWindow("You have tried four times. Please proceed to exit this subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountsClick");
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
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountsClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
        }

        private void PaymentTransfer_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 2)
            {
                if (errors == 2)
                {
                    errors = 0;
                    new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "PaymentTransferClick");
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

                    this.Content = new SignOn();

                    Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "SubTaskStart");
                    dictionary3.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary3.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary3);

                    new DialogWindow("Subtask 4 description:\nNow that you are back from the grocery store and paid your bills. You need to transfer some money into your checking account to make sure you have some for the future. You want to transfer $70.00 from you saving account to your checking account.", "Continue").ShowDialog();

                }
                else
                {
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "PaymentTransferClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
            else if (MainWindow.subtask == 3)
            {
                if (errors == 2)
                {
                    errors = 0;

                    new DialogWindow("You have tried four times. Please proceed to exit this subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "PaymentTransferClick");
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
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "PaymentTransferClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
        }

        private void Investments_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 2)
            {
                if (errors == 2)
                {
                    errors = 0;
                    new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "InvestmentsClick");
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

                    this.Content = new SignOn();

                    Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "SubTaskStart");
                    dictionary3.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary3.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary3);

                    new DialogWindow("Subtask 4 description:\nNow that you are back from the grocery store and paid your bills. You need to transfer some money into your checking account to make sure you have some for the future. You want to transfer $70.00 from you saving account to your checking account.", "Continue").ShowDialog();

                }
                else
                {
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "InvestmentsClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
            else if (MainWindow.subtask == 3)
            {
                if (errors == 2)
                {
                    errors = 0;

                    new DialogWindow("You have tried four times. Please proceed to exit this subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "InvestmentsClick");
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
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "InvestmentsClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
        }

        private void FinancialTools_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 2)
            {
                if (errors == 2)
                {
                    errors = 0;
                    new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "FinancialToolsClick");
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

                    this.Content = new SignOn();

                    Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "SubTaskStart");
                    dictionary3.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary3.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary3);

                    new DialogWindow("Subtask 4 description:\nNow that you are back from the grocery store and paid your bills. You need to transfer some money into your checking account to make sure you have some for the future. You want to transfer $70.00 from you saving account to your checking account.", "Continue").ShowDialog();

                }
                else
                {
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "FinancialToolsClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
            else if (MainWindow.subtask == 3)
            {
                if (errors == 2)
                {
                    errors = 0;

                    new DialogWindow("You have tried four times. Please proceed to exit this subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "FinancialToolsClick");
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
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "FinancialToolsClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
        }

        private void AccountManagement_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 2)
            {
                if (errors == 2)
                {
                    errors = 0;
                    new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountManagementClick");
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

                    this.Content = new SignOn();

                    Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "SubTaskStart");
                    dictionary3.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary3.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary3);

                    new DialogWindow("Subtask 4 description:\nNow that you are back from the grocery store and paid your bills. You need to transfer some money into your checking account to make sure you have some for the future. You want to transfer $70.00 from you saving account to your checking account.", "Continue").ShowDialog();

                }
                else
                {
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountManagementClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
            else if (MainWindow.subtask == 3)
            {
                if (errors == 2)
                {
                    errors = 0;

                    new DialogWindow("You have tried four times. Please proceed to exit this subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountManagementClick");
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
                    new DialogWindow("Please sign off your online account.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountManagementClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
        }

        private void SignOff_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 2)
            {
                new DialogWindow("You have successfully completed subtask 3 and logged out your account.", "Confirm").ShowDialog();

                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "SignOffClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Correct");
                MainWindow.taskDataList.Add(dictionary);

                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "SubTaskCompleted");
                dictionary1.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary1.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dictionary1);

                new DialogWindow("Please proceed to the next subtask.", "Continue").ShowDialog();

                MainWindow.subtask++;
                errors = 0;

                this.Content = new SignOn();

                Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                dictionary2.Add("time", DateTime.Now.ToString());
                dictionary2.Add("eventType", "SubTaskStart");
                dictionary2.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary2.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dictionary2);

                new DialogWindow("Subtask 4 description:\nNow that you are back from the grocery store and paid your bills. You need to transfer some money into your checking account to make sure you have some for the future. You want to transfer $70.00 from you saving account to your checking account.", "Continue").ShowDialog();
            }

            else if (MainWindow.subtask == 3)
            {
                new DialogWindow("You have successfully completed subtask 4 and logged out your account.", "Confirm").ShowDialog();

                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "SignOffClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Correct");
                MainWindow.taskDataList.Add(dictionary);

                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "SubTaskCompleted");
                dictionary1.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary1.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dictionary1);

                Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                dictionary2.Add("time", DateTime.Now.ToString());
                dictionary2.Add("eventType", "TaskClose");
                dictionary2.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary2.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dictionary2);

                new DialogWindow("You have completed all the tasks. Thank you very much!", "Continue").ShowDialog();

                Window parent = Window.GetWindow(this);
                parent.Close();
            }
        }
    }
}
