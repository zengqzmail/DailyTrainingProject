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
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : System.Windows.Controls.UserControl
    {
        int errors;
        
        public MainControl()
        {
            InitializeComponent();
            errors = 0;
            textBlock1.Text = MetroInstructionStrings.ToBeginMakeSelection;
            textBlock2.Text = MetroInstructionStrings.BuyCardOrTicket;
            textBlock3.Text = MetroInstructionStrings.LoadCardOrTicket;
            textBlock4.Text = MetroInstructionStrings.CheckSchedule;

        }

        private void buyTicket_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 1 || MainWindow.subtask == 0)
            {
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "BuyTicketButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Correct");
                MainWindow.taskDataList.Add(dictionary);

                this.Content = new PassChoices();
            }
            else
            {
                if (errors == 3)
                {
                    errors = 0;
                    MainWindow.MetroSubtaskTimer.Stop();
                    MainWindow.makeMessage(MetroInstructionStrings.ThankYouForCompletingTask, MetroInstructionStrings.Exit);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary);
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary1.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary1);
                    Window parent = Window.GetWindow(this);
                    parent.Close();


                }
                else
                {
                    MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs + ":\n" + MainWindow.subtasks[MainWindow.subtask], MetroInstructionStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "BuyTicketButtonClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }
            }
        }

        private void loadTicket_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 0 || MainWindow.subtask == 1)
            {
                if (errors == 3)
                {
                    errors = 0;
                    MainWindow.subtask++;
                    MainWindow.MetroSubtaskTimer.Stop();
                    MainWindow.makeMessage(MetroInstructionStrings.ProceedNextTaskPrompt, MetroInstructionStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary);
                    MainWindow.showSubtaskInstructions();
                    MainWindow.resetMetroSubtaskTimer();

                }
                else
                {
                    MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs + ":\n" + MainWindow.subtasks[MainWindow.subtask], MetroInstructionStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LoadTicketButtonClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }
            }
            else
            {
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "LoadTicketButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Correct");
                MainWindow.taskDataList.Add(dictionary);
                this.Content = new LoadCardAmount();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "InvalidButtonClick");
            dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
            dictionary.Add("eventSummary", "");
            MainWindow.taskDataList.Add(dictionary);
        }

        private void checkSched_Click(object sender, RoutedEventArgs e)
        {
            if (errors == 3)
            {
                if (MainWindow.subtask == 2)
                {
                    MainWindow.MetroSubtaskTimer.Stop();
                    MainWindow.makeMessage(MetroInstructionStrings.ThankYouForCompletingTask, MetroInstructionStrings.Exit);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary);
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary1.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary1);
                    Window parent = Window.GetWindow(this);
                    parent.Close();
                    return;

                } 
                errors = 0;
                MainWindow.subtask++;
                MainWindow.MetroSubtaskTimer.Stop();
                MainWindow.makeMessage(MetroInstructionStrings.ProceedNextTaskPrompt, MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                dictionary8.Add("time", DateTime.Now.ToString());
                dictionary8.Add("eventType", "MaxErrorsReached");
                dictionary8.Add("eventData", "");
                dictionary8.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dictionary8);
                MainWindow.showSubtaskInstructions();
                MainWindow.resetMetroSubtaskTimer();

            }
            else
            {
                MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs + ":\n" + MainWindow.subtasks[MainWindow.subtask], MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "CheckScheduleButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Incorrect");
                MainWindow.taskDataList.Add(dictionary);
                errors++;
            }
        }
    }
}
