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

namespace MetroTickets
{
    /// <summary>
    /// Interaction logic for PassChoices.xaml
    /// </summary>
    public partial class PassChoices : UserControl
    {
        int errors;
        public PassChoices()
        {
            InitializeComponent();
            errors = 0;
            //
            textBlock1.Text = MetroInstructionStrings.MakeSelectionBelow;
            textBlock2.Text = MetroInstructionStrings.BuyEasyCard;
            textBlock3.Text = MetroInstructionStrings.BuyEasyCardDetails;
            textBlock4.Text = MetroInstructionStrings.BuyEasyTicket;
            textBlock5.Text = MetroInstructionStrings.BuyEasyTicketDetails;

        }

        private void buyEasyTicket_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 1)
            {
                if (errors == 3)
                {
                    errors = 0;
                    MainWindow.subtask++;
                    MainWindow.makeMessage(MetroInstructionStrings.ProceedNextTaskPrompt, MetroInstructionStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary);
                    this.Content = new MainControl();
                    MainWindow.showSubtaskInstructions();
                    MainWindow.resetMetroSubtaskTimer();

                }
                else
                {
                    MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs+":\n" + MainWindow.subtasks[MainWindow.subtask], MetroInstructionStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "BuyCardButtonClick");
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
                dictionary.Add("eventType", "BuyCardButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Correct");
                MainWindow.taskDataList.Add(dictionary);
                this.Content = new BuyCardOptions();
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
             if (MainWindow.subtask == 0)
            {
                if (errors == 3)
                {
                    errors = 0;
                    MainWindow.subtask++;
                    MainWindow.makeMessage(MetroInstructionStrings.ProceedNextTaskPrompt, MetroInstructionStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    MainWindow.taskDataList.Add(dictionary);
                    this.Content = new MainControl();
                    MainWindow.showSubtaskInstructions();
                    
                }
                else
                {
                    MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs+":\n" + MainWindow.subtasks[MainWindow.subtask], MetroInstructionStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "BuyTicketButtonClick");
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
                dictionary.Add("eventType", "BuyTicketButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Correct");
                MainWindow.taskDataList.Add(dictionary);

                this.Content = new BuyTicketOptions();
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
        
    }
}
