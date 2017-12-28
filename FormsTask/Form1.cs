using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using ATM;
using System.Threading;
using System.Globalization;

namespace FormsTask
{
    public partial class Form1 : Form
    {
        private List<Dictionary<String, String>> taskDataList;
        ATM.Form1 form = new ATM.Form1();
        ATM.Form2 form2 = new ATM.Form2();
        Answer answer;
        int currentPage;
        int exitKeysCount = 0;
        bool shown;
        string taskLanguage = "EN";
        System.Timers.Timer myTimer1, myTimer2;
        int moreTimePromptDelay = 3 * 60 * 1000; //3 minutes
      
        public Form1()
        {
            InitializeComponent();
            answer = new Answer();
            currentPage = Constants.PAGE_WELCOME;
            taskDataList = new List<Dictionary<String, String>>();
            myTimer1 = new System.Timers.Timer(moreTimePromptDelay);
            myTimer1.Elapsed += new ElapsedEventHandler(ProcessTimer1);
            
            myTimer1.AutoReset = false;
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "TaskStart");
            dictionary.Add("eventData", "");
            dictionary.Add("eventSummary", "");
            taskDataList.Add(dictionary);
            //  
            
            
        }

        public Form1(string taskLanguage)
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
                System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
            }
        }


        //this method allows the user to escape back to the Task Select Menu
        //using the hotkey sequence:
        // Ctrl+Shift+\
        //
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
               
                try
                {

                    myTimer1.Dispose();
                }
                catch
                {
                }
                try
                {
                    //myTimer2.Dispose();
                }
                catch
                {
                }
                Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                dictionary1.Add("time", DateTime.Now.ToString());
                dictionary1.Add("eventType", "TaskClosed");
                dictionary1.Add("eventData", "");
                dictionary1.Add("eventSummary", "EscapeEntered");
                taskDataList.Add(dictionary1);
                this.Close();
                Application.Exit();
            }
            else
            {
                exitKeysCount = 0;
            }
        }


        /*
         * fires on myTimer1.Elapsed
         * prompts user for more time
         * if yes, continues Forms task and records 'ExtraTime' eventType
         * else, closes Forms task
         */
        private void ProcessTimer1(Object obj, ElapsedEventArgs e)
        {
            //myTimer2 = new System.Timers.Timer(60 * 1000);
            //myTimer2.Elapsed += new ElapsedEventHandler(ProcessTimer2);
            //myTimer2.Start();
            //myTimer2.AutoReset = false;
            shown = true;
            form2.ShowBox("Do you need more time to complete this task?", "No", "Yes");
            shown = false; //wtf is this??!?!
            
            if (form2.yesClick == true)
            {
                //System.Timers.Timer myTimer1 = new System.Timers.Timer(this.moreTimePromptDelay);
                //myTimer1.Elapsed += new ElapsedEventHandler(ProcessTimer1);
                this.myTimer1.Start();
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

                Dictionary<String, String> question1 = new Dictionary<String, String>();
                question1.Add("time", DateTime.Now.ToString());
                question1.Add("eventType", "SkippingPage"+currentPage);
                question1.Add("eventData", "");
                question1.Add("eventSummary", "ClickedNo");
                taskDataList.Add(question1);
                try
                {
                    if (panelWelcome.InvokeRequired)
                    {
                        //this.setAnswer(); //<-- we need to call this so that answers will be recorded, even on a skip
                        panelWelcome.Invoke(new MethodInvoker(delegate 
                            {
                                setAnswerVariablesOnPageSkip();
                                this.moveToNextSubTask(); 
                            }));
                    }
                   
                }
                catch
                {
                }
              
            }

        }


        /*
         * closes this form and (hopefully) brings user back to Task Select Menu
         */
        private void endButton_Click(object sender, EventArgs e)
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
                //myTimer2.Dispose();
            }
            catch
            {
            }
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.panelWelcome.Size = this.Size;
            this.panelGroup1.Size = this.Size;
            this.panelGroup2.Size = this.Size;
            this.panelGroup3.Size = this.Size;
            this.KeyPreview = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
           
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            this.panelWelcome.Size = this.Size;
            this.panelGroup1.Size = this.Size;
            this.panelGroup2.Size = this.Size;
            this.panelGroup3.Size = this.Size;
        }

        private void buttonContinueEn_Click(object sender, EventArgs e)
        {
            answer.formLanguage = Constants.ENGLISH;
            setFormText(Constants.ENGLISH);
            this.Controls.Remove(panelWelcome);
            this.Controls.Add(this.panelGroup1);
            this.panelGroup1.Controls.Add(this.labelInstruction, 2, 0);
            this.labelInstruction.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelGroup1.Controls.Add(this.panelButtons, 1, this.panelGroup1.RowCount - 2);
            this.panelGroup1.SetColumnSpan(this.panelButtons, this.panelGroup1.ColumnCount - 2);
            
            currentPage = Constants.PAGE_GROUP_1; 
            myTimer1.Start();
            //
        }

        private void buttonContinueSp_Click(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("es"); //just to make sure...
            answer.formLanguage = Constants.SPANISH;
            setFormText(Constants.SPANISH);
            this.Controls.Remove(panelWelcome);
            this.Controls.Add(this.panelGroup1);
            this.panelGroup1.Controls.Add(this.labelInstruction, 2, 0);
            this.labelInstruction.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelGroup1.Controls.Add(this.panelButtons, 1, this.panelGroup1.RowCount - 2);
            this.panelGroup1.SetColumnSpan(this.panelButtons, this.panelGroup1.ColumnCount -2);
            currentPage = Constants.PAGE_GROUP_1;
            myTimer1.Start();
        }

        private void setFormText(int language)
        {
            if (language == Constants.ENGLISH)
            {
                this.labelQuestion_1.Text = Constants.EnglishText.QUESTION_TITLE_1;
                this.labelQuestion_2.Text = Constants.EnglishText.QUESTION_TITLE_2;
                this.labelQuestion_3.Text = Constants.EnglishText.QUESTION_TITLE_3;
                this.labelQuestion_4.Text = Constants.EnglishText.QUESTION_TITLE_4;
                this.labelQuestion_5.Text = Constants.EnglishText.QUESTION_TITLE_5;
                this.labelQuestion_6.Text = Constants.EnglishText.QUESTION_TITLE_6;
                this.labelQuestion_7.Text = Constants.EnglishText.QUESTION_TITLE_7;
                this.labelQuestion_8.Text = Constants.EnglishText.QUESTION_TITLE_8;
                this.labelQuestion_9.Text = Constants.EnglishText.QUESTION_TITLE_9;
                this.labelQuestion_10.Text = Constants.EnglishText.QUESTION_TITLE_10;
                this.labelQuestion_10_A.Text = Constants.EnglishText.QUESTION_TITLE_10_A;
                this.labelQuestion_10_B.Text = Constants.EnglishText.QUESTION_TITLE_10_B;
                this.labelQuestion_11.Text = Constants.EnglishText.QUESTION_TITLE_11;
                this.radioButtonRaceOther.Text = Constants.EnglishText.OTHER;
                this.radioButtonAfricanAmerican.Text = Constants.EnglishText.AFRICAN_AMERICAN;
                this.radioButtonCaucasian.Text = Constants.EnglishText.CAUCASIAN;
                this.radioButtonFemale.Text = Constants.EnglishText.FEMALE;
                this.radioButtonLatino.Text = Constants.EnglishText.YES;
                this.radioButtonLatinoNo.Text = Constants.EnglishText.NO;
                this.radioButtonMale.Text = Constants.EnglishText.MALE;
                this.radioButtonMiddleSchool.Text = Constants.EnglishText.MIDDLE_SCHOOL;
                this.radioButtonHighSchool.Text = Constants.EnglishText.HIGH_SCHOOL;
                this.radioButtonHighSchoolDiploma.Text = Constants.EnglishText.HIGH_SCHOOL_DIPLOMA;
                this.radioButtonCollege.Text = Constants.EnglishText.COLLEGE;
                this.radioButtonCollegeDegree.Text = Constants.EnglishText.COLLEGE_DEGREE;
                this.radioButtonHigher.Text = Constants.EnglishText.HIGHER;
                this.radioButtonEducationIDK.Text = Constants.EnglishText.IDK;
                this.radioButtonEnglish.Text = Constants.EnglishText.ENGLISH;
                this.radioButtonSpanish.Text = Constants.EnglishText.SPANISH;
                this.radioButtonLanguageOther.Text = Constants.EnglishText.OTHER;
                this.radioButtonPartTime.Text = Constants.EnglishText.PART_TIME;
                this.radioButtonFullTime.Text = Constants.EnglishText.FULL_TIME;
                this.radioButtonUnemployed.Text = Constants.EnglishText.UNEMPLOYED;
                this.radioButtonRetired.Text = Constants.EnglishText.RETIRED;
                this.radioButtonOneMonth.Text = Constants.EnglishText.ONE_MONTHS;
                this.radioButtonTwoMonths.Text = Constants.EnglishText.TWO_MONTHS;
                this.radioButtonSixMonths.Text = Constants.EnglishText.SIX_MONTHS;
                this.radioButtonOneYear.Text = Constants.EnglishText.ONE_YEAR;
                this.radioButtonTwoYear.Text = Constants.EnglishText.TWO_YEAR;
                this.radioButtonMore.Text = Constants.EnglishText.MORE;
                this.radioButtonUnemploymentIDK.Text = Constants.EnglishText.IDK;
                this.radioButtonEverWorked.Text = Constants.EnglishText.YES;
                this.radioButtonEverWorkedNo.Text = Constants.EnglishText.NO;
                this.radioButtonAlone.Text= Constants.EnglishText.ALONE;
                this.radioButtonFriendOrfamily.Text= Constants.EnglishText.FRIEND_OR_FAMILY;
                this.radioButtonSupervised.Text= Constants.EnglishText.SUPERVISED_FACILITY;
                this.radioButtonUnsupervised.Text= Constants.EnglishText.UNSUPERVISED_FACILITY;
                this.buttonNext.Text = Constants.EnglishText.NEXT_PAGE;
                this.buttonPrevious.Text = Constants.EnglishText.PREVIOUS_PAGE;
                this.labelInstruction.Text = Constants.EnglishText.Instruction;
            }
            else
            {
                this.labelQuestion_1.Text = Constants.SpanishText.QUESTION_TITLE_1;
                this.labelQuestion_2.Text = Constants.SpanishText.QUESTION_TITLE_2;
                this.labelQuestion_3.Text = Constants.SpanishText.QUESTION_TITLE_3;
                this.labelQuestion_4.Text = Constants.SpanishText.QUESTION_TITLE_4;
                this.labelQuestion_5.Text = Constants.SpanishText.QUESTION_TITLE_5;
                this.labelQuestion_6.Text = Constants.SpanishText.QUESTION_TITLE_6;
                this.labelQuestion_7.Text = Constants.SpanishText.QUESTION_TITLE_7;
                this.labelQuestion_8.Text = Constants.SpanishText.QUESTION_TITLE_8;
                this.labelQuestion_9.Text = Constants.SpanishText.QUESTION_TITLE_9;
                this.labelQuestion_10.Text = Constants.SpanishText.QUESTION_TITLE_10;
                this.labelQuestion_10_A.Text = Constants.SpanishText.QUESTION_TITLE_10_A;
                this.labelQuestion_10_B.Text = Constants.SpanishText.QUESTION_TITLE_10_B;
                this.labelQuestion_11.Text = Constants.SpanishText.QUESTION_TITLE_11;
                this.radioButtonRaceOther.Text = Constants.SpanishText.OTHER;
                this.radioButtonAfricanAmerican.Text = Constants.SpanishText.AFRICAN_AMERICAN;
                this.radioButtonCaucasian.Text = Constants.SpanishText.CAUCASIAN;
                this.radioButtonFemale.Text = Constants.SpanishText.FEMALE;
                this.radioButtonLatino.Text = Constants.SpanishText.YES;
                this.radioButtonLatinoNo.Text = Constants.SpanishText.NO;
                this.radioButtonMale.Text = Constants.SpanishText.MALE;
                this.radioButtonMiddleSchool.Text = Constants.SpanishText.MIDDLE_SCHOOL;
                this.radioButtonHighSchool.Text = Constants.SpanishText.HIGH_SCHOOL;
                this.radioButtonHighSchoolDiploma.Text = Constants.SpanishText.HIGH_SCHOOL_DIPLOMA;
                this.radioButtonCollege.Text = Constants.SpanishText.COLLEGE;
                this.radioButtonCollegeDegree.Text = Constants.SpanishText.COLLEGE_DEGREE;
                this.radioButtonHigher.Text = Constants.SpanishText.HIGHER;
                this.radioButtonEducationIDK.Text = Constants.SpanishText.IDK;
                this.radioButtonEnglish.Text = Constants.SpanishText.ENGLISH;
                this.radioButtonSpanish.Text = Constants.SpanishText.SPANISH;
                this.radioButtonLanguageOther.Text = Constants.SpanishText.OTHER;
                this.radioButtonPartTime.Text = Constants.SpanishText.PART_TIME;
                this.radioButtonFullTime.Text = Constants.SpanishText.FULL_TIME;
                this.radioButtonUnemployed.Text = Constants.SpanishText.UNEMPLOYED;
                this.radioButtonRetired.Text = Constants.SpanishText.RETIRED;
                this.radioButtonOneMonth.Text = Constants.SpanishText.ONE_MONTHS;
                this.radioButtonTwoMonths.Text = Constants.SpanishText.TWO_MONTHS;
                this.radioButtonSixMonths.Text = Constants.SpanishText.SIX_MONTHS;
                this.radioButtonOneYear.Text = Constants.SpanishText.ONE_YEAR;
                this.radioButtonTwoYear.Text = Constants.SpanishText.TWO_YEAR;
                this.radioButtonMore.Text = Constants.SpanishText.MORE;
                this.radioButtonUnemploymentIDK.Text = Constants.SpanishText.IDK;
                this.radioButtonEverWorked.Text = Constants.SpanishText.YES;
                this.radioButtonEverWorkedNo.Text = Constants.SpanishText.NO;
                this.radioButtonAlone.Text = Constants.SpanishText.ALONE;
                this.radioButtonFriendOrfamily.Text = Constants.SpanishText.FRIEND_OR_FAMILY;
                this.radioButtonSupervised.Text = Constants.SpanishText.SUPERVISED_FACILITY;
                this.radioButtonUnsupervised.Text = Constants.SpanishText.UNSUPERVISED_FACILITY;
                this.buttonNext.Text = Constants.SpanishText.NEXT_PAGE;
                this.buttonPrevious.Text = Constants.SpanishText.PREVIOUS_PAGE;
                this.labelInstruction.Text = Constants.SpanishText.Instruction;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (!setAnswer())
            {
                return;
            }
            else
            {
                moveToNextSubTask();
            }
        }

        private void moveToNextSubTask()
        {
            myTimer1.Stop();
            //no need to start the timer if we're leaving the Forms Task....
            if (currentPage != Constants.PAGE_GROUP_3)
            {
                myTimer1.Start();
            }
            
            if (currentPage == Constants.PAGE_GROUP_1)
            {
                this.Controls.Remove(this.panelGroup1);
                this.Controls.Add(this.panelGroup2);
                currentPage = Constants.PAGE_GROUP_2;
                this.panelGroup2.Controls.Add(this.labelInstruction, 2, 0);
                this.panelGroup2.SetColumnSpan(this.labelInstruction, 3);
                this.labelInstruction.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.panelGroup2.SetColumnSpan(this.panelButtons, 3);
                this.panelButtons.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
                this.panelGroup2.Controls.Add(this.panelButtons, 1, this.panelGroup2.RowCount - 2);
                this.panelGroup2.SetColumnSpan(this.panelButtons, 3);
                this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
                
            }
            else if (currentPage == Constants.PAGE_GROUP_2)
            {
                this.panelGroup3.Controls.Clear();
                while (this.panelGroup3.RowCount > 0)
                {
                    this.panelGroup3.RowStyles.RemoveAt(0);
                    this.panelGroup3.RowCount--;
                }
                if (answer.employmentStatus == Constants.UNEMPLOYED)
                {
                    this.panelGroup3.RowCount = 10;
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
                    this.panelGroup3.Controls.Add(this.labelQuestion_10_A, 1, 1);
                    this.panelGroup3.Controls.Add(this.groupBoxUnemployment, 2, 2);
                    this.panelGroup3.Controls.Add(this.labelQuestion_10_B, 1, 3);
                    this.panelGroup3.Controls.Add(this.groupBoxEverWorked, 2, 4);
                    this.panelGroup3.Controls.Add(this.labelQuestion_11, 1, 5);
                    this.panelGroup3.Controls.Add(this.groupBoxLivingStatus, 2, 6);
                    this.panelGroup3.SetColumnSpan(this.labelQuestion_10_A, 2);
                    this.panelGroup3.SetColumnSpan(this.labelQuestion_10_B, 2);
                    this.panelGroup3.SetColumnSpan(this.labelQuestion_11, 2);
                }
                else if (answer.employmentStatus == Constants.FULL_TIME || answer.employmentStatus == Constants.PART_TIME)
                {
                    this.panelGroup3.Controls.Add(this.labelQuestion_10, 1, 1);
                    this.panelGroup3.Controls.Add(this.textBoxCurrentJob, 2, 2);
                    this.panelGroup3.Controls.Add(this.labelQuestion_11, 1, 3);
                    this.panelGroup3.Controls.Add(this.groupBoxLivingStatus, 2, 4);
                    this.panelGroup3.RowCount = 8;
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));

                    this.panelGroup3.SetColumnSpan(this.labelQuestion_10, 2);
                    this.panelGroup3.SetColumnSpan(this.labelQuestion_11, 2);
                }
                else if (answer.employmentStatus == Constants.RETIRED || answer.employmentStatus == Constants.NOT_ANSWERED) //kludgy hack for when user skips this page
                {
                    this.panelGroup3.Controls.Add(this.labelQuestion_11, 1, 1);
                    this.panelGroup3.Controls.Add(this.groupBoxLivingStatus, 2, 2);
                    this.panelGroup3.RowCount = 6;
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 56F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6F));
                    this.panelGroup3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12F));
                    this.panelGroup3.SetColumnSpan(this.labelQuestion_11, 2);
                } //kludgy hack for when user skips
                

                this.Controls.Remove(this.panelGroup2);
                this.Controls.Add(this.panelGroup3);
                currentPage = Constants.PAGE_GROUP_3;
                this.panelGroup3.Controls.Add(this.labelInstruction, 1, 0);
                this.labelInstruction.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.panelGroup3.Controls.Add(this.panelButtons, 1, this.panelGroup3.RowCount - 2); Console.WriteLine(this.panelGroup3.RowCount.ToString());
                this.panelGroup3.SetColumnSpan(this.panelButtons, 3);
                this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
                if (answer.formLanguage == Constants.ENGLISH)
                {
                    buttonNext.Text = Constants.EnglishText.SUBMIT_FORM;
                }
                else
                {
                    buttonNext.Text = Constants.SpanishText.SUBMIT_FORM;
                }
                Console.WriteLine(panelButtons.Location.Y.ToString());
                Console.WriteLine(panelButtons.Size.Height.ToString());
            }
            else if (currentPage == Constants.PAGE_GROUP_3)
            {

                this.Controls.Remove(this.panelGroup3);
                this.Controls.Add(this.labelEnd);
                currentPage = Constants.PAGE_END;
                form.ShowBox(FormStrings.ThankYouMessage, FormStrings.Exit);
                getAnswers(taskDataList);
                Dictionary<String, String> question1 = new Dictionary<String, String>();
                question1.Add("time", DateTime.Now.ToString());
                question1.Add("eventType", "TaskClosed");
                question1.Add("eventData", "");
                question1.Add("eventSummary", "");
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
                    //myTimer2.Dispose();
                }
                catch
                {
                }
                this.Close();
                Application.Exit();
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            myTimer1.Stop();
            myTimer1.Start();
            if (answer.formLanguage == Constants.ENGLISH)
            {
                buttonNext.Text = Constants.EnglishText.NEXT_PAGE;
            }
            else
            {
                buttonNext.Text = Constants.SpanishText.NEXT_PAGE;
            }

            if (currentPage == Constants.PAGE_GROUP_1)
            {
                this.Controls.Remove(this.panelGroup1);
                this.Controls.Add(this.panelWelcome);
                currentPage = Constants.PAGE_WELCOME;
            }
            else if (currentPage == Constants.PAGE_GROUP_2)
            {
                this.Controls.Remove(panelGroup2);
                this.Controls.Add(this.panelGroup1);
                this.panelGroup1.Controls.Add(this.labelInstruction, 2, 0);
                this.labelInstruction.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.panelGroup1.Controls.Add(this.panelButtons, 1, this.panelGroup1.RowCount - 2);
                this.panelGroup1.SetColumnSpan(this.panelButtons, this.panelGroup1.ColumnCount - 2);
                currentPage = Constants.PAGE_GROUP_1;
                
            }
            else if (currentPage == Constants.PAGE_GROUP_3)
            {
                this.Controls.Remove(this.panelGroup3);
                this.Controls.Add(this.panelGroup2);
                currentPage = Constants.PAGE_GROUP_2;
                this.panelGroup2.Controls.Add(this.labelInstruction, 2, 0);
                this.panelGroup2.SetColumnSpan(this.labelInstruction, 3);
                this.labelInstruction.Anchor = System.Windows.Forms.AnchorStyles.None;
                this.panelGroup2.Controls.Add(this.panelButtons, 1, this.panelGroup2.RowCount - 2);
                this.panelGroup2.SetColumnSpan(this.panelButtons, 3);
                this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            } 
        }

        //returns bool:
        //if all answers filled out + valid, return true
        //else false
        private bool setAnswer()
        {
            if (currentPage == Constants.PAGE_GROUP_1)
            {
                if (textBoxAge.Text == Constants.NOT_ANSWERED_STR)
                {
                    showEntryErrorMessage("age", 1);
                    return false;
                }
                   
                if (!radioButtonFemale.Checked && !radioButtonMale.Checked)
                {
                    showEntryErrorMessage("gender", 3); 
                    return false;
                }

                if (!radioButtonCaucasian.Checked && !radioButtonAfricanAmerican.Checked && !radioButtonRaceOther.Checked)
                {
                    showEntryErrorMessage("race", 4);
                    return false;
                }

                if (!radioButtonLatino.Checked && !radioButtonLatinoNo.Checked)
                {
                    showErrorMessage(FormStrings.SpecifyIfHispanic, FormStrings.EntryError, 5);
                    return false;
                }

                int age = Int32.Parse(this.textBoxAge.Text);

                if (age < 18 || age > 200)
                {
                    if(answer.formLanguage == Constants.ENGLISH)
                    showErrorMessage("Age should be between 18 and 200.", "Entry Error!", -1);
                    else
                        showErrorMessage("Edad debe estar entre 18 y 200.", "Entry Error!", -1);
                    return false;
                }

                answer.age = age;
                answer.gender = radioButtonFemale.Checked ? Constants.FEMALE : Constants.MALE;
                if(radioButtonCaucasian.Checked)
                {
                    answer.race = Constants.CAUCASIAN;
                }
                else if (radioButtonAfricanAmerican.Checked)
                {
                    answer.race = Constants.AFRICAN_AMERICAN;
                }
                else if (radioButtonRaceOther.Checked)
                {
                    answer.race = Constants.OTHER;
                    answer.otherRace = textBoxRaceOther.Text;
                }
                answer.latino = radioButtonLatino.Checked ? Constants.YES : Constants.NO;     
            }
            else if (currentPage == Constants.PAGE_GROUP_2)
            {
                bool educationChecked = false;
                foreach (Control control in groupBoxEducation.Controls)                
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null)
                    {
                        if (radioButton.Checked)
                        {
                            educationChecked = true;
                            break;
                        }
                    }
                }
                if (!educationChecked)
                {
                    showEntryErrorMessage("highest Education level", 6);
                    return false;
                }

                if (textBoxEducationDuration.Text == Constants.NOT_ANSWERED_STR || textBoxEducationDuration.TextLength > 2)
                {
                    showErrorMessage(FormStrings.SpecifyYearsOfEducation, FormStrings.EntryError, 7);
                    return false;
                }
                              
                bool languageChecked = false;
                foreach (Control control in groupBoxLanguage.Controls)
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null)
                    {
                        if (radioButton.Checked)
                        {
                            languageChecked = true;
                            break;
                        }
                    }
                }
                if (!languageChecked)
                {
                    showEntryErrorMessage("primary language", 8);
                    return false;
                }

                bool employmentChecked = false;
                foreach (Control control in groupBoxEmployment.Controls)
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null)
                    {
                        if (radioButton.Checked)
                        {
                            employmentChecked = true;
                            break;
                        }
                    }
                }
                if (!employmentChecked)
                {
                    showEntryErrorMessage("current employment status", 9);
                    return false;
                }

                if (radioButtonMiddleSchool.Checked)
                {
                    answer.education = Constants.MIDDLE_SCHOOL;
                }
                else if (radioButtonHighSchool.Checked)
                {
                    answer.education = Constants.HIGH_SCHOOL;
                }
                else if (radioButtonHighSchoolDiploma.Checked)
                {
                    answer.education = Constants.HIGH_SCHOOL_DIPLOMA;
                }
                else if (radioButtonCollege.Checked)
                {
                    answer.education = Constants.COLLEGE;
                }
                else if (radioButtonCollegeDegree.Checked)
                {
                    answer.education = Constants.COLLEGE_DEGREE;
                }
                else if (radioButtonHigher.Checked)
                {
                    answer.education = Constants.HIGHER;
                }
                if (radioButtonMiddleSchool.Checked)
                {
                    answer.education = Constants.MIDDLE_SCHOOL;
                }
                answer.educationDuration = Int32.Parse(textBoxEducationDuration.Text);

                if (radioButtonEnglish.Checked)
                {
                    answer.primaryLanguage = Constants.ENGLISH;
                } 
                else if (radioButtonSpanish.Checked)
                {
                    answer.primaryLanguage = Constants.SPANISH;
                }
                else if (radioButtonLanguageOther.Checked)
                {
                    answer.primaryLanguage = Constants.OTHER;
                    answer.primaryLanguageOther = textBoxLanguageOther.Text;
                }

                if(radioButtonPartTime.Checked)
                {
                    answer.employmentStatus = Constants.PART_TIME;
                }
                else if(radioButtonFullTime.Checked)
                {
                    answer.employmentStatus = Constants.FULL_TIME;
                }
                else if(radioButtonUnemployed.Checked)
                {
                    answer.employmentStatus = Constants.UNEMPLOYED;
                }                                
                else if(radioButtonRetired.Checked)
                {
                    answer.employmentStatus = Constants.RETIRED;
                }
            }
            else if (currentPage == Constants.PAGE_GROUP_3)
            {
                if (answer.employmentStatus == Constants.UNEMPLOYED)
                {
                    bool unEmploymentDurationChecked = false;
                    foreach (Control control in groupBoxUnemployment.Controls)
                    {
                        RadioButton radioButton = control as RadioButton;
                        if (radioButton != null)
                        {
                            if (radioButton.Checked)
                            {
                                unEmploymentDurationChecked = true;
                                break;
                            }
                        }
                    }
                    
                    if(!unEmploymentDurationChecked)
                    {
                        showEntryErrorMessage("Unemployment Duration", 10);
                        return false;
                    }
                                        
                    bool everWorkedChecked = false;
                    foreach (Control control in groupBoxEverWorked.Controls)
                    {
                        RadioButton radioButton = control as RadioButton;
                        if (radioButton != null)
                        {
                            if (radioButton.Checked)
                            {
                                everWorkedChecked = true;
                                break;
                            }
                        }
                    }
                                        
                    if(!everWorkedChecked)
                    {
                        showErrorMessage(FormStrings.SpecifyIfEverWorked, FormStrings.EntryError, 10);
                        return false;
                    }

                    if (radioButtonOneMonth.Checked)
                    {
                        answer.unemploymentDuration = Constants.ONE_MONTH;
                    }
                    else if (radioButtonTwoMonths.Checked)
                    {
                        answer.unemploymentDuration = Constants.TWO_MONTHS;
                    }
                    else if (radioButtonSixMonths.Checked)
                    {
                        answer.unemploymentDuration = Constants.SIX_MONTHS;
                    }
                    else if (radioButtonOneYear.Checked)
                    {
                        answer.unemploymentDuration = Constants.ONE_YEAR;
                    }
                    else if (radioButtonTwoYear.Checked)
                    {
                        answer.unemploymentDuration = Constants.TWO_YEAR;
                    }
                    else if (radioButtonMore.Checked)
                    {
                        answer.unemploymentDuration = Constants.MORE;
                    }
                    else if (radioButtonUnemploymentIDK.Checked)
                    {
                        answer.unemploymentDuration = Constants.IDK;
                    }

                    answer.currentJob = Constants.NOT_ANSWERED_STR;
                }
                else if (answer.employmentStatus == Constants.FULL_TIME || answer.employmentStatus == Constants.PART_TIME)
                {
                    if (textBoxCurrentJob.Text == Constants.NOT_ANSWERED_STR)
                    {
                        showEntryErrorMessage("Current Job", 10);
                        return false;
                    }
                    answer.currentJob = textBoxCurrentJob.Text;
                    answer.unemploymentDuration = Constants.NOT_ANSWERED;
                    answer.everWorked = Constants.NOT_ANSWERED;
                }

                bool livingStatusChecked = false;
                foreach (Control control in groupBoxLivingStatus.Controls)
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null)
                    {
                        if (radioButton.Checked)
                        {
                            livingStatusChecked = true;
                            break;
                        }
                    }
                }

                if (!livingStatusChecked)
                {
                    showEntryErrorMessage("Living Status", 11);
                    return false;
                }

                if (radioButtonAlone.Checked)
                {
                    answer.livingStatus = Constants.TWO_YEAR;
                }
                else if (radioButtonFriendOrfamily.Checked)
                {
                    answer.livingStatus = Constants.FRIEND_OR_FAMILY;
                }
                else if (radioButtonSupervised.Checked)
                {
                    answer.livingStatus = Constants.SUPERVISED_FACILITY;
                }
                else if (radioButtonUnsupervised.Checked)
                {
                    answer.livingStatus = Constants.UNSUPERVISED_FACILITY;
                }
            }

            return true;
        }


        //sets answer variables 
        //if invalid string input, eg age out of bounds or not a number, enter -999
        //remember, age default value is Constants.NOT_ANSWERED (i.e., -1)
        private void setAnswerVariablesOnPageSkip()
        {
            if (currentPage == Constants.PAGE_GROUP_1)
            {

                //AGE
                if (!textBoxAge.Text.Equals(Constants.NOT_ANSWERED_STR))
                {
                    try
                    {
                        int age = Int32.Parse(this.textBoxAge.Text);
                        answer.age = (age < 18 || age > 200) ?  Constants.INVALID_INPUT : age;
                    }
                    catch (Exception ex) //string not convertible to int, most likely
                    {
                        answer.age = Constants.INVALID_INPUT;
                    }
                }
                //
                //GENDER
                if (radioButtonFemale.Checked || radioButtonMale.Checked)
                {
                    answer.gender = radioButtonFemale.Checked ? Constants.FEMALE : Constants.MALE;
                }
               
                //
                //RACE
                if (radioButtonCaucasian.Checked || radioButtonAfricanAmerican.Checked || radioButtonRaceOther.Checked)
                {
                    if (radioButtonCaucasian.Checked)
                    {
                        answer.race = Constants.CAUCASIAN;
                    }
                    else if (radioButtonAfricanAmerican.Checked)
                    {
                        answer.race = Constants.AFRICAN_AMERICAN;
                    }
                    else if (radioButtonRaceOther.Checked)
                    {
                        answer.race = Constants.OTHER;
                        answer.otherRace = textBoxRaceOther.Text; 
                    }
                }
                
                //
                //HISPANIC/LATINO
                if (radioButtonLatino.Checked || radioButtonLatinoNo.Checked)
                {
                    answer.latino = radioButtonLatino.Checked ? Constants.YES : Constants.NO;
                }
               
                //        
            }
            //
            else if (currentPage == Constants.PAGE_GROUP_2)
            {
                //HIGHEST LEVEL OF EDUCATION
                bool educationChecked = false;
                foreach (Control control in groupBoxEducation.Controls)
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null && radioButton.Checked)
                    {
                        educationChecked = true;
                    }
                }
                if (educationChecked)
                {
                    if (radioButtonMiddleSchool.Checked)
                    {
                        answer.education = Constants.MIDDLE_SCHOOL;
                    }
                    else if (radioButtonHighSchool.Checked)
                    {
                        answer.education = Constants.HIGH_SCHOOL;
                    }
                    else if (radioButtonHighSchoolDiploma.Checked)
                    {
                        answer.education = Constants.HIGH_SCHOOL_DIPLOMA;
                    }
                    else if (radioButtonCollege.Checked)
                    {
                        answer.education = Constants.COLLEGE;
                    }
                    else if (radioButtonCollegeDegree.Checked)
                    {
                        answer.education = Constants.COLLEGE_DEGREE;
                    }
                    else if (radioButtonHigher.Checked)
                    {
                        answer.education = Constants.HIGHER;
                    }
                    else if (radioButtonMiddleSchool.Checked)
                    {
                        answer.education = Constants.MIDDLE_SCHOOL;
                    }
                }
                //
                //DURATION OF EDUCATION IN YEARS
                try
                {
                    if (!answer.educationDuration.Equals(Constants.NOT_ANSWERED_STR))
                    {
                        if (textBoxEducationDuration.TextLength > 2)
                        {
                            answer.educationDuration = Constants.INVALID_INPUT;
                        }
                        else
                        {
                            answer.educationDuration = Int32.Parse(textBoxEducationDuration.Text);
                        }
                    }

                }
                catch (Exception ex)//string not convertivle to int, most likely
                {
                    answer.educationDuration = Constants.INVALID_INPUT;
                }
                //
                //PRIMARY LANGUAGE
                bool languageChecked = false;
                foreach (Control control in groupBoxLanguage.Controls)
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null)
                    {
                        if (radioButton.Checked)
                        {
                            languageChecked = true;
                            break;
                        }
                    }
                }
                if (languageChecked)
                {
                    if (radioButtonEnglish.Checked)
                    {
                        answer.primaryLanguage = Constants.ENGLISH;
                    }
                    else if (radioButtonSpanish.Checked)
                    {
                        answer.primaryLanguage = Constants.SPANISH;
                    }
                    else if (radioButtonLanguageOther.Checked)
                    {
                        answer.primaryLanguage = Constants.OTHER;
                        answer.primaryLanguageOther = textBoxLanguageOther.Text;
                    }

                }
                //
                //EMPLOYMENT STATUS
                bool employmentChecked = false;
                foreach (Control control in groupBoxEmployment.Controls)
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null)
                    {
                        if (radioButton.Checked)
                        {
                            employmentChecked = true;
                            break;
                        }
                    }
                }
                if (employmentChecked)
                {
                    if (radioButtonPartTime.Checked)
                    {
                        answer.employmentStatus = Constants.PART_TIME;
                    }
                    else if (radioButtonFullTime.Checked)
                    {
                        answer.employmentStatus = Constants.FULL_TIME;
                    }
                    else if (radioButtonUnemployed.Checked)
                    {
                        answer.employmentStatus = Constants.UNEMPLOYED;
                    }
                    else if (radioButtonRetired.Checked)
                    {
                        answer.employmentStatus = Constants.RETIRED;
                    }
                }
      
            }
            //
            else if (currentPage == Constants.PAGE_GROUP_3)
            {
                //UNEMPLOYMENT QUESTIONS
                if (answer.employmentStatus == Constants.UNEMPLOYED)
                {
                    bool unEmploymentDurationChecked = false;
                    foreach (Control control in groupBoxUnemployment.Controls)
                    {
                        RadioButton radioButton = control as RadioButton;
                        if (radioButton != null)
                        {
                            if (radioButton.Checked)
                            {
                                unEmploymentDurationChecked = true;
                                break;
                            }
                        }
                    }
                    //
                    //UNEMPLOYMENT DURATION
                    if (unEmploymentDurationChecked)
                    {
                        if (radioButtonOneMonth.Checked)
                        {
                            answer.unemploymentDuration = Constants.ONE_MONTH;
                        }
                        else if (radioButtonTwoMonths.Checked)
                        {
                            answer.unemploymentDuration = Constants.TWO_MONTHS;
                        }
                        else if (radioButtonSixMonths.Checked)
                        {
                            answer.unemploymentDuration = Constants.SIX_MONTHS;
                        }
                        else if (radioButtonOneYear.Checked)
                        {
                            answer.unemploymentDuration = Constants.ONE_YEAR;
                        }
                        else if (radioButtonTwoYear.Checked)
                        {
                            answer.unemploymentDuration = Constants.TWO_YEAR;
                        }
                        else if (radioButtonMore.Checked)
                        {
                            answer.unemploymentDuration = Constants.MORE;
                        }
                        else if (radioButtonUnemploymentIDK.Checked)
                        {
                            answer.unemploymentDuration = Constants.IDK;
                        }

                        answer.currentJob = Constants.NOT_ANSWERED_STR;
                    }

                    //
                    //HAS PARTICIPANT EVER WORKED
                    bool everWorkedChecked = false;
                    foreach (Control control in groupBoxEverWorked.Controls)
                    {
                        RadioButton radioButton = control as RadioButton;
                        if (radioButton != null)
                        {
                            if (radioButton.Checked)
                            {
                                everWorkedChecked = true;
                                break;
                            }
                        }
                    }

                    if (everWorkedChecked)
                    {
                        answer.everWorked = radioButtonEverWorked.Checked ? Constants.YES : Constants.NO;
                    }
 
                }
                //
                //CURRENT EMPLOYMENT QUESTIONS
                else if (answer.employmentStatus == Constants.FULL_TIME || answer.employmentStatus == Constants.PART_TIME)
                {
                    if (!textBoxCurrentJob.Text.Equals(Constants.NOT_ANSWERED_STR))
                    {
                        answer.currentJob = textBoxCurrentJob.Text;
                        answer.unemploymentDuration = Constants.NOT_ANSWERED;
                        answer.everWorked = Constants.NOT_ANSWERED;
                    }
                    
                }

                bool livingStatusChecked = false;
                foreach (Control control in groupBoxLivingStatus.Controls)
                {
                    RadioButton radioButton = control as RadioButton;
                    if (radioButton != null)
                    {
                        if (radioButton.Checked)
                        {
                            livingStatusChecked = true;
                            break;
                        }
                    }
                }

                if (livingStatusChecked)
                {
                    if (radioButtonAlone.Checked)
                    {
                        answer.livingStatus = Constants.TWO_YEAR;
                    }
                    else if (radioButtonFriendOrfamily.Checked)
                    {
                        answer.livingStatus = Constants.FRIEND_OR_FAMILY;
                    }
                    else if (radioButtonSupervised.Checked)
                    {
                        answer.livingStatus = Constants.SUPERVISED_FACILITY;
                    }
                    else if (radioButtonUnsupervised.Checked)
                    {
                        answer.livingStatus = Constants.UNSUPERVISED_FACILITY;
                    }
                }
               
            }
          
        }

        private void showErrorMessage(string error, string errorTitle, int questionNumber)
        {
            if (answer.formLanguage == Constants.ENGLISH || questionNumber < 0)            
                FormsTaskNotifier.Notify(error);
            else
                FormsTaskNotifier.Notify("Usted debe responder pregunta " + questionNumber + " antes de proseguir");
        }

        private void showEntryErrorMessage(string entry, int questionNumber)
        {
            if (answer.formLanguage == Constants.ENGLISH)
                FormsTaskNotifier.Notify("Please specify your " + entry + ", before you can proceed.");
            else
                FormsTaskNotifier.Notify("Usted debe responder pregunta #" + questionNumber + " antes de proseguir");
        }

        private void textBoxAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            string keyInput = e.KeyChar.ToString();
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                e.KeyChar = '\0';
            }
        }

        private void textBoxEducationDuration_KeyPress(object sender, KeyPressEventArgs e)
        {
            string keyInput = e.KeyChar.ToString();
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b' )
            {
                e.KeyChar = '\0';
            }
        }

        private void radioButtonRaceOther_CheckedChanged(object sender, EventArgs e)
        {
            showOrHideTextbox(radioButtonRaceOther, textBoxRaceOther);
        }

        private void radioButtonLanguageOther_CheckedChanged(object sender, EventArgs e)
        {
            showOrHideTextbox(radioButtonLanguageOther, textBoxLanguageOther);
        }

        private void radioButtonLivingStatusOther_CheckedChanged(object sender, EventArgs e)
        {
            showOrHideTextbox(radioButtonLivingStatusOther, textBoxLivingStatus);
        }

        private void showOrHideTextbox(RadioButton radioButton, TextBox textBox)
        {
            if (answer.formLanguage == Constants.ENGLISH)
                radioButton.Text = radioButton.Checked ? Constants.EnglishText.OTHER_SPECIFY : Constants.EnglishText.OTHER;
            else
                radioButton.Text = radioButton.Checked ? Constants.SpanishText.OTHER_SPECIFY : Constants.SpanishText.OTHER;
            textBox.Visible = radioButton.Checked;

        }

        public List<Dictionary<String, String>> getEventData()
        {
            return taskDataList;
        }
        public List<Dictionary<String, String>> getAnswers(List<Dictionary<String, String>> taskDataList)
        {
            Dictionary<String, String> formLanguage = new Dictionary<String, String>();
            Dictionary<String, String> age = new Dictionary<String, String>();
            Dictionary<String, String> gender = new Dictionary<String, String>();
            Dictionary<String, String> race = new Dictionary<String, String>();
            Dictionary<String, String> latino = new Dictionary<String, String>();
            Dictionary<String, String> education = new Dictionary<String, String>();
            Dictionary<String, String> educationDuration = new Dictionary<String, String>();
            Dictionary<String, String> primaryLanguage = new Dictionary<String, String>();
            Dictionary<String, String> employmentStatus = new Dictionary<String, String>();
            Dictionary<String, String> unemploymentDuration = new Dictionary<String, String>();
            Dictionary<String, String> everWorked = new Dictionary<String, String>();
            Dictionary<String, String> livingStatus = new Dictionary<String, String>();
            Dictionary<String, String> livingStatusOther = new Dictionary<String, String>();
            Dictionary<String, String> otherRace = new Dictionary<String, String>();
            Dictionary<String, String> primaryLanguageOther = new Dictionary<String, String>();
            Dictionary<String, String> currentJob = new Dictionary<String, String>(); 

            formLanguage.Add("time", DateTime.Now.ToString());
            formLanguage.Add("eventType", "formLanguage");
            formLanguage.Add("eventData", answer.formLanguage.ToString());
            formLanguage.Add("eventSummary", "");
            taskDataList.Add(formLanguage);

            age.Add("time", DateTime.Now.ToString());
            age.Add("eventType", "age");
            age.Add("eventData", answer.age.ToString());
            age.Add("eventSummary", "");
            taskDataList.Add(age);

            gender.Add("time", DateTime.Now.ToString());
            gender.Add("eventType", "gender");
            gender.Add("eventData", answer.gender.ToString());
            gender.Add("eventSummary", "");
            taskDataList.Add(gender);

            race.Add("time", DateTime.Now.ToString());
            race.Add("eventType", "race");
            race.Add("eventData", answer.race.ToString());
            race.Add("eventSummary", "");
            taskDataList.Add(race);

            latino.Add("time", DateTime.Now.ToString());
            latino.Add("eventType", "latino");
            latino.Add("eventData", answer.latino.ToString());
            latino.Add("eventSummary", "");
            taskDataList.Add(latino);

            education.Add("time", DateTime.Now.ToString());
            education.Add("eventType", "education");
            education.Add("eventData", answer.education.ToString());
            education.Add("eventSummary", "");
            taskDataList.Add(education);

            educationDuration.Add("time", DateTime.Now.ToString());
            educationDuration.Add("eventType", "educationDuration");
            educationDuration.Add("eventData", answer.educationDuration.ToString());
            educationDuration.Add("eventSummary", "");
            taskDataList.Add(educationDuration);

            primaryLanguage.Add("time", DateTime.Now.ToString());
            primaryLanguage.Add("eventType", "primaryLanguage");
            primaryLanguage.Add("eventData", answer.primaryLanguage.ToString());
            primaryLanguage.Add("eventSummary", "");
            taskDataList.Add(primaryLanguage);

            employmentStatus.Add("time", DateTime.Now.ToString());
            employmentStatus.Add("eventType", "employmentStatus");
            employmentStatus.Add("eventData", answer.employmentStatus.ToString());
            employmentStatus.Add("eventSummary", "");
            taskDataList.Add(employmentStatus);

            unemploymentDuration.Add("time", DateTime.Now.ToString());
            unemploymentDuration.Add("eventType", "unemploymentDuration");
            unemploymentDuration.Add("eventData", answer.unemploymentDuration.ToString());
            unemploymentDuration.Add("eventSummary", "");
            taskDataList.Add(unemploymentDuration);

            everWorked.Add("time", DateTime.Now.ToString());
            everWorked.Add("eventType", "everWorked");
            everWorked.Add("eventData", answer.everWorked.ToString());
            everWorked.Add("eventSummary", "");
            taskDataList.Add(everWorked);

            livingStatus.Add("time", DateTime.Now.ToString());
            livingStatus.Add("eventType", "livingStatus");
            livingStatus.Add("eventData", answer.livingStatus.ToString());
            livingStatus.Add("eventSummary", "");
            taskDataList.Add(livingStatus);

            livingStatusOther.Add("time", DateTime.Now.ToString());
            livingStatusOther.Add("eventType", "livingStatusOther");
            livingStatusOther.Add("eventData", answer.livingStatusOther.ToString());
            livingStatusOther.Add("eventSummary", "");
            taskDataList.Add(livingStatusOther);

            otherRace.Add("time", DateTime.Now.ToString());
            otherRace.Add("eventType", "otherRace");
            otherRace.Add("eventData", answer.otherRace.ToString());
            otherRace.Add("eventSummary", "");
            taskDataList.Add(otherRace);

            primaryLanguageOther.Add("time", DateTime.Now.ToString());
            primaryLanguageOther.Add("eventType", "primaryLanguageOther");
            primaryLanguageOther.Add("eventData", answer.primaryLanguageOther.ToString());
            primaryLanguageOther.Add("eventSummary", "");
            taskDataList.Add(primaryLanguageOther);

            currentJob.Add("time", DateTime.Now.ToString());
            currentJob.Add("eventType", "currentJob");
            currentJob.Add("eventData", answer.currentJob.ToString());
            currentJob.Add("eventSummary", "");
            taskDataList.Add(currentJob);

            return taskDataList;
        }
    }
}
