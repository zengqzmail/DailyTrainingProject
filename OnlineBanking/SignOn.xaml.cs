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
    /// Interaction logic for SignOn.xaml
    /// </summary>
    public partial class SignOn : UserControl
    {
        int errors;
        public SignOn()
        {
            InitializeComponent();
            errors = 0;
        }

        private void SignOnClick(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 0)
            {
                if (validateID(textBoxID.Text) && validatePassword(passwordBox.Password))
                {                    
                    //var instruction3 = Regex.Replace("Task 1 description :\nAssume you are going to the grocery store and you need to have some cash to pay for the metro and for your groceries. The metro will cost $5.00 for a round trip day ticket and the groceries will cost about $55.00. You are going to stop at the ATM machine on your way to the metro to use your debit card to get cash for your outing.", @"$5.00", @"<b>$0</b>");
                    string instruction1 = "Task 1 description :\nAssume you are going to the grocery store and you need to have some cash to pay for the metro and for your groceries. The metro will cost $5.00 for a round trip day ticket and the groceries will cost $55.00. You are going to stop at the ATM machine on your way to the metro to use your debit card to get cash for your outing.";
                    //instruction1.Replace("$5.00", "<b>" + "$5.00" + "</b>");
                    string instruction2 = "Before you go you need to go online to your Ubank account to check how much money you have in your checking account(the account you use when withdrawing cash at the ATM).You need to make sure you have exactly enough cash in your account when you get to the ATM. Only put in what you need. If you do not have enough money you need to transfer money from your savings account to your checking account.";
                    new DialogWindow(instruction1, "Continue").ShowDialog();
                    new DialogWindow(instruction2, "Confirm").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "SignOn");
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
                        dictionary.Add("eventType", "SignOn");
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
                        new DialogWindow("The user Id or the password is invalid, please check your input and try again. Thanks!", "OK").ShowDialog();
                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "SignOn");
                        dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                        dictionary.Add("eventSummary", "Incorrect");
                        MainWindow.taskDataList.Add(dictionary);
                        errors++;
                    }
                }
            }

            else if (MainWindow.subtask == 3)
            {
                if (validateID(textBoxID.Text) && validatePassword(passwordBox.Password))
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "SignOn");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Correct");
                    MainWindow.taskDataList.Add(dictionary);

                    this.Content = new SelectTask1();
                }
                else
                {
                    if (errors == 2)
                    {
                        errors = 0;

                        new DialogWindow("You have tried four times. Please proceed to exit this subtask.", "Continue").ShowDialog();

                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "SignOn");
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
                        new DialogWindow("The user Id or the password is invalid, please check your input and try again. Thanks!", "OK").ShowDialog();
                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "SignOn");
                        dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                        dictionary.Add("eventSummary", "Incorrect");
                        MainWindow.taskDataList.Add(dictionary);
                        errors++;
                    }

                    
                }
            }
        }

        private bool validateID(string ID)
        {
            if (ID == "Ubankuser") return true;
            else return false;
        }

        private bool validatePassword(string password)
        {
            if (password == "coa1234") return true;
            else return false;
        }

    }
}
