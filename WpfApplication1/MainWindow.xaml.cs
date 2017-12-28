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


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string taskLanguage = "EN"; //default to English
        public static int allowedNumberReplays = 2;

        public MainWindow()
        {
            InitializeComponent();
            taskDataList = new List<Dictionary<String, String>>();
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "TaskStart");
            dictionary.Add("eventData", "");
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);
            
        }

        public MainWindow(string taskLanguage)
            : this()
        {
            this.taskLanguage = taskLanguage;
            if (taskLanguage.Equals("EN"))
            {
                UserControl1 InstructionsEnglish = new UserControl1();
                this.Content = InstructionsEnglish;
            }
            else if(taskLanguage.Equals("ES"))
            {
                UserControl2 InstructionsSpan = new UserControl2();
                this.Content = InstructionsSpan;
            }
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

        public bool test;
        public static List<Dictionary<String, String>> taskDataList;
        
        

        MediaElement media = new MediaElement();

        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

        public static void recordVideoReplay(string videoname, int replayCount)
        {
            Dictionary<String, String> replayVideoData = new Dictionary<String, String>();
            replayVideoData.Add("time", DateTime.Now.ToString());
            replayVideoData.Add("eventType", "videoReplay");
            replayVideoData.Add("eventData", videoname);
            replayVideoData.Add("eventSummary", replayCount.ToString());
            MainWindow.taskDataList.Add(replayVideoData);
        }

        //TODO: delete ALL OF THIS! unused code = TO BE DELETED
        public void SetPage(UserControl page)
        {
            this.Content = page;

        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            UserControl1 InstructionsEnglish = new UserControl1();
            this.Content =  InstructionsEnglish;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            UserControl2 InstructionsSpan = new UserControl2();
            this.Content = InstructionsSpan;
        }

        public List<Dictionary<String, String>> getEventData()
        {
            return taskDataList;
        }
       
    }
}
