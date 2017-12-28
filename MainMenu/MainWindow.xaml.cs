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
using ATM;
using PrescriptionRefill;
using FormsTask;
//using WpfApplication1; //Package name for Doctor Test
using ReactionTest;

namespace MainMenu_bran
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        new MainMenuContainer context = new MainMenu_bran.MainMenuContainer();  // master object for interacting with entities
        int studyID, siteID, partIdForDel, partIdAssign, taskTypeID;
        string gender, taskTitle, status;
        short age;
        System.Windows.Data.CollectionViewSource participantsViewSource;        // used in LoadParticipants() to populate data grids
        LoginWindow login = new LoginWindow();                                  // WPF form used by assessor to log in
        Assesor logggedInAssessor;                                              // used for managing assessor's log-in sessions


        public MainWindow()   //Class constructor
        {
            InitializeComponent(); //This method creates and initializes all GUI elements. Not for human consumption - do not alter.
        }


        //Entry point of all application code for this GUI.
        private void Window_Loaded(object sender, RoutedEventArgs e)  
        {

            adminTab.IsEnabled = false;                                 //disable admin functions by default
            adminTab.Visibility = System.Windows.Visibility.Collapsed;  //collapse admin functions by default

            login.Show();                                               //ask for assessor to log in
            login.Closed += new EventHandler(loadParticipantsAndTaskTypes); //called once login info is entered and login window closes.
        }

        //load participant and task type data into MainWindow 
        void loadParticipantsAndTaskTypes(object sender, EventArgs e)    
        {
            var _login = (LoginWindow)sender;                           //cast sender object into something we can use
            this.logggedInAssessor = _login.loggedInAssesor;            //initialize loggedInAssesor with info from login window

            //load data for participants
            participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
            var participantsQuery = this.GetParticipantsQuery(context);
            var participants = context.Participants;
            var studies = context.Studies;
            participantsViewSource.Source = participantsQuery.ToList();
            // Load data into TaskTypes. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource taskTypesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("taskTypesViewSource")));
            System.Data.Objects.ObjectQuery<MainMenu_bran.TaskType> taskTypesQuery = this.GetTaskTypesQuery(context);
            taskTypesViewSource.Source = taskTypesQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
            //

            bool isAdmin = (this.logggedInAssessor.AdminPrivileges == true);     // enable admin functions
            if(isAdmin)                                                         
            {
                this.adminTab.Visibility = System.Windows.Visibility.Visible;
                this.adminTab.IsEnabled = true;
            }
        }

        //Auto-generated method to query Entity Framework + SQL Server for participants
        private System.Data.Objects.ObjectQuery<Participant> GetParticipantsQuery(MainMenuContainer mainMenuContainer)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<MainMenu_bran.Participant> participantsQuery = mainMenuContainer.Participants;
            // To explicitly load data, you may need to add Include methods like below:
            // participantsQuery = participantsQuery.Include("Participants.Assesor").
            // For more information, please see http://go.microsoft.com/fwlink/?LinkId=157380
            // Returns an ObjectQuery.
            // Update the query to include ParticipantExternalStudyAssociatives data in Participants. You can modify this code as needed.
            participantsQuery = participantsQuery.Include("ParticipantExternalStudyAssociatives");
            participantsQuery = participantsQuery.Include("TaskInstances");
            return participantsQuery;
        }

        //Saves a new Participant to COA_Functional.Participants
        private void addParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            getNewParticipantGender();
            getStudyId();
            Participant participant = new Participant();
            participant.Age = age;
            participant.Gender = gender;
            participant.EffectiveDate = DateTime.Now;
            try
            {
                participant.SiteId = siteID;
            }
            catch
            {
                MessageBox.Show("Must be a valid SiteID");
            }
            participant.StudyId = studyID;
            participant.AssesorId = 1;                              //Hard-coded value. TODO: add ability to assign participant's assessor id
            context.AddToParticipants(participant);
            context.SaveChanges();                                  //finalize changes to database
     
            //refreshes GUI's list of participants
            var query = this.GetParticipantsQuery(context);
            participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
            participantsViewSource.Source = query.ToList();
        }

        //validates user input for new participant age
        private void newParticipantAgeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                age = short.Parse(newParticipantAgeTextBox.Text);
            } catch  {
                MessageBox.Show("Must be an int");
            }
        }

    
        private void getNewParticipantGender()
        {
            gender = newParticipantGenderComboBox.SelectionBoxItem.ToString();
        }


        private void newParticipantStudyIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
          //      studyID = Int32.Parse(comboBox3.SelectionBoxItem.ToString()); <-- No idea why this is commented out right now
            }
            catch
            {
                //    MessageBox.Show(newParticipantStudyIdComboBox.Text); <-- trace statement for debugging
            }
        }

        private void getStudyId()
        {
            studyID = Int32.Parse(newParticipantStudyIdComboBox.SelectionBoxItem.ToString());
        }

        private void newParticipantSiteIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                siteID = Int32.Parse(newParticipantSiteIdTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Must be an int");
            }
        }

        //Delete a participant
        //The UI element for this handler cannot be found.
        //TODO: find it!
        private void button4_Click(object sender, RoutedEventArgs e)
        {
          
            try
            {   
                var deletions = from participants in context.Participants where participants.ParticipantId == partIdForDel select participants;
                foreach (var del in deletions)
                {
                   context.Participants.DeleteObject(del);
                }
            }
            catch
            {
                MessageBox.Show("Error deleting participant.");
            }
            var query = this.GetParticipantsQuery(context);
            participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
            participantsViewSource.Source = query.ToList();
            context.SaveChanges();
        }

        //Assign a task to a participant
        private void assignTaskButton_Click(object sender, RoutedEventArgs e)
        {
            getTaskDesc();
            status = "Incomplete";
            partIdAssign = Int32.Parse(participantIdTextBox.Text);
            studyID = Int32.Parse(studyIdTextBox.Text);
            getTaskTypeID();
            TaskInstance task = new TaskInstance();
            task.TaskTitle = taskTitle;
            task.Status = status;
            task.StartDate = DateTime.Now;
            task.ParticipantId = partIdAssign;
            task.StudyId = studyID;
            task.TaskTypeId = taskTypeID;
          
            context.AddToTaskInstances(task);
            context.SaveChanges();
            var query = this.GetParticipantsQuery(context);
            participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
            participantsViewSource.Source = query.ToList();
            
        }

        //Auto-generated method to query Entity Framework + SQL Server for task types
        private System.Data.Objects.ObjectQuery<TaskType> GetTaskTypesQuery(MainMenuContainer mainMenuContainer)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<MainMenu_bran.TaskType> taskTypesQuery = mainMenuContainer.TaskTypes;
            // Returns an ObjectQuery.
            return taskTypesQuery;
        }


        private void getTaskDesc()
        {
            TaskType task = new TaskType();
            task = ((TaskType)taskTypeDescComboBox.SelectedItem);
            taskTitle = task.TaskTypeDesc;
            taskTypeID = task.TaskTypeId;
            
        }

        // called in assignTaskButton_Click()
        private void getTaskTypeID()
        {
            //TODO: implement this method or delete it
        }

        private void participantIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           //TODO: implement this method or delete it
        }

        //Saves the data entered in Edit Pane
        private void editParticipantButton_Click(object sender, RoutedEventArgs e)
        {
            Participant part = ((Participant)participantsDataGrid.SelectedItem);
            try
            {
                part.Age = short.Parse(ageTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Please enter a valid age");
            }
            part.Gender = editParticipantGenderComboBox.SelectionBoxItem.ToString();
            try
            {
            ParticipantExternalStudyAssociative ext = ((ParticipantExternalStudyAssociative) extStudyParticipantIdComboBox.SelectedItem);
            ext.ExtStudyParticipantId = extStudyParticipantIdTextBox.Text;

            }
            catch
            {
                MessageBox.Show("No external Study");
            }
                context.SaveChanges();
                var query = this.GetParticipantsQuery(context);
                participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
                participantsViewSource.Source = query.ToList();
            
           
        }

        //Launch task based on selection in ASsign/Run pane
        //TODO: try a case/switch block instead of consecutive if/else
        private void executeTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskInstance task = (TaskInstance)taskInstancesDataGrid.SelectedItem;

            if (task.TaskTypeId == 1)           // Launch ATM task 
            {
                //atm
                //prescrip
                //forms
                CitiBankForm form = new CitiBankForm("");
                form.Show();
                //assuming CitiBankForm has method getTaskEventData()
                form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form_FormClosedATMSave);

            }
            else if (task.TaskTypeId == 2)      // Launch Prescription Refill task
            {
                PrescriptionRefill.Form1 form = new PrescriptionRefill.Form1();
                form.Show();
                form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form_FormClosedPrescriptionSave);


            }
            else if (task.TaskTypeId == 3)      // Launch Forms Task 
            {
                FormsTask.Form1 form = new FormsTask.Form1();
                form.Show();
                form.FormClosed += new System.Windows.Forms.FormClosedEventHandler(form_FormClosedFormSave);
            }
            else if (task.TaskTypeId == 4)      // Launch Doctor's Test
            {
                //WpfApplication1.MainWindow form = new WpfApplication1.MainWindow();
              
                //form.Show();
                //form.Closed += new EventHandler(doctorTest_Closed);
                
            }
            else if (task.TaskTypeId == 5)       // Launch Reaction Test
            {
                ReactionTest.ReactionTestWindow reactionTest = new ReactionTestWindow();
                reactionTest.Show();
                reactionTest.Closed += new EventHandler(reactionTest_Closed);
            }
        }


         /*
         * The two methods below handle saving event data from the ATM and Forms tasks.
         */

        //
        void form_FormClosedATMSave(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            CitiBankForm form_ref = (CitiBankForm)sender;
            //do stuff
            TaskInstance task = (TaskInstance)taskInstancesDataGrid.SelectedItem;
            //throw new NotImplementedException();
            //do stuff
            List<Dictionary<String, String>> li = form_ref.getEventData();
            foreach (Dictionary<String, String> d in li)
            {
                TaskData taskData = new TaskData();
                //taskData.Time = TimeSpan.Parse(d["time"]);
                taskData.Time = DateTime.Parse(d["time"]);
                taskData.EventType = d["eventType"];
                taskData.EventData = d["eventData"];
                taskData.EventSummary = d["eventSummary"];
                taskData.SiteId = 1;
                taskData.TaskInstanceId = task.TaskInstanceId;
                context.AddToTaskDatas(taskData);



            }
            context.SaveChanges();
            var query = this.GetParticipantsQuery(context);
            participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
            participantsViewSource.Source = query.ToList();

            //do entity saving stuff
        }


        void form_FormClosedFormSave(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            FormsTask.Form1 form_ref = (FormsTask.Form1)sender;
            //do stuff
            TaskInstance task = (TaskInstance)taskInstancesDataGrid.SelectedItem;
            //throw new NotImplementedException();
            //do stuff
            // commented out by Rick on 1/14/2013
            List<Dictionary<String, String>> li = form_ref.getEventData();
            foreach (Dictionary<String, String> d in li)
            {
                TaskData taskData = new TaskData();
                //taskData.Time = TimeSpan.Parse(d["time"]);
                taskData.Time = DateTime.Parse(d["time"]);
                taskData.EventType = d["eventType"];
                taskData.EventData = d["eventData"];
                taskData.EventSummary = d["eventSummary"];
                taskData.SiteId = 1;
                taskData.TaskInstanceId = task.TaskInstanceId;
                context.AddToTaskDatas(taskData);



            }
            context.SaveChanges();
            var query = this.GetParticipantsQuery(context);
            participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
            participantsViewSource.Source = query.ToList();

            //do entity saving stuff
             
        }

        void form_FormClosedPrescriptionSave(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            PrescriptionRefill.Form1 form_ref = (PrescriptionRefill.Form1)sender;
            //do stuff
            TaskInstance task = (TaskInstance)taskInstancesDataGrid.SelectedItem;
            //throw new NotImplementedException();
            //do stuff
            List<Dictionary<String, String>> li = form_ref.getEventData();
            foreach (Dictionary<String, String> d in li)
            {
                TaskData taskData = new TaskData();
                //taskData.Time = TimeSpan.Parse(d["time"]);
                taskData.Time = DateTime.Parse(d["time"]);
                taskData.EventType = d["eventType"];
                taskData.EventData = d["eventData"];
                taskData.EventSummary = d["eventSummary"];
                taskData.SiteId = 1;
                taskData.TaskInstanceId = task.TaskInstanceId;
                context.AddToTaskDatas(taskData);



            }
            context.SaveChanges();
            var query = this.GetParticipantsQuery(context);
            participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
            participantsViewSource.Source = query.ToList();

            //do entity saving stuff
        }

        void doctorTest_Closed(object sender, EventArgs e)
        {
            //WpfApplication1.MainWindow form_ref = (WpfApplication1.MainWindow)sender;
            //do stuff
            TaskInstance task = (TaskInstance)taskInstancesDataGrid.SelectedItem;
            //throw new NotImplementedException();
            //do stuff
            /**List<Dictionary<String, String>> li = form_ref.getEventData();
            foreach (Dictionary<String, String> d in li)
            {
                TaskData taskData = new TaskData();
                //taskData.Time = TimeSpan.Parse(d["time"]);
                taskData.Time = DateTime.Parse(d["time"]);
                taskData.EventType = d["eventType"];
                taskData.EventData = d["eventData"];
                taskData.EventSummary = d["eventSummary"];
                taskData.SiteId = 1;
                taskData.TaskInstanceId = task.TaskInstanceId;
                context.AddToTaskDatas(taskData);

            */

           // }
            context.SaveChanges();
            var query = this.GetParticipantsQuery(context);
            participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
            participantsViewSource.Source = query.ToList();
        }

        void reactionTest_Closed(object sender, EventArgs  e)
        {
            ReactionTest.ReactionTestWindow reactiontest = (ReactionTestWindow)sender;
            //do stuff
            TaskInstance task = (TaskInstance)taskInstancesDataGrid.SelectedItem;
            //throw new NotImplementedException();
            //do stuff
            List<Dictionary<String, String>> li = reactiontest.getEventData();
            foreach (Dictionary<String, String> d in li)
            {
                TaskData taskData = new TaskData();
                //taskData.Time = TimeSpan.Parse(d["time"]);
                taskData.Time = DateTime.Parse(d["time"]);
                taskData.EventType = d["eventType"];
                taskData.EventData = d["eventData"];
                taskData.EventSummary = d["eventSummary"];
                taskData.SiteId = 1;
                taskData.TaskInstanceId = task.TaskInstanceId;
                context.AddToTaskDatas(taskData);



            }
            context.SaveChanges();
            var query = this.GetParticipantsQuery(context);
            participantsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("participantsViewSource")));
            participantsViewSource.Source = query.ToList();

            //do entity saving stuff
        }
    }
}
