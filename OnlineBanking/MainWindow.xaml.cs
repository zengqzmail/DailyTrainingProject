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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Dictionary<String, String>> taskDataList;
        public static int subtask;
        int errors;

        public MainWindow()
        {
            InitializeComponent();

            subtask = 0;
            errors = 0;
        }

        public MainWindow(string taskLanguage) 
            : this()
        {
            if (taskLanguage.Equals("ES"))
            {
                this.Content = new MainWindowsp();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            taskDataList = new List<Dictionary<String, String>>();
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "TaskStart");
            dictionary.Add("eventData", "");
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);

            this.Topmost = true;
        }

        private void Welcome_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new SignOn();
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "WelcomeClicked");
            dictionary.Add("eventData", "");
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);

            Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
            dictionary1.Add("time", DateTime.Now.ToString());
            dictionary1.Add("eventType", "SubTaskStart");
            dictionary1.Add("eventData", "");
            dictionary1.Add("eventSummary", "");
            taskDataList.Add(dictionary1);

        }

        public List<Dictionary<String, String>> getEventData()
        {
            return taskDataList;
        }


    }
}
