using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WMPLib;
using System.Timers;
//
using System.Speech.Synthesis;
using System.Speech.AudioFormat;
namespace PrescriptionRefill
{

    public partial class Form1 : Form
    {
        Telephone telephone;
        private List<Dictionary<String, String>> taskDataList;
        ATM.Form1 form = new ATM.Form1();
        ATM.Form2 form2 = new ATM.Form2();
        int exitKeysCount = 0;
        bool shown;
        bool correct;
        System.Timers.Timer myTimer2;
        public static System.Timers.Timer myTimer3;
        bool startTimer, language, eligibleRefill, pickTime;
        string taskLanguage = "EN";

        public Form1(string taskLanguage)
        {
            this.taskLanguage = taskLanguage;
            this.telephone = new Telephone(this, null);
            InitializeComponent();
            correct = true;
            myTimer1 = new System.Timers.Timer(600000);
            myTimer1.Elapsed += new ElapsedEventHandler(ProcessTimer1);
            myTimer1.Start();
            myTimer1.AutoReset = false;
            taskDataList = new List<Dictionary<String, String>>();
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "TaskStart");
            dictionary.Add("eventSummary", "");
            dictionary.Add("eventData", "");
            taskDataList.Add(dictionary);
            startTimer = false;
            language = false;
            eligibleRefill = true;
            pickTime = true;

            //taken out of playTrack and placed in the constructor where (hopefully)
            //it'll only ever get called once
            player.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(player_PlayStateChange);
            //
            if (taskLanguage.Equals("ES"))
            {
                this.pictureBox1.BackgroundImage = global::PrescriptionRefill.Properties.Resources.Ampliex_ES;
                this.pictureBox2.BackgroundImage = global::PrescriptionRefill.Properties.Resources.Megalith_ES;
            }
           
        }

      

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (exitKeysCount == 0 && e.KeyCode == Keys.ControlKey){
                exitKeysCount = 1;
            }
            else if (exitKeysCount == 1 && e.KeyCode == Keys.ShiftKey){
                exitKeysCount = 2;
            }
            else if (exitKeysCount == 2 && e.KeyCode == Keys.X)
            {
                try
                {
                    myTimer1.Dispose();
                }
                catch
                {
                }
                try
                {
                    myTimer2.Dispose();
                }
                catch
                {
                }
                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "EscapeEntered");
                dictionary1.Add("eventData", "");
                dictionary1.Add("eventSummary", "");
                taskDataList.Add(dictionary1);
                this.Close();
                Application.Exit();
            }
            else
            {
                exitKeysCount = 0;
            }
        }
        System.Timers.Timer myTimer1;
       
        private void ProcessTimer1(Object obj, ElapsedEventArgs e)
        {
            myTimer2 = new System.Timers.Timer(60000);
            myTimer2.Elapsed += new ElapsedEventHandler(ProcessTimer2);
            myTimer2.Start();
            myTimer2.AutoReset = false;
            shown = true;
            form2.ShowBox("Do you need more time to complete this task?", "No", "Yes");
            shown = false;
            if (form2.yesClick == true)
            {
                myTimer1 = new System.Timers.Timer(600000);
                myTimer1.Elapsed += new ElapsedEventHandler(ProcessTimer1);
                myTimer1.Start();
                myTimer1.AutoReset = false;
                Dictionary<String, String> question1 = new Dictionary<String, String>();
                question1.Add("time", DateTime.Now.ToString());
                question1.Add("eventType", "ExtraTime");
                question1.Add("eventSummary", "");
                question1.Add("eventData", "");
                taskDataList.Add(question1);
            }
            else
            {
                Dictionary<String, String> question1 = new Dictionary<String, String>();
                question1.Add("time", DateTime.Now.ToString());
                question1.Add("eventType", "TaskClosed");
                question1.Add("eventData", "");
                question1.Add("eventSummary", "ClickedNo");
                taskDataList.Add(question1);
                try
                {
                    myTimer1.Dispose();
                }
                catch
                {
                }
                try
                {
                    myTimer2.Dispose();
                } catch {
                }
                this.Close();
                Application.Exit();
            }
            myTimer2.Stop();
            

        }

        private void ProcessTimer2(Object obj, ElapsedEventArgs e)
        {
            if (shown == true){
                Dictionary<String, String> question1 = new Dictionary<String, String>();
                question1.Add("time", DateTime.Now.ToString());
                question1.Add("eventType", "TaskClosed");
                question1.Add("eventData", "");
                question1.Add("eventSummary", "TimerExpired");
                taskDataList.Add(question1);
                try
                {
                    myTimer1.Dispose();
                }
                catch
                {
                }
                try
                {
                    myTimer2.Dispose();
                }
                catch
                {
                }
                this.Close();
                Application.Exit();
            }
        }

        private void buttonCall_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            lableWelcomeEn.Visible = false;
            lableWelcomeSp.Visible = false;
            buttonCall.Visible = false;

            panelPicture.Visible = false;
            this.tableLayoutPanel1.Controls.Add(this.panelPhonePicture, 1, 1);
            this.tableLayoutPanel1.SetRowSpan(this.panelPhonePicture, 4);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox2, 3, 3);

            this.tableLayoutPanel1.ColumnStyles.RemoveAt(0);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(0);
            this.tableLayoutPanel1.ColumnStyles.RemoveAt(0);    
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left;
           
            pictureBox1.Dock = DockStyle.None;
            pictureBox2.Dock = DockStyle.None;

            playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
            telephone.singleTrack = true;

            telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "CallHospital");
            dictionary.Add("eventData", "");
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);
            startTimer = true;
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            Size panelPictureSize = panelPicture.Size;
            Size pictureBoxSize = new Size(panelPicture.Size.Width/2,panelPicture.Size.Height);
            player.Visible = false;
            tableLayoutPanel1.Size = this.Size;
            playTrack(ConstValues.AudioFile.INTRODUCTION);
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown); 
        }

        public static void timerDispose()
        {
            try
            {
                myTimer3.Dispose();
            }
            catch
            {
            }
        }

        //performs actions based on current key being pressed
        //@param int numkey: number key pressed by user
        private void numberPad_Process(int numkey)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop(); 
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false;
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "1";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.SPANISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Spanish");
                    dictionary.Add("eventData", "1");
                    taskDataList.Add(dictionary);
                    telephone.refill();

                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Correct");
                    dictionary1.Add("eventData", "1");
                    taskDataList.Add(dictionary1);
                    telephone.enterRxNumber();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 1;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Correct");
                    dictionary3.Add("eventData", "Today");
                    taskDataList.Add(dictionary3);
                    telephone.today = true;
                    telephone.enterPickUpTime();
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 1;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();
                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Valid");
                            dictionary5.Add("eventData", "AM");
                            taskDataList.Add(dictionary5);
                    telephone.convertTo24Hour(false);
                    telephone.checkTimeRange();
                    if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                    {
                        Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                        dictionary6.Add("time", DateTime.Now.ToString());
                        dictionary6.Add("eventType", "DuringPharmacyHours");
                        dictionary6.Add("eventSummary", "Invalid");
                        dictionary6.Add("eventData", telephone.pickUpTime.ToString());
                        taskDataList.Add(dictionary6);
                    }
                    else if (telephone.curStatus == ConstValues.Status.START_OVER)
                    {
                        Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                        dictionary6.Add("time", DateTime.Now.ToString());
                        dictionary6.Add("eventType", "DuringPharmacyHours");
                        dictionary6.Add("eventSummary", "Valid");
                        dictionary6.Add("eventData", telephone.pickUpTime.ToString());
                        taskDataList.Add(dictionary6);
                    }
                    break;
                case ConstValues.Status.START_OVER:
                    if (correct == true)
                    {
                        Dictionary<String, String> dictionary7 = new Dictionary<String, String>();
                        dictionary7.Add("time", DateTime.Now.ToString());
                        dictionary7.Add("eventType", "RefillAnotherPrescription");
                        dictionary7.Add("eventSummary", "Correct");
                        dictionary7.Add("eventData", "");
                        taskDataList.Add(dictionary7);
                        correct = false;
                    }
                    else
                    {
                        Dictionary<String, String> dictionary7 = new Dictionary<String, String>();
                        dictionary7.Add("time", DateTime.Now.ToString());
                        dictionary7.Add("eventType", "RefillAnotherPrescription");
                        dictionary7.Add("eventSummary", "Incorrect");
                        dictionary7.Add("eventData", "");
                        taskDataList.Add(dictionary7);
                    }
                    telephone.enterRxNumber();
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;

        }


        private void button2_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false; 
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "2";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "English");
                    dictionary.Add("eventData", "2");
                    taskDataList.Add(dictionary);
                    telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                      Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "2");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.REFILL;
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 2;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Correct");
                    dictionary3.Add("eventData", "Tomorrow");
                    taskDataList.Add(dictionary3);
                    telephone.today = false;
                    telephone.enterPickUpTime();
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 2;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();
                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Valid");
                            dictionary5.Add("eventData", "PM");
                            taskDataList.Add(dictionary5);
                    telephone.convertTo24Hour(true);
                    telephone.checkTimeRange();
                    if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                    {
                        Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                        dictionary6.Add("time", DateTime.Now.ToString());
                        dictionary6.Add("eventType", "DuringPharmacyHours");
                        dictionary6.Add("eventSummary", "Invalid");
                        dictionary6.Add("eventData", telephone.pickUpTime.ToString());
                        taskDataList.Add(dictionary6);
                    }
                    else if (telephone.curStatus == ConstValues.Status.START_OVER)
                    {
                        Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                        dictionary6.Add("time", DateTime.Now.ToString());
                        dictionary6.Add("eventType", "DuringPharmacyHours");
                        dictionary6.Add("eventSummary", "Valid");
                        dictionary6.Add("eventData", telephone.pickUpTime.ToString());
                        taskDataList.Add(dictionary6);
                    }
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();
                    
                    Dictionary<String, String> dictionary7 = new Dictionary<String, String>();
                    dictionary7.Add("time", DateTime.Now.ToString());
                    dictionary7.Add("eventType", "RefillAnotherPrescription");
                    dictionary7.Add("eventSummary", "Incorrect");
                    dictionary7.Add("eventData", "2");
                    taskDataList.Add(dictionary7);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false;

            lablePhoneScreen.Text = lablePhoneScreen.Text + "3";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "3");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "3");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 3;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                    Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "3");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 3;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();
                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                    Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Invalid");
                            dictionary5.Add("eventData", "3");
                            taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();
                    
                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "3");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false; 
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "4";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "4");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "4");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 4;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                     Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "4");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 4;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();
                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                     Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Invalid");
                            dictionary5.Add("eventData", "4");
                            taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();
                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "4");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false; 
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "5";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "5");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "5");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 5;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                     Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "5");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 5;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();
                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                     Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Invalid");
                            dictionary5.Add("eventData", "5");
                            taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();
                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "5");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false; 
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "6";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "6");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "6");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 6;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                     Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "6");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 6;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();
                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                     Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Invalid");
                            dictionary5.Add("eventData", "6");
                            taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();

                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "6");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false; 
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "7";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "7");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "7");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 7;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                     Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "7");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 7;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();
                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                     Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Invalid");
                            dictionary5.Add("eventData", "7");
                            taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();

                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "7");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false;
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "8";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "8");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "8");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 8;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                     Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "8");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 8;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();
                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                     Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Invalid");
                            dictionary5.Add("eventData", "8");
                            taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();

                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "8");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false;
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "9";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "9");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "9");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10 + 9;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                     Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "9");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10 + 9;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();
                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                     Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Invalid");
                            dictionary5.Add("eventData", "9");
                            taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();

                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "4");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void buttonStar_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false;
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "*";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "*");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "*");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                        dictionary2.Add("time", DateTime.Now.ToString());
                        dictionary2.Add("eventType", "RxNumberEntered");
                        dictionary2.Add("eventSummary", "Incorrect");
                        dictionary2.Add("eventData", telephone.rxNumber.ToString());
                        taskDataList.Add(dictionary2);
                    telephone.errorRxNumber();
                    player.Ctlcontrols.stop();
                    telephone.enterRxNumber();
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:

                    Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "*");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    string time = telephone.pickUpTime.ToString();
                      Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                      dictionary4.Add("time", DateTime.Now.ToString());
                      dictionary4.Add("eventType", "PickUpTime");
                      dictionary4.Add("eventSummary", "Invalid");
                      dictionary4.Add("eventData", time);
                      taskDataList.Add(dictionary4);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.enterPickUpTime();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.AM_OR_PM:
                     Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Invalid");
                            dictionary5.Add("eventData", "*");
                            taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();

                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "*");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false;
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "0";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "0");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "0");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    telephone.rxNumber = telephone.rxNumber * 10;
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                     Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "0");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                    telephone.pickUpTime = telephone.pickUpTime * 10;
                    telephone.keyPressedCount++;
                    if (telephone.keyPressedCount == 4)
                    {
                        string time = telephone.pickUpTime.ToString();

                        telephone.validatePickUpTime();
                        if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Invalid");
                            dictionary4.Add("eventData", time);
                            taskDataList.Add(dictionary4);
                        }
                        else if (telephone.curStatus == ConstValues.Status.AM_OR_PM)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);
                        }
                        //Qingzhou starts adding here
                        else if (telephone.curStatus == ConstValues.Status.VALID_PM_TIME)
                        {
                            Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                            dictionary4.Add("time", DateTime.Now.ToString());
                            dictionary4.Add("eventType", "PickUpTime");
                            dictionary4.Add("eventSummary", "Valid");
                            dictionary4.Add("eventData", telephone.pickUpTime.ToString());
                            taskDataList.Add(dictionary4);

                            Dictionary<String, String> dictionary8 = new Dictionary<String, String>();
                            dictionary8.Add("time", DateTime.Now.ToString());
                            dictionary8.Add("eventType", "AMorPM");
                            dictionary8.Add("eventSummary", "Valid");
                            dictionary8.Add("eventData", "PM");
                            taskDataList.Add(dictionary8);

                            telephone.checkTimeRange();
                            if (telephone.curStatus == ConstValues.Status.PICK_UP_TIME)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Invalid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                            else if (telephone.curStatus == ConstValues.Status.START_OVER)
                            {
                                Dictionary<String, String> dictionary9 = new Dictionary<String, String>();
                                dictionary9.Add("time", DateTime.Now.ToString());
                                dictionary9.Add("eventType", "DuringPharmacyHours");
                                dictionary9.Add("eventSummary", "Valid");
                                dictionary9.Add("eventData", telephone.pickUpTime.ToString());
                                taskDataList.Add(dictionary9);
                            }
                        }
                        //Qingzhou ends adding here
                    }
                    break;
                case ConstValues.Status.AM_OR_PM:
                     Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                            dictionary5.Add("time", DateTime.Now.ToString());
                            dictionary5.Add("eventType", "AMorPM");
                            dictionary5.Add("eventSummary", "Invalid");
                            dictionary5.Add("eventData", "0");
                            taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();

                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "0");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void buttonPound_Click(object sender, EventArgs e)
        {

            language = true;
            player.Ctlcontrols.stop();
            playTrack(ConstValues.AudioFile.BUTTON_SOUND);
            telephone.singleTrack = false;
            
            lablePhoneScreen.Text = lablePhoneScreen.Text + "#";
            if (lablePhoneScreen.Text.Length > 10)
                lablePhoneScreen.Text = lablePhoneScreen.Text.Substring(lablePhoneScreen.Text.Length - 10, 10);
            switch (telephone.curStatus)
            {
                case ConstValues.Status.LANGUAGE_SELECTION:
                    playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
                    telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
                    //telephone.language = ConstValues.Language.ENGLISH;
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "LanguageSelection");
                    dictionary.Add("eventSummary", "Invalid");
                    dictionary.Add("eventData", "#");
                    taskDataList.Add(dictionary);
                    //telephone.refill();
                    break;
                case ConstValues.Status.REFILL:
                     Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "RefillSelection");
                    dictionary1.Add("eventSummary", "Incorrect");
                    dictionary1.Add("eventData", "#");
                    taskDataList.Add(dictionary1);
                    telephone.refill();
                    break;
                case ConstValues.Status.ENTER_RX_NUMBER:
                    if (telephone.isCorrectRxNumber())
                    {
                        Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                        dictionary2.Add("time", DateTime.Now.ToString());
                        dictionary2.Add("eventType", "RxNumberEntered");
                        dictionary2.Add("eventSummary", "Correct");
                        dictionary2.Add("eventData", telephone.rxNumber.ToString());
                        taskDataList.Add(dictionary2);
                        telephone.eligibleRxNumber();
                        timerDispose();

                    }
                    else
                    {
                        Dictionary<String, String> dictionary2 = new Dictionary<String, String>();
                        dictionary2.Add("time", DateTime.Now.ToString());
                        dictionary2.Add("eventType", "RxNumberEntered");
                        dictionary2.Add("eventSummary", "Incorrect");
                        dictionary2.Add("eventData", telephone.rxNumber.ToString());
                        taskDataList.Add(dictionary2);
                        telephone.errorRxNumber();
                        telephone.singleTrack = false;
                        telephone.enterRxNumber();
                        telephone.singleTrack = true;
                    }
                    break;
                case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                     Dictionary<String, String> dictionary3 = new Dictionary<String, String>();
                    dictionary3.Add("time", DateTime.Now.ToString());
                    dictionary3.Add("eventType", "PickUpDaySelection");
                    dictionary3.Add("eventSummary", "Incorrect");
                    dictionary3.Add("eventData", "#");
                    taskDataList.Add(dictionary3);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.eligibleRxNumber();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.PICK_UP_TIME:
                   
                    string time = telephone.pickUpTime.ToString();
                    Dictionary<String, String> dictionary4 = new Dictionary<String, String>();
                    dictionary4.Add("time", DateTime.Now.ToString());
                    dictionary4.Add("eventType", "PickUpTime");
                    dictionary4.Add("eventSummary", "Invalid");
                    dictionary4.Add("eventData", time);
                    taskDataList.Add(dictionary4);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.enterPickUpTime();
                    telephone.singleTrack = true;
                       
                   
                    break;
                case ConstValues.Status.AM_OR_PM:

                     Dictionary<String, String> dictionary5 = new Dictionary<String, String>();
                    dictionary5.Add("time", DateTime.Now.ToString());
                    dictionary5.Add("eventType", "AMorPM");
                    dictionary5.Add("eventSummary", "Invalid");
                    dictionary5.Add("eventData", "#");
                    taskDataList.Add(dictionary5);
                    telephone.errorInvalidChoice();
                    telephone.singleTrack = false;
                    telephone.askAmOrPm();
                    telephone.singleTrack = true;
                    break;
                case ConstValues.Status.START_OVER:
                    //telephone.errorInvalidChoice();

                    Dictionary<String, String> dictionary6 = new Dictionary<String, String>();
                    dictionary6.Add("time", DateTime.Now.ToString());
                    dictionary6.Add("eventType", "RefillAnotherPrescription");
                    dictionary6.Add("eventSummary", "Incorrect");
                    dictionary6.Add("eventData", "#");
                    taskDataList.Add(dictionary6);
                    telephone.refill();
                    telephone.curStatus = ConstValues.Status.START_OVER;
                    //telephone.singleTrack = false;
                    //telephone.singleTrack = true;
                    break;
                default:
                    break;
            }
            telephone.singleTrack = true;
        }
        private void buttonRestart_Click(object sender, EventArgs e)
        {
            telephone.rxNumber = 0;
            telephone.pickUpTime = 0;
            playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
            telephone.curStatus = ConstValues.Status.LANGUAGE_SELECTION;
            telephone.timerStart = DateTime.Now;
        }
        private void buttonHangUp_Click(object sender, EventArgs e)
        {
            player.Ctlcontrols.stop();
            telephone.goodBye();
            Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
            dictionary1.Add("time", DateTime.Now.ToString());
            dictionary1.Add("eventType", "TaskClosed");
            dictionary1.Add("eventSummary", "");
            dictionary1.Add("eventData", "");
            taskDataList.Add(dictionary1);
            try
            {
                myTimer1.Dispose();
            }
            catch
            {
            }
            try
            {
                myTimer2.Dispose();
            }
            catch
            {
            }
            this.Close();

        }

        public void playTrack(string trackName)
        {
#if DEBUG
            Console.WriteLine("telephone.singleTrack = " + telephone.singleTrack.ToString());
#endif
            if (telephone.singleTrack)
            {
                player.currentPlaylist.clear();
            }
            
            IWMPMedia audio = player.newMedia(trackName);
#if DEBUG
            Console.WriteLine("Appending " + trackName + " to playlist.");
#endif
            player.currentPlaylist.appendItem(audio);
#if DEBUG
            Console.WriteLine("player.currentPlaylist length = " + player.currentPlaylist.count.ToString());
#endif
            player.Ctlcontrols.playItem(player.currentPlaylist.Item[0]);
            
            timerDispose();
            
        }

        void player_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == 8)
            {
#if DEBUG
                Console.WriteLine("Just finished playing = " + player.currentMedia.name);
                Console.WriteLine(String.Format("newState = {0} ...", e.newState.ToString()));
#endif
                try
                {
                    myTimer3.Dispose();
                }
                catch
                {
                }
                
                if(!player.currentMedia.name.Equals("buttonSound"))
                {
                    myTimer3 = new System.Timers.Timer(10 * 1000);
                    myTimer3.Elapsed += new ElapsedEventHandler(ProcessTimer3);
                    myTimer3.Start();
                    myTimer3.AutoReset = false;
                }
            }
        }
        private void ProcessTimer3(Object obj, ElapsedEventArgs e)
        {
            Console.WriteLine("Here we go again...");
            if (startTimer == true && language == true)
            {
                try
                {
                    myTimer3.Dispose();
                }
                catch
                {
                }


                switch (telephone.curStatus)
                {
                    case ConstValues.Status.LANGUAGE_SELECTION:
                        telephone.refill();
                        break;
                    case ConstValues.Status.REFILL:
                        telephone.refill();
                        break;
                    case ConstValues.Status.ENTER_RX_NUMBER:
                        telephone.enterRxNumber();
                        break;
                    case ConstValues.Status.ELIGIBLE_RX_NUMBER:
                        if (eligibleRefill == true)
                        {
                            telephone.eligibleRxNumber();
                            telephone.singleTrack = true;

                        }
                        else
                        {
                            eligibleRefill = true;
                        }
                        break;
                    case ConstValues.Status.PICK_UP_TIME:
                        if (pickTime == true)
                        {
                            telephone.enterPickUpTime();
                            telephone.singleTrack = true;
                        }
                        else
                        {
                            pickTime = true;
                        }
                        break;
                    case ConstValues.Status.AM_OR_PM:
                        telephone.askAmOrPm();
                        telephone.singleTrack = true;
                        break;
                    case ConstValues.Status.START_OVER:
                        //telephone.readyToPickUp();

                        telephone.curStatus = ConstValues.Status.START_OVER;
                        if (telephone.language == ConstValues.Language.ENGLISH)
                        {
                            telephone.form.playTrack(ConstValues.AudioFile.START_OVER_EN);
                        }
                        else
                        {
                            telephone.form.playTrack(ConstValues.AudioFile.START_OVER_SP);
                        }
                        telephone.singleTrack = true;
                        Form1.timerDispose();
                        break;
                    default:
                        break;
                }
                telephone.singleTrack = true;
            }
            else if ((startTimer == true && language == false))
            {
                playTrack(ConstValues.AudioFile.LANGUAGE_SELECTION);
            }
            else
            {
                playTrack(ConstValues.AudioFile.INTRODUCTION);
            }
        }
        public void stopPlayer()
        {
            player.currentPlaylist.clear();
            player.Ctlcontrols.stop();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            tableLayoutPanel1.Size = this.Size;

        }

        public List<Dictionary<String, String>> getEventData()
        {
            return taskDataList;
        }
    }
}
