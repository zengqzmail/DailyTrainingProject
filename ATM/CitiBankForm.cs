using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Media;
using System.Threading;
using System.Timers;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Threading;


namespace ATM
{
    public partial class CitiBankForm : Form
    {
        private List<Dictionary<String, String>> taskDataList;
        private string participantID;
        private User user;
        private bool fromChecking;
        private bool fromSaving;
        private bool setPIN;
        private List<User> userlist;
        private Log log;
        private const String RESOURCE_DIRECTORY = "Resources/";
        private const String BUTTON_SOUND = RESOURCE_DIRECTORY + "buttonSound.wav";
        private const String PANNEL_SOUND = RESOURCE_DIRECTORY + "panelSound.wav";
        int distanceX = 70;
        int distanceY = 60;
        Point startingPoint;
        int subtask = 0;
        int errors = 0;
        int attempt = 0;
        Form1 form = new Form1();
        Form2 form2 = new Form2();
        int exitKeysCount = 0;
        bool shown, skip, open;
        string taskLanguage = "EN"; //english as default
        string currentSubtaskLanguage = "EN";//english as default
        System.Timers.Timer myTimer1;
        int myTimer1_Interval = 300 * 1000; //5 minutes
        int accountInfoDisplay_Interval = 8 * 1000; // 8 sec
        System.Timers.Timer myTimer2, myTimer3;

       

        string[] subtasksEn = {
                                  "Please use the ATM below to check the balance in your checking account.\nYour Account PIN is 1234.",
                                  "Please use the ATM below to transfer 50 dollars from your savings account\n to your checking account. Your Account PIN is 1234.",
                                  "Please use the ATM below to withdraw 100 dollars from your checking account.\n Your Account PIN is 1234.",
                                  ""
                              };
        string[] subtasksSp ={
                                 "Por favor, use el cajero automático para verificar el saldo de\n su cuenta de cheques. El PIN de cuenta es 1234.",
                                 "Por favor, use el cajero automático para transferir 50 dólares de\n su cuenta de ahorros a su cuenta de cheques. El PIN de cuenta es 1234.",
                                 "Por favor, use el cajero automático para retirar 100 dólares de\n su cuenta de cheques. El PIN de cuenta es 1234.",
                                 ""
                             };
    


        public CitiBankForm(string participantID)
        {

            //added by Rick on Jan 25th
            Application.EnableVisualStyles();
            //
            InitializeComponent();
            open = true;

            // attempt to scale the program to ThinkPad screen resolution - 1366x768
            //
            /////////////////////////////////
            
            this.Scale(1.0f, 0.9f);
            this.tableLayoutPanel1.Scale(1.0f, 0.9f);
            for (int i = 0; i < this.tableLayoutPanel1.Controls.Count; i++)
            {
                Control c = this.tableLayoutPanel1.Controls[i];
                c.Scale(1.0f, 0.9f);
                //need to scale font, too, if oftype button
                if (c is Button)
                {
                    c.Font = new Font(c.Font.FontFamily, c.Font.Size/0.9f);
                }
                Console.WriteLine(c.Name);
            }
            //scale the font, too
            GetSelfAndChildrenRecursive(this).OfType<Button>().ToList()
                .ForEach(b => b.Font = new Font(b.Font.FontFamily, b.Font.Size / 0.9f));
             
            // the above code works; delete at your own risk
            //
            ////////////////////////////////

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            
            this.participantID = participantID;
            userlist = IO.loadUserlist("usersData.txt");
            this.user = userlist[0];
            log = new Log(user);
            hideMenuButtons();
            startingPoint = new Point(distanceX, distanceY);
            timer1.Start();
          
             this.KeyPreview = true;
             this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.Shown += new EventHandler(CitiBankForm_Shown);
            

            //initialize taskDataList
            taskDataList = new List<Dictionary<String, String>>();
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "TaskStart");
            dictionary.Add("eventData", "");
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);
            //
            
            //make sure that timer threads die
            this.FormClosed += new FormClosedEventHandler(CitiBankForm_FormClosed);
            
            
        }

        public CitiBankForm(string participantID, string taskLanguage)
            : this(participantID)
        {
            this.taskLanguage = taskLanguage;
            this.currentSubtaskLanguage = taskLanguage;
            //
            if (this.taskLanguage.Equals("ES"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
            }
            else if (this.taskLanguage.Equals("EN"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            }
            //
            initLocalizedStrings();
        }

        public void CitiBankFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == System.Windows.Forms.CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Closing called, are you sure you want to close it?", "Closing", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Dictionary<String, String> dict1 = new Dictionary<String, String>();
                    dict1.Add("time", DateTime.Now.ToString());
                    dict1.Add("eventType", "TaskClosedByUser");
                    dict1.Add("eventData", "");
                    dict1.Add("eventSummary", "");
                    taskDataList.Add(dict1);
                    Dictionary<String, String> dict2 = new Dictionary<String, String>();
                    dict2.Add("time", DateTime.Now.ToString());
                    dict2.Add("eventType", "TaskClosed");
                    dict2.Add("eventData", "");
                    dict2.Add("eventSummary", "");
                    taskDataList.Add(dict2);
                }
                else if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void initLocalizedStrings()
        {
            cancelButton.Text = ATMStrings.Cancel;
            buttonCash.Text = ATMStrings.Cash;
            buttonNumberClear.Text = ATMStrings.NumPadClear;
            buttonKeyClear.Text = ATMStrings.NumPadClear;
            buttonNumberEnter.Text = ATMStrings.NumPadEnter;
            buttonEnter.Text = ATMStrings.NumPadEnter;
            buttonExit.Text = ATMStrings.Exit;
            buttonExitTask.Text = ATMStrings.ExitTaskButton;
            labelLanguage.Text = "Please select a language\nPor favor seleccione un idioma";//ATMStrings.PleaseSelectLanguage;
            buttonAccountInfo.Text = ATMStrings.AcctInformation;
            buttonDeposit.Text = ATMStrings.DepositButton;
            buttonTransfer.Text = ATMStrings.TransferOrPayment;
            buttonPreference.Text = ATMStrings.PreferenceButton;
            buttonExit.Text = ATMStrings.Exit;
            buttonExitTask.Text = ATMStrings.ExitTaskButton;
            buttonTakeBills.Text = ATMStrings.PleaseTakeBills;
            labelPINRequest.Text = ATMStrings.PleaseEnterPIN;
            labelMainMenue.Text = ATMStrings.PleaseMakeSelection + "\r\n\r\n";
            buttonAccountSummary.Text = ATMStrings.SeeSummaryAccts;
            buttonAccountDetail.Text = ATMStrings.SeeAcctDetails;
            labelTransfer.Text = ATMStrings.WhatWouldYouLikeToDo;
            buttonTransferMyAccount.Text = ATMStrings.TransferBetweenMyAccts;
            buttonTransferOtherAccount.Text = ATMStrings.TransferToOtherAcct;
            label20.Text = ATMStrings.WhereToTransferFrom;
            button1.Text = ATMStrings.CheckingToSavings;
            button4.Text = ATMStrings.SavingsToChecking;
            labelAccountSelection.Text = ATMStrings.WhereToWithdrawMoney;
            label9.Text = ATMStrings.OnlyHave10and20 + "\r\n\r\n" + ATMStrings.HowMuchCashToWithdraw;
            //
            radioButtonCheckingToSaving.Text = ATMStrings.CheckingToSavings;
            radioButtonSavingToChecking.Text = ATMStrings.SavingsToChecking;
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (exitKeysCount == 0 && e.KeyCode == Keys.ControlKey)
            {
                exitKeysCount = 1;
            }
            else if (exitKeysCount == 1 && e.KeyCode == Keys.ShiftKey)
            {
                exitKeysCount = 2;
            }
            else if (exitKeysCount == 2 && e.KeyCode == Keys.X)
            {
                myTimer1.Stop();
                myTimer1.Dispose();
                open = false;
                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                dictionary9.Add("time", DateTime.Now.ToString());
                dictionary9.Add("eventType", "SubTaskCompleted");
                dictionary9.Add("eventData", (subtask + 1).ToString());
                dictionary9.Add("eventSummary", "EscapeEntered");
                taskDataList.Add(dictionary9);
                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "EscapeEntered");
                dictionary1.Add("eventData", "");
                dictionary1.Add("eventSummary", "");
                taskDataList.Add(dictionary1);
                this.Close();
                Application.Exit();
                return;
            }
            else
            {
                exitKeysCount = 0;
            }
        }

        void CitiBankForm_FormClosed(object sender, FormClosedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("killing all threads from ATM...");
#endif
            if (myTimer1 != null)
            {
                myTimer1.Stop();
                myTimer1.Dispose(); //one more time....
            }
            //Environment.Exit(0); 
        }

        //the purpose of the Cancel button is to take the user back to the beginning of the subtask
        private void cancelButton_Click(object sender, EventArgs e)
        {
            //make sure it's in the initially selected language...
            if (this.taskLanguage.Equals("ES"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
            }
            else if (this.taskLanguage.Equals("EN"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            }

            //TODO: maybe throw a dialog box asking to confirm cancel
            Form2 confirmCancelForm = new Form2();
            confirmCancelForm.ShowBox(ATMStrings.ConfirmCancelSubtaskMessage, "NO", "OK");
            //
            if (confirmCancelForm.yesClick)
            {
                restartSubtask(subtask);
            }
        }

        /*
         * takes ATM task back to beginning of current subtask
         * @param int subtask, using this.subtask
         */
        private void restartSubtask(int subtask)
        {

            //

            Dictionary<String, String> taskDataDict = new Dictionary<String, String>();
            taskDataDict.Add("time", DateTime.Now.ToString());
            taskDataDict.Add("eventType", "Error");
            taskDataDict.Add("eventData", "Subtask #" + (subtask + 1).ToString());
            taskDataDict.Add("eventSummary", "SubtaskRestart");
            taskDataList.Add(taskDataDict);

            colorTabControl1.SelectedTab = tabLanguage;
        }
       
        //
        public IEnumerable<Control> GetSelfAndChildrenRecursive(Control parent)
        {
            List<Control> controls = new List<Control>();

            foreach (Control child in parent.Controls)
            {
                controls.AddRange(GetSelfAndChildrenRecursive(child));
            }

            controls.Add(parent);

            return controls;
        }
        //
        void CitiBankForm_Shown(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
            showMessage();

        }

        //general method to handle user errors
        //what it does:
        //increments this.errors
        //if errors == 3, prompt for next task or prompt for exit (depending on subtask)
        //else
        //written by rblanco2 on Nov 8 2013
        private void handleATMUserError()
        {
            errors++;

            Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
            dictionary2.Add("time", DateTime.Now.ToString());
            dictionary2.Add("eventType", "WithdrawFromSavings");
            dictionary2.Add("eventData", (subtask + 1).ToString());
            dictionary2.Add("eventSummary", "Incorrect");
            taskDataList.Add(dictionary2);

            if (errors == 3) //max errors reached, either move to the next subtask or exit ATM task
            {
                Dictionary<String, String> taskDataDict = new Dictionary<String, String>();
                taskDataDict.Add("time", DateTime.Now.ToString());
                taskDataDict.Add("eventType", "MaxErrorsReached");
                taskDataDict.Add("eventData", (subtask + 1).ToString());
                taskDataDict.Add("eventSummary", "");
                taskDataList.Add(taskDataDict);

                labelInstruction.Text = "";
                labelInstructionEs.Text = "";
                colorTabControl1.SelectedTab = tabLanguage;
                hideMenuButtons();

                if (subtask == 2)
                {
                    form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventData", "");
                    dictionary1.Add("eventSummary", "");
                    taskDataList.Add(dictionary1);
                    myTimer1.Stop();
                    myTimer1.Dispose();
                    open = false;
                    this.Close();
                    Application.Exit();
                }
                else
                {
                    subtask++;
                    form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                    errors = 0;
                    showMessage(); //<-- least descriptive method name in the UNIVERSE
                }
            }
            else //remind user of subtask objectives
            {
                String taskReminderString = "";
                switch (subtask)
                {
                    case 0:
                        taskReminderString = ATMStrings.CheckBalanceReminder;
                        break;
                    case 1:
                        taskReminderString = ATMStrings.Transfer50DollarsReminder;
                        break;
                    case 2:
                        taskReminderString = ATMStrings.WithdrawCashReminder;
                        break;
                }
                form.ShowBox(taskReminderString, ATMStrings.Continue);
            }
        }

        private void showMessage(){
            
            if (this.taskLanguage == "EN"){
                form.ShowBox(subtasksEn[subtask], ATMStrings.Continue);
                
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "SubTaskStart");
                dictionary.Add("eventData", (subtask+1).ToString());
                dictionary.Add("eventSummary", "");
                taskDataList.Add(dictionary);
                labelInstruction.Text = subtasksEn[subtask];
                labelInstructionEs.Text = subtasksSp[3]; //this is an empty string.
                myTimer1 = new System.Timers.Timer(myTimer1_Interval); //5 minutes 
                myTimer1.Elapsed += new ElapsedEventHandler(ProcessTimerEvent);
                myTimer1.Start();
                myTimer1.AutoReset = false;
            }else{
                form.ShowBox(subtasksSp[subtask], ATMStrings.Continue);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "SubTaskStart");
                dictionary.Add("eventData", (subtask + 1).ToString());
                dictionary.Add("eventSummary", "");
                taskDataList.Add(dictionary);
                labelInstruction.Text = subtasksEn[3]; //this is an empty string.
                labelInstructionEs.Text = subtasksSp[subtask];
                myTimer1 = new System.Timers.Timer(myTimer1_Interval); 
                myTimer1.Elapsed += new ElapsedEventHandler(ProcessTimerEvent);
                myTimer1.Start();
                myTimer1.AutoReset = false;
            }
            
        }


        private void hideMenuButtons()
        {
            buttonCash.Hide();
            buttonAccountInfo.Hide();
            buttonDeposit.Hide();
            buttonTransfer.Hide();
            buttonPreference.Hide();
            buttonExit.Hide();
        }

        private void showMenuButtons()
        {
            buttonCash.Show();
            buttonAccountInfo.Show();
            buttonDeposit.Show();
            buttonTransfer.Show();
            buttonPreference.Show();
        }

        int billHoldTime = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (buttonTakeBills.Visible == true)
            {
                billHoldTime += 1;
                if (billHoldTime == 20)
                    inputBill();
            }

        }

        

        private void ProcessTimer2(Object obj, ElapsedEventArgs e)
        {           
            showMessage();
        }

        //
        //this gets called when myTimer1 elapses
        //this (usually means) that the user has used their allotted time for the subtask
        private  void ProcessTimerEvent(Object obj, ElapsedEventArgs e)
        {
            //this is done to ensure that form2 displays in the correct language.
            //TODO: find out why this is necessary
            if (this.currentSubtaskLanguage.Equals("ES"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
            }
            else if (this.currentSubtaskLanguage.Equals("EN"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            }
            //
            //
            if (open == true)
            {
                skip = false;
                myTimer2 = new System.Timers.Timer(60 * 1000); //one minute
                myTimer2.Elapsed += new ElapsedEventHandler(ProcessTimerClose);
                myTimer2.Start();
                myTimer2.AutoReset = false;
                shown = true;
                form2.ShowBox(ATMStrings.PromptForMoreTime, ATMStrings.No, ATMStrings.Yes);
                shown = false;
                if (skip == true)
                {
                    return;
                }
                if (form2.yesClick == true)
                {
                    myTimer1 = new System.Timers.Timer(myTimer1_Interval);
                    myTimer1.Elapsed += new ElapsedEventHandler(ProcessTimerEvent);
                    myTimer1.Start();
                    myTimer1.AutoReset = false;
                    Dictionary<String, String> question1 = new Dictionary<String, String>();
                    question1.Add("time", DateTime.Now.ToString());
                    question1.Add("eventType", "ExtraTime");
                    question1.Add("eventData", "");
                    question1.Add("eventSummary", "");
                    taskDataList.Add(question1);
                }
                else
                {
                    
                    if (skip == false)
                    {
                        if (subtask == 2)
                        {
                            form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);

                            Dictionary<String, String> dic1 = new Dictionary<String, String>();
                            dic1.Add("time", DateTime.Now.ToString());
                            dic1.Add("eventType", "SubTaskSkip");
                            dic1.Add("eventData", (subtask).ToString());
                            dic1.Add("eventSummary", "ClickedNo");
                            taskDataList.Add(dic1);

                            Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                            dictionary1.Add("time", DateTime.Now.ToString());
                            dictionary1.Add("eventType", "TaskClosed");
                            dictionary1.Add("eventData", "");
                            dictionary1.Add("eventSummary", "");
                            taskDataList.Add(dictionary1);
                            myTimer1.Stop();
                            myTimer1.Dispose();
                            open = false;
                            this.Close();
                            Application.Exit();
                            this.Close();
                            Application.Exit();
                            return;

                        }
                        subtask++;
                        //
                        try
                        {
                            labelInstruction.Text = "";
                            labelInstructionEs.Text = "";
                            //
                            
                            colorTabControl1.SelectedTab = tabLanguage;
                        }
                        catch (Exception ex)
                        {
                            //
                        }
                        
                        //
                        hideMenuButtons();

                        form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                        

                        errors = 0;
                        Dictionary<String, String> question1 = new Dictionary<String, String>();
                        question1.Add("time", DateTime.Now.ToString());
                        question1.Add("eventType", "SubTaskSkip");
                        question1.Add("eventData", (subtask).ToString());
                        question1.Add("eventSummary", "ClickedNo");
                        taskDataList.Add(question1);

                        showMessage();
                    }
                }

                myTimer2.Stop();
            }
        }

        private void ProcessTimerClose(Object obj, ElapsedEventArgs e)
        {
            skip = true;
            form2.Close();
            if (shown == true)
            {
               
                if (subtask == 2)
                {
                    form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);

                    Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                    dictionary2.Add("time", DateTime.Now.ToString());
                    dictionary2.Add("eventType", "SubTaskSkip");
                    dictionary2.Add("eventData", (subtask).ToString());
                    dictionary2.Add("eventSummary", "TimerExpired");
                    taskDataList.Add(dictionary2);

                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventData", "");
                    dictionary1.Add("eventSummary", "TimerExpired");
                    taskDataList.Add(dictionary1);
                    myTimer1.Stop();
                    myTimer1.Dispose();
                    open = false;
                    this.Close();
                    Application.Exit();
                    this.Close();
                    Application.Exit();
                    return;

                }
                subtask++;
                labelInstruction.Text = "";
                labelInstructionEs.Text = "";

                colorTabControl1.SelectedTab = tabLanguage;
                hideMenuButtons();

                form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                showMessage();

                errors = 0;
                Dictionary<String, String> question1 = new Dictionary<String, String>();
                question1.Add("time", DateTime.Now.ToString());
                question1.Add("eventType", "SubTaskSkip");
                question1.Add("eventData", (subtask ).ToString());
                question1.Add("eventSummary", "TimerExpired");
                taskDataList.Add(question1);
                myTimer2.Stop();
            }
            myTimer2.Stop();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            setSubtaskLanguage("EN");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            setSubtaskLanguage("ES");
        }

        private void setSubtaskLanguage(string selectedLanguage)
        {
#if DEBUG
            Console.WriteLine("setting subtask language to " + selectedLanguage);
#endif
            
            //do we even need this logging?
            if(selectedLanguage.Equals("EN"))
            {
                log.addAction("select English as the user's language", Action.BehaviorType.normal);
            }
            else if(selectedLanguage.Equals("ES"))
            {
                log.addAction("select Spanish as the user's language", Action.BehaviorType.normal);
            }
            

            //instructions from Sank:
            /*
             * 1. keep top instructions in initially selected language
             * 2. change subtask instructions to currently selected language.
             * how do we do this?
             * 1. create a new instance variable currentSubtaskLanguage
             * 2. change CurrentUICulture according to currentSubtaskLanguage
             * 
             * ShowMessage() (lol @ the name, refactor later) takes care of keeping the top label instructions in the 
             * initially selected language according to this.taskLanguage
             */
            this.currentSubtaskLanguage = selectedLanguage;
            if (this.currentSubtaskLanguage.Equals("ES"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
            }
            else if (this.currentSubtaskLanguage.Equals("EN"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            }

            errors = 0;     //reset number of user errors
            initLocalizedStrings(); //reset localized strings to subtask language
            threadPlayButtonSound(); //play beep
            colorTabControl1.SelectedTab = tabPin; //move to PIN entry screen
            
            //
            //write it all to taskDataList
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "LanguageSelection");
            if (selectedLanguage.Equals("EN"))
            {
                dictionary.Add("eventData", "English");
            }
            else if (selectedLanguage.Equals("ES"))
            {
                dictionary.Add("eventData", "Spanish");
            }
            
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            textBoxPIN.Text = "";
            log.addAction("clear PIN number", Action.BehaviorType.normal);
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "ClearClick");
            dictionary.Add("eventData", "ClearPin");
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBoxPIN.Text.Length > 4)
            {
                showInformation(ATMStrings.InvalidPIN, tabPin);
                textBoxPIN.Text = "";
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "PinTooLong");
                dictionary.Add("eventData", "");
                dictionary.Add("eventSummary", "Incorrect");
                taskDataList.Add(dictionary);
            }
        }

        private void buttonCash_Click(object sender, EventArgs e)
        {
            if (subtask == 0 || subtask == 1)
            {
              
                errors++;
                if (subtask == 0)
                {
                    form.ShowBox(ATMStrings.CheckBalanceReminder, ATMStrings.Continue);
                    
                   Dictionary<String, String> dictionary = new Dictionary<String, String>();
                   dictionary.Add("time", DateTime.Now.ToString());
                   dictionary.Add("eventType", "CashButtonClick");
                   dictionary.Add("eventData", "1");
                   dictionary.Add("eventSummary", "Incorrect");
                   taskDataList.Add(dictionary);
                }
                else if (subtask == 1)
                {
                    form.ShowBox(ATMStrings.Transfer50DollarsReminder, ATMStrings.Continue);
                    
                     Dictionary<String, String> dictionary = new Dictionary<String, String>();
                     dictionary.Add("time", DateTime.Now.ToString());
                     dictionary.Add("eventType", "CashButtonClick");
                     dictionary.Add("eventData", "2");
                     dictionary.Add("eventSummary", "Incorrect");
                     taskDataList.Add(dictionary);
                }

                
                if (errors == 3)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);
                    subtask++;
                    labelInstruction.Text = "";
                    labelInstructionEs.Text = "";
                    colorTabControl1.SelectedTab = tabLanguage;
                    hideMenuButtons();
                    form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                    
                    errors = 0;
                    showMessage();
                   

                }
                return;
            }
            threadPlayButtonSound();
           

            if (colorTabControl1.SelectedTab != tabCash && colorTabControl1.SelectedTab != tabAmount)
            {
                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "CashButtonClick");
                dictionary1.Add("eventData", "3");
                dictionary1.Add("eventSummary", "Correct");
                taskDataList.Add(dictionary1);
                errors = 0;
                log.addAction("click Cash tab", Action.BehaviorType.normal);
            }

            colorTabControl1.SelectedTab = tabCash;

        }

        private void buttonAccountInfo_Click(object sender, EventArgs e)
        {
            if (subtask == 1 || subtask == 2)
            {
                errors++;
                
                if (subtask == 1)
                {
                    form.ShowBox(ATMStrings.Transfer50DollarsReminder, ATMStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountInfoButton");
                    dictionary.Add("eventData", "2");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                    
                }
                else if (subtask == 2)
                {
                    form.ShowBox(ATMStrings.WithdrawCashReminder, ATMStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "AccountInfoButton");
                    dictionary.Add("eventData", "3");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                }

                if (errors == 3 && subtask == 2)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);

                    form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                    
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventData", "");
                    dictionary1.Add("eventSummary", "");
                    taskDataList.Add(dictionary1);
                    myTimer1.Stop();
                    myTimer1.Dispose();
                    open = false;
                    this.Close();
                    Application.Exit();
                    return;
                }
                if (errors == 3)
                {
                    subtask++;
                    labelInstruction.Text = "";
                    labelInstructionEs.Text = "";
                    colorTabControl1.SelectedTab = tabLanguage;
                    hideMenuButtons();
                    form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                    errors = 0;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);
                    showMessage();

                }
                return;

            }
            threadPlayButtonSound();

            

            if (colorTabControl1.SelectedTab != tabAccountInfo)
            {
                Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                dictionary2.Add("time", DateTime.Now.ToString());
                dictionary2.Add("eventType", "AccountInfoButton");
                dictionary2.Add("eventData", "1");
                dictionary2.Add("eventSummary", "Correct");
                taskDataList.Add(dictionary2);
                log.addAction("click Account Info tab", Action.BehaviorType.normal);
                errors = 0;
            }
            colorTabControl1.SelectedTab = tabAccountInfo;
        }

        
        private void buttonDeposit_Click(object sender, EventArgs e)
        {
            if (subtask == 0 || subtask == 1 || subtask ==2)
            {
                
                errors++;
                if (subtask == 0)
                {
                    form.ShowBox(ATMStrings.CheckBalanceReminder, ATMStrings.Continue);
                    
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "DepositButtonClick");
                    dictionary.Add("eventData", "1");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                }
                else if (subtask == 1)
                {
                    form.ShowBox(ATMStrings.Transfer50DollarsReminder, ATMStrings.Continue);
                   
                     Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "DepositButtonClick");
                    dictionary.Add("eventData", "2");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                
                }
                else if (subtask == 2)
                {
                    form.ShowBox(ATMStrings.WithdrawCashReminder, ATMStrings.Continue);
                   
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "DepositButtonClick");
                    dictionary.Add("eventData", "3");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                }

                if (errors == 3 && subtask == 2)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);
                    form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                    
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventData", "");
                    dictionary1.Add("eventSummary", "");
                    taskDataList.Add(dictionary1);
                    myTimer1.Stop();
                    myTimer1.Dispose();
                    open = false;
                    this.Close();
                    Application.Exit();
                    return;
                }
                if (errors == 3)
                {
                    subtask++;
                    labelInstruction.Text = "";
                    labelInstructionEs.Text = "";
                    colorTabControl1.SelectedTab = tabLanguage;
                    hideMenuButtons();
                    form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);
                    
                    errors = 0;
                    showMessage();
                }
                return;
            }
            threadPlayButtonSound();
            colorTabControl1.SelectedTab = tabDeposit;
            //this should never happen
            Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
            dictionary2.Add("time", DateTime.Now.ToString());
            dictionary2.Add("eventType", "DepositButtonClick");
            dictionary2.Add("eventData", "");
            dictionary2.Add("eventSummary", "Correct");
            taskDataList.Add(dictionary2);
            log.addAction("click Deposit tab", Action.BehaviorType.normal);
        }
        
        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            if (subtask == 0 || subtask == 2)
            {
                errors++;
                if (subtask == 0)
                {
                    form.ShowBox(ATMStrings.CheckBalanceReminder, ATMStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "TransferButtonClick");
                    dictionary.Add("eventData", "1");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                }
                else if (subtask == 2)
                {
                    form.ShowBox(ATMStrings.WithdrawCashReminder, ATMStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "TransferButtonClick");
                    dictionary.Add("eventData", "3");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                }
                if (errors == 3 && subtask == 2)
                {
                    /*Dictionary<String, String> dictionary7 = new Dictionary<String, String>();
                    dictionary7.Add("time", DateTime.Now.ToString());
                    dictionary7.Add("eventType", "TransferButtonClick");
                    dictionary7.Add("eventData", "1");
                    dictionary7.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary7);*/
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);
                    form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventData", "");
                    dictionary1.Add("eventSummary", "");
                    taskDataList.Add(dictionary1);
                    myTimer1.Stop();
                    myTimer1.Dispose();
                    open = false;
                    this.Close();
                    Application.Exit();
                    return;
                }
                if (errors == 3)
                {
                    subtask++;
                    labelInstruction.Text = "";
                    labelInstructionEs.Text = "";
                    colorTabControl1.SelectedTab = tabLanguage;
                    hideMenuButtons();
                    form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                    errors = 0;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);
                    showMessage();
                }
                return;
            }
            threadPlayButtonSound();
            
            
            if (colorTabControl1.SelectedTab != tabTransfer && colorTabControl1.SelectedTab != tabBetweenMyAccounts)
            {

                Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                dictionary2.Add("time", DateTime.Now.ToString());
                dictionary2.Add("eventType", "TransferButtonClick");
                dictionary2.Add("eventData", "2");
                dictionary2.Add("eventSummary", "Correct");
                taskDataList.Add(dictionary2);
                errors = 0;
                colorTabControl1.SelectedTab = tabTransfer;
            }
            //colorTabControl1.SelectedTab = tabTransfer;
            log.addAction("click Transfer tab", Action.BehaviorType.normal);
        }

        private void buttonPreference_Click(object sender, EventArgs e)
        {
            if (subtask == 0 || subtask == 1 || subtask == 2)
            {
                
                errors++;

                if (subtask == 0)
                {
                    form.ShowBox(ATMStrings.CheckBalanceReminder, ATMStrings.Continue);
                    //MessageBox.Show("Your task is to check the balance in your checking account " + (3 - errors) + " attempts remianing");
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "PreferenceButtonClick");
                    dictionary.Add("eventData", "1");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                }
                else if (subtask == 1)
                {
                    form.ShowBox(ATMStrings.Transfer50DollarsReminder, ATMStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "PreferenceButtonClick");
                    dictionary.Add("eventData", "2");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                }
                else if (subtask == 2)
                {
                    form.ShowBox(ATMStrings.WithdrawCashReminder, ATMStrings.Continue);
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "PreferenceButtonClick");
                    dictionary.Add("eventData", "3");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                }
                if (errors == 3 && subtask == 2)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);
                    form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventData", "");
                    dictionary1.Add("eventSummary", "");
                    taskDataList.Add(dictionary1);
                    myTimer1.Stop();
                    myTimer1.Dispose();
                    open = false;
                    this.Close();
                    Application.Exit();
                    return;
                }
                if (errors == 3)
                {
                    subtask++;
                    labelInstruction.Text = "";
                    labelInstructionEs.Text = "";
                    colorTabControl1.SelectedTab = tabLanguage;
                    hideMenuButtons();
                    form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                    errors = 0;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "MaxErrorsReached");
                    dictionary.Add("eventData", "");
                    dictionary.Add("eventSummary", "");
                    taskDataList.Add(dictionary);
                    hideMenuButtons();
                    showMessage();
                }
                return;
            }
            threadPlayButtonSound();
            colorTabControl1.SelectedTab = tabPreference;
            //this should never happen
            Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
            dictionary2.Add("time", DateTime.Now.ToString());
            dictionary2.Add("eventType", "PreferenceButtonClick");
            dictionary2.Add("eventData", "");
            dictionary2.Add("eventSummary", "Correct");
            taskDataList.Add(dictionary2);
            log.addAction("click Preference tab", Action.BehaviorType.normal);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            DialogResult dialog = MessageBox.Show(ATMStrings.EndSubtaskEarlyMessage, ATMStrings.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.No) 
            {
                log.addAction("click Exit button by mistake", Action.BehaviorType.error);
                return;
            }
                
            colorTabControl1.SelectedTab = tabExit;
            hideMenuButtons();
            log.addAction("click Exit button and exited the task " + (subtask + 1), Action.BehaviorType.normal);
            Thread.Sleep(3000);
            textBoxPIN.Text = "";
            if (++subtask < 3)
            {
                labelInstruction.Text = subtasksEn[subtask];
                labelInstructionEs.Text = subtasksSp[subtask];
                colorTabControl1.SelectedTab = tabPin;
            }
            else
            {              
                this.Controls.Remove(this.tableLayoutPanel1);
                this.Controls.Add(this.labelEnd);
                Thread.Sleep(3000);
                this.Close();
                return;
            }


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            handleColorTabChanges();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabCash;
            log.addAction("go back to the cash page from the page of the withdrawl amont", Action.BehaviorType.normal);
        }

        private void textBoxAmount_TextChanged(object sender, EventArgs e)
        {
            if (textBoxAmount.Text.Length > 4)
            {
                showInformation(ATMStrings.InvalidPIN, tabPin);
                textBoxAmount.Text = "";
                log.addAction("type in a invalid PIN number. It is over 4 digit.", Action.BehaviorType.error);
            }
        }

        private void buttonChecking_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            colorTabControl1.SelectedTab = tabAmount;
            fromChecking = true;
            fromSaving = false;
            label31.Text = "";
            textBoxAmount.Text = "";
            log.addAction("select the checking account to withdrawl cash", Action.BehaviorType.normal);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            handleATMUserError();
            /*
            colorTabControl1.SelectedTab = tabAmount;
            fromChecking = false;
            fromSaving = true;
            label31.Text = "";
            textBoxAmount.Text = "";
            log.addAction("select the saving account to withdrawl cash", Action.BehaviorType.normal);
             * */
        }

        private void button31_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabAccountSummary;
            //disable all input.
            
            String summary = ATMStrings.YourCheckingAcctID + user.checking.accountID;
            summary = summary + "\n";
            summary = summary + ATMStrings.Available + user.checking.amount;
            summary = summary + "\n\n";
            summary = summary + ATMStrings.YourSavingsAcctID + user.saving.accountID;
            summary = summary + "\n";
            summary = summary + ATMStrings.Available + user.saving.amount;
            label2.Text = summary;
            log.addAction("show the summary of the user's accounts", Action.BehaviorType.normal);
            Thread.Sleep(500); //why?!?
            disableATMForm();
            //
            myTimer3 = new System.Timers.Timer(accountInfoDisplay_Interval);
            myTimer3.Elapsed += new ElapsedEventHandler(ProcessTimer3);
            myTimer3.Start();//Brandon
            myTimer3.AutoReset = false;

           // Thread.Sleep(4000);
            if (subtask == 0)
            {
               
                subtask++;
                labelInstruction.Text = "";
                labelInstructionEs.Text = "";
                hideMenuButtons();
                //colorTabControl1.SelectedTab = tabLanguage; 
                hideMenuButtons();
              
                errors = 0;
               
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "SummaryAccountClick");
                dictionary.Add("eventData", "1");
                dictionary.Add("eventSummary", "Correct");
                taskDataList.Add(dictionary);
                log.addAction("clear PIN number", Action.BehaviorType.normal);
                return;
            }
           
            
        }

       
        private void reEnableATMForm()
        {
            try
            {
                GetSelfAndChildrenRecursive(this).OfType<Button>().ToList()
               .ForEach(b => b.Enabled = true);
            }
            catch (Exception e)
            {
                GetSelfAndChildrenRecursive(this).OfType<Button>().ToList()
               .ForEach(b => b.Enabled = true);
            }
            
        }

        private void disableATMForm()
        {
            GetSelfAndChildrenRecursive(this).OfType<Button>().ToList()
                .ForEach(b => b.Enabled = false);
        }

        private void ProcessTimer3(Object obj, ElapsedEventArgs e)
        {
            //re-enable input
            reEnableATMForm();
            //
            resetToTaskLanguage();
            form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);

            try
            {
                colorTabControl1.SelectedTab = tabLanguage;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            
            //colorTabControl1.SelectedTab = tabLanguage;
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "SubTaskCompleted");
            dictionary.Add("eventData", "1");
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);
            log.addAction("clear PIN number", Action.BehaviorType.normal);
            //Thread.Sleep(1000);
            showMessage();
        }

        private void resetToTaskLanguage()
        {
            //going to attempt a messy workaround for this - Rick, Oct 29 2013
            //problem: even when culture is set to Spanish, 
            //the English-language version of NextTaskPrompt gets displayed.
            //the workaround: set the culture AGAIN. yes, copying and pasting code
            //from one of the constructors. deal with it.
            //
            if (this.taskLanguage.Equals("ES"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es");
            }
            else if (this.taskLanguage.Equals("EN"))
            {
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");
            }
        }

        void CitiBankForm_Shown1(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
            form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
        }

        private void buttonBackAccSummary_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabAccountInfo;
            log.addAction("go back to account information from the summary of accounts", Action.BehaviorType.normal);
        }

        private void buttonBackAccDetail_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabAccountInfo;
            log.addAction("go back to account information from the details of accounts", Action.BehaviorType.normal);
        }

        private void button32_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "AccountDetailsClick");
            dictionary.Add("eventData", "1");
            dictionary.Add("eventSummary", "Incorrect");
            taskDataList.Add(dictionary);
            colorTabControl1.SelectedTab = tabAccountDetail;
            String details = "transactions history.";
            // TODO: add transcations history
            label10.Text = details;
            log.addAction("show the details of the user's accounts", Action.BehaviorType.normal);
            //Thread.Sleep(1000);
            errors++;
            if (errors == 3)
            {
                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "MaxErrorsReached");
                dictionary1.Add("eventData", (subtask + 1).ToString());
                dictionary1.Add("eventSummary", "");
                taskDataList.Add(dictionary1);
                subtask++;
                labelInstruction.Text = "";
                labelInstructionEs.Text = "";
                colorTabControl1.SelectedTab = tabLanguage;
                hideMenuButtons();
                form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                errors = 0;
                showMessage();
                return;
            }
            form.ShowBox(ATMStrings.CheckBalanceReminder, ATMStrings.Continue);
            
            log.addAction("clear PIN number", Action.BehaviorType.normal);
            colorTabControl1.SelectedTab = tabAccountInfo;
            return;
        }

        private void button35_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabInsertCash;
            label16.Text = "0";
            log.addAction("save money by cash", Action.BehaviorType.normal);
        }

        private void button36_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabInsertCheck;
            textBox3.Text = "";
            log.addAction("save money by checks", Action.BehaviorType.normal);
        }

        private void button38_Click(object sender, EventArgs e)
        {
            int amount = Int32.Parse(label16.Text);
            amount += 10;
            label16.Text = amount.ToString();
            log.addAction("insert $10 to save", Action.BehaviorType.normal);
        }

        private void button39_Click(object sender, EventArgs e)
        {
            int amount = Int32.Parse(label16.Text);
            amount += 20;
            label16.Text = amount.ToString();
            log.addAction("insert $20 to save", Action.BehaviorType.normal);
        }

        private void button40_Click(object sender, EventArgs e)
        {
            int amount = Int32.Parse(label16.Text);
            amount += 50;
            label16.Text = amount.ToString();
            log.addAction("insert $50 to save", Action.BehaviorType.normal);
        }

        private void button41_Click(object sender, EventArgs e)
        {
            int amount = Int32.Parse(label16.Text);
            amount += 100;
            label16.Text = amount.ToString();
            log.addAction("insert $100 to save", Action.BehaviorType.normal);
        }

        private void button42_Click(object sender, EventArgs e)
        {
            int amount = Int32.Parse(label16.Text);
            if (radioButton1.Checked)
            {
                user.checking.amount += amount;
                showInformation(ATMStrings.YouHaveDeposited + amount + " " + ATMStrings.InYourCheckingAcctTwice + user.checking.amount + " " + ATMStrings.IsAvailable, tabMainMenu);
                log.addAction("save $" + amount + "to the checking account by cash", Action.BehaviorType.normal);
            }
            else
            {
                user.saving.amount += amount;
                showInformation(ATMStrings.YouHaveDeposited + amount + " " + ATMStrings.InYourSavingsAcctTwice + user.saving.amount + " " + ATMStrings.IsAvailable, tabMainMenu);
                log.addAction("save $" + amount + "to the saving account by cash", Action.BehaviorType.normal);
            }
        }

        private void button46_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabBetweenMyAccounts;
            errors = 0;
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "TransferBetweenAccountsClick");
            dictionary.Add("eventData", (subtask + 1).ToString());
            dictionary.Add("eventSummary", "Correct");
            taskDataList.Add(dictionary);
            log.addAction("transfer between the user's accounts", Action.BehaviorType.normal);
        }

        private void button47_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabPayeeAccount;
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "TransferToOtherAccountClick");
            dictionary.Add("eventData", (subtask + 1).ToString());
            dictionary.Add("eventSummary", "Incorrect");
            taskDataList.Add(dictionary);
            log.addAction("transfer to another account", Action.BehaviorType.normal);
            errors++;
            if (errors == 3)
            {
                
                labelInstruction.Text = "";
                labelInstructionEs.Text = "";
                colorTabControl1.SelectedTab = tabLanguage;
                hideMenuButtons();
                form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "MaxErrorsReached");
                dictionary1.Add("eventData", (subtask + 1).ToString());
                dictionary1.Add("eventSummary", "");
                taskDataList.Add(dictionary1);
                subtask++;
                errors = 0;
                showMessage();
                return;
            }
            form.ShowBox(ATMStrings.Transfer50DollarsReminder, ATMStrings.Continue);
            colorTabControl1.SelectedTab = tabTransfer;
        }

        private void button43_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabDeposit;
            if (label16.Text != "")
                showInformation("Please take back your cash.", tabDeposit);
            log.addAction("cancel saving by cash", Action.BehaviorType.normal);
        }

        private void button50_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabTransfer;
            log.addAction("go back to deposit page from user's accounts transfer", Action.BehaviorType.normal);
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (textBoxCheckingToSaving.Visible == true)
            {
                textBoxCheckingToSaving.Visible = true;
            }
            else
            {
                textBoxCheckingToSaving.Visible = false;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (textBoxSavingToChecking.Visible == true)
            {
                textBoxSavingToChecking.Visible = true;
            }
            else
            {
                textBoxSavingToChecking.Visible = false;                
            }
        }

        private void button52_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabTransfer;
            log.addAction("go back to transfer page from other accounts transfer", Action.BehaviorType.normal);
        }

        private void button55_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabProfile;
            log.addAction("set the user's preference", Action.BehaviorType.normal);
        }

        private void button56_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabModifyPin;
            log.addAction("reset the user's PIN number", Action.BehaviorType.normal);
        }

        private void button69_Click(object sender, EventArgs e)
        {
            colorTabControl1.SelectedTab = tabPreference;
            log.addAction("go back to preference page from PIN number modification", Action.BehaviorType.normal);
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text.Length > 4)
            {
                textBox7.Text = textBox7.Text.Substring(0, 4);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text.Length > 4)
            {
                textBox8.Text = textBox8.Text.Substring(0, 4);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (userlist == null || userlist.Count == 0 || this.user == null)
            {
                return;
            }
            for (int i = 0; i < userlist.Count; i++)
            {
                User user = userlist[i];
                if (user.ID == this.user.ID)
                {
                    userlist[i] = this.user;
                    break;
                }
            }
            IO.storeUserlist(userlist, "usersData.txt");
            IO.storeContent(log.ToString(), this.user.name + "-FullLog.txt", true);
            IO.storeContent(log.ToString(Action.BehaviorType.error), this.user.name + "-ErrorLog.txt", true);
        }

        private void button49_Click(object sender, EventArgs e)
        {
            int amount = Int32.Parse(label16.Text);
            amount += 1;
            label16.Text = amount.ToString();
            log.addAction("insert $1 to save", Action.BehaviorType.normal);
        }

        private void button37_Click(object sender, EventArgs e)
        {
            int amount = Int32.Parse(label16.Text);
            amount += 5;
            label16.Text = amount.ToString();
            log.addAction("insert $5 to save", Action.BehaviorType.normal);
        }

        private void threadPlayButtonSound()
        {
            Thread thread = new Thread(new ThreadStart(playButtonSound));
            thread.Start();
        }

        private void threadPlayPanelSound()
        {
            Thread thread = new Thread(new ThreadStart(playPanelSound));
            thread.Start();
        }

        private void playButtonSound()
        {
            SoundPlayer soundPlayer = new SoundPlayer(BUTTON_SOUND);
            soundPlayer.Play();
        }

        private void playPanelSound()
        {
            SoundPlayer soundPlayer = new SoundPlayer(PANNEL_SOUND);
            soundPlayer.Play();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            threadPlayPanelSound();
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void buttonKey1_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "1";
                log.addAction("type \"1\" in the withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "1";
                log.addAction("type \"1\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "1";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "1";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "1";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "1";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "1";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "1";
                    log.addAction("type \"1\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "1";
                    log.addAction("type \"1\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKey2_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "2";
                log.addAction("type \"2\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "2";
                log.addAction("type \"2\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "2";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "2";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "2";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "2";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "2";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "2";
                    log.addAction("type \"2\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "2";
                    log.addAction("type \"2\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKey3_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "3";
                log.addAction("type \"3\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "3";
                log.addAction("type \"3\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "3";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "3";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "3";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "3";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "3";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "3";
                    log.addAction("type \"3\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "3";
                    log.addAction("type \"3\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKey4_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "4";
                log.addAction("type \"4\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "4";
                log.addAction("type \"4\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "4";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "4";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "4";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "4";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "4";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "4";
                    log.addAction("type \"4\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "4";
                    log.addAction("type \"4\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKey5_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "5";
                log.addAction("type \"5\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "5";
                log.addAction("type \"5\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "5";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "5";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "5";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "5";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "5";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "5";
                    log.addAction("type \"5\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "5";
                    log.addAction("type \"5\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKey6_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "6";
                log.addAction("type \"6\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "6";
                log.addAction("type \"6\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "6";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "6";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "6";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "6";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "6";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "6";
                    log.addAction("type \"6\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "6";
                    log.addAction("type \"6\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKey7_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "7";
                log.addAction("type \"7\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "7";
                log.addAction("type \"7\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "7";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "7";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "7";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "7";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "7";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "7";
                    log.addAction("type \"7\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "7";
                    log.addAction("type \"7\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKey8_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "8";
                log.addAction("type \"8\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "8";
                log.addAction("type \"8\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "8";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "8";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "8";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "8";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "8";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "8";
                    log.addAction("type \"8\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "8";
                    log.addAction("type \"8\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKey9_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "9";
                log.addAction("type \"9\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "9";
                log.addAction("type \"9\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "9";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "9";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "9";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "9";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "9";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "9";
                    log.addAction("type \"9\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "9";
                    log.addAction("type \"9\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKey10_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "0";
                log.addAction("type \"0\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "0";
                log.addAction("type \"0\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "0";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "0";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "0";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "0";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "0";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "0";
                    log.addAction("type \"0\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "0";
                    log.addAction("type \"0\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonKeyClear_Click_1(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = "";
                log.addAction("clear the withdrawl amount", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                colorTabControl1.SelectedTab = tabDeposit;
                if (textBox3.Text != "")
                    showInformation(ATMStrings.TakeBackYourChecks, tabDeposit);
                log.addAction("cancel saving by checks", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                textBoxCheckingToSaving.Text = "";
                textBoxSavingToChecking.Text = "";
                log.addAction("cancel transfer request", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                textBox5.Text = "";
                textBox6.Text = "";
                log.addAction("clear the payee's information in tranfer processing", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabModifyPin)
            {
                textBox7.Text = "";
                textBox8.Text = "";
                setPIN = true;
                log.addAction("clear the new PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = "";
                log.addAction("clear PIN number", Action.BehaviorType.normal);
            }
        }

        private void withdrawl(double amount)
        {
            if (amount % 10 != 0)
            {
                showInformation(ATMStrings.PleaseRetypeAmount10and20, tabAmount);
                log.addAction("submit an invalid amount", Action.BehaviorType.error);
            }
            else
            {
                if (fromChecking)
                {
                    if (amount != 100)
                    {
                        attempt++;
                        form.ShowBox(ATMStrings.PleaseEnterAmtRequired100, ATMStrings.Continue);
                        Dictionary<String, String> dictionary111 = new Dictionary<String, String>();
                        dictionary111.Add("time", DateTime.Now.ToString());
                        dictionary111.Add("eventType", "EnteredWrongAmount");
                        dictionary111.Add("eventData", (subtask + 1).ToString());
                        dictionary111.Add("eventSummary", "Incorrect");
                        taskDataList.Add(dictionary111);
                        if (attempt == 3)
                        {
                           // subtask++;
                            labelInstruction.Text = subtasksEn[subtask];
                            labelInstructionEs.Text = subtasksSp[subtask];
                            colorTabControl1.SelectedTab = tabMainMenu;
                            //heree
                            Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                            dictionary1.Add("time", DateTime.Now.ToString());
                            dictionary1.Add("eventType", "MaxErrorsReached");
                            dictionary1.Add("eventData", (subtask + 1).ToString());
                            dictionary1.Add("eventSummary", "");
                            taskDataList.Add(dictionary1);
                            form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                            Dictionary<String, String> dictionary11 = new Dictionary<String, String>();
                            dictionary11.Add("time", DateTime.Now.ToString());
                            dictionary11.Add("eventType", "TaskClosed");
                            dictionary11.Add("eventData", (subtask + 1).ToString());
                            dictionary11.Add("eventSummary", "");
                            taskDataList.Add(dictionary11);
                            myTimer1.Stop();
                            myTimer1.Dispose();
                            open = false;
                                this.Close();
                                Application.Exit();
                           
                            // the end
                            errors = 0;
                            return;

                        }
                        

                        return;
                        
                    }

                    if (amount < user.checking.amount)
                    {
                        user.checking.amount = user.checking.amount - amount;
                        this.amount += amount;
                        Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                        dictionary1.Add("time", DateTime.Now.ToString());
                        dictionary1.Add("eventType", "WithdrewCash");
                        dictionary1.Add("eventData", amount.ToString());
                        dictionary1.Add("eventSummary", "Correct");
                        taskDataList.Add(dictionary1);

                        Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                        dictionary9.Add("time", DateTime.Now.ToString());
                        dictionary9.Add("eventType", "SubTaskCompleted");
                        dictionary9.Add("eventData", (subtask + 1).ToString());
                        dictionary9.Add("eventSummary", "");
                        taskDataList.Add(dictionary9);
                        log.addAction("successfully withdrawl cash $" + amount + " from the user's checking account", Action.BehaviorType.normal);
                        showInformation(ATMStrings.PleaseTakeBills+"\n\n"+ATMStrings.CheckingAcct + user.checking.amount +"\n\n" + ATMStrings.SavingsAcct + user.saving.amount, tabMainMenu);
                        outputBill();
                        disableATMForm();
                        Thread.Sleep(3000);
                        reEnableATMForm();
                        resetToTaskLanguage();
                        colorTabControl1.SelectedTab = tabLanguage;
                        hideMenuButtons();
                        subtask++;
                        form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                       // the end again
                        
                        Dictionary<String, String> dictionary11 = new Dictionary<String, String>();
                        dictionary11.Add("time", DateTime.Now.ToString());
                        dictionary11.Add("eventType", "TaskClosed");
                        dictionary11.Add("eventData", (subtask).ToString());
                        dictionary11.Add("eventSummary", "");
                        taskDataList.Add(dictionary11);
                        myTimer1.Stop();
                        myTimer1.Dispose();
                        open = false;
                            this.Close();
                            Application.Exit();
                        
                        errors = 0;
                        return;
                    }
                    else
                    {
                        showInformation(ATMStrings.PleaseRetypeAmtChecking + " " + user.checking.amount, tabAmount);
                        log.addAction("the amount to withdrawl is over the available amount of the user's checking account", Action.BehaviorType.normal);
                    }
                }
                if (fromSaving)
                {
                    errors++;
                    if (errors == 3)
                    {
                        subtask++;
                        labelInstruction.Text = "";
                        labelInstructionEs.Text = "";
                        colorTabControl1.SelectedTab = tabMainMenu;
                        form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "MaxErrorsReached");
                        dictionary.Add("eventData", (subtask ).ToString());
                        dictionary.Add("eventSummary", "");
                        taskDataList.Add(dictionary);
                        Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                        dictionary3.Add("time", DateTime.Now.ToString());
                        dictionary3.Add("eventType", "TaskClosed");
                        dictionary3.Add("eventData", (subtask ).ToString());
                        dictionary3.Add("eventSummary", "");
                        taskDataList.Add(dictionary3);

                        myTimer1.Stop();
                        myTimer1.Dispose();
                        open = false;
                            this.Close();
                            Application.Exit();
                      
                        errors = 0;
                        return;
                    }
                    form.ShowBox(ATMStrings.WithdrawCashReminder, ATMStrings.Continue);
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "WithdrawFromSavings");
                    dictionary1.Add("eventData", (subtask + 1).ToString());
                    dictionary1.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary1);
                    colorTabControl1.SelectedTab = tabCash;
                    return;
                    if (amount < user.saving.amount)
                    {
                        user.saving.amount = user.saving.amount - amount;
                        this.amount += amount;
                        log.addAction("successfully withdrawl cash $" + amount + " from the user's saving account", Action.BehaviorType.normal);
                        showInformation(ATMStrings.PleaseTakeBills+ATMStrings.CheckingAcct + user.checking.amount + ATMStrings.SavingsAcct + user.saving.amount, tabMainMenu);
                        outputBill();
                    }
                    else
                    {
                        showInformation(ATMStrings.PleaseRetypeAmtSavings+ "  " + user.checking.amount, tabAmount);
                        log.addAction("the amount to withdrawl is over the available amount of the user's saving account", Action.BehaviorType.normal);
                    }
                }
            }
        }

        private void buttonKeyDot_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + ".";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                //here
                if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + ".";
                }
            }
            else if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text += ".";
            }
        }

        private void tabLanguage_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabPin_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabMainMenu_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
            labelMainMenue.Location = startingPoint;
        }

        private void tabCash_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabAmount_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabAccountInfo_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
            buttonAccountSummary.Location = startingPoint;
            buttonAccountDetail.Location = new Point(startingPoint.X, buttonAccountSummary.Location.Y + buttonAccountSummary.Size.Height + distanceY / 2);
        }

        private void tabAccountSummary_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
           

        }

        private void tabAccountDetail_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabDeposit_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabInsertCash_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabInsertCheck_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabTransfer_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
            labelTransfer.Location = startingPoint;
            buttonTransferMyAccount.Location = new Point(labelTransfer.Location.X, labelTransfer.Location.Y + labelTransfer.Size.Height + distanceY);
            buttonTransferOtherAccount.Location = new Point(buttonTransferMyAccount.Location.X, buttonTransferMyAccount.Location.Y + buttonTransferMyAccount.Size.Height + distanceY/2);
        }

        private void tabBetweenMyAccounts_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabPayeeAccount_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabPreference_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabProfile_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabModifyPin_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void tabExit_Click(object sender, EventArgs e)
        {
            threadPlayPanelSound();
        }

        private void showInformation(String content, TabPage goBackTab)
        {            
            colorTabControl1.SelectedTab = tabInformation;
            labelInfo.Text = content;
            tabInformation.Refresh();
            DateTime now = DateTime.UtcNow;
            while ((long)(DateTime.UtcNow - now).TotalMilliseconds < 3000) ;
          //  Thread.Sleep(3000);
            colorTabControl1.SelectedTab = goBackTab;
            textBoxPIN.Text = "";
        }

        int original = 0;

        private void outputBill()
        {
            if (original == 0)
                original = pictureBox1.Location.Y;
            int delta = 0;
            while (delta < pictureBox1.Size.Height + 10)
            {
                Thread.Sleep(30);
                delta += 3;
                pictureBox1.Location = new Point(pictureBox1.Location.X, original + delta);
                pictureBox1.Refresh();
            }
            buttonTakeBills.Visible = true;
        }

        double amount = 0.0;
        private void inputBill()
        {
            buttonTakeBills.Visible = false;
            billHoldTime = 0;
            while (pictureBox1.Location.Y > original)
            {
                Thread.Sleep(30);
                pictureBox1.Location = new Point(pictureBox1.Location.X, pictureBox1.Location.Y - 3);
                pictureBox1.Refresh();
            }
            user.checking.amount += amount;
            log.addAction("Taking bills is timeout. The bills are drawn back.", Action.BehaviorType.error);
        }

        private void buttonTakeBills_Click(object sender, EventArgs e)
        {
            pictureBox1.Location = new Point(pictureBox1.Location.X, original);
            buttonTakeBills.Visible = false;
            amount = 0.0;
        }

        int quickCash = 0;
        private void buttonBill40_Click(object sender, EventArgs e)
        {
           // quickCash = 40;
           // colorTabControl1.SelectedTab = tabPin;
            errors++;
            if (errors == 3 && subtask == 2)
            {
                form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                myTimer1.Stop();
                myTimer1.Dispose();
                open = false;
                this.Close();
                Application.Exit();
            }
            if (errors == 3)
            {

                labelInstruction.Text = "";
                labelInstructionEs.Text = "";
                colorTabControl1.SelectedTab = tabLanguage;
                hideMenuButtons();
                subtask = subtask + 1;
                form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                
                errors = 0;
                showMessage();
                return;
            }
            form.ShowBox(ATMStrings.MustSelectLanguage, ATMStrings.Continue);
        }

        private void buttonBill100_Click(object sender, EventArgs e)
        {
          //  quickCash = 100;
          //  colorTabControl1.SelectedTab = tabPin;
            errors++;
            if (errors == 3 && subtask == 2)
            {
                form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                myTimer1.Stop();
                myTimer1.Dispose();
                open = false;
                this.Close();
                Application.Exit();
            }
            if (errors == 3)
            {

                labelInstruction.Text = "";
                labelInstructionEs.Text = "";
                colorTabControl1.SelectedTab = tabLanguage;
                hideMenuButtons();
                subtask = subtask + 1;
                form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                
                errors = 0;
                showMessage();
                return;
            }
            form.ShowBox(ATMStrings.MustSelectLanguage, ATMStrings.Continue);
        }

        private void buttonBill200_Click(object sender, EventArgs e)
        {
          //  quickCash = 200;
          //  colorTabControl1.SelectedTab = tabPin;
            errors++;
            if (errors == 3 && subtask == 2)
            {
                form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                myTimer1.Stop();
                myTimer1.Dispose();
                open = false;
                this.Close();
                Application.Exit();
            }
            if (errors == 3)
            {

                labelInstruction.Text = "";
                labelInstructionEs.Text = "";
                colorTabControl1.SelectedTab = tabLanguage;
                hideMenuButtons();
                subtask = subtask + 1;
                form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                
                errors = 0;
                showMessage();
                return;
            }
            form.ShowBox(ATMStrings.MustSelectLanguage, ATMStrings.Continue);
        }

        private void buttonBill300_Click(object sender, EventArgs e)
        {
          //  quickCash = 300;
          //  colorTabControl1.SelectedTab = tabPin;
            errors++;
            if (errors == 3 && subtask == 2)
            {
                form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                myTimer1.Stop();
                myTimer1.Dispose();
                open = false;
                this.Close();
                Application.Exit();
            }
            if (errors == 3)
            {

                labelInstruction.Text = "";
                labelInstructionEs.Text = "";
                colorTabControl1.SelectedTab = tabLanguage;
                hideMenuButtons();
                subtask = subtask + 1;
                form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                errors = 0;
                showMessage();
                return;
            }
            form.ShowBox(ATMStrings.MustSelectLanguage, ATMStrings.Continue);
        }
        private void enterAction()
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                if (textBoxAmount.Text == "")
                {
                    showInformation(ATMStrings.PleaseTypeInCashAmt, tabAmount);
                    log.addAction("submit an empty amount as the amount of cash withdrawl", Action.BehaviorType.error);
                }
                else
                {
                    double amount = Double.Parse(textBoxAmount.Text);
                    withdrawl(amount);
                }
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                double amount = Double.Parse(textBox3.Text);
                if (!radioButton3.Checked)
                {
                    user.checking.amount += amount;
                    showInformation(ATMStrings.YouHaveDeposited+ amount + " "+ ATMStrings.InYourCheckingAcct, tabDeposit);
                    log.addAction("save $" + amount + "to the checking account by checks", Action.BehaviorType.normal);
                }
                else
                {
                    user.saving.amount += amount;
                    showInformation(ATMStrings.YouHaveDeposited + amount + " " + ATMStrings.InYourSavingsAcct, tabDeposit);
                    log.addAction("save $" + amount + "to the saving account by checks", Action.BehaviorType.normal);
                }
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                {
                    errors++;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "CheckingToSavingTransfer");
                    dictionary.Add("eventData", (subtask + 1).ToString());
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                    if (errors == 3)
                    {
                        
                        labelInstruction.Text = "";
                        labelInstructionEs.Text = "";
                        colorTabControl1.SelectedTab = tabLanguage;
                        hideMenuButtons();
                        form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                        Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                        dictionary1.Add("time", DateTime.Now.ToString());
                        dictionary1.Add("eventType", "MaxErrorsReached");
                        dictionary1.Add("eventData", (subtask + 1).ToString());
                        dictionary1.Add("eventSummary", "");
                        taskDataList.Add(dictionary1);
                        subtask++;
                        errors = 0;
                        showMessage();
                        return;

                    }
                    form.ShowBox(ATMStrings.Transfer50DollarsReminder, ATMStrings.Continue);

                    return;
                    if (textBoxCheckingToSaving.Text == "")
                    {
                        showInformation(ATMStrings.PleaseTypeTransferAmt, tabTransfer);
                        log.addAction("not set the amount of money to be transferred between the user's accounts", Action.BehaviorType.error);
                        return;
                    }
                    double amount = Double.Parse(textBoxCheckingToSaving.Text);
                    if (amount < user.checking.amount)
                    {
                        user.checking.amount -= amount;
                        user.saving.amount += amount;
                        //showInformation(ATMStrings.TransactionDoneSuccessfully + "\n" + ATMStrings.NowCheckingAcctHas + user.checking.amount + "\n" + ATMStrings.YourSavingsAcctHas + user.saving.amount + ".", tabTransfer);
                        showInformation(ATMStrings.TransactionDoneSuccessfully, tabTransfer);
                        //radioButtonCheckingToSaving.Text = ATMStrings.CheckingToSavings;
                        //radioButtonSavingToChecking.Text = ATMStrings.SavingsToChecking;
                        labelTransChecking.Text = ATMStrings.Available + user.checking.amount;
                        labelTransSaving.Text = ATMStrings.Available + user.saving.amount;
                        log.addAction("successfully transfer $" + amount + "from the checking acount to the saving account", Action.BehaviorType.normal);
                    }
                    else
                    {
                        showInformation(ATMStrings.PleaseRetypeTransferAmt+ATMStrings.TheAvailableAmtInCheckingIs+" " + user.checking.amount, tabTransfer);
                        log.addAction("The amount of money to be tranferred is over than the available amount in the checking account", Action.BehaviorType.error);
                    }
                }
                else
                {
                    
                    if (textBoxSavingToChecking.Text == "")
                    {
                        showInformation(ATMStrings.PleaseTypeTransferAmt, tabTransfer);
                        log.addAction("not set the amount of money to be transferred between the user's accounts", Action.BehaviorType.error);
                        return;
                    }
                    double amount = Double.Parse(textBoxSavingToChecking.Text);
                    if (amount != 50)
                    {
                        errors++;
                        Dictionary<String, String> dictionary = new Dictionary<String, String>();
                        dictionary.Add("time", DateTime.Now.ToString());
                        dictionary.Add("eventType", "EnteredWrongAmount");
                        dictionary.Add("eventData", (subtask + 1).ToString());
                        dictionary.Add("eventSummary", "Incorrect");
                        taskDataList.Add(dictionary);
                        if (errors == 3)
                        {
                            
                            labelInstruction.Text = "";
                            labelInstructionEs.Text = "";
                            colorTabControl1.SelectedTab = tabLanguage;
                            hideMenuButtons();
                            form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                            Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                            dictionary1.Add("time", DateTime.Now.ToString());
                            dictionary1.Add("eventType", "MaxErrorsReached");
                            dictionary1.Add("eventData", (subtask + 1).ToString());
                            dictionary1.Add("eventSummary", "");
                            taskDataList.Add(dictionary1);
                            subtask++;
                            errors = 0;
                            showMessage();
                            return;
                        }
                        form.ShowBox(ATMStrings.PleaseEnterAmtRequired50, ATMStrings.Continue);
                        
                        return;
                    }
                    if (amount < user.saving.amount)
                    {
                        disableATMForm();
                        Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                        dictionary3.Add("time", DateTime.Now.ToString());
                        dictionary3.Add("eventType", "SavingToCheckingTransfer");
                        dictionary3.Add("eventData", "50");
                        dictionary3.Add("eventSummary", "Correct");
                        taskDataList.Add(dictionary3);
                        user.saving.amount -= amount;
                        user.checking.amount += amount;
                        //showInformation(ATMStrings.TransactionDoneSuccessfully + ATMStrings.NowCheckingAcctHas + user.checking.amount + ATMStrings.YourSavingsAcctHas + user.saving.amount + ".", tabTransfer);
                        showInformation(ATMStrings.TransactionDoneSuccessfully + "\n" + ATMStrings.NowCheckingAcctHas + user.checking.amount + "\n" + ATMStrings.YourSavingsAcctHas + user.saving.amount + ".", tabTransfer);
                        radioButtonCheckingToSaving.Text = ATMStrings.CheckingToSavings;
                        radioButtonSavingToChecking.Text = ATMStrings.SavingsToChecking;
                        labelTransChecking.Text = ATMStrings.Available + user.checking.amount;
                        labelTransSaving.Text = ATMStrings.Available + user.saving.amount;
                        log.addAction("successfully transfer $" + amount + "from the saving acount to the checking account", Action.BehaviorType.normal);
                        Thread.Sleep(3000);
                        subtask++;
                        labelInstruction.Text = "";
                        labelInstructionEs.Text = "";
                        colorTabControl1.SelectedTab = tabLanguage;
                        hideMenuButtons();
                        form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);
                        reEnableATMForm();
                        resetToTaskLanguage();
                        errors = 0;
                        Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                        dictionary1.Add("time", DateTime.Now.ToString());
                        dictionary1.Add("eventType", "SubTaskCompleted");
                        dictionary1.Add("eventData", (subtask).ToString());
                        dictionary1.Add("eventSummary", "");
                        taskDataList.Add(dictionary1);
                        showMessage();
                        return;

                    }
                    else
                    {
                        showInformation(ATMStrings.PleaseRetypeTransferAmt+ATMStrings.TheAvailableAmtInSavingsIs + user.saving.amount, tabTransfer);
                        log.addAction("The amount of money to be tranferred is over than the available amount in the saving account", Action.BehaviorType.error);
                    }
                }
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount) //wtf is this?? if we're not using it, DELETE IT! -Rick
            {
                if (textBox4.Text == "")
                {
                    showInformation("Please type in the payee's name.", tabPayeeAccount);
                    log.addAction("transfer to other accounts with an empty payee's name", Action.BehaviorType.error);
                    return;
                }
                if (textBox5.Text == "")
                {
                    showInformation("Please type in the payee's account.", tabPayeeAccount);
                    log.addAction("transfer to other accounts with an empty payee's account", Action.BehaviorType.error);
                    return;
                }
                if (textBox6.Text == "")
                {
                    showInformation("Please type in the amount you want to transfer.", tabPayeeAccount);
                    log.addAction("transfer to other accounts with an empty amount", Action.BehaviorType.error);
                    return;
                }
                double amount = Double.Parse(textBox6.Text);
                User payee = null;
                foreach (User user in userlist)
                {
                    if (user.ID == textBox5.Text)
                    {
                        payee = user;
                    }
                }
                if (payee == null)
                {
                    showInformation("The payee does not exist. Please type in the payee's account again.", tabPayeeAccount);
                    log.addAction("transfer to other accounts with an nonexistent account", Action.BehaviorType.error);
                    return;
                }
                else if (payee.name != textBox4.Text)
                {
                    showInformation("The payee's name does not match to the name you typed in. Please type the payee's name again.", tabPayeeAccount);
                    log.addAction("transfer to other accounts with an mismatching payee's name and account", Action.BehaviorType.error);
                    return;
                }
                if (amount <= this.user.checking.amount)
                {
                    DialogResult result = MessageBox.Show("Are you sure to transfer $" + amount + " to " + textBox4.Text + "(" + textBox5.Text + ")", "Are you sure", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        user.checking.amount -= amount;
                        payee.checking.amount += amount;
                        showInformation("The transcation is done successfully.\n $" + amount + " has been transferred to " + payee.name + ".", tabPayeeAccount);
                        log.addAction("successfully tranfer $" + amount + "to account" + payee.ID, Action.BehaviorType.error);
                    }
                }
                else
                {
                    showInformation("Please retype the amount you want to transfer.\nThe available amount in your checking account is" + user.checking.amount, tabPayeeAccount);
                    log.addAction("the amount to be transferred is over the available amount of the user's checking account", Action.BehaviorType.error);
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
               
                user.pinTime = user.pinTime + 1;
                string trialTimes = user.pinTime == 1 ? user.pinTime + " time" : user.pinTime + " times";
                if (textBoxPIN.Text == user.PIN)
                {
                    showMenuButtons();
                    user.pinTime = 0;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "EnterClick");
                    dictionary.Add("eventData", "Pin");
                    dictionary.Add("eventSummary", "Correct");
                    taskDataList.Add(dictionary);
                    log.addAction("clear PIN number", Action.BehaviorType.normal);
                    log.addAction("enter the PIN number and successfully sign in", Action.BehaviorType.normal);
                  //  myTimer1 = new System.Timers.Timer(30000);//brandon
                  //  myTimer1.Elapsed += new ElapsedEventHandler(ProcessTimerEvent);
                  //  myTimer1.Start();//Brandon
                    if (quickCash == 0)
                    {
                        colorTabControl1.SelectedTab = tabMainMenu;
                    }
                    else
                    {
                        fromChecking = true;
                        withdrawl(quickCash);
                        colorTabControl1.SelectedTab = tabExit;
                    }
                }
                else if (user.pinTime < 3)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "EnterClick");
                    dictionary.Add("eventData", "Pin");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                    log.addAction("clear PIN number", Action.BehaviorType.normal);
                    //showInformation("Your PIN number is not correct. Please retry.\nYou have " + (3 - user.pinTime) + " trials", tabPin);
                    showInformation(ATMStrings.InvalidPIN + "\n" + (3 - user.pinTime) + " " + ATMStrings.PinTriesRemaining, tabPin);
                    textBoxPIN.Text = "";
                    log.addAction("type a wrong PIN number " + trialTimes , Action.BehaviorType.error);
                }
                else
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "EnterClick");
                    dictionary.Add("eventData", "Pin");
                    dictionary.Add("eventSummary", "Incorrect");
                    taskDataList.Add(dictionary);
                    log.addAction("clear PIN number", Action.BehaviorType.normal);
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "MaxErrorsReached");
                    dictionary1.Add("eventData", "3 Pin errors");
                    dictionary1.Add("eventSummary", "");
                    taskDataList.Add(dictionary1);
                    if (subtask == 2)
                    {
                        form.ShowBox(ATMStrings.ExitAtmTaskMessage, ATMStrings.Exit);
                        Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                        dictionary2.Add("time", DateTime.Now.ToString());
                        dictionary2.Add("eventType", "TaskClosed");
                        dictionary2.Add("eventData", "");
                        dictionary2.Add("eventSummary", "");
                        taskDataList.Add(dictionary2);
                        log.addAction("clear PIN number", Action.BehaviorType.normal);
                        myTimer1.Stop();
                        myTimer1.Dispose();
                        open = false;
                        this.Close();
                        Application.Exit();
                        return;
                    }
                   // showInformation("You have tried " + trialTimes  + "\nBecause of account protection, your account is locked.", tabExit);
                    form.ShowBox(ATMStrings.NextTaskPrompt, ATMStrings.Continue);

                   
                    log.addAction("clear PIN number", Action.BehaviorType.normal);
                    showMenuButtons();
                    
                    subtask++;
                     labelInstruction.Text = "";
                     labelInstructionEs.Text = "";
                    user.pinTime = 0;
                    colorTabControl1.SelectedTab = tabLanguage;
                    hideMenuButtons();
                    showMessage();

                    log.addAction("type a wrong PIN number " + trialTimes + ",\nand the user's account is locked", Action.BehaviorType.error);
                }
            }
            else if (colorTabControl1.SelectedTab == tabModifyPin)  //wtf is this?! 
            {
                if (setPIN)
                {
                    if (textBox7.Text.Length != 4)
                    {
                        showInformation("Please set a 4-digit PIN number.", tabModifyPin);
                        log.addAction("the new PIN number is not equal to 4 digits", Action.BehaviorType.error);
                        return;
                    }
                    textBox7.BackColor = Color.Gray;
                    textBox8.BackColor = Color.White;
                    setPIN = false;
                }
                else
                {
                    if (textBox7.Text != textBox8.Text)
                    {
                        showInformation("Two PIN number does not match. Please set it again.", tabModifyPin);
                        textBox7.Text = "";
                        textBox8.Text = "";
                        textBox7.BackColor = Color.White;
                        textBox8.BackColor = Color.Gray;
                        setPIN = true;
                        log.addAction("the confirmation and original PIN number does not match", Action.BehaviorType.error);
                    }
                    else
                    {
                        user.PIN = textBox7.Text;
                        showInformation("New PIN number is set successfully.", tabMainMenu);
                        log.addAction("the new PIN number is successfully set", Action.BehaviorType.normal);
                    }
                }
            }
        }

        private void buttonEnter_Click(object sender, EventArgs e)
        {
            enterAction();
        }

        private void CitiBankForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                enterAction();
        }

        private void colorTabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Brush BackBrush = new SolidBrush(Color.Black);

            e.Graphics.FillRectangle(BackBrush, e.Bounds);

            BackBrush.Dispose();
        }

        private void fixButtonSizeAndLocation()
        {
            Size buttonSize = new System.Drawing.Size(150, 120);
           
            this.buttonCash.Size = buttonSize;
            this.buttonAccountInfo.Size = buttonSize; 
            this.buttonDeposit.Size = buttonSize;
            this.buttonTransfer.Size = buttonSize;
            this.buttonPreference.Size = buttonSize; 
            this.buttonExit.Size = buttonSize;

            int buttonLocationHeight = 15;

            this.buttonCash.Location = new Point(calculateButtonLocationX(0), buttonLocationHeight);
            this.buttonAccountInfo.Location = new Point(calculateButtonLocationX(1), buttonLocationHeight);
            this.buttonDeposit.Location = new Point(calculateButtonLocationX(2), buttonLocationHeight);
            this.buttonTransfer.Location = new Point(calculateButtonLocationX(3), buttonLocationHeight);
            this.buttonPreference.Location = new Point(calculateButtonLocationX(4), buttonLocationHeight);
            this.buttonExit.Location = new Point(calculateButtonLocationX(5), buttonLocationHeight);
        }

        /**
         * indexes start from 0
         */
        private int calculateButtonLocationX(int buttonIndex)
        {
            return (buttonIndex + 1) * 8 + buttonIndex * this.buttonCash.Size.Width;
        }

        private void colorTabControl1_Selected(object sender, TabControlEventArgs e)
        {            
            if (colorTabControl1.SelectedTab == tabExit)
            {
                Thread.Sleep(1000);
            }
            handleColorTabChanges();
        }

        private void handleColorTabChanges()
        {
            if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = "";
            }
            else if (colorTabControl1.SelectedTab == tabCash)
            {
                buttonChecking.Text = ATMStrings.CheckingAsterisk + /*user.checking.accountID.Substring(user.checking.accountID.Length - 4) +*/ "\t" + ATMStrings.Available + user.checking.amount.ToString();
                buttonSaving.Text = ATMStrings.SavingsAsterisk + /*user.saving.accountID.Substring(user.saving.accountID.Length - 4) +*/ "\t" + ATMStrings.Available + user.saving.amount.ToString();
            }
            else if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = "";
            }
            else if (colorTabControl1.SelectedTab == tabInsertCash)
            {
                label16.Text = "";
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = "";
            }
            else if (colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                radioButtonCheckingToSaving.Text = ATMStrings.CheckingToSavings;
                labelTransChecking.Text = ATMStrings.CheckingAsterisk + user.checking.amount;
                radioButtonSavingToChecking.Text = ATMStrings.SavingsToChecking;
                labelTransSaving.Text = ATMStrings.SavingsAsterisk + user.saving.amount;
                textBoxCheckingToSaving.Text = "";
                textBoxSavingToChecking.Text = "";
                textBoxCheckingToSaving.Location = new Point(radioButtonCheckingToSaving.Location.X + 7, textBoxCheckingToSaving.Location.Y);
                textBoxSavingToChecking.Location = new Point(radioButtonSavingToChecking.Location.X + 7, textBoxSavingToChecking.Location.Y);
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                labelPayeeInfo.Text = "$" + user.checking.amount + " is available in your checking account.";
            }
            else if (colorTabControl1.SelectedTab == tabModifyPin)
            {
                textBox7.Text = "";
                textBox8.Text = "";
                setPIN = true;
                textBox7.BackColor = Color.White;
                textBox8.BackColor = Color.Gray;
            }
            else if (colorTabControl1.SelectedTab == tabExit)
            {
                tabExit.Refresh();
                //System.Threading.Thread.Sleep(1000);

            }
        
        }

        private void buttonContinueInstruction_Click(object sender, EventArgs e)
        {
            this.textBoxPIN.Focus();

            this.tableLayoutPanel1.Size = this.Size;
            this.colorTabControl1.Size = this.Size;
            this.colorTabControl1.Dock = DockStyle.Fill;
            tabLanguage.Size = this.colorTabControl1.Size;
            tabPin.Size = this.colorTabControl1.Size;
            tabMainMenu.Size = this.colorTabControl1.Size;
            tabCash.Size = this.colorTabControl1.Size;
            tabAmount.Size = this.colorTabControl1.Size;
            tabAccountInfo.Size = this.colorTabControl1.Size;
            tabAccountDetail.Size = this.colorTabControl1.Size;
            tabDeposit.Size = this.colorTabControl1.Size;
            tabInsertCash.Size = this.colorTabControl1.Size;
            tabInsertCheck.Size = this.colorTabControl1.Size;
            tabTransfer.Size = this.colorTabControl1.Size;
            tabPayeeAccount.Size = this.colorTabControl1.Size;
            tabPreference.Size = this.colorTabControl1.Size;
            tabProfile.Size = this.colorTabControl1.Size;
            tabModifyPin.Size = this.colorTabControl1.Size;
            tabExit.Size = this.colorTabControl1.Size;
            tabInformation.Size = this.colorTabControl1.Size;

            fixButtonSizeAndLocation();
            textBoxCheckingToSaving.Location = new Point(85, 187);
            textBoxSavingToChecking.Location = new Point(85, 320);
            this.Controls.Remove(this.panelInstruction);
            this.Controls.Add(this.tableLayoutPanel1);          
        }

        private void textBoxPIN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBoxPIN.ReadOnly) 
            {
                e.KeyChar = '\0';
            }
            
        }

        private void buttonExitTask_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
            dictionary1.Add("time", DateTime.Now.ToString());
            dictionary1.Add("eventType", "TaskClosed");
            dictionary1.Add("eventData", (subtask + 1).ToString());
            dictionary1.Add("eventSummary", "");
            taskDataList.Add(dictionary1);
            myTimer1.Stop();
            myTimer1.Dispose();
            open = false;
            this.Close();
            Application.Exit();
            return;

        }

        private void buttonNumber1_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "1";
                log.addAction("type \"1\" in the withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "1";
                log.addAction("type \"1\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "1";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer 
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts )
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "1";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "1";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "1";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "1";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "1";
                    log.addAction("type \"1\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "1";
                    log.addAction("type \"1\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumber2_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "2";
                log.addAction("type \"2\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "2";
                log.addAction("type \"2\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "2";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "2";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "2";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "2";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "2";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "2";
                    log.addAction("type \"2\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "2";
                    log.addAction("type \"2\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumber3_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "3";
                log.addAction("type \"3\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "3";
                log.addAction("type \"3\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "3";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "3";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "3";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "3";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "3";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "3";
                    log.addAction("type \"3\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "3";
                    log.addAction("type \"3\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumber4_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "4";
                log.addAction("type \"4\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "4";
                log.addAction("type \"4\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "4";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "4";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "4";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "4";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "4";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "4";
                    log.addAction("type \"4\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "4";
                    log.addAction("type \"4\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumber5_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "5";
                log.addAction("type \"5\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "5";
                log.addAction("type \"5\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "5";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "5";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "5";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "5";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "5";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "5";
                    log.addAction("type \"5\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "5";
                    log.addAction("type \"5\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumber6_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "6";
                log.addAction("type \"6\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "6";
                log.addAction("type \"6\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "6";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "6";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "6";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "6";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "6";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "6";
                    log.addAction("type \"6\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "6";
                    log.addAction("type \"6\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumber7_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "7";
                log.addAction("type \"7\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "7";
                log.addAction("type \"7\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "7";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "7";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "7";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "7";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "7";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "7";
                    log.addAction("type \"7\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "7";
                    log.addAction("type \"7\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumber8_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "8";
                log.addAction("type \"8\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "8";
                log.addAction("type \"8\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "8";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "8";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "8";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "8";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "8";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "8";
                    log.addAction("type \"8\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "8";
                    log.addAction("type \"8\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumber9_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "9";
                log.addAction("type \"9\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "9";
                log.addAction("type \"9\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "9";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "9";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "9";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "9";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "9";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "9";
                    log.addAction("type \"9\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "9";
                    log.addAction("type \"9\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumber10_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = textBoxAmount.Text + "0";
                log.addAction("type \"0\" in withdrawl amount setting", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = textBoxPIN.Text + "0";
                log.addAction("type \"0\" in the PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + "0";
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + "0";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + "0";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox5.Focused)
                {
                    textBox5.Text = textBox5.Text + "0";
                }
                else if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + "0";
                }
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                if (setPIN)
                {
                    textBox7.Text += "0";
                    log.addAction("type \"0\" in a new PIN number", Action.BehaviorType.normal);
                }
                else
                {
                    textBox8.Text += "0";
                    log.addAction("type \"0\" for the new PIN number confirmation", Action.BehaviorType.normal);
                }
            }
        }

        private void buttonNumberDot_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                textBox3.Text = textBox3.Text + ".";
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                if (textBox6.Focused)
                {
                    textBox6.Text = textBox6.Text + ".";
                }
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
               || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                if (textBoxCheckingToSaving.Visible == true)
                    textBoxCheckingToSaving.Text = textBoxCheckingToSaving.Text + ".";
                if (textBoxSavingToChecking.Visible == true)
                    textBoxSavingToChecking.Text = textBoxSavingToChecking.Text + ".";
            }
        }

        private void buttonNumberClear_Click(object sender, EventArgs e)
        {
            threadPlayButtonSound();
            if (colorTabControl1.SelectedTab == tabAmount)
            {
                textBoxAmount.Text = "";
                log.addAction("clear the withdrawl amount", Action.BehaviorType.normal);
                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "ClearClick");
                dictionary1.Add("eventData", "tabAmount");
                dictionary1.Add("eventSummary", "");
                taskDataList.Add(dictionary1);
            }
            else if (colorTabControl1.SelectedTab == tabInsertCheck)
            {
                colorTabControl1.SelectedTab = tabDeposit;
                if (textBox3.Text != "")
                    showInformation(ATMStrings.TakeBackYourChecks, tabDeposit);
                log.addAction("cancel saving by checks", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabTransfer
                || colorTabControl1.SelectedTab == tabBetweenMyAccounts)
            {
                textBoxCheckingToSaving.Text = "";
                textBoxSavingToChecking.Text = "";
                log.addAction("cancel transfer request", Action.BehaviorType.normal);
                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "ClearClick");
                dictionary1.Add("eventData", "tabTransfer");
                dictionary1.Add("eventSummary", "");
                taskDataList.Add(dictionary1);
            }
            else if (colorTabControl1.SelectedTab == tabPayeeAccount)
            {
                textBox5.Text = "";
                textBox6.Text = "";
                log.addAction("clear the payee's information in tranfer processing", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabModifyPin)
            {
                textBox7.Text = "";
                textBox8.Text = "";
                setPIN = true;
                log.addAction("clear the new PIN number", Action.BehaviorType.normal);
            }
            else if (colorTabControl1.SelectedTab == tabPin)
            {
                textBoxPIN.Text = "";
                //this should never happen
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "ClearClick");
                dictionary.Add("eventData", "Pin");
                dictionary.Add("eventSummary", "");
                taskDataList.Add(dictionary);
                log.addAction("clear PIN number", Action.BehaviorType.normal);
            }
        }

        private void buttonNumberEnter_Click(object sender, EventArgs e)
        {
            enterAction();
          
        }

        private void panelInstructionTop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public List<Dictionary<String,String>> getEventData()
        {
            return taskDataList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBoxCheckingToSaving.Visible == false)
            {
                textBoxCheckingToSaving.Visible = true;
                textBoxSavingToChecking.Visible = false;
            }
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBoxSavingToChecking.Visible == false)
            {
                textBoxSavingToChecking.Visible = true;
                textBoxCheckingToSaving.Visible = false;
            }

        }

        private void labelTransChecking_Click(object sender, EventArgs e)
        {

        }

       
    }
}
