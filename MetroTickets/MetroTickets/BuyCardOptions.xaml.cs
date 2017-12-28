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
    /// Interaction logic for BuyCardOptions.xaml
    /// </summary>
    public partial class BuyCardOptions : UserControl
    {
        int errors;
        public BuyCardOptions()
        {
            InitializeComponent();
            errors = 0;
            //
            textBlock1.Text = MetroInstructionStrings.MakeSelectionBelow;
            textBlock2.Text = MetroInstructionStrings.DayPasses;
            textBlock3.Text = MetroInstructionStrings.OneMonthPass;
            textBlock10.Text = MetroInstructionStrings.OneMonthPassDetails;
            textBlock4.Text = MetroInstructionStrings.OneMonthPassPlusParking;
            textBlock5.Text = MetroInstructionStrings.OneMonthPassPlusParkingDetails;
            textBlock6.Text = MetroInstructionStrings.AddCashValue;
            //
        }

        

        private void buyPass_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "DayPassesButtonClick");
            dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
            dictionary.Add("eventSummary", "Correct");
            MainWindow.taskDataList.Add(dictionary);
            this.Content = new DayPasses();
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

        private void MonthButton_Click(object sender, RoutedEventArgs e)
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
                this.Content = new MainControl();
                
                MainWindow.showSubtaskInstructions();
                MainWindow.resetMetroSubtaskTimer();
            }
            else
            {
                MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs+":\n" + MainWindow.subtasks[MainWindow.subtask], MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "MonthPassButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Incorrect");
                MainWindow.taskDataList.Add(dictionary);
                errors++;
            }
        }

        private void MonthPark_Click(object sender, RoutedEventArgs e)
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
                this.Content = new MainControl();
                
                MainWindow.showSubtaskInstructions();
                MainWindow.resetMetroSubtaskTimer();

            }
            else
            {
                MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs+":\n" + MainWindow.subtasks[MainWindow.subtask], MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "MonthPassParkClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Incorrect");
                MainWindow.taskDataList.Add(dictionary);
                errors++;
            }
        }

        private void AddCASHButton_Click(object sender, RoutedEventArgs e)
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
                this.Content = new MainControl();
                
                MainWindow.showSubtaskInstructions();
                MainWindow.resetMetroSubtaskTimer();

            }
            else
            {
                MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs+":\n" + MainWindow.subtasks[MainWindow.subtask], MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "CashButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Incorrect");
                MainWindow.taskDataList.Add(dictionary);
                errors++;
            }
        }

    }
}
