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
    /// Interaction logic for CurrentBalance.xaml
    /// </summary>
    public partial class CurrentBalance : UserControl
    {
        int errors;
        public CurrentBalance()
        {
            InitializeComponent();
            errors = 0;
            //
            textBlock1.Text = MetroInstructionStrings.MakeSelectionBelow + "\n" + MetroInstructionStrings.CurrentBalance10USD;
            textBlock4.Text = MetroInstructionStrings.AddCashValue;
            textBlock5.Text = MetroInstructionStrings.ReturnToMainScreen;
        }

        private void AddCash_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "AddCashClick");
            dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
            dictionary.Add("eventSummary", "Correct");
            MainWindow.taskDataList.Add(dictionary);
            this.Content = new AddCash();
        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            if (errors == 3)
            {
                errors = 0; //<--- why, if we're exiting the task?
                MainWindow.MetroSubtaskTimer.Stop();
                MainWindow.subtask++;
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
                MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs+"\n" + MainWindow.subtasks[MainWindow.subtask], MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "ReturnButtonClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Incorrect");
                MainWindow.taskDataList.Add(dictionary);
                errors++;
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
