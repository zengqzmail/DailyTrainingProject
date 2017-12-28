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
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using ATM;

namespace MetroTickets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window
    {
        
        public static List<Dictionary<String, String>> taskDataList;
        public static string[] subtasks;
       
        public static int subtask;
        public static System.Timers.Timer MetroSubtaskTimer = new System.Timers.Timer(3 * 60 * 1000); //20 seconds
        
        int errors;
        
        string taskLanguage = "EN";
        

        public MainWindow()
        {
            InitializeComponent();
            //
            //
            subtask = 0;
            errors = 0;

            //assign localized subtask instruction strings to subtasks[]  instance var
            this.loadSubStaskInstructions();

            //init MetroSubtaskTimer
            MetroSubtaskTimer.AutoReset = false;
            MetroSubtaskTimer.Elapsed += new System.Timers.ElapsedEventHandler(MetroSubtaskTimer_Elapsed);
        }

        public MainWindow(string taskLanguage)
            : this()
        {
            this.taskLanguage = taskLanguage;
            if (this.taskLanguage.Equals("ES"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
            }
            else if (this.taskLanguage.Equals("EN"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            }
            //
            this.loadSubStaskInstructions();
            //load localized strings
            textBlock1.Text = MetroInstructionStrings.ToBeginMakeSelection;
            textBlock2.Text = MetroInstructionStrings.BuyCardOrTicket;
            textBlock3.Text = MetroInstructionStrings.LoadCardOrTicket;
            textBlock4.Text = MetroInstructionStrings.CheckSchedule;
        }

        public void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*MessageBoxResult result = System.Windows.MessageBox.Show("Closing called, are you sure you want to close it?", "Closing", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Dictionary<String, String> dict1 = new Dictionary<String, String>();
                dict1.Add("time", DateTime.Now.ToString());
                dict1.Add("eventType", "TaskClosedByUser");
                dict1.Add("eventData", "");
                dict1.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dict1);
                Dictionary<String, String> dict2 = new Dictionary<String, String>();
                dict2.Add("time", DateTime.Now.ToString());
                dict2.Add("eventType", "TaskClosed");
                dict2.Add("eventData", "");
                dict2.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dict2);
            }
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }*/
        }


        //calls Stop(), then Start() on MainWindow.MetroSubtaskTimer
        public static void resetMetroSubtaskTimer()
        {
            Console.WriteLine("Resetting MetroSubtaskTimer. Called from subtask " + subtask.ToString());
            MetroSubtaskTimer.Stop();
            MetroSubtaskTimer.Start();
        }

        void MetroSubtaskTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var dialog = new MetroSubtaskMoreTimeDialog();
             
                var result = dialog.ShowDialog();

                if (result == false)    //User chose to move on. if on last subtask, exit. else move to next subtask
                {
                    Console.WriteLine("Chose NO");
                    if (subtask == 2) //last subtask. exit the task
                    {
                        MainWindow.MetroSubtaskTimer.Stop();
                        MainWindow.makeMessage(MetroInstructionStrings.ThankYouForCompletingTask, MetroInstructionStrings.Exit);
                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "SkippedLastSubtask");
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
                    else
                    {
                        Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                        dictionary8.Add("time", DateTime.Now.ToString());
                        dictionary8.Add("eventType", "SkippedToNextSubtask");
                        dictionary8.Add("eventData", "");
                        dictionary8.Add("eventSummary", "SkippedSubtask"+subtask.ToString());
                        MainWindow.taskDataList.Add(dictionary8);
                        this.Content = new MainControl(); //load main screen again.
                        MainWindow.subtask++;
                        MainWindow.showSubtaskInstructions();
                        MetroSubtaskTimer.Start();
                    }
                }
                else if (result == true)
                {
                    Console.WriteLine("Chose YES");
                    MetroSubtaskTimer.Start();
                }
            }));
 
        }

       

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            MetroSubtaskTimer.Stop(); //make sure that it doesn't pop up outside of the task instance
            this.Close();
        }

        //assign localized subtask instruction strings to subtasks[]  instance var
        private void loadSubStaskInstructions()
        {
            subtasks = new string[]{
                MetroInstructionStrings.SubTask1_Instructions,
                MetroInstructionStrings.SubTask2_Instructions,
                MetroInstructionStrings.SubTask3_Instructions               
            };
        }

        /*
         * 1. add TaskStart tag to eventData
         * 2. show intro message
         * 3. show instructions for first subtask
         * 4. make sure MainWindow has Topmost = true
         */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            taskDataList = new List<Dictionary<String, String>>();
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "TaskStart");
            dictionary.Add("eventData", "");
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);
            new DialogWindow(MetroInstructionStrings.IntroMessage, MetroInstructionStrings.Continue).ShowDialog();
            //
            showSubtaskInstructions(); 
            this.Topmost = true;   
        }

        public static void showSubtaskInstructions()
        {
            new DialogWindow(subtasks[subtask], MetroInstructionStrings.Continue).ShowDialog();
            //
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "SubTaskStart");
            dictionary.Add("eventData", (subtask + 1).ToString());
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);
        }

        public static void makeMessage(string string1,string string2)
        {
            new DialogWindow(string1, string2).ShowDialog();

        }
        private void buyTicket_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "BuyTicketButtonClick");
            dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
            dictionary.Add("eventSummary", "Correct");
            taskDataList.Add(dictionary);

            this.Content = new PassChoices();
        }

        private void loadTicket_Click(object sender, RoutedEventArgs e)
        {
            if (subtask == 0 || subtask == 1)
            {
                if (errors == 3)
                {
                    errors = 0;
                    subtask++;
                    MainWindow.MetroSubtaskTimer.Stop();
                    MainWindow.makeMessage(MetroInstructionStrings.ProceedNextTaskPrompt, MetroInstructionStrings.Continue );
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);

                    this.Content = new MainControl();
                    showSubtaskInstructions();
                    MainWindow.resetMetroSubtaskTimer();

                }
                else
                {
                    MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs + ":\n" + subtasks[subtask], MetroInstructionStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LoadTicketButtonClick");
                    dictionary.Add("eventData", (subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                    errors++;
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            
               
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "InvalidButtonClick");
                dictionary.Add("eventData", (subtask + 1).ToString());
                dictionary.Add("eventSummary", "");
                taskDataList.Add(dictionary);
                
            
        }

        private void CheckSched_Click(object sender, RoutedEventArgs e)
        {
            if (errors == 3)
            {
                errors = 0;
                subtask++;
                MainWindow.MetroSubtaskTimer.Stop();
                MainWindow.makeMessage(MetroInstructionStrings.ProceedNextTaskPrompt, MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "MaxErrorsReached");
                dictionary.Add("eventData", "");
                dictionary.Add("eventSummary", "");
                taskDataList.Add(dictionary);

                this.Content = new MainControl();
                showSubtaskInstructions();
                MainWindow.resetMetroSubtaskTimer();

            }
            else
            {
                MainWindow.makeMessage(MetroInstructionStrings.YourTaskIs + ":\n" + subtasks[subtask], MetroInstructionStrings.Continue);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "CheckScheduleButtonClick");
                dictionary.Add("eventData", (subtask + 1).ToString());
                dictionary.Add("eventSummary", "Incorrect");
                taskDataList.Add(dictionary);
                errors++;
            }

        }
        public List<Dictionary<String, String>> getEventData()
        {
            return taskDataList;
        }

        
    }
}
