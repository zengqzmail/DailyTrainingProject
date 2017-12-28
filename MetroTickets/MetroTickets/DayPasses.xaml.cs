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
    /// Interaction logic for DayPasses.xaml
    /// </summary>
    public partial class DayPasses : UserControl
    {
        int errors;
        public DayPasses()
        {
            InitializeComponent();
            //
            textBlock1.Text = MetroInstructionStrings.MakeSelectionBelow;
            textBlock2.Text = MetroInstructionStrings.OneDayTicket;
            textBlock3.Text = MetroInstructionStrings.OneDayTicketDetails;
            textBlock4.Text = MetroInstructionStrings.SevenDayTicket;
            textBlock5.Text = MetroInstructionStrings.SevenDayTicketDetails;
        }

        private void oneDayTick_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 1)
            {
                MainWindow.MetroSubtaskTimer.Stop();
                MainWindow.makeMessage(MetroInstructionStrings.ProceedNextTaskPrompt, MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                dictionary9.Add("time", DateTime.Now.ToString());
                dictionary9.Add("eventType", "SubTaskCompleted");
                dictionary9.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary9.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dictionary9);
                MainWindow.subtask++;
                errors = 0;
                this.Content = new MainControl();
                
                MainWindow.showSubtaskInstructions();
                MainWindow.resetMetroSubtaskTimer();
            }
            else
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
                    dictionary.Add("eventType", "OneDayTicketButtonClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }
            }
        }

        private void sevenDayTick_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 0)
            {
                MainWindow.MetroSubtaskTimer.Stop();
                MainWindow.makeMessage(MetroInstructionStrings.ProceedNextTaskPrompt, MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                dictionary9.Add("time", DateTime.Now.ToString());
                dictionary9.Add("eventType", "SubTaskCompleted");
                dictionary9.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary9.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dictionary9);
                MainWindow.subtask++;
                errors = 0;
                this.Content = new MainControl();
                MainWindow.showSubtaskInstructions();
                MainWindow.resetMetroSubtaskTimer();
            }
            else
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
                    dictionary.Add("eventType", "SevenDayTicketButtonClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }
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
