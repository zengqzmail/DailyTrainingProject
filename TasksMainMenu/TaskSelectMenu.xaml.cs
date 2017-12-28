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
using System.Windows.Shapes;
//
using ATM;
using FormsTask;
using PrescriptionRefill;
using ReactionTest;
using MetroTickets;
using WpfApplication1;
using OnlineBanking;
using System.Windows.Forms;



namespace TasksMainMenu
{
    /// <summary>
    /// Interaction logic for TaskSelectMenu.xaml
    /// </summary>
    /// 

    public partial class TaskSelectMenu : Window
    {
        //fields
        newParticipant participant;
        new tasksDataContainer context = new tasksDataContainer();
        LanguageSelectScreen langSelectScreen;
        string currentSelectedTask;
        string currentSelectedLanguage;
        private string taskNotSelectedMessage = "You need to select a task in order to continue.";
        //

        //constructor
        //@params: none
        //task data saved anonymously (no participant id)
        public TaskSelectMenu()
        {
            InitializeComponent();
        }

        public void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*MessageBoxResult result = System.Windows.MessageBox.Show("Closing called, are you sure you want to close it?", "Closing", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }*/
        }


        //constructor
        //@params:
        //newParticipant participant
        //used to save task data and display info in instructionsTextBlk
        //
        public TaskSelectMenu(newParticipant p)
        {
            participant = p;
            string pID = p.ParticipantID;
            string pTimepoint = p.Timepoint.ToString();

            InitializeComponent();
            //
            string instructions = String.Format("Participant: {0} \nTime-point: {1} \nSelect your task below and click the \"Continue\" button", pID, pTimepoint);
            this.instructionsTextBlk.Text = instructions;
        }

        private void taskBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        //
        // 
        /* does 2 things:
         * 1. clears task selection combobox
         * 2. gets event data and saves to sdf
         */
        void taskClosedHandler(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = null;
            saveTaskData(sender);

            var dialog = new PopUp();
            var result = dialog.ShowDialog();
            if (result == true)
            {
                PopUpScore(sender);
            }

        }


        /**
         * Not working
         * */
        void taskClosingHandler(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Closing called, are you sure you want to close it?", "Closing", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    //Dictionary<String, String> dict1 = new Dictionary<String, String>();
                    //dict1.Add("time", DateTime.Now.ToString());
                    //dict1.Add("eventType", "TaskClosedByUser");
                    //dict1.Add("eventData", "");
                    //dict1.Add("eventSummary", "");
                    //eventData.Add(dict1);
                    //Dictionary<String, String> dict2 = new Dictionary<String, String>();
                    //dict2.Add("time", DateTime.Now.ToString());
                    //dict2.Add("eventType", "TaskClosed");
                    //dict2.Add("eventData", "");
                    //dict2.Add("eventSummary", "");
                    //eventData.Add(dict2);
                }
                else if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
                Console.WriteLine(sender);
                Console.WriteLine("Form closed");
            }

        }

        private void PopUpScore(object task)
        {
            List<Dictionary<String, String>> eventData = null;
            string taskName = null;
            try
            {
                string taskType = task.GetType().ToString();
                switch (taskType)
                {
                    case "ATM.CitiBankForm":
                        taskName = "ATM";
                        break;
                    case "PrescriptionRefill.Form1":
                        taskName = "Prescription Refill";
                        break;
                    case "FormsTask.Form1":
                        taskName = "Forms";
                        break;
                    case "ReactionTest.ReactionTestWindow":
                        taskName = "Reaction Test";
                        break;
                    case "MetroTickets.MainWindow":
                        taskName = "MetroTickets";
                        break;
                    case "WpfApplication1.MainWindow":
                        taskName = "DoctorTest";
                        break;
                    case "OnlineBanking.MainWindow":
                        taskName = "OnlineBanking";
                        break;
                    default:
                        break;
                }
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show("Error collecting task data: " + ex.Message);
            }
            PopUpTaskScore(taskName);
        }

        private void PopUpTaskScore(string taskName)
        {
            var taskDataList = context.TaskDatas;
            var partTotalData = taskDataList.Where(c => c.SubjectID == participant.ParticipantID);
            if (partTotalData.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data for " + participant.ParticipantID + ", please press OK!";
                System.Windows.MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                StringBuilder CsvSb = new StringBuilder();
                switch (taskName)
                {
                    case "ATM":
                        try
                        {
                            var ATMTotalData = partTotalData.Where(c => c.TaskName == "ATM");
                            if (ATMTotalData.AsEnumerable().Count() != 0)
                            {
                                CsvSb.Append("ATM\n");
                                var ATMStarts = ATMTotalData.Where(c => c.EventType == "TaskStart");
                                var ATMCloses = ATMTotalData.Where(c => c.EventType == "TaskClosed");
                                int start = ATMStarts.AsEnumerable().Last().TaskDataId;
                                int end = ATMCloses.AsEnumerable().Last().TaskDataId;
                                var ATMSingleTaskData = ATMTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                                CsvSb = SummarizeATM(ATMSingleTaskData, CsvSb);
                            }
                        }
                        catch
                        {
                        }
                        break;

                    case "OnlineBanking":
                        try
                        {
                            var OnlineBankingTotalData = partTotalData.Where(c => c.TaskName == "OnlineBanking");
                            if (OnlineBankingTotalData.AsEnumerable().Count() != 0)
                            {
                                CsvSb.Append("OnlineBanking\n");
                                var OnlineBankingStarts = OnlineBankingTotalData.Where(c => c.EventType == "TaskStart");
                                var OnlineBankingCloses = OnlineBankingTotalData.Where(c => c.EventType == "TaskClose");
                                int start = OnlineBankingStarts.AsEnumerable().Last().TaskDataId;
                                int end = OnlineBankingCloses.AsEnumerable().Last().TaskDataId;
                                var OnlineBankingSingleTaskData = OnlineBankingTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                                CsvSb = SummarizeOnlineBanking(OnlineBankingSingleTaskData, CsvSb);
                            }
                        }
                        catch
                        {

                        }
                        break;

                    case "MetroTickets":
                        try
                        {
                            var MetTotalData = partTotalData.Where(c => c.TaskName == "MetroTickets");
                            CsvSb.Append("MetroTickets\n");
                            var MetStarts = MetTotalData.Where(c => c.EventType == "TaskStart");
                            var MetCloses = MetTotalData.Where(c => c.EventType == "TaskClosed");
                            int start = MetStarts.AsEnumerable().Last().TaskDataId;
                            int end = MetCloses.AsEnumerable().Last().TaskDataId;
                            var MetSingleTaskData = MetTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                            CsvSb = SummarizeMetroTickets(MetSingleTaskData, CsvSb);
                        }
                        catch
                        {
                        }
                        break;
                    case "Forms":
                        try
                        {
                            var FormsTotalData = partTotalData.Where(c => c.TaskName == "Forms");
                            CsvSb.Append("Forms\n");
                            var FormsStarts = FormsTotalData.Where(c => c.EventType == "TaskStart");
                            var FormsCloses = FormsTotalData.Where(c => c.EventType == "TaskClosed");
                            TimeSpan formsTotalTime = FormsCloses.AsEnumerable().Last().Time - FormsStarts.AsEnumerable().Last().Time;
                            CsvSb.Append("The Score is "); CsvSb.Append("\n");
                            CsvSb.Append(formsTotalTime.ToString()); CsvSb.Append("\n");
                        }
                        catch
                        {
                        }
                        break;
                    case "Prescription Refill":
                        try
                        {
                            var PresTotalData = partTotalData.Where(c => c.TaskName == "Prescription Refill");
                            CsvSb.Append("Prescription Refill\n");
                            var PresStarts = PresTotalData.Where(c => c.EventType == "TaskStart");
                            var PresCloses = PresTotalData.Where(c => c.EventType == "TaskClosed");
                            int start = PresStarts.AsEnumerable().Last().TaskDataId;
                            int end = PresCloses.AsEnumerable().Last().TaskDataId;
                            var PresSingleTaskData = PresTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                            CsvSb = SummarizePres(PresSingleTaskData, CsvSb);
                        }
                        catch
                        {
                        }
                        break;
                    case "DoctorTest":
                        try
                        {
                            var DocTotalData = partTotalData.Where(c => c.TaskName == "DoctorTest");
                            CsvSb.Append("DoctorTest\n");
                            var DocStarts = DocTotalData.Where(c => c.EventType == "TaskStart");
                            var DocCloses = DocTotalData.Where(c => c.EventType == "TaskComplete");
                            int start = DocStarts.AsEnumerable().Last().TaskDataId;
                            int end = DocCloses.AsEnumerable().Last().TaskDataId;
                            var DocSingleTaskData = DocTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                            CsvSb = SummarizeDoc(DocSingleTaskData, CsvSb);
                        }
                        catch
                        {
                        }
                        break;
                    default:
                        break;
                }
                new MetroTickets.DialogWindow(CsvSb.ToString(), "Exit").ShowDialog();
            }
        }

        private StringBuilder SummarizeATM(IQueryable<TaskData> taskData, StringBuilder CsvSb)
        {
            string partID = taskData.AsEnumerable().ElementAt(0).SubjectID;
            string subtaskNum, numCorrect, numIncorrect;
            string taskStartTime = taskData.AsEnumerable().ElementAt(0).Time.ToString();
            TimeSpan t1_time = new TimeSpan(0), t2_time = new TimeSpan(0), t3_time = new TimeSpan(0), total_time = new TimeSpan(0);
            int t1_correct = 0, t2_correct = 0, t3_correct = 0, total_correct = 0, t1_errors = 0, t2_errors = 0, t3_errors = 0, total_errors = 0;
            var subTaskStarts = taskData.Where(c => c.EventType == "SubTaskStart");
            var subTaskEnds = taskData.Where(c => c.EventType == "SubTaskCompleted" || c.EventType == "MaxErrorsReached" || c.EventType == "SubTaskSkip");
            double timeToSecond = 0;

            for (int i = 0; i < subTaskStarts.AsEnumerable().Count(); i++)
            {
                subtaskNum = (i + 1).ToString();
                int start = subTaskStarts.AsEnumerable().ElementAt(i).TaskDataId;
                int end = subTaskEnds.AsEnumerable().ElementAt(i).TaskDataId;
                var ATMSingleSubTaskData = taskData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);

                numCorrect = ATMSingleSubTaskData.Where(c => c.EventSummary == "Correct").AsEnumerable().Count().ToString();
                numIncorrect = ATMSingleSubTaskData.Where(c => c.EventSummary == "Incorrect").AsEnumerable().Count().ToString();

                switch (subtaskNum)
                {
                    case "1":

                        t1_correct = Int32.Parse(numCorrect);
                        t1_errors = Int32.Parse(numIncorrect);
                        total_correct += t1_correct;
                        total_errors += t1_errors;
                        break;
                    case "2":

                        t2_correct = Int32.Parse(numCorrect);
                        t2_errors = Int32.Parse(numIncorrect);
                        total_correct += t2_correct;
                        total_errors += t2_errors;
                        break;
                    case "3":

                        t3_correct = Int32.Parse(numCorrect);
                        t3_errors = Int32.Parse(numIncorrect);
                        total_correct += t3_correct;
                        total_errors += t3_errors;
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < subTaskEnds.AsEnumerable().Count(); i++)
            {
                subtaskNum = (i + 1).ToString();
                switch (subtaskNum)
                {
                    case "1":
                        t1_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - taskData.AsEnumerable().ElementAt(0).Time;
                        break;
                    case "2":
                        t2_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - subTaskEnds.AsEnumerable().ElementAt(i - 1).Time;
                        break;
                    case "3":
                        t3_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - subTaskEnds.AsEnumerable().ElementAt(i - 1).Time;
                        break;
                    default:
                        break;
                }

            }
            total_time = t1_time + t2_time + t3_time;

            timeToSecond = total_time.TotalSeconds;
            double Rate = total_correct / timeToSecond;
            double Ratio = (double)total_correct / (total_correct + total_errors);

            CsvSb.Append("Rate is ");
            CsvSb.Append(Rate.ToString("F6")); CsvSb.Append("\n");
            CsvSb.Append("Ratio is ");
            CsvSb.Append(Ratio.ToString("F4")); CsvSb.Append("\n");

            return CsvSb;
        }

        private StringBuilder SummarizeOnlineBanking(IQueryable<TaskData> taskData, StringBuilder CsvSb)
        {
            string partID = taskData.AsEnumerable().ElementAt(0).SubjectID;
            string subtaskNum, numCorrect, numIncorrect, Status;
            string taskStartTime = taskData.AsEnumerable().ElementAt(0).Time.ToString();
            TimeSpan t1_time = new TimeSpan(0), t2_time = new TimeSpan(0), t3_time = new TimeSpan(0), t4_time = new TimeSpan(0), total_time = new TimeSpan(0);
            int t1_correct = 0, t2_correct = 0, t3_correct = 0, t4_correct = 0, total_correct = 0, t1_errors = 0, t2_errors = 0, t3_errors = 0, t4_errors = 0, total_errors = 0;
            var subTaskStarts = taskData.Where(c => c.EventType == "SubTaskStart");
            var subTaskEnds = taskData.Where(c => c.EventType == "SubTaskCompleted");
            double timeToSecond = 0;

            for (int i = 0; i < subTaskStarts.AsEnumerable().Count(); i++)
            {
                subtaskNum = (i + 1).ToString();
                int start = subTaskStarts.AsEnumerable().ElementAt(i).TaskDataId;
                int end = subTaskEnds.AsEnumerable().ElementAt(i).TaskDataId;
                var OnlineBankingSingleSubTaskData = taskData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                numCorrect = OnlineBankingSingleSubTaskData.Where(c => c.EventSummary == "Correct").AsEnumerable().Count().ToString();
                numIncorrect = OnlineBankingSingleSubTaskData.Where(c => c.EventSummary == "Incorrect").AsEnumerable().Count().ToString();

                switch (subtaskNum)
                {
                    case "1":
                        t1_correct = Int32.Parse(numCorrect);
                        t1_errors = Int32.Parse(numIncorrect);
                        total_correct += t1_correct;
                        total_errors += t1_errors;
                        break;
                    case "2":
                        t2_correct = Int32.Parse(numCorrect);
                        t2_errors = Int32.Parse(numIncorrect);
                        total_correct += t2_correct;
                        total_errors += t2_errors;
                        break;
                    case "3":
                        t3_correct = Int32.Parse(numCorrect);
                        t3_errors = Int32.Parse(numIncorrect);
                        total_correct += t3_correct;
                        total_errors += t3_errors;
                        break;
                    case "4":
                        t4_correct = Int32.Parse(numCorrect);
                        t4_errors = Int32.Parse(numIncorrect);
                        total_correct += t4_correct;
                        total_errors += t4_errors;
                        break;
                    default:
                        break;
                }
            }

            for (int i = 0; i < subTaskEnds.AsEnumerable().Count(); i++)
            {
                subtaskNum = (i + 1).ToString();
                switch (subtaskNum)
                {
                    case "1":
                        t1_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - taskData.AsEnumerable().ElementAt(0).Time;
                        break;
                    case "2":
                        t2_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - subTaskEnds.AsEnumerable().ElementAt(i - 1).Time;
                        break;
                    case "3":
                        t3_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - subTaskEnds.AsEnumerable().ElementAt(i - 1).Time;
                        break;
                    case "4":
                        t4_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - subTaskEnds.AsEnumerable().ElementAt(i - 1).Time;
                        break;
                    default:
                        break;
                }
            }
            total_time = t1_time + t2_time + t3_time + t4_time;
            var statusJudge = subTaskEnds.Where(c => c.EventType == "MaxErrorsReached");
            if (statusJudge.AsEnumerable().Count() == 0) Status = "Complete";
            else Status = "Incomplete";

            timeToSecond = total_time.TotalSeconds;
            double Rate = total_correct / timeToSecond;
            double Ratio = (double)total_correct / (total_correct + total_errors);

            CsvSb.Append("Rate is ");
            CsvSb.Append(Rate.ToString("F6")); CsvSb.Append("\n");
            CsvSb.Append("Ratio is ");
            CsvSb.Append(Ratio.ToString("F4")); CsvSb.Append("\n");

            return (CsvSb);
        }

        private StringBuilder SummarizeMetroTickets(IQueryable<TaskData> metroKioskSingleTaskData, StringBuilder CsvSb)
        {
            string partID = metroKioskSingleTaskData.AsEnumerable().ElementAt(0).SubjectID; //grab SJID
            string taskStartTime = metroKioskSingleTaskData.AsEnumerable().ElementAt(0).Time.ToString(); //grab task trial start time
            string subtaskNum, numCorrect, numIncorrect;
            string Status = "Complete";
            //DateTime subStartTime;
            TimeSpan t1_time = new TimeSpan(0), t2_time = new TimeSpan(0), t3_time = new TimeSpan(0), total_task_time = new TimeSpan(0);
            //totalSubtaskTime = new TimeSpan(0),
            int t1_correct = 0, t2_correct = 0, t3_correct = 0, total_correct = 0, t1_errors = 0, t2_errors = 0, t3_errors = 0, total_errors = 0;

            var subTaskStarts = metroKioskSingleTaskData.Where(c => c.EventType == "SubTaskStart");
            var subTaskEnds = metroKioskSingleTaskData.Where(c => c.EventType == "SubTaskCompleted" || c.EventType == "MaxErrorsReached" || c.EventType == "SkippedToNextSubtask" || c.EventType == "SkippedLastSubtask");

            for (int i = 0; i < subTaskStarts.AsEnumerable().Count(); i++) //iterate through subtasks
            {
                subtaskNum = (i + 1).ToString();                                    //subtask ordinality - why a string??

                int start = subTaskStarts.AsEnumerable().ElementAt(i).TaskDataId;   //taskdata ID for subtask start
                int end = subTaskEnds.AsEnumerable().ElementAt(i).TaskDataId;
                var metroSingleSubTaskData = metroKioskSingleTaskData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end); //all data btwn start and end
                //subStartTime = metroSingleSubTaskData.AsEnumerable().ElementAt(0).Time; //start time for subtask
                //total subtask time
                //totalSubtaskTime = (metroSingleSubTaskData.AsEnumerable().ElementAt((metroSingleSubTaskData.AsEnumerable().Count()) - 1).Time - subStartTime); //time taken for this subtask

                numCorrect = metroSingleSubTaskData.Where(c => c.EventSummary == "Correct").AsEnumerable().Count().ToString();
                numIncorrect = metroSingleSubTaskData.Where(c => c.EventSummary == "Incorrect" || c.EventType == "InvalidButtonClick").AsEnumerable().Count().ToString();

                switch (subtaskNum)
                {
                    case "1":
                        t1_time = metroSingleSubTaskData.AsEnumerable().Last().Time - metroKioskSingleTaskData.AsEnumerable().ElementAt(0).Time;
                        total_task_time += t1_time;

                        t1_correct = Int32.Parse(numCorrect);
                        t1_errors = Int32.Parse(numIncorrect);

                        if (metroSingleSubTaskData.AsEnumerable().Last().EventType == "SubTaskCompleted") t1_correct++;
                        else { t1_errors++; Status = "Incomplete"; }

                        total_correct += t1_correct;
                        total_errors += t1_errors;

                        break;
                    case "2":
                        t2_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - subTaskEnds.AsEnumerable().ElementAt(i - 1).Time;
                        total_task_time += t2_time;

                        t2_correct = Int32.Parse(numCorrect);
                        t2_errors = Int32.Parse(numIncorrect);

                        if (metroSingleSubTaskData.AsEnumerable().Last().EventType == "SubTaskCompleted") t2_correct++;
                        else { t2_errors++; Status = "Incomplete"; }

                        total_correct += t2_correct;
                        total_errors += t2_errors;

                        break;
                    case "3":
                        t3_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - subTaskEnds.AsEnumerable().ElementAt(i - 1).Time;
                        total_task_time += t3_time;

                        t3_correct = Int32.Parse(numCorrect);
                        t3_errors = Int32.Parse(numIncorrect);

                        if (metroSingleSubTaskData.AsEnumerable().Last().EventType == "SubTaskCompleted") t3_correct++;
                        else { t3_errors++; Status = "Incomplete"; }

                        total_correct += t3_correct;
                        total_errors += t3_errors;

                        break;
                    default:
                        break;
                }
            }

            double timeToSecond = 0;
            timeToSecond = total_task_time.TotalSeconds;
            double Rate = total_correct / timeToSecond;
            double Ratio = (double)total_correct / (total_correct + total_errors);

            CsvSb.Append("Rate is ");
            CsvSb.Append(Rate.ToString("F6")); CsvSb.Append("\n");
            CsvSb.Append("Ratio is ");
            CsvSb.Append(Ratio.ToString("F4")); CsvSb.Append("\n");
            return CsvSb;
        }

        private StringBuilder SummarizePres(IQueryable<TaskData> taskData, StringBuilder CsvSb)
        {
            string partID = taskData.AsEnumerable().ElementAt(0).SubjectID;
            string startTime = taskData.AsEnumerable().ElementAt(0).Time.ToString();
            string Status;
            TimeSpan timeTaken;
            int numCorrect = 0, numIncorrect = 0, numValid = 0, numInvalid = 0, total_cor = 0, total_err = 0;

            var RxNumberInfo = taskData.Where(c => c.EventType == "RxNumberEntered" && c.EventSummary == "Correct");
            var Rx1Info = taskData.Where(c => c.EventData == "44237908");
            var Rx2Info = taskData.Where(c => c.EventData == "25177789");

            if (Rx1Info.AsEnumerable().Count() == 1 && Rx2Info.AsEnumerable().Count() == 1)
            {
                numCorrect = taskData.Where(c => c.EventSummary == "Correct").AsEnumerable().Count();
                numIncorrect = taskData.Where(c => c.EventSummary == "Incorrect").AsEnumerable().Count();
                numValid = taskData.Where(c => c.EventSummary == "Valid").AsEnumerable().Count();
                numInvalid = taskData.Where(c => c.EventSummary == "Invalid").AsEnumerable().Count();
                total_cor = numCorrect + numValid;
                total_err = numIncorrect + numInvalid;
                timeTaken = taskData.AsEnumerable().ElementAt((taskData.AsEnumerable().Count()) - 1).Time - taskData.AsEnumerable().ElementAt(0).Time;
            }

            else if (taskData.Where(c => c.EventType == "RefillAnotherPrescription" && c.EventSummary == "Correct").AsEnumerable().Count() == 0)
            {
                numCorrect = taskData.Where(c => c.EventSummary == "Correct").AsEnumerable().Count();
                numIncorrect = taskData.Where(c => c.EventSummary == "Incorrect").AsEnumerable().Count();
                numValid = taskData.Where(c => c.EventSummary == "Valid").AsEnumerable().Count();
                numInvalid = taskData.Where(c => c.EventSummary == "Invalid").AsEnumerable().Count();
                total_cor = numCorrect + numValid;
                total_err = numIncorrect + numInvalid;
                timeTaken = taskData.AsEnumerable().ElementAt((taskData.AsEnumerable().Count()) - 1).Time - taskData.AsEnumerable().ElementAt(0).Time;
                //err add 6, and double the time
                total_err = total_err + 6;
                timeTaken = timeTaken + timeTaken;
            }

            else if (Rx1Info.AsEnumerable().Count() >= 2 || Rx2Info.AsEnumerable().Count() >= 2)
            {
                if (RxNumberInfo.AsEnumerable().ElementAt(0).EventData == "44237908" && RxNumberInfo.AsEnumerable().ElementAt(1).EventData == "25177789" || RxNumberInfo.AsEnumerable().ElementAt(1).EventData == "44237908" && RxNumberInfo.AsEnumerable().ElementAt(0).EventData == "25177789")
                {
                    numCorrect = taskData.Where(c => c.EventSummary == "Correct").AsEnumerable().Count();
                    numIncorrect = taskData.Where(c => c.EventSummary == "Incorrect").AsEnumerable().Count();
                    numValid = taskData.Where(c => c.EventSummary == "Valid").AsEnumerable().Count();
                    numInvalid = taskData.Where(c => c.EventSummary == "Invalid").AsEnumerable().Count();
                    total_cor = numCorrect + numValid;
                    total_err = numIncorrect + numInvalid;
                    timeTaken = taskData.AsEnumerable().ElementAt((taskData.AsEnumerable().Count()) - 1).Time - taskData.AsEnumerable().ElementAt(0).Time;
                }
                else
                {
                    //take the first refill, err add 6, and double the time
                    int endRx1 = taskData.Where(c => c.EventType == "RefillAnotherPrescription").AsEnumerable().ElementAt(0).TaskDataId;
                    var Rx1Data = taskData.Where(c => c.TaskDataId <= (endRx1 - 1));

                    numCorrect = Rx1Data.Where(c => c.EventSummary == "Correct").AsEnumerable().Count();
                    numIncorrect = Rx1Data.Where(c => c.EventSummary == "Incorrect").AsEnumerable().Count();
                    numValid = Rx1Data.Where(c => c.EventSummary == "Valid").AsEnumerable().Count();
                    numInvalid = Rx1Data.Where(c => c.EventSummary == "Invalid").AsEnumerable().Count();
                    total_cor = numCorrect + numValid;
                    total_err = numIncorrect + numInvalid;
                    timeTaken = Rx1Data.AsEnumerable().ElementAt((Rx1Data.AsEnumerable().Count()) - 1).Time - Rx1Data.AsEnumerable().ElementAt(0).Time;

                    total_err = total_err + 6;
                    timeTaken = timeTaken + timeTaken;
                }
            }

            else
            {
                numCorrect = taskData.Where(c => c.EventSummary == "Correct").AsEnumerable().Count();
                numIncorrect = taskData.Where(c => c.EventSummary == "Incorrect").AsEnumerable().Count();
                numValid = taskData.Where(c => c.EventSummary == "Valid").AsEnumerable().Count();
                numInvalid = taskData.Where(c => c.EventSummary == "Invalid").AsEnumerable().Count();
                total_cor = numCorrect + numValid;
                total_err = numIncorrect + numInvalid;
                timeTaken = taskData.AsEnumerable().ElementAt((taskData.AsEnumerable().Count()) - 1).Time - taskData.AsEnumerable().ElementAt(0).Time;
            }


            if (taskData.Where(c => c.EventType == "CallHospital").AsEnumerable().Count() != 0) total_cor += 1;

            if (taskData.Where(c => c.EventType == "LanguageSelection").AsEnumerable().Count() != 0)
            {
                if (taskData.Where(c => c.EventType == "LanguageSelection").AsEnumerable().Last().EventData == "1" || taskData.Where(c => c.EventType == "LanguageSelection").AsEnumerable().Last().EventData == "2")
                    total_cor += 1;
            }

            if (taskData.Where(c => c.EventType == "TaskClosed").AsEnumerable().Count() != 0) total_cor += 1;

            int end = taskData.AsEnumerable().Last().TaskDataId;
            if (taskData.Where(c => c.TaskDataId == (end - 1)).AsEnumerable().ElementAt(0).EventType == "DuringPharmacyHours" && taskData.Where(c => c.TaskDataId == (end - 1)).AsEnumerable().ElementAt(0).EventSummary == "Valid" && total_cor >= 12 && Rx1Info.AsEnumerable().Count() == 1 && Rx2Info.AsEnumerable().Count() == 1)
                Status = "Complete";
            else
                Status = "Incomplete";

            double timeToSecond = 0;
            timeToSecond = timeTaken.TotalSeconds;
            double Rate = total_cor / timeToSecond;
            double Ratio = (double)total_cor / (total_cor + total_err);

            CsvSb.Append("Rate is ");
            CsvSb.Append(Rate.ToString("F6")); CsvSb.Append("\n");
            CsvSb.Append("Ratio is ");
            CsvSb.Append(Ratio.ToString("F4")); CsvSb.Append("\n");
            return CsvSb;
        }

        private StringBuilder SummarizeDoc(IQueryable<TaskData> taskData, StringBuilder CsvSb)
        {
            int TOTAL_QUESTIONS = 21;
            int SUBTASK_1_QUESTIONS = 7,
                SUBTASK_2_QUESTIONS = 2,
                SUBTASK_3_QUESTIONS = 8, SUBTASK_4_QUESTIONS = 4;

            string partID = taskData.AsEnumerable().ElementAt(0).SubjectID, timeTaken;
            string taskStartTime = taskData.AsEnumerable().ElementAt(0).Time.ToString();

            //total time taken. why is it being computed twice???
            //Because after you finished the task4, there was a small pause, this margin should not be included in the total time. --Qingzhou
            DateTime taskBeginTime = taskData.AsEnumerable().ElementAt(0).Time;
            DateTime taskEndTime = taskData.AsEnumerable().ElementAt((taskData.AsEnumerable().Count()) - 1).Time;


            int numCorrects = 0; //total correct for whole task instance
            int t1_correct = 0, t2_correct = 0, t3_correct = 0, t4_correct = 0;
            TimeSpan t1_time = taskData.Where(c => c.EventType == "Part 1: Q7").First().Time - taskData.Where(c => c.EventType == "TaskStart").First().Time,
                t2_time = taskData.Where(c => c.EventType == "Part 1: Q9").First().Time - taskData.Where(c => c.EventType == "Part 1: Q7").First().Time,
                t3_time = taskData.Where(c => c.EventType == "Part 1: Q13").First().Time - taskData.Where(c => c.EventType == "Part 1: Q9").First().Time,
                t4_time = taskData.Where(c => c.EventType == "Part 2: Q4").First().Time - taskData.Where(c => c.EventType == "Part 1: Q13").First().Time;
            timeTaken = (t1_time + t2_time + t3_time + t4_time).ToString();
            /*
                BEGIN SUBTASK 1
            */
            var questions = taskData.Where(c => c.EventType == "Part 1: Q1");
            if (questions.Count() == 2)
            {
                int checkCorrect = 0;
                foreach (TaskData td in questions)
                {
                    if (td.EventData.Contains("Linophen") || td.EventData.Contains("Cyclomeovan"))
                    {
                        checkCorrect++;
                    }
                }
                if (checkCorrect == 2)
                {
                    numCorrects++;
                    t1_correct++;
                }
            }
            questions = taskData.Where(c => c.EventType == "Part 1: Q2");
            if (questions.AsEnumerable().ElementAt(0).EventData == "5 days")
            {
                numCorrects++;
                t1_correct++;
            }

            questions = taskData.Where(c => c.EventType == "Part 1: Q3");
            if (questions.AsEnumerable().ElementAt(0).EventData == "18")
            {
                numCorrects++;
                t1_correct++;
            }
            questions = taskData.Where(c => c.EventType == "Part 1: Q4");
            if (questions.AsEnumerable().ElementAt(0).EventData == "24")
            {
                numCorrects++;
                t1_correct++;
            }
            questions = taskData.Where(c => c.EventType == ("Part 1: Q5"));
            if (questions.AsEnumerable().ElementAt(0).EventData == "No")
            {
                numCorrects++;
                t1_correct++;
            }
            questions = taskData.Where(c => c.EventType == ("Part 1: Q6"));
            if (questions.AsEnumerable().ElementAt(0).EventData == "3")
            {
                numCorrects++;
                t1_correct++;
            }
            questions = taskData.Where(c => c.EventType == ("Part 1: Q7"));
            if (questions.AsEnumerable().ElementAt(0).EventData == "600")
            {
                numCorrects++;
                t1_correct++;
            }
            /*
               BEGIN SUBTASK 2
           */
            questions = taskData.Where(c => c.EventType == ("Part 1: Q8"));
            if (questions.AsEnumerable().ElementAt(0).EventData == "16")
            {
                numCorrects++;
                t2_correct++;
            }
            questions = taskData.Where(c => c.EventType == ("Part 1: Q9"));
            if (questions.AsEnumerable().ElementAt(0).EventData == "Stop the medication and contact my doctor.")
            {
                numCorrects++;
                t2_correct++;
            }
            /*
               BEGIN SUBTASK 3
           */
            questions = taskData.Where(c => c.EventType == ("Part 1: Q10"));
            if (questions.AsEnumerable().ElementAt(0).EventData == "15")
            {
                numCorrects++;
                t3_correct++;
            }
            questions = taskData.Where(c => c.EventType == ("Part 1: Q11"));
            if (questions.AsEnumerable().ElementAt(0).EventData == "24")
            {
                numCorrects++;
                t3_correct++;
            }
            questions = taskData.Where(c => c.EventType == ("Part 1: Q12"));
            if (questions.AsEnumerable().ElementAt(0).EventData == "No")
            {
                numCorrects++;
                t3_correct++;
            }
            //question13
            questions = taskData.Where(c => c.EventType == ("Part 1: Q13"));
            if (questions.AsEnumerable().ElementAt(0).EventData.Contains("6"))
            {
                numCorrects++;
                t3_correct++;
            }
            if (questions.AsEnumerable().ElementAt(1).EventData.Contains("6"))
            {
                numCorrects++;
                t3_correct++;
            }
            if (questions.AsEnumerable().ElementAt(2).EventData.Contains("3"))
            {
                numCorrects++;
                t3_correct++;
            }
            if (questions.AsEnumerable().ElementAt(3).EventData.Contains("2"))
            {
                numCorrects++;
                t3_correct++;
            }
            if (questions.AsEnumerable().ElementAt(4).EventData.Contains("3"))
            {
                numCorrects++;
                t3_correct++;
            }
            /*
               BEGIN SUBTASK 4
           */
            questions = taskData.Where(c => c.EventType == ("Part 2: Q1"));
            if (questions.AsEnumerable().ElementAt(0).EventData == "Two weeks from today in the morning")
            {
                numCorrects++;
                t4_correct++;
            }
            questions = taskData.Where(c => c.EventType == ("Part 2: Q2"));
            if (questions.AsEnumerable().ElementAt(0).EventSummary == "DrinkOnlyWater")
            {
                numCorrects++;
                t4_correct++;
            }
            questions = taskData.Where(c => c.EventType == ("Part 2: Q3"));
            if (questions.Count() == 2)
            {
                int checkCorrect = 0;
                foreach (TaskData td in questions)
                {
                    if (td.EventData.Contains("Your insurance card") || td.EventData.Contains("A list of your medications"))
                    {
                        checkCorrect++;
                    }
                }
                if (checkCorrect == 2)
                {
                    numCorrects++;
                    t4_correct++;
                }
            }
            questions = taskData.Where(c => c.EventType == ("Part 2: Q4"));
            if (questions.AsEnumerable().ElementAt(0).EventSummary == "CMO24HrsAdvance")
            {
                numCorrects++;
                t4_correct++;
            }


            int t1_errors = SUBTASK_1_QUESTIONS - t1_correct, t2_errors = SUBTASK_2_QUESTIONS - t2_correct,
                t3_errors = SUBTASK_3_QUESTIONS - t3_correct, t4_errors = SUBTASK_4_QUESTIONS - t4_correct,
                total_errors = TOTAL_QUESTIONS - numCorrects;

            string Status = "Complete";

            double timeToSecond = 0;
            timeToSecond = (t1_time + t2_time + t3_time + t4_time).TotalSeconds;
            double Rate = numCorrects / timeToSecond;
            double Ratio = (double)numCorrects / (numCorrects + total_errors);

            CsvSb.Append("Rate is ");
            CsvSb.Append(Rate.ToString("F6")); CsvSb.Append("\n");
            CsvSb.Append("Ratio is ");
            CsvSb.Append(Ratio.ToString("F4")); CsvSb.Append("\n");
            return CsvSb;
        }

        private void saveTaskData(object task)
        {
            List<Dictionary<String, String>> eventData = null;
            string taskName = null;
            try
            {
                //for casting
                string taskType = task.GetType().ToString();
                switch (taskType)
                {
                    case "ATM.CitiBankForm":
                        taskName = "ATM";
                        try
                        {
                            ATM.CitiBankForm cf = (ATM.CitiBankForm)task;
                            eventData = cf.getEventData();
                        }
                        catch (Exception err)
                        {
                            Dictionary<String, String> dict1 = new Dictionary<String, String>();
                            dict1.Add("time", DateTime.Now.ToString());
                            dict1.Add("eventType", "TaskClosedWithError");
                            dict1.Add("eventData", err.Message);
                            dict1.Add("eventSummary", "");
                            eventData.Add(dict1);
                            Dictionary<String, String> dict2 = new Dictionary<String, String>();
                            dict2.Add("time", DateTime.Now.ToString());
                            dict2.Add("eventType", "TaskClosed");
                            dict2.Add("eventData", "");
                            dict2.Add("eventSummary", "");
                            eventData.Add(dict2);
                            Console.WriteLine("[" + taskName + "] closed with error, message: " + err.Message);
                        }
                        break;
                    case "PrescriptionRefill.Form1":
                        taskName = "Prescription Refill";
                        try
                        {
                            PrescriptionRefill.Form1 f1 = (PrescriptionRefill.Form1)task;
                            eventData = f1.getEventData();
                        }
                        catch (Exception err)
                        {
                            Dictionary<String, String> dict1 = new Dictionary<String, String>();
                            dict1.Add("time", DateTime.Now.ToString());
                            dict1.Add("eventType", "TaskClosedWithError");
                            dict1.Add("eventData", err.Message);
                            dict1.Add("eventSummary", "");
                            eventData.Add(dict1);
                            Dictionary<String, String> dict2 = new Dictionary<String, String>();
                            dict2.Add("time", DateTime.Now.ToString());
                            dict2.Add("eventType", "TaskClosed");
                            dict2.Add("eventData", "");
                            dict2.Add("eventSummary", "");
                            eventData.Add(dict2);
                            Console.WriteLine("[" + taskName + "] closed with error, message: " + err.Message);
                        }    
                        break;
                    case "FormsTask.Form1":
                        taskName = "Forms";
                        try
                        {
                            FormsTask.Form1 f = (FormsTask.Form1)task;
                            eventData = f.getEventData();
                        }
                        catch (Exception err)
                        {
                            Dictionary<String, String> dict1 = new Dictionary<String, String>();
                            dict1.Add("time", DateTime.Now.ToString());
                            dict1.Add("eventType", "TaskClosedWithError");
                            dict1.Add("eventData", err.Message);
                            dict1.Add("eventSummary", "");
                            eventData.Add(dict1);
                            Dictionary<String, String> dict2 = new Dictionary<String, String>();
                            dict2.Add("time", DateTime.Now.ToString());
                            dict2.Add("eventType", "TaskClosed");
                            dict2.Add("eventData", "");
                            dict2.Add("eventSummary", "");
                            eventData.Add(dict2);
                            Console.WriteLine("[" + taskName + "] closed with error, message: " + err.Message);
                        }
                        break;
                    case "ReactionTest.ReactionTestWindow": 
                        taskName = "Reaction Test";
                        try
                        {
                            ReactionTest.ReactionTestWindow rw = (ReactionTest.ReactionTestWindow)task;
                            eventData = rw.getEventData();
                        }
                        catch (Exception err)
                        {
                            Dictionary<String, String> dict1 = new Dictionary<String, String>();
                            dict1.Add("time", DateTime.Now.ToString());
                            dict1.Add("eventType", "TaskClosedWithError");
                            dict1.Add("eventData", err.Message);
                            dict1.Add("eventSummary", "");
                            eventData.Add(dict1);
                            Dictionary<String, String> dict2 = new Dictionary<String, String>();
                            dict2.Add("time", DateTime.Now.ToString());
                            dict2.Add("eventType", "TaskClosed");
                            dict2.Add("eventData", "");
                            dict2.Add("eventSummary", "");
                            eventData.Add(dict2);
                            Console.WriteLine("[" + taskName + "] closed with error, message: " + err.Message);
                        }
                        break;
                    case "MetroTickets.MainWindow":
                        taskName = "MetroTickets";
                        try
                        {
                            MetroTickets.MainWindow mw = (MetroTickets.MainWindow)task;
                            eventData = mw.getEventData();
                        }
                        catch (Exception err)
                        {
                            Dictionary<String, String> dict1 = new Dictionary<String, String>();
                            dict1.Add("time", DateTime.Now.ToString());
                            dict1.Add("eventType", "TaskClosedWithError");
                            dict1.Add("eventData", err.Message);
                            dict1.Add("eventSummary", "");
                            eventData.Add(dict1);
                            Dictionary<String, String> dict2 = new Dictionary<String, String>();
                            dict2.Add("time", DateTime.Now.ToString());
                            dict2.Add("eventType", "TaskClosed");
                            dict2.Add("eventData", "");
                            dict2.Add("eventSummary", "");
                            eventData.Add(dict2);
                            Console.WriteLine("[" + taskName + "] closed with error, message: " + err.Message);
                        }
                        break;
                    case "WpfApplication1.MainWindow":  
                        taskName = "DoctorTest";
                        try
                        {
                            WpfApplication1.MainWindow dw = (WpfApplication1.MainWindow)task;
                            eventData = dw.getEventData();
                        }
                        catch (Exception err)
                        {
                            Dictionary<String, String> dict1 = new Dictionary<String, String>();
                            dict1.Add("time", DateTime.Now.ToString());
                            dict1.Add("eventType", "TaskClosedWithError");
                            dict1.Add("eventData", err.Message);
                            dict1.Add("eventSummary", "");
                            eventData.Add(dict1);
                            Dictionary<String, String> dict2 = new Dictionary<String, String>();
                            dict2.Add("time", DateTime.Now.ToString());
                            dict2.Add("eventType", "TaskComplete");
                            dict2.Add("eventData", "");
                            dict2.Add("eventSummary", "");
                            eventData.Add(dict2);
                            Console.WriteLine("[" + taskName + "] closed with error, message: " + err.Message);
                        }
                        break;
                    case "OnlineBanking.MainWindow":
                        taskName = "OnlineBanking";
                        try
                        {
                            OnlineBanking.MainWindow ow = (OnlineBanking.MainWindow)task;
                            eventData = ow.getEventData();
                        }
                        catch (Exception err)
                        {
                            Dictionary<String, String> dict1 = new Dictionary<String, String>();
                            dict1.Add("time", DateTime.Now.ToString());
                            dict1.Add("eventType", "TaskClosedWithError");
                            dict1.Add("eventData", err.Message);
                            dict1.Add("eventSummary", "");
                            eventData.Add(dict1);
                            Dictionary<String, String> dict2 = new Dictionary<String, String>();
                            dict2.Add("time", DateTime.Now.ToString());
                            dict2.Add("eventType", "TaskClosed");
                            dict2.Add("eventData", "");
                            dict2.Add("eventSummary", "");
                            eventData.Add(dict2);
                            Console.WriteLine("[" + taskName + "] closed with error, message: " + err.Message);
                        }
                        break;
                    default:
                        break;
                }

                if (eventData != null)
                {
                    foreach (Dictionary<String, String> d in eventData)
                    {
                        saveTaskDataRow(taskName, d);
                    }
                    //
                    //
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error saving task data: " + ex.Message);
            }
        }

        

        
        //
        private void saveTaskDataRow(string taskName, Dictionary<String, String> taskDataRow)
        {
            TaskData taskData = new TaskData();
            taskData.TaskDataId = generateTaskDataId();
            taskData.Time = DateTime.Parse(taskDataRow["time"]);
            taskData.EventType = taskDataRow["eventType"];
            taskData.EventData = taskDataRow["eventData"];
            taskData.TaskName = taskName;
            taskData.EventSummary = taskDataRow["eventSummary"];
            taskData.SubjectID = participant.ParticipantID;
            //
            //finalize saving task data
            context.AddToTaskDatas(taskData);
            context.SaveChanges();
        }

        private int generateTaskDataId()
        {
            var mostRecentTaskData = (from t in context.TaskDatas orderby t.TaskDataId select t)
                                         .OrderByDescending(t => t.TaskDataId);
            int newTaskDataId;
            if (mostRecentTaskData.Count() == 0)
            {

                newTaskDataId = 0;
 
            }
            else
            {
                newTaskDataId = mostRecentTaskData.First().TaskDataId;
                
            }
            
            return newTaskDataId + 1;
        }

        //TODO: refactor this to use Panes. Very inefficient!!!!
        private void backToMainMenuBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        //if no task selected, prompts user to select task
        //else...
        //sets currentSelectedTask 
        //shows language select screen
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                System.Windows.MessageBox.Show(taskNotSelectedMessage);
            }
            else
            {
                currentSelectedTask = comboBox1.SelectionBoxItem.ToString();
                this.langSelectScreen = new LanguageSelectScreen();
                this.Content = this.langSelectScreen;
                this.langSelectScreen.englishSelectBtn.Click += new RoutedEventHandler(languageSelected);
                this.langSelectScreen.spanishSelectBtn.Click += new RoutedEventHandler(languageSelected);
            }
            
        }


        void languageSelected(object sender, RoutedEventArgs e)
        {
            currentSelectedLanguage = this.langSelectScreen.selectedLanguage;
            this.Content = taskSelectGrid;
            this.langSelectScreen = null; //does this even do anything??
            initFunctionalTask(currentSelectedTask, currentSelectedLanguage);
        }



        private void initFunctionalTask(string selectedTask, string lang)
        { 
            switch (comboBox1.SelectionBoxItem.ToString())
            {
                case "ATM":
                    System.Windows.Forms.Form task = new ATM.CitiBankForm("",lang);
                    task.FormClosed += new System.Windows.Forms.FormClosedEventHandler(taskClosedHandler);
                    task.Show();
                    break;
                case "Forms":
                    task = new FormsTask.Form1(lang);
                    task.FormClosed += new System.Windows.Forms.FormClosedEventHandler(taskClosedHandler);
                    task.Show();
                    break;
                case "Prescription Refill":
                    task = new PrescriptionRefill.Form1(lang);
                    task.FormClosed += new System.Windows.Forms.FormClosedEventHandler(taskClosedHandler);
                    task.Show();
                    break;
                case "Reaction Test":
                    Window rTask = new ReactionTest.ReactionTestWindow(lang);
                    rTask.Closed += new EventHandler(taskClosedHandler);
                    rTask.Show();
                    break;
                case "MetroTickets":
                    Window mTask = new MetroTickets.MainWindow(lang);
                    mTask.Closed += new EventHandler(taskClosedHandler);
                    mTask.Show();
                    //this.WindowState = WindowState.Minimized;
                    break;
                case "DoctorTest":
                    Window dTask = new WpfApplication1.MainWindow(lang);
                    dTask.Closed += new EventHandler(taskClosedHandler);
                    dTask.Show();
                    break;
                case "OnlineBanking":
                    Window oTask = new OnlineBanking.MainWindow(lang);
                    oTask.Closed += new EventHandler(taskClosedHandler);
                    oTask.Show();
                    break;
                default:
                    task = null;
                    break;
            }
             
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine(instructionsTextBlk.ActualHeight.ToString());
        }
    }
}
