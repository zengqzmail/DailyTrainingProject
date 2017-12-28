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
using System.Data.SqlServerCe;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.EntityClient;
using System.Threading;
using System.Timers;
using System.Text.RegularExpressions;
using System.Data.Objects;
//


namespace TasksMainMenu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        tasksDataContainer context;
        string gender, partID;
        int age, timePoint;
        //string runTasksInstructions = "";
        //string exportInstructions = "";
       
        public MainWindow()
        {
            string errorText = "";
            try
            {
                
                context = new tasksDataContainer();
                InitializeComponent();
                
            }
            catch (Exception e)
            {
                errorText += "\n" + e.ToString();
                MessageBox.Show("There was an error starting functional tasks. Please email q.zeng2@umiami.edu for assistance. Please copy the following exception message in the email." + errorText);
            }
            try
            {
                this.runTasksInstructionLbl.Content = "";
                this.exportDataInstructionLbl.Content = "";
            }
            catch (Exception e)
            {
                errorText += "\n" + e.ToString();
                MessageBox.Show("There was an error starting functional tasks. Please email q.zeng2@umiami.edu for assistance. Please copy the following exception message in the email." + errorText);
            }


            bool isFTS_NG = false;
#if FTSNG
            isFTS_NG = true;
#endif
           
        }

      

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception) args.ExceptionObject;
            MessageBox.Show(e.ToString());
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {

            if (subjectIdValidated(textBoxID.Text))
            {
                Console.WriteLine("sjid ok");
                //
                newParticipant participant = saveNewParticipant();

                TaskSelectMenu taskSelect;
                if (participant == null) //problem saving new participant
                {


                }
                else
                {
                    taskSelect = new TaskSelectMenu(participant);
                    taskSelect.Show();
                    this.Close();
                }
                //
            }
            else
            {
                Console.WriteLine("sjid bad");
                MessageBox.Show("Invalid participant ID. Please try another one.");
            }
                 
        } 
  

        //
        //returns true if param subjectID is a string of numerals followed by two uppercase letters
        //edit on Dec 27: not concerned with validation; just return true.
        private bool subjectIdValidated(string subjectID)
        {
            /*
            string subjectIdRegex = @"^\d+[A-Z]{2}$";
            Regex _regex = new Regex(subjectIdRegex);
            return _regex.IsMatch(subjectID);
             * */

            var taskDataList = context.TaskDatas;
            Console.WriteLine(taskDataList.Count());
            if(taskDataList.Count() > 0 ) 
            {
                var IDList = taskDataList.Where(c => c.SubjectID == subjectID);
                Console.WriteLine(IDList);
                if (IDList.Count() == 0)
                {
                    return true;
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("Data found with this inputted ID. Are you sure you want to use this ID?","ID has been taken",MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        return true;
                    }
                    else if(result == MessageBoxResult.No)
                    {
                        return false;
                    }
                }
            }
            return true;
            //
            
        }

        //
        // saves a newParticipant entity to data.sdf
        // returns saved newParticipant 
        // to be used in TasksSelectMenu
        // returns null if error
        private newParticipant saveNewParticipant()
        {
            newParticipant participant = null;
            //
            getNewParticipantGender();
            try
            {
                getAge();
            }
            catch
            {
                MessageBox.Show("Please Enter a Valid Age");
                return null;
            }
            getPartID();
            try
            {
                getTimePoint();
            }
            catch
            {
                MessageBox.Show("Please Enter a Valid Time Point");
                return null;
            }
            
            try
            {
                // workaround to newParticipants PK id not auto-incrementing
                var mostRecentParticipant = (from p in context.newParticipants orderby p.Id select p)
                                            .OrderByDescending(p => p.Id);
                int topId;
                if (mostRecentParticipant.Count() == 0)
                {

                    topId = 0;
                }
                else
                {
                    topId = mostRecentParticipant.First().Id;
                }

                participant = new newParticipant();
                participant.Id = topId + 1;
                participant.ParticipantID = partID;
                participant.Age = age;
                participant.Gender = gender;
                participant.Timepoint = timePoint;
                participant.Timestamp = DateTime.Now;

            }
            catch (Exception ex)
            {
                string errorMsg = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMsg += "::" + ex.InnerException.Message; //TODO: recursive concating of nested inner exception messages
                }
                MessageBox.Show("There was an error storing the participant's information. Please send an e-mail to rblanco2@med.miami.edu with the following details: " + errorMsg);
               
            }

            return participant;
            //
        }

        private void getNewParticipantGender()
        {
            gender = comboBoxGender.SelectionBoxItem.ToString();
        }
        private void getAge()
        {
            
                age = Int32.Parse(textBoxAge.Text);
            
        }
        private void getPartID()
        {
            partID = textBoxID.Text;
        }
        private void getTimePoint()
        {

            timePoint = Int32.Parse(textBoxTimePoint.Text);
            
        }


        /*
            Exports all data rows to CSV.
        */
        private void exportDataBtn_Click(object sender, RoutedEventArgs e)
        {
            exportDatatoCSV();
        }

        //open a filesave dialog box to choose save location
        //get all TaskData entity objects from sdf and mangle them into csv form
        //write to csv in selected location
        private void exportDatatoCSV()
        {
            //open file dialog
            //note to self:
            //get all task data from sdf
            var taskDataList = context.TaskDatas;
            if (taskDataList.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data to save! Aborting csv export. Press OK to return to the main window.";
                MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                StringBuilder CsvSb = new StringBuilder();
                CsvSb.Append("TaskDataID,ParticipantID,Time,TaskName,EventType,EventSummary,EventData\n");
                foreach (TaskData td in taskDataList)
                {
                    CsvSb.Append(TaskDataToCsvData(td));
                }
                
                //save to file
                System.Windows.Forms.SaveFileDialog saveCsvDialog = new System.Windows.Forms.SaveFileDialog();
                saveCsvDialog.Filter = "CSV|*.csv";
                saveCsvDialog.Title = "Save a CSV File";
                saveCsvDialog.ShowDialog();
                if (saveCsvDialog.FileName != "") //no empty filenames!
                {
                    
                    try
                    {
                        string csvData = CsvSb.ToString();
                        System.IO.File.WriteAllText(saveCsvDialog.FileName, csvData);
                        string csvSavedMsg = String.Format("CSV file saved to {0}. Click OK to return to the main menu.", saveCsvDialog.FileName);
                        MessageBox.Show(csvSavedMsg);
                    }
                    catch (Exception ex) //TODO: specifiy IOException
                    {
                        string IoExMsg = "There was an error saving your csv file. Click OK to return to the main menu.";
                        MessageBox.Show(IoExMsg+ ex.ToString());
                    }
                }
            }
            
        }


        //code adapted from http://stackoverflow.com/questions/3199604/is-there-a-quick-way-to-convert-an-entity-to-csv-file
        private string TaskDataToCsvData(TaskData td)
        {
            if (td == null)
            {
                throw new ArgumentNullException("td", "Value can not be null or Nothing!");
            }
            StringBuilder sb = new StringBuilder();
            
            sb.Append(td.TaskDataId.ToString()); sb.Append(",");
            sb.Append(td.SubjectID); sb.Append(","); 
            sb.Append(td.Time.ToString()); sb.Append(",");
            sb.Append(td.TaskName); sb.Append(",");
            sb.Append(td.EventType); sb.Append(",");
            sb.Append(td.EventSummary); sb.Append(",");
            sb.Append(td.EventData); sb.Append("\n");

            return sb.ToString();
        }


        /*
            Export "summary" report for all users, by task
        */
        private void expSummButton_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxSummary.SelectionBoxItem.Equals("ATM"))
            {
                expATMData(context.TaskDatas);
            }
            else if (comboBoxSummary.SelectionBoxItem.Equals("Prescription Refill"))
            {
                
                expPresRefill(context.TaskDatas);
            }
            else if (comboBoxSummary.SelectionBoxItem.Equals("Reaction Test"))
            {

                expReaction(context.TaskDatas);
            }
            else if (comboBoxSummary.SelectionBoxItem.Equals("Forms"))
            {

                expForms(context.TaskDatas);
            }
            else if (comboBoxSummary.SelectionBoxItem.Equals("DoctorTest"))
            {

                expDoctor(context.TaskDatas);
            }
            else if (comboBoxSummary.SelectionBoxItem.Equals("Metro Kiosk"))
            {

                expMetroKiosk(context.TaskDatas);
            }
            else if (comboBoxSummary.SelectionBoxItem.Equals("OnlineBanking"))
            {
                expOnlineBanking(context.TaskDatas);
            }
            else
            {
                MessageBox.Show(comboBoxSummary.SelectionBoxItem.ToString() + "Please make a Selection for summary!");
            }
        }

        private void expMetroKiosk(ObjectSet<TaskData> taskDataList)
        {
            if (taskDataList.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data to save! Aborting csv export. Press OK to return to the main window.";
                MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                try
                {
                    StringBuilder CsvSb = new StringBuilder();
                    CsvSb.Append("MetroTickets\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, SubTask1_Time, SubTask2_Time, SubTask3_Time, Total_Correct, SubTask1_Correct, SubTask2_Correct, SubTask3_Correct, Total_Incorrect, SubTask1_Incorrect, SubTask2_Incorrect, SubTask3_Incorrect, Rate, Ratio\n");
                    var KioskTotalData = taskDataList.Where(c => c.TaskName == "MetroTickets");
                    var KioskStarts = KioskTotalData.Where(c => c.EventType == "TaskStart");
                    var KioskCloses = KioskTotalData.Where(c => c.EventType == "TaskClosed");

                    for (int i = 0; i < KioskStarts.AsEnumerable().Count(); i++)
                    {
                        try
                        {
                            int start = KioskStarts.AsEnumerable().ElementAt(i).TaskDataId;
                            int end = KioskCloses.AsEnumerable().ElementAt(i).TaskDataId;
                            var ShoppingSingleTaskData = KioskTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                            CsvSb = SummarizeMetroKiosk(ShoppingSingleTaskData, CsvSb);
                        }
                        catch (Exception ex)
                        {
                            //to-do: give an informative message to the user
                            MessageBox.Show("Error Saving MetroKiosk Data:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
                        }
                    }
                    saveToFile(CsvSb);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Saving Shopping Data:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
                }
            }
        }


        /*
            Summarizes data for single instance of MetroKiosk task
        */
        private StringBuilder SummarizeMetroKiosk(IQueryable<TaskData> metroKioskSingleTaskData, StringBuilder CsvSb)
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
						else {t1_errors++; Status = "Incomplete";}
						
                        total_correct += t1_correct;
                        total_errors += t1_errors;

                        break;
                    case "2":
                        t2_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - subTaskEnds.AsEnumerable().ElementAt(i-1).Time;
                        total_task_time += t2_time;
                        
                        t2_correct = Int32.Parse(numCorrect);
                        t2_errors = Int32.Parse(numIncorrect);
						
						if (metroSingleSubTaskData.AsEnumerable().Last().EventType == "SubTaskCompleted") t2_correct++;
						else {t2_errors++; Status = "Incomplete";}
                        
                        total_correct += t2_correct;
                        total_errors += t2_errors;

                        break;
                    case "3":
                        t3_time = subTaskEnds.AsEnumerable().ElementAt(i).Time - subTaskEnds.AsEnumerable().ElementAt(i-1).Time;
                        total_task_time += t3_time;
                        
                        t3_correct = Int32.Parse(numCorrect);
                        t3_errors = Int32.Parse(numIncorrect);
						
						if (metroSingleSubTaskData.AsEnumerable().Last().EventType == "SubTaskCompleted") t3_correct++;
						else {t3_errors++; Status = "Incomplete";}
                        
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

            //Now let's construct the row
            CsvSb.Append(partID); CsvSb.Append(", ");
            CsvSb.Append(taskStartTime); CsvSb.Append(", ");
            CsvSb.Append(Status); CsvSb.Append(", ");
            CsvSb.Append(total_task_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_time.ToString()); CsvSb.Append(", ");

            CsvSb.Append(total_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_correct.ToString()); CsvSb.Append(", ");

            CsvSb.Append(total_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(Rate.ToString()); CsvSb.Append(", ");
            CsvSb.Append(Ratio.ToString()); CsvSb.Append("\n");
            
            return (CsvSb);
        }






        /*
           Exports ATM data for all participants.
           11-23-2015: 
           New design, new fields:
               sjid, task_begin, t1_time, t2_time, t3_time, total_time, t1_correct, t2_correct, t3_correct, total_correct, 
               t1_errors, t2_errors, t3_errors, total_errors
       */
        private StringBuilder SummarizeATM(IQueryable<TaskData> taskData, StringBuilder CsvSb)
        {
            string partID = taskData.AsEnumerable().ElementAt(0).SubjectID;
            string subtaskNum, numCorrect, numIncorrect, Status;
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

            var statusJudge = subTaskEnds.Where(c => c.EventType == "MaxErrorsReached" || c.EventType == "SubTaskSkip");
            if (statusJudge.AsEnumerable().Count() == 0) Status = "Complete";
            else Status = "Incomplete";

            timeToSecond = total_time.TotalSeconds;
            double Rate = total_correct / timeToSecond;
            double Ratio = (double)total_correct / (total_correct + total_errors);

            CsvSb.Append(partID); CsvSb.Append(", ");
            CsvSb.Append(taskStartTime); CsvSb.Append(", ");
            CsvSb.Append(Status); CsvSb.Append(", ");
            CsvSb.Append(total_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(total_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(total_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(Rate.ToString()); CsvSb.Append(", ");
            CsvSb.Append(Ratio.ToString()); CsvSb.Append("\n");

            return (CsvSb);

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

            CsvSb.Append(partID); CsvSb.Append(", ");
            CsvSb.Append(taskStartTime); CsvSb.Append(", ");
            CsvSb.Append(Status); CsvSb.Append(", ");
            CsvSb.Append(total_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t4_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(total_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t4_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(total_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t4_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(Rate.ToString()); CsvSb.Append(", ");
            CsvSb.Append(Ratio.ToString()); CsvSb.Append("\n");

            return (CsvSb);
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


            CsvSb.Append(partID); CsvSb.Append(", ");
            CsvSb.Append(startTime); CsvSb.Append(", ");
            CsvSb.Append(Status); CsvSb.Append(", ");
            CsvSb.Append(timeTaken.ToString()); CsvSb.Append(", ");
            CsvSb.Append(total_cor.ToString()); CsvSb.Append(", ");
            CsvSb.Append(total_err.ToString()); CsvSb.Append(", ");
            CsvSb.Append(Rate.ToString()); CsvSb.Append(", ");
            CsvSb.Append(Ratio.ToString()); CsvSb.Append("\n");

            return (CsvSb);
        }

        /// <summary>
        /// used to summarize one instance of Doctor's Visit task
        /// </summary>
        /// <param name="taskData"></param>
        /// <param name="CsvSb"></param>
        /// <returns></returns>
        private StringBuilder SummarizeDoc(IQueryable<TaskData> taskData, StringBuilder CsvSb)
        {

            int TOTAL_QUESTIONS = 21;
            int SUBTASK_1_QUESTIONS = 7, 
                SUBTASK_2_QUESTIONS = 2, 
                SUBTASK_3_QUESTIONS = 8,  SUBTASK_4_QUESTIONS = 4;

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

            //Now let's construct the row
            CsvSb.Append(partID); CsvSb.Append(", ");
            CsvSb.Append(taskStartTime); CsvSb.Append(", ");
            CsvSb.Append(Status); CsvSb.Append(", ");
            CsvSb.Append(timeTaken); CsvSb.Append(", ");
            CsvSb.Append(t1_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_time.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t4_time.ToString()); CsvSb.Append(", ");

            CsvSb.Append(numCorrects.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_correct.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t4_correct.ToString()); CsvSb.Append(", ");

            CsvSb.Append(total_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t1_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t2_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t3_errors.ToString()); CsvSb.Append(", ");
            CsvSb.Append(t4_errors.ToString()); CsvSb.Append(", ");

            CsvSb.Append(Rate.ToString()); CsvSb.Append(", ");
            CsvSb.Append(Ratio.ToString()); CsvSb.Append("\n");

            return CsvSb;
        }





        private StringBuilder SummarizeReac(IQueryable<TaskData> taskData, StringBuilder CsvSb)
        {
            string partID = taskData.AsEnumerable().ElementAt(0).SubjectID;
            string startTime = taskData.AsEnumerable().ElementAt(0).Time.ToString();
            string  numIncorrect;
            string Status = "Complete";
            double numCorrect, meanS, medianS, stDevS, meanC, medianC, stDevC, adjMeanS, adjMedianS, adjStDevS, adjMeanC, adjMedianC, adjStDevC, numOutliersS, numOutliersC;

            var simples = taskData.Where(c => c.EventSummary.Contains("ReactionType:S") && c.EventType == "KeyPress");
            var choices = taskData.Where(c => c.EventSummary.Contains("ReactionType:C") && c.EventType == "KeyPress");
            var corrChoices = choices.Where(c => c.EventSummary.Contains("Correct:T"));

            if (taskData.Where(c => c.EventType == "KeyPressTimeOut").AsEnumerable().Count() != 0) Status = "Incomplete";

            if (simples.AsEnumerable().Count() == 0)
            {
                meanS = 0; medianS = 0; stDevS = 0; numOutliersS = 0; adjMeanS = 0; adjMedianS = 0; adjStDevS = 0;
            }

            else if (simples.AsEnumerable().Count() == 1)
            {
                meanS = Convert.ToDouble(simples.AsEnumerable().ElementAt(0).EventData);
                medianS = meanS; stDevS = 0; numOutliersS = 0; adjMeanS = meanS; adjMedianS = meanS; adjStDevS = 0;
            }
            else
            {
                meanS = 0;
                foreach (TaskData td in simples)
                {
                    meanS += Convert.ToDouble(td.EventData);
                }
                meanS = meanS / simples.AsEnumerable().Count();

                stDevS = 0;
                foreach (TaskData td in simples)
                {
                    stDevS += Math.Pow((Convert.ToDouble(td.EventData) - meanS), 2);
                }
                stDevS = Math.Sqrt((stDevS) / (simples.AsEnumerable().Count() - 1));

                //make a list of doubles
                List<double> simList = new List<double>();
                foreach (TaskData td in simples)
                {
                    simList.Add(Convert.ToDouble(td.EventData));
                }
                simList = simList.OrderBy(l => l).ToList();

                if (simList.AsEnumerable().Count() % 2 == 0)//even
                {
                    int midIndex = simList.AsEnumerable().Count() / 2;
                    medianS = (simList.AsEnumerable().ElementAt(midIndex - 1) + simList.AsEnumerable().ElementAt(midIndex)) / 2;
                }
                else //odd
                {
                    double element = (double)simList.AsEnumerable().Count() / 2;
                    element = Math.Round(element, MidpointRounding.AwayFromZero);

                    medianS = Convert.ToDouble(simList.AsEnumerable().ElementAt((int)(element - 1)));

                }

                // Get adjusted values
                adjMeanS = 0;
                numOutliersS = 0;
                foreach (TaskData td in simples)
                {
                    if ((Convert.ToDouble(td.EventData) < (meanS + (stDevS * 3))) && (Convert.ToDouble(td.EventData) > (meanS - (stDevS * 3))))
                    {
                        adjMeanS += Convert.ToDouble(td.EventData);
                    }
                    else
                    {
                        numOutliersS++;
                    }
                }
                adjMeanS = adjMeanS / (simples.AsEnumerable().Count() - numOutliersS);

                adjStDevS = 0;
                foreach (TaskData td in simples)
                {
                    if ((Convert.ToDouble(td.EventData) < (meanS + (stDevS * 3))) && (Convert.ToDouble(td.EventData) > (meanS - (stDevS * 3))))
                    {
                        adjStDevS += Math.Pow((Convert.ToDouble(td.EventData) - adjMeanS), 2);
                    }
                }
                adjStDevS = Math.Sqrt((adjStDevS) / (simples.AsEnumerable().Count() - numOutliersS - 1));

                //make a list of doubles
                List<double> adjSimList = new List<double>();
                foreach (TaskData td in simples)
                {
                    if ((Convert.ToDouble(td.EventData) < (meanS + (stDevS * 3))) && (Convert.ToDouble(td.EventData) > (meanS - (stDevS * 3))))
                    {
                        adjSimList.Add(Convert.ToDouble(td.EventData));
                    }
                }
                adjSimList = adjSimList.OrderBy(l => l).ToList();

                if (adjSimList.AsEnumerable().Count() % 2 == 0)//even
                {
                    int midIndex = adjSimList.AsEnumerable().Count() / 2;
                    adjMedianS = (adjSimList.AsEnumerable().ElementAt(midIndex - 1) + adjSimList.AsEnumerable().ElementAt(midIndex)) / 2;
                }
                else //odd
                {
                    double element = (double)adjSimList.AsEnumerable().Count() / 2;
                    element = Math.Round(element, MidpointRounding.AwayFromZero);
                    adjMedianS = Convert.ToDouble(adjSimList.AsEnumerable().ElementAt((int)(element - 1)));

                }
            }

            if (corrChoices.AsEnumerable().Count() == 0)
            {
                meanC = 0; medianC = 0; stDevC = 0; numOutliersC = 0; adjMeanC = 0; adjMedianC = 0; adjStDevC = 0;
            }

            else if (corrChoices.AsEnumerable().Count() == 1)
            {
                meanC = Convert.ToDouble(corrChoices.AsEnumerable().ElementAt(0).EventData);
                medianC = meanC; stDevC = 0; numOutliersC = 0; adjMeanC = meanC; adjMedianC = meanC; adjStDevC = 0;
            }
            else
            {
                meanC = 0;
                foreach (TaskData td in corrChoices)
                {
                    meanC += Convert.ToDouble(td.EventData);
                }
                meanC = meanC / corrChoices.AsEnumerable().Count();

                stDevC = 0;
                foreach (TaskData td in corrChoices)
                {
                    stDevC += Math.Pow((Convert.ToDouble(td.EventData) - meanC), 2);
                }
                stDevC = Math.Sqrt((stDevC) / (corrChoices.AsEnumerable().Count() - 1));

                //Make list of doubles
                List<double> cList = new List<double>();
                foreach (TaskData td in corrChoices)
                {
                    cList.Add(Convert.ToDouble(td.EventData));
                }
                cList = cList.OrderBy(l => l).ToList();

                if (cList.AsEnumerable().Count() % 2 == 0)//even
                {
                    int midIndex = cList.AsEnumerable().Count() / 2;
                    medianC = ((cList.AsEnumerable().ElementAt(midIndex - 1)) + (cList.AsEnumerable().ElementAt(midIndex))) / 2;
                }
                else //odd
                {
                    double element = (double)cList.AsEnumerable().Count() / 2;
                    element = Math.Round(element, MidpointRounding.AwayFromZero);

                    medianC = Convert.ToDouble(cList.AsEnumerable().ElementAt((int)(element - 1)));

                }




                adjMeanC = 0;
                numOutliersC = 0;
                foreach (TaskData td in corrChoices)
                {
                    if ((Convert.ToDouble(td.EventData) < (meanC + (stDevC * 3))) && (Convert.ToDouble(td.EventData) > (meanC - (stDevC * 3))))
                    {
                        adjMeanC += Convert.ToDouble(td.EventData);
                    }
                    else
                    {
                        numOutliersC++;
                    }
                }
                adjMeanC = adjMeanC / (corrChoices.AsEnumerable().Count() - numOutliersC);

                adjStDevC = 0;
                foreach (TaskData td in corrChoices)
                {
                    if ((Convert.ToDouble(td.EventData) < (meanC + (stDevC * 3))) && (Convert.ToDouble(td.EventData) > (meanC - (stDevC * 3))))
                    {
                        adjStDevC += Math.Pow((Convert.ToDouble(td.EventData) - adjMeanC), 2);
                    }
                }
                adjStDevC = Math.Sqrt((adjStDevC) / (corrChoices.AsEnumerable().Count() - numOutliersC - 1));

                //make a list of doubles
                List<double> adjChoList = new List<double>();
                foreach (TaskData td in corrChoices)
                {
                    if ((Convert.ToDouble(td.EventData) < (meanC + (stDevC * 3))) && (Convert.ToDouble(td.EventData) > (meanC - (stDevC * 3))))
                    {
                        adjChoList.Add(Convert.ToDouble(td.EventData));
                    }
                }
                adjChoList = adjChoList.OrderBy(l => l).ToList();

                if (adjChoList.AsEnumerable().Count() % 2 == 0)//even
                {
                    int midIndex = adjChoList.AsEnumerable().Count() / 2;
                    adjMedianC = (adjChoList.AsEnumerable().ElementAt(midIndex - 1) + adjChoList.AsEnumerable().ElementAt(midIndex)) / 2;
                }
                else //odd
                {
                    double element = (double)adjChoList.AsEnumerable().Count() / 2;
                    element = Math.Round(element, MidpointRounding.AwayFromZero);
                    adjMedianC = Convert.ToDouble(adjChoList.AsEnumerable().ElementAt((int)(element - 1)));

                }
            }
            numCorrect = taskData.Where(c => c.EventSummary.Contains("Correct:T")).AsEnumerable().Count();
            numIncorrect = taskData.Where(c => c.EventSummary.Contains("Correct:F")).AsEnumerable().Count().ToString();

            CsvSb.Append(partID.ToString()); CsvSb.Append(", ");
            CsvSb.Append(startTime); CsvSb.Append(", ");
            CsvSb.Append(Status); CsvSb.Append(", ");
            CsvSb.Append("SimpleTest"); CsvSb.Append(", ");
            CsvSb.Append(meanS.ToString()); CsvSb.Append(", ");
            CsvSb.Append(medianS.ToString()); CsvSb.Append(", ");
            CsvSb.Append(stDevS.ToString()); CsvSb.Append(", ");
            CsvSb.Append(numOutliersS.ToString()); CsvSb.Append(", ");
            CsvSb.Append(adjMeanS.ToString()); CsvSb.Append(", ");
            CsvSb.Append(adjMedianS.ToString()); CsvSb.Append(", ");
            CsvSb.Append(adjStDevS.ToString()); CsvSb.Append(", ");
            CsvSb.Append("ComplexTest"); CsvSb.Append(", ");
            CsvSb.Append(numCorrect.ToString()); CsvSb.Append(", ");
            CsvSb.Append(numIncorrect); CsvSb.Append(", ");
            CsvSb.Append(meanC.ToString()); CsvSb.Append(", ");
            CsvSb.Append(medianC.ToString()); CsvSb.Append(", ");
            CsvSb.Append(stDevC.ToString()); CsvSb.Append(", ");
            CsvSb.Append(numOutliersC.ToString()); CsvSb.Append(", ");
            CsvSb.Append(adjMeanC.ToString()); CsvSb.Append(", ");
            CsvSb.Append(adjMedianC.ToString()); CsvSb.Append(", ");
            CsvSb.Append(adjStDevC.ToString()); CsvSb.Append("\n");


            //appendSimpleData
            /*  CsvSb.Append(partID); CsvSb.Append(", ");
              CsvSb.Append(startTime); CsvSb.Append(", ");
              CsvSb.Append("S"); CsvSb.Append(", ");
               CsvSb.Append(", ");
               CsvSb.Append(", ");
              CsvSb.Append(meanS); CsvSb.Append(", ");
              CsvSb.Append(medianS); CsvSb.Append(", ");
              CsvSb.Append(stDevS); CsvSb.Append(",");
              CsvSb.Append(numOutliersS); CsvSb.Append(",");
               CsvSb.Append(", ");
              CsvSb.Append(adjMeanS); CsvSb.Append(", ");
              CsvSb.Append(adjMedianS); CsvSb.Append(", ");
              CsvSb.Append(adjStDevS); CsvSb.Append("\n"); */

            //appendComplexData
            /*    CsvSb.Append(partID); CsvSb.Append(", ");
                CsvSb.Append(startTime); CsvSb.Append(", ");
                CsvSb.Append("C"); CsvSb.Append(", ");
                CsvSb.Append(numCorrect); CsvSb.Append(", ");
                CsvSb.Append(numIncorrect); CsvSb.Append(", ");
                CsvSb.Append(meanC); CsvSb.Append(", ");
                CsvSb.Append(medianC); CsvSb.Append(", ");
                CsvSb.Append(stDevC); CsvSb.Append(", ");
                CsvSb.Append(numOutliersC); CsvSb.Append(",");
                CsvSb.Append(numCorrect-numOutliersC); CsvSb.Append(", ");
                CsvSb.Append(adjMeanC); CsvSb.Append(", ");
                CsvSb.Append(adjMedianC); CsvSb.Append(", ");
                CsvSb.Append(adjStDevC); CsvSb.Append("\n"); */


            return (CsvSb);
        }

        /// <summary>
        /// for exporting doctor needs to be added to drop down menu to call this methos when summarize doc selected
        /// </summary>
        /// <param name="task"></param>
        /// <param name="e"></param>
        private void expDoctor(System.Data.Objects.IObjectSet<TaskData> taskDataList)
        {
            if (taskDataList.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data to save! Aborting csv export. Press OK to return to the main window.";
                MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                try
                {
                    StringBuilder CsvSb = new StringBuilder();
                    CsvSb.Append("DoctorTest\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, SubTask1_Time, SubTask2_Time, SubTask3_Time, SubTask4_Time, Total_Correct, SubTask1_Correct, SubTask2_Correct, SubTask3_Correct, SubTask4_Correct, Total_Incorrect, SubTask1_Incorrect, SubTask2_Incorrect, SubTask3_Incorrect, SubTask4_Incorrect, Rate, Ratio\n");
                    var DocTotalData = taskDataList.Where(c => c.TaskName == "DoctorTest");
                    var DocStarts = DocTotalData.Where(c => c.EventType == "TaskStart");
                    var DocCloses = DocTotalData.Where(c => c.EventType == "TaskComplete");

                    for (int i = 0; i < DocStarts.AsEnumerable().Count(); i++)
                    {
                        try
                        {
                            int start = DocStarts.AsEnumerable().ElementAt(i).TaskDataId;
                            int end = DocCloses.AsEnumerable().ElementAt(i).TaskDataId;
                            var DocSingleTaskData = DocTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                            CsvSb = SummarizeDoc(DocSingleTaskData, CsvSb);
                        }
                        catch (Exception ex)
                        {
                            //to-do: give an informative message to the user
                            //suppress for now...
                            //MessageBox.Show("Error Saving Doctors' Task Data:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace + "\n\nTaskDataId range: " + start.ToString() + "::" + end.ToString());
                        }
                    }
                    saveToFile(CsvSb);
                }
                catch
                {
                    MessageBox.Show("Error Saving Doctor's Task Data");
                }
            }
        }

        private void expReaction(System.Data.Objects.IObjectSet<TaskData> taskDataList)
        {
            
            if (taskDataList.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data to save! Aborting csv export. Press OK to return to the main window.";
                MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                try
                {
                    StringBuilder CsvSb = new StringBuilder();
                    CsvSb.Append("Reaction Test\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Test_Type, Mean, Median, Standard_Deviation, Outliers, Adjusted_Mean, Adjusted_Median, Adjusted_SD, Test_Type, N_Correct, N_Incorrect, Mean, Median, Standard_Deviation, Outliers, Adjusted_Mean, Adjusted_Median, Adjusted_SD\n");
                    var ReacTotalData = taskDataList.Where(c => c.TaskName == "Reaction Test");
                    var ReacStarts = ReacTotalData.Where(c => c.EventType == "TaskStarted");
                    var ReacCloses = ReacTotalData.Where(c => c.EventType == "TaskClosed");

                    for (int i = 0; i < ReacStarts.AsEnumerable().Count(); i++)
                    {
                        try
                        {
                        int start = ReacStarts.AsEnumerable().ElementAt(i).TaskDataId;
                        int end = ReacCloses.AsEnumerable().ElementAt(i).TaskDataId;
                        var PresSingleTaskData = ReacTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                        CsvSb = SummarizeReac(PresSingleTaskData, CsvSb);                        
                        }
                        catch
                        {
                            // suppressing for now. investigate later.
                           // MessageBox.Show("Skipping an instance: Escaped before completing task!!");
                        }
                    }
                    saveToFile(CsvSb);
                }
                catch
                {
                    MessageBox.Show("Error Saving Reaction Data");
                }
            }
        }

        private void saveToFile(StringBuilder CsvSb)
        {

            //save to file
            System.Windows.Forms.SaveFileDialog saveCsvDialog = new System.Windows.Forms.SaveFileDialog();
            saveCsvDialog.Filter = "CSV|*.csv";
            saveCsvDialog.Title = "Save a CSV File";
            saveCsvDialog.ShowDialog();
            if (saveCsvDialog.FileName != "") //no empty filenames!
            {

                try
                {
                    string csvData = CsvSb.ToString();
                    System.IO.File.WriteAllText(saveCsvDialog.FileName, csvData);
                    string csvSavedMsg = String.Format("CSV file saved to {0}. Click OK to return to the main menu.", saveCsvDialog.FileName);
                    MessageBox.Show(csvSavedMsg);
                }
                catch (Exception ex) //TODO: specifiy IOException
                {
                    string IoExMsg = "There was an error saving your csv file. Click OK to return to the main menu.";
                    MessageBox.Show(IoExMsg + ex.ToString());
                }
            }



        }
        private void expPresRefill(System.Data.Objects.IObjectSet<TaskData> taskDataList)
        {

            if (taskDataList.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data to save! Aborting csv export. Press OK to return to the main window.";
                MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                try
                {
                    StringBuilder CsvSb = new StringBuilder();
                    CsvSb.Append("Prescription Refill\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, Total_Correct, Total_Error, Rate, Ratio\n");
                    var PresTotalData = taskDataList.Where(c => c.TaskName == "Prescription Refill");
                    var PresStarts = PresTotalData.Where(c => c.EventType == "TaskStart");
                    var PresCloses = PresTotalData.Where(c => c.EventType == "TaskClosed");

                    for (int i = 0; i < PresStarts.AsEnumerable().Count(); i++)
                    {
                        try
                        {
                        int start = PresStarts.AsEnumerable().ElementAt(i).TaskDataId;
                        int end = PresCloses.AsEnumerable().ElementAt(i).TaskDataId;
                        var PresSingleTaskData = PresTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                        
                            CsvSb = SummarizePres(PresSingleTaskData, CsvSb);
                        }
                        catch(Exception ex)
                        {
                            string IoExMsg = "There was an error saving your csv file. Click OK to return to the main menu.";
                            MessageBox.Show(IoExMsg + ex.ToString());
                        }

                    }
                    saveToFile(CsvSb);
                }
                catch
                {
                    MessageBox.Show("Error Saving Prescription Data");
                }
            }
        }

        /*
            Handles Data Summary Functions for Forms task.
        */
        private void expForms(System.Data.Objects.IObjectSet<TaskData> taskDataList)
        {

            if (taskDataList.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data to save! Aborting csv export. Press OK to return to the main window.";
                MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                try
                {
                    StringBuilder CsvSb = new StringBuilder();
                    CsvSb.Append("Forms\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time\n");
                    var FormsTotalData = taskDataList.Where(c => c.TaskName == "Forms");
                    var FormStarts = FormsTotalData.Where(c => c.EventType == "TaskStart");
                    var FormCloses = FormsTotalData.Where(c => c.EventType == "TaskClosed");


                    for (int i = 0; i < FormStarts.AsEnumerable().Count(); i++)
                    {
                        try
                        {
                            int start = FormStarts.AsEnumerable().ElementAt(i).TaskDataId;
                            int end = FormCloses.AsEnumerable().ElementAt(i).TaskDataId;
                            var FormSingleTaskData = FormsTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);

                            TimeSpan formsTotalTime = FormCloses.AsEnumerable().ElementAt(i).Time - FormStarts.AsEnumerable().ElementAt(i).Time;
                            String Status = "Completed";
                            if (FormSingleTaskData.Where(c => c.EventSummary == "ClickedNo").AsEnumerable().Count() != 0) Status = "Incomplete";

                            CsvSb.Append(FormsTotalData.AsEnumerable().ElementAt(0).SubjectID.ToString()); CsvSb.Append(", ");
                            CsvSb.Append(FormStarts.AsEnumerable().ElementAt(i).Time.ToString()); CsvSb.Append(", ");
                            CsvSb.Append(Status); CsvSb.Append(", ");
                            CsvSb.Append(formsTotalTime.ToString()); CsvSb.Append("\n");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error Saving Forms Data:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
                        }
                    }
                    saveToFile(CsvSb);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error Saving Forms Data:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
                }
            }
        }



        private StringBuilder SummarizeForms(IQueryable<TaskData> taskData, StringBuilder CsvSb)
        {
            string partID = taskData.AsEnumerable().ElementAt(0).SubjectID; //grab SJID
            DateTime taskStartTime = taskData.AsEnumerable().ElementAt(0).Time; //grab task trial start time
            DateTime taskEndTime = taskData.AsEnumerable().Last().Time; //grab task trial start time
            TimeSpan taskCompletionTime = taskEndTime - taskStartTime;

            CsvSb.Append(partID); CsvSb.Append(", ");
            CsvSb.Append(taskStartTime); CsvSb.Append(", ");
            CsvSb.Append(taskCompletionTime); CsvSb.Append("\r\n");

            return CsvSb;
        }


        /*
            Exports ATM data for all participants.
            11-23-2015: 
            New design, new fields:
                sjid, task_begin, t1_time, t2_time, t3_time, total_time, t1_correct, t2_correct, t3_correct, total_correct, 
                t1_errors, t2_errors, t3_errors, total_errors
        */
        private void expATMData(System.Data.Objects.IObjectSet<TaskData> taskDataList)
        {
            
            if (taskDataList.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data to save! Aborting csv export. Press OK to return to the main window.";
                MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                try
                {
                    StringBuilder CsvSb = new StringBuilder();
                    CsvSb.Append("ATM\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, SubTask1_Time, SubTask2_Time, SubTask3_Time, Total_Correct, SubTask1_Correct, SubTask2_Correct, SubTask3_Correct, Total_Incorrect, SubTask1_Incorrect, SubTask2_Incorrect, SubTask3_Incorrect, Rate, Ratio\n");
                    var ATMTotalData = taskDataList.Where(c => c.TaskName == "ATM");
                    var ATMStarts = ATMTotalData.Where(c => c.EventType == "TaskStart");
                    var ATMCloses = ATMTotalData.Where(c => c.EventType == "TaskClosed");
                    for (int i = 0; i < ATMStarts.AsEnumerable().Count(); i++) 
                    {
                        try
                        {
                            int start = ATMStarts.AsEnumerable().ElementAt(i).TaskDataId;
                            int end = ATMCloses.AsEnumerable().ElementAt(i).TaskDataId;
                            var ATMSingleTaskData = ATMTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);

                            CsvSb = SummarizeATM(ATMSingleTaskData, CsvSb);
                        }
                        catch (Exception ex)
                        {                           
                            MessageBox.Show("Error Saving ATM Data:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace );
                        }
                    }
                    saveToFile(CsvSb);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error Saving ATM Data:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
                }
            }
        }

        private void expOnlineBanking(System.Data.Objects.IObjectSet<TaskData> taskDataList)
        {
            if (taskDataList.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data to save! Aborting csv export. Press OK to return to the main window.";
                MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                try
                {
                    StringBuilder CsvSb = new StringBuilder();
                    CsvSb.Append("OnlineBanking\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, SubTask1_Time, SubTask2_Time, SubTask3_Time, SubTask4_Time, Total_Correct, SubTask1_Correct, SubTask2_Correct, SubTask3_Correct, SubTask4_Correct, Total_Incorrect, SubTask1_Incorrect, SubTask2_Incorrect, SubTask3_Incorrect, SubTask4_Incorrect, Rate, Ratio\n");
                    var OnlineBankingTotalData = taskDataList.Where(c => c.TaskName == "OnlineBanking");
                    var OnlineBankingStarts = OnlineBankingTotalData.Where(c => c.EventType == "TaskStart");
                    var OnlineBankingCloses = OnlineBankingTotalData.Where(c => c.EventType == "TaskClose");
                    for (int i = 0; i < OnlineBankingStarts.AsEnumerable().Count(); i++)
                    {
                        try
                        {
                            int start = OnlineBankingStarts.AsEnumerable().ElementAt(i).TaskDataId;
                            int end = OnlineBankingCloses.AsEnumerable().ElementAt(i).TaskDataId;
                            var OnlineBankingSingleTaskData = OnlineBankingTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);

                            CsvSb = SummarizeOnlineBanking(OnlineBankingSingleTaskData, CsvSb);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error Saving OnlineBanking Data:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
                        }
                    }
                    saveToFile(CsvSb);

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error Saving OnlineBanking Data:\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
                }
            }
        }

        private System.Data.Objects.ObjectQuery<TaskData> GetTaskDatasQuery(tasksDataContainer tasksDataContainer)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<TasksMainMenu.TaskData> taskDatasQuery = tasksDataContainer.TaskDatas;
            // Returns an ObjectQuery.
            return taskDatasQuery;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            TasksMainMenu.tasksDataContainer tasksDataContainer = new TasksMainMenu.tasksDataContainer();
            // Load data into TaskDatas. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource taskDatasViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("taskDatasViewSource")));
            System.Data.Objects.ObjectQuery<TasksMainMenu.TaskData> taskDatasQuery = this.GetTaskDatasQuery(tasksDataContainer);
            taskDatasViewSource.Source = taskDatasQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private void exportPartSumm_Click(object sender, RoutedEventArgs e)
        {
            var taskDataList = context.TaskDatas;
            string text = subjectIDTextBox.Text;
            var partTotalData = taskDataList.Where(c => c.SubjectID == text);
            if (partTotalData.Count() == 0)
            {
                string noDataToSaveMsg = "There is no data to save for " + subjectIDTextBox.Text+ "! Aborting csv export. Press OK to return to the main window.";
                MessageBox.Show(noDataToSaveMsg);
            }
            else
            {
                StringBuilder CsvSb = new StringBuilder();
                CsvSb.Append("Summary for "+subjectIDTextBox.Text+"\n\n\n");
                try
                {
                    CsvSb.Append("ATM Summary\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, SubTask1_Time, SubTask2_Time, SubTask3_Time, Total_Correct, SubTask1_Correct, SubTask2_Correct, SubTask3_Correct, Total_Incorrect, SubTask1_Incorrect, SubTask2_Incorrect, SubTask3_Incorrect, Rate, Ratio\n");
                    var ATMTotalData = partTotalData.Where(c => c.TaskName == "ATM");
                    var ATMStarts = ATMTotalData.Where(c => c.EventType == "TaskStart");
                    var ATMCloses = ATMTotalData.Where(c => c.EventType == "TaskClosed");

                    for (int i = 0; i < ATMStarts.AsEnumerable().Count(); i++)
                    {
                        int start = ATMStarts.AsEnumerable().ElementAt(i).TaskDataId;
                        int end = ATMCloses.AsEnumerable().ElementAt(i).TaskDataId;
                        var ATMSingleTaskData = ATMTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                        CsvSb = SummarizeATM(ATMSingleTaskData, CsvSb);

                    }
                }
                catch
                {
                }
                try
                {
                    CsvSb.Append("\nForms Summary\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time\n");
                    var FormsTotalData = partTotalData.Where(c => c.TaskName == "Forms");
                    var FormsStarts = FormsTotalData.Where(c => c.EventType == "TaskStart");
                    var FormsCloses = FormsTotalData.Where(c => c.EventType == "TaskClosed");

                    for (int i = 0; i < FormsCloses.AsEnumerable().Count(); i++)
                    {
                        int start = FormsStarts.AsEnumerable().ElementAt(i).TaskDataId;
                        int end = FormsCloses.AsEnumerable().ElementAt(i).TaskDataId;
                        var FormsSingleTaskData = FormsTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);

                        TimeSpan formsTotalTime = FormsCloses.AsEnumerable().ElementAt(i).Time - FormsStarts.AsEnumerable().ElementAt(i).Time;
                        String Status = "Completed";
                        if (FormsSingleTaskData.Where(c => c.EventSummary == "ClickedNo").AsEnumerable().Count() != 0) Status = "Incomplete";

                        CsvSb.Append(FormsTotalData.AsEnumerable().ElementAt(0).SubjectID.ToString()); CsvSb.Append(", ");
                        CsvSb.Append(FormsStarts.AsEnumerable().ElementAt(i).Time.ToString()); CsvSb.Append(", ");
                        CsvSb.Append(Status); CsvSb.Append(", ");
                        CsvSb.Append(formsTotalTime.ToString()); CsvSb.Append("\n");
                        /*foreach (TaskData td in FormsSingleTaskData)
                        {
                            CsvSb.Append(TaskDataToCsvData(td));
                        } */
                    }
                }
                catch
                {
                }
                try
                {
                    //here
                    CsvSb.Append("\nPrescriptionRefill Summary\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, Total_Correct, Total_Error, Rate, Ratio\n");
                    var PresTotalData = partTotalData.Where(c => c.TaskName == "Prescription Refill");
                    var PresStarts = PresTotalData.Where(c => c.EventType == "TaskStart");
                    var PresCloses = PresTotalData.Where(c => c.EventType == "TaskClosed");

                    for (int i = 0; i < PresStarts.AsEnumerable().Count(); i++)
                    {
                        int start = PresStarts.AsEnumerable().ElementAt(i).TaskDataId;
                        int end = PresCloses.AsEnumerable().ElementAt(i).TaskDataId;
                        var PresSingleTaskData = PresTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                        CsvSb = SummarizePres(PresSingleTaskData, CsvSb);

                    }
                }
                catch
                {
                }
                try
                {
                    CsvSb.Append("\nDoctorTask Summary\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, SubTask1_Time, SubTask2_Time, SubTask3_Time, SubTask4_Time, Total_Correct, SubTask1_Correct, SubTask2_Correct, SubTask3_Correct, SubTask4_Correct, Total_Incorrect, SubTask1_Incorrect, SubTask2_Incorrect, SubTask3_Incorrect, SubTask4_Incorrect, Rate, Ratio\n");
                    var DocTotalData = partTotalData.Where(c => c.TaskName == "DoctorTest");
                    var DocStarts = DocTotalData.Where(c => c.EventType == "TaskStart");
                    var DocCloses = DocTotalData.Where(c => c.EventType == "TaskComplete");

                    for (int i = 0; i < DocStarts.AsEnumerable().Count(); i++)
                    {
                        int start = DocStarts.AsEnumerable().ElementAt(i).TaskDataId;
                        int end = DocCloses.AsEnumerable().ElementAt(i).TaskDataId;
                        var DocSingleTaskData = DocTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                        CsvSb = SummarizeDoc(DocSingleTaskData, CsvSb);
                    }
                }
                catch
                {

                }
                try
                {
                    CsvSb.Append("\nMetroTickets Summary\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, SubTask1_Time, SubTask2_Time, SubTask3_Time, Total_Correct, SubTask1_Correct, SubTask2_Correct, SubTask3_Correct, Total_Incorrect, SubTask1_Incorrect, SubTask2_Incorrect, SubTask3_Incorrect, Rate, Ratio\n");
                    var MetTotalData = partTotalData.Where(c => c.TaskName == "MetroTickets");
                    var MetStarts = MetTotalData.Where(c => c.EventType == "TaskStart");
                    var MetCloses = MetTotalData.Where(c => c.EventType == "TaskClosed");

                    for (int i = 0; i < MetStarts.AsEnumerable().Count(); i++)
                    {
                        int start = MetStarts.AsEnumerable().ElementAt(i).TaskDataId;
                        int end = MetCloses.AsEnumerable().ElementAt(i).TaskDataId;
                        var MetSingleTaskData = MetTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                        CsvSb = SummarizeMetroKiosk(MetSingleTaskData, CsvSb);
                    }
                }
                catch
                {

                }
                try
                {
                    CsvSb.Append("\nReaction Summary\n");
                    // CsvSb.Append("ParticipantID, Start_Date, Test, N_Correct, N_Incorrect, Mean, Median, SD, Outliers, adjCorrect, adjMean, adjMedian, adjSD\n");

                    CsvSb.Append("ParticipantID, Start_Time, Status, Test_Type, Mean, Median, Standard_Deviation, Outliers, Adjusted_Mean, Adjusted_Median, Adjusted_SD, Test_Type, N_Correct, N_Incorrect, Mean, Median, Standard_Deviation, Outliers, Adjusted_Mean, Adjusted_Median, Adjusted_SD\n");
                    var ReacTotalData = partTotalData.Where(c => c.TaskName == "Reaction Test");
                    var ReacStarts = ReacTotalData.Where(c => c.EventType == "TaskStarted");
                    var ReacCloses = ReacTotalData.Where(c => c.EventType == "TaskClosed");

                    for (int i = 0; i < ReacStarts.AsEnumerable().Count(); i++)
                    {
                        int start = ReacStarts.AsEnumerable().ElementAt(i).TaskDataId;
                        int end = ReacCloses.AsEnumerable().ElementAt(i).TaskDataId;
                        var ReacSingleTaskData = ReacTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                        CsvSb = SummarizeReac(ReacSingleTaskData, CsvSb);

                    }
                }
                catch
                {

                }
                try
                {
                    CsvSb.Append("\nOnlineBanking Summary\n");
                    CsvSb.Append("ParticipantID, Start_Time, Status, Total_Time, SubTask1_Time, SubTask2_Time, SubTask3_Time, SubTask4_Time, Total_Correct, SubTask1_Correct, SubTask2_Correct, SubTask3_Correct, SubTask4_Correct, Total_Incorrect, SubTask1_Incorrect, SubTask2_Incorrect, SubTask3_Incorrect, SubTask4_Incorrect, Rate, Ratio\n");
                    var OnlineBankingTotalData = partTotalData.Where(c => c.TaskName == "OnlineBanking");
                    var OnlineBankingStarts = OnlineBankingTotalData.Where(c => c.EventType == "TaskStart");
                    var OnlineBankingCloses = OnlineBankingTotalData.Where(c => c.EventType == "TaskClose");

                    for (int i = 0; i < OnlineBankingStarts.AsEnumerable().Count(); i++)
                    {
                        int start = OnlineBankingStarts.AsEnumerable().ElementAt(i).TaskDataId;
                        int end = OnlineBankingCloses.AsEnumerable().ElementAt(i).TaskDataId;
                        var OnlineBankingSingleTaskData = OnlineBankingTotalData.Where(c => c.TaskDataId >= start && c.TaskDataId <= end);
                        CsvSb = SummarizeOnlineBanking(OnlineBankingSingleTaskData, CsvSb);
                    }
                }
                catch
                {

                }
                saveToFile(CsvSb);
            }
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
            System.Diagnostics.Process.Start(@"Resources\FunctionalTasksSuiteInstallationInstruction_20161026.pdf");
        }

        //uppercases all letters
        private void textBoxID_TextChanged(object sender, TextChangedEventArgs e)
        {
            int currCaretIndex = textBoxID.CaretIndex;
     
            textBoxID.Text = textBoxID.Text.ToUpper(); //assign altered string back to textBoxID
            
            textBoxID.CaretIndex = currCaretIndex; //make sure caret is back at the end of the line
        }

        //
        //suppress non-alphanumeric characters
        private void textBoxID_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            if (rgx.IsMatch(e.Text))
            {
                Console.WriteLine("no good");
                e.Handled = true;
            }
        }

      
    }
}
