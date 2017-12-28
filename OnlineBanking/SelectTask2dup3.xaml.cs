﻿using System;
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
    /// Interaction logic for SelectTask2dup3.xaml
    /// </summary>
    public partial class SelectTask2dup3 : UserControl
    {
        int errors;
        public SelectTask2dup3()
        {
            InitializeComponent();
            errors = 0;
        }

        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 2)
            {
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "PaymentClick");
                dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                dictionary.Add("eventSummary", "Correct");
                MainWindow.taskDataList.Add(dictionary);

                this.Content = new SelectPayeedup2();
            }
        }

        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 2)
            {
                if (errors == 2)
                {
                    errors = 0;
                    new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "TransferClick");
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
                    new DialogWindow("Your task is to pay some bills online.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "TransferClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
        }

        private void AddPayee_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.subtask == 2)
            {
                if (errors == 2)
                {
                    errors = 0;
                    new DialogWindow("You have tried four times. Please proceed to the next subtask.", "Continue").ShowDialog();

                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AddPayeeClick");
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
                    new DialogWindow("Your task is to pay some bills online.", "OK").ShowDialog();
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AddPayeeClick");
                    dictionary.Add("eventData", (MainWindow.subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    MainWindow.taskDataList.Add(dictionary);
                    errors++;
                }

                
            }
        }
    }
}
