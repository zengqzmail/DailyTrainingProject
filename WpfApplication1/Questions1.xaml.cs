using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Windows.Threading;
using System.Threading;
using System.Globalization;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Questions.xaml
    /// </summary>
    public partial class Questions1 : UserControl
    {
        public Questions1()
        {
            InitializeComponent();
            indexx = 0;
            color = button1.Background;
            green = Brushes.Green;//(Brush)converter.ConvertFromString("Green");
            //until I grok WPF localization, set mediaElement1.Source and image7.Source here, etc.
            mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.MSG01), UriKind.Relative);
            image7.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.DriTab), UriKind.Relative));
            //
            driTabInsert = new DriTabInsert();
            driTabInsert.DriTabInsertImage.Source  = new BitmapImage(new Uri(String.Format(@"{0}", Images.DriTab), UriKind.Relative));
            driTabInsert.CloseDriTabInsertButton.Content = InstructionStrings.Close;
            //
            clickToEnlargeInstructionsTextBlock.Text = InstructionStrings.ClickToEnlarge;
            button1.Content = InstructionStrings.Continue;
            button2.Content = InstructionStrings.Replay;
            skipQuestionButton.Content = InstructionStrings.SkipQuestion;
            button13.Content = InstructionStrings.Clear;
            //
            rb1TextBlock.Text = QuestionStrings.FiveDays;
            rb2TextBlock.Text = QuestionStrings.TenDays;
            rb3TextBlock.Text = QuestionStrings.FifteenDays;
            rb4TextBlock.Text = QuestionStrings.TwentyDays;
            rb5TextBlock.Text = QuestionStrings.TwentyfiveDays;
            //
            showDriTabTextBlock.Text = InstructionStrings.ShowDriTabInsert;
            closeDriTab.Content = InstructionStrings.Close;
            //
           
        }
        System.Windows.Media.Brush color;
        System.Windows.Media.Brush green;
        BrushConverter converter = new System.Windows.Media.BrushConverter();
        int videoIndex = 0;
        int replayCount = 0;
        
        bool replay = true;
        string questionNum;
        int indexx;
        //
        bool _SKIPPED_QUESTION = true;
        bool _NO_SKIP = false;
        //
        DriTabInsert driTabInsert;
        //
        /*
         * this method does two things:
         * 1. handles flow of video content
         * 2. shows/hides UI elements according to current question
         */
        private void mediaElement1_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (((videoIndex > 0) && (videoIndex <= 14)) && replay.Equals(true) && replayCount < MainWindow.allowedNumberReplays)
            { 
                button2.Visibility = System.Windows.Visibility.Visible;
            }
            else if(videoIndex != 0)
            {
                //done with replays. reset replayCount
                // and show skipQuestionButton
                replay = false;
                replayCount = 0;
                //
                skipQuestionButton.Visibility = Visibility.Visible;
            }

            if (videoIndex == 0)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.MSG02), UriKind.Relative);
                videoIndex++;
            }
            else if (videoIndex == 1)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q01_01), UriKind.Relative);
                videoIndex++;
            }
            else if (videoIndex == 2)
            {
                stackPanel1.Visibility = System.Windows.Visibility.Visible;

            }
            else if (videoIndex == 3)
            {
                checkBox2.Content = QuestionStrings.FiveDays;//"5 days";
                checkBox3.Content = QuestionStrings.TenDays;//"10 days";
                checkBox4.Content = QuestionStrings.FifteenDays;// "15 days";
                checkBox1.Content = QuestionStrings.TwentyDays;//"20 days";
                checkBox5.Content = QuestionStrings.TwentyfiveDays;//"30 days";

                checkBox2.IsChecked = false;
                checkBox3.IsChecked = false;
                checkBox4.IsChecked = false;
                checkBox1.IsChecked = false;
                checkBox5.IsChecked = false;

                stackPanel1.Visibility = System.Windows.Visibility.Hidden;
                stackPanel2.Visibility = System.Windows.Visibility.Visible;
               

            }
            else if (videoIndex == 4)
            {

                grid2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (videoIndex == 5)
            {

                
                grid2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (videoIndex == 6)
            {

                rb1TextBlock.Text = QuestionStrings.Yes;//"Yes";
                radioButton2.Visibility = System.Windows.Visibility.Hidden;
                rb3TextBlock.Text = QuestionStrings.No;//"No";
                radioButton4.Visibility = System.Windows.Visibility.Hidden;
                rb5TextBlock.Text = QuestionStrings.IDK;//"I Don't Know";
                
                stackPanel2.Visibility = System.Windows.Visibility.Visible;

               

            }
            else if (videoIndex == 7)
            {
                //make checkbox 6 visible
                radioButton2.Visibility = System.Windows.Visibility.Visible;
                radioButton4.Visibility = System.Windows.Visibility.Visible;
                radioButton6.Visibility = System.Windows.Visibility.Visible;

                rb1TextBlock.Text = "1";
                rb2TextBlock.Text = "2";
                rb3TextBlock.Text = "3";
                rb4TextBlock.Text = "4";
                rb5TextBlock.Text = "5";
                rb6TextBlock.Text = "6";
   
                stackPanel2.Visibility = System.Windows.Visibility.Visible;
                 
               

            }
            else if (videoIndex == 8)
            {


               rb1TextBlock.Text = "100";
               rb2TextBlock.Text = "200";
               rb3TextBlock.Text = "300";
               rb4TextBlock.Text = "400";
               rb5TextBlock.Text = "500";
               rb6TextBlock.Text = "600";

           
                stackPanel2.Visibility = System.Windows.Visibility.Visible;
                
               

            }
            else if (videoIndex == 9)
            {
                showDriTab.Visibility = System.Windows.Visibility.Visible;
                
                
                grid2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (videoIndex == 10)
            {

                showDriTab.Visibility = System.Windows.Visibility.Visible;
                
                checkBox5.Visibility = System.Windows.Visibility.Hidden;
                checkBox6.Visibility = System.Windows.Visibility.Hidden;
                radioButton5.Visibility = System.Windows.Visibility.Hidden;
                radioButton6.Visibility = System.Windows.Visibility.Hidden;
              
                radioButton1.FontSize = 32;
                radioButton2.FontSize = 32;
                radioButton3.FontSize = 32;
                radioButton4.FontSize = 32;
                //
                rb1TextBlock.Text = QuestionStrings.DriTabContinue;//"Continue to take the same dose of the \nDri-Tab medication.";
                rb2TextBlock.Text = QuestionStrings.DriTabStop;//"Stop the medication and contact my doctor.\n";
                rb3TextBlock.Text = QuestionStrings.DriTabDoubleDose;//"Double the dose of the Dri-Tab medication.\n";
                rb4TextBlock.Text = QuestionStrings.DriTabTakeWithMilk;//"Take the Dri-Tab medication with milk\n";

                checkBox2.IsChecked = false;
                checkBox3.IsChecked = false;
                checkBox4.IsChecked = false;
                checkBox1.IsChecked = false;
                checkBox5.IsChecked = false;
                checkBox6.IsChecked = false;

                stackPanel2.Visibility = System.Windows.Visibility.Visible;
                

            }
            else if (videoIndex == 11)
            {

                
                grid2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (videoIndex == 12)
            {

               
                grid2.Visibility = System.Windows.Visibility.Visible;
            }
            else if (videoIndex == 13)
            {

                checkBox3.Visibility = System.Windows.Visibility.Hidden;
                checkBox1.Visibility = System.Windows.Visibility.Hidden;
                checkBox6.Visibility = System.Windows.Visibility.Hidden;

                radioButton1.FontSize = 36;
                radioButton2.FontSize = 36;
                radioButton3.FontSize = 36;
                radioButton4.FontSize = 36;
                //
                rb1TextBlock.Text = QuestionStrings.Yes;//"Yes ";
                rb3TextBlock.Text = QuestionStrings.No;//"No";
                rb5TextBlock.Text = QuestionStrings.IDK;//"I Don't Know";

                radioButton2.Visibility = System.Windows.Visibility.Hidden;
                radioButton4.Visibility = System.Windows.Visibility.Hidden;
                radioButton6.Visibility = System.Windows.Visibility.Hidden;

                checkBox2.IsChecked = false;
                checkBox3.IsChecked = false;
                checkBox4.IsChecked = false;
                checkBox1.IsChecked = false;
                checkBox5.IsChecked = false;
                checkBox6.IsChecked = false;

                stackPanel2.Visibility = System.Windows.Visibility.Visible;
                

            }
            else if (videoIndex == 14)
            {
                //stackPanel9.Visibility = System.Windows.Visibility.Visible; 
                //
                this.pillOrganizer.Visibility = Visibility.Visible;
                showContinue(); // due to nature of question, we need to show Continue button by default
                
            }
        }


        /*
         * 
         */
        private string GetResxNameByValue(string value, string namespace_name, string resx_filename)
        {

           System.Resources.ResourceManager rm = new System.Resources.ResourceManager(String.Format("{0}.{1}",namespace_name,resx_filename), this.GetType().Assembly);

            var entry=
                rm.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Value.ToString() ==value);

            var key = entry.Key.ToString();
            return key;

        }

        /*
         * checks if answer needs to get replaced by Resource Name
         */
        private String getEventSummaryString(string val)
        {
          
            if (Thread.CurrentThread.CurrentUICulture == CultureInfo.GetCultureInfo("es"))
            {
                Console.WriteLine("Getting Name value for Spanish string");
            }
            else
            {
                Console.WriteLine("Getting Name value for English string");
            }
            bool isAnswerLocaleDependent = (Array.IndexOf(new[] { 2, 5, 9, 12 }, indexx) > -1);
            return isAnswerLocaleDependent ? GetResxNameByValue(val, "WpfApplication1", "QuestionStrings") : val;
        }

        /*
         * gets user's answer and stores it in taskDataList
         */
        private void getData()
        {
            questionNum = "Part 1: Q" + (indexx).ToString();

            /*
             * some answer values require that the ResourceName property be saved, instead
             */
           

            if (stackPanel1.Visibility == System.Windows.Visibility.Visible)
            {
                if (checkBox1.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", checkBox1.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox1.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox2.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", checkBox2.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox2.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox3.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", checkBox3.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox3.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox4.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", checkBox4.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox4.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox5.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", checkBox5.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox5.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox6.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", checkBox6.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox6.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
            }
            if (stackPanel2.Visibility == System.Windows.Visibility.Visible)
            {
                if (radioButton1.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb1TextBlock.Text.Replace('\n', ' '));
                    dictionary.Add("eventSummary", getEventSummaryString(rb1TextBlock.Text.Replace('\n',' ')));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (radioButton2.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb2TextBlock.Text.Replace('\n', ' '));
                    dictionary.Add("eventSummary", getEventSummaryString(rb2TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (radioButton3.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb3TextBlock.Text.Replace('\n', ' '));
                    dictionary.Add("eventSummary", getEventSummaryString(rb3TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (radioButton4.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb4TextBlock.Text.Replace('\n', ' '));
                    dictionary.Add("eventSummary", getEventSummaryString(rb4TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (radioButton5.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb5TextBlock.Text);
                    dictionary.Add("eventSummary", getEventSummaryString(rb5TextBlock.Text));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (radioButton6.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb6TextBlock.Text);
                    dictionary.Add("eventSummary", getEventSummaryString(rb6TextBlock.Text));
                    MainWindow.taskDataList.Add(dictionary);
                }
            }
             if (grid2.Visibility == System.Windows.Visibility.Visible)
            {
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", questionNum);
                dictionary.Add("eventData", textBox1.Text.ToString());
                dictionary.Add("eventSummary", getEventSummaryString(textBox1.Text.ToString()));
                MainWindow.taskDataList.Add(dictionary);
            }
             if (pillOrganizer.Visibility == Visibility.Visible)
             {
                 PillOrganizer p = pillOrganizer;
                 Dictionary<String, Int32> pillOrgAnswers = p.GetMedicationAmounts();
                 //
                 foreach (String medicationLabel in pillOrgAnswers.Keys)
                 {
                     Dictionary<String, String> dictionary = new Dictionary<String, String>();
                     dictionary.Add("time", DateTime.Now.ToString());
                     dictionary.Add("eventType", questionNum);
                     dictionary.Add("eventData", String.Format("{0} {1}", medicationLabel, pillOrgAnswers[medicationLabel]));
                     dictionary.Add("eventSummary", getEventSummaryString(String.Format("{0} {1}", medicationLabel, pillOrgAnswers[medicationLabel] )));
                     MainWindow.taskDataList.Add(dictionary);
                 }
                 
             }

             
        }
        //continue button
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            processAnswer(_NO_SKIP);
        }

        //records answer to taskData
        //hides input UI elements
        //shows question video UI elements
        //starts new question video
        //
        //if question answered, calls getData()
        //else, writes directly to taskData
        private void processAnswer(bool SKIPPED_QUESTION)
        {
            this.replayCount = 0;
            if (skipQuestionButton.Visibility.Equals(Visibility.Visible))
            {
                skipQuestionButton.Visibility = Visibility.Collapsed;
            }
            //
            indexx++;
            if (SKIPPED_QUESTION)
            {
               string questionNum = "Part 1: Q" + (indexx).ToString();
               Dictionary<String, String> dictionary = new Dictionary<String, String>();
               dictionary.Add("time", DateTime.Now.ToString());
               dictionary.Add("eventType", questionNum);
               dictionary.Add("eventData", "QuestionSkipped");
               dictionary.Add("eventSummary", "");
               MainWindow.taskDataList.Add(dictionary);
            }
            else
            {
                getData();
            }
            
            image2.Visibility = System.Windows.Visibility.Hidden;
            radioButton1.IsChecked = false;
            radioButton2.IsChecked = false;
            radioButton3.IsChecked = false;
            radioButton4.IsChecked = false;
            radioButton5.IsChecked = false;
            radioButton6.IsChecked = false;

            button1.Visibility = System.Windows.Visibility.Hidden;
            stackPanel1.Visibility = System.Windows.Visibility.Hidden;
            stackPanel2.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;
            grid2.Visibility = System.Windows.Visibility.Hidden;
            //reset all checkboxes on grid2 to visible if needed to hide
            checkBox2.Visibility = System.Windows.Visibility.Visible;
            checkBox3.Visibility = System.Windows.Visibility.Visible;
            checkBox4.Visibility = System.Windows.Visibility.Visible;
            checkBox1.Visibility = System.Windows.Visibility.Visible;
            checkBox5.Visibility = System.Windows.Visibility.Visible;
            showDriTab.Visibility = System.Windows.Visibility.Hidden;
            image1.Visibility = System.Windows.Visibility.Visible;
            image3.Visibility = System.Windows.Visibility.Visible;
            image4.Visibility = System.Windows.Visibility.Visible;
            image5.Visibility = System.Windows.Visibility.Visible;
            image6.Visibility = System.Windows.Visibility.Visible;
            label1.Visibility = System.Windows.Visibility.Visible;
            label2.Visibility = System.Windows.Visibility.Visible;
            label3.Visibility = System.Windows.Visibility.Visible;
            label4.Visibility = System.Windows.Visibility.Visible;
            label5.Visibility = System.Windows.Visibility.Visible;
            label6.Visibility = System.Windows.Visibility.Visible;
            if (videoIndex == 2)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q02_01), UriKind.Relative);
                videoIndex++;
                replay = true;

            }
            else if (videoIndex == 3)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q03_01), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 4)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q04_01), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 5)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q05_01), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 6)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q06_01), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 7)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q07_01), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 8)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q08_01), UriKind.Relative);
                videoIndex++;
                replay = true;
                image1.Visibility = System.Windows.Visibility.Hidden;
                image3.Visibility = System.Windows.Visibility.Hidden;
                image4.Visibility = System.Windows.Visibility.Hidden;
                image5.Visibility = System.Windows.Visibility.Hidden;
                image6.Visibility = System.Windows.Visibility.Hidden;
                label1.Visibility = System.Windows.Visibility.Hidden;
                label2.Visibility = System.Windows.Visibility.Hidden;
                label3.Visibility = System.Windows.Visibility.Hidden;
                label4.Visibility = System.Windows.Visibility.Hidden;
                label5.Visibility = System.Windows.Visibility.Hidden;
                label6.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (videoIndex == 9)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q09_01), UriKind.Relative);
                videoIndex++;
                replay = true;
                image1.Visibility = System.Windows.Visibility.Hidden;
                image3.Visibility = System.Windows.Visibility.Hidden;
                image4.Visibility = System.Windows.Visibility.Hidden;
                image5.Visibility = System.Windows.Visibility.Hidden;
                image6.Visibility = System.Windows.Visibility.Hidden;
                label1.Visibility = System.Windows.Visibility.Hidden;
                label2.Visibility = System.Windows.Visibility.Hidden;
                label3.Visibility = System.Windows.Visibility.Hidden;
                label4.Visibility = System.Windows.Visibility.Hidden;
                label5.Visibility = System.Windows.Visibility.Hidden;
                label6.Visibility = System.Windows.Visibility.Hidden;
            }
            else if (videoIndex == 10)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q10_01), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 11)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q11_01), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 12)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q12_01), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 13)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q13_01), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 14)
            {
                //
                InstructionsPart2 part2 = new InstructionsPart2();
                this.Content = part2;

            }
            //reset textbox
            textBox1.Text = "";
        }

        
        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            button1.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox3_Checked(object sender, RoutedEventArgs e)
        {
            button1.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox4_Checked(object sender, RoutedEventArgs e)
        {
            button1.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox5_Checked(object sender, RoutedEventArgs e)
        {
            button1.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox6_Checked(object sender, RoutedEventArgs e)
        {
            button1.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox6_Checked_1(object sender, RoutedEventArgs e)
        {
            button1.Visibility = System.Windows.Visibility.Visible;
        }

        //replay button
        private void button2_Click(object sender, RoutedEventArgs e)
        {

            pillOrganizer.Visibility = Visibility.Hidden; //stackPanel9.Visibility = System.Windows.Visibility.Hidden; 
            radioButton1.IsChecked = false;
            radioButton2.IsChecked = false;
            radioButton3.IsChecked = false;
            radioButton4.IsChecked = false;
            radioButton5.IsChecked = false;
            radioButton6.IsChecked = false;
            stackPanel1.Visibility = System.Windows.Visibility.Hidden;
            grid2.Visibility = System.Windows.Visibility.Hidden;
            stackPanel2.Visibility = System.Windows.Visibility.Hidden;
            textBox1.Text = "";
            button1.Visibility = System.Windows.Visibility.Hidden;
            //
            replayCount++;
            //
            if (videoIndex == 2)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q01_01), UriKind.Relative);
            }
            else if (videoIndex == 3)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q02_01), UriKind.Relative);
            }
            else if (videoIndex == 4)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q03_01), UriKind.Relative);
            }
            else if (videoIndex == 5)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q04_01), UriKind.Relative);
            }
            else if (videoIndex == 6)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q05_01), UriKind.Relative);
            }
            else if (videoIndex == 7)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q06_01), UriKind.Relative);
            }
            else if (videoIndex == 8)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q07_01), UriKind.Relative);
            }
            else if (videoIndex == 9)
            {
                showDriTab.Visibility = System.Windows.Visibility.Hidden;
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q08_01), UriKind.Relative);
            }
            else if (videoIndex == 10)
            {
                showDriTab.Visibility = System.Windows.Visibility.Hidden;

                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q09_01), UriKind.Relative);
            }
            else if (videoIndex == 11)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q10_01), UriKind.Relative);
            }
            else if (videoIndex == 12)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q11_01), UriKind.Relative);
            }
            else if (videoIndex == 13)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q12_01), UriKind.Relative);
            }
            else if (videoIndex == 14)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q13_01), UriKind.Relative);
            }
            button2.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.recordVideoReplay(mediaElement1.Source.ToString(), replayCount);
        }



        private void button3_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "1";
            showContinue();

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "2";
            showContinue();
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "3";
            showContinue();

        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "4";
            showContinue();
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "5";
            showContinue();
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "6";
            showContinue();
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "7";
            showContinue();
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "8";
            showContinue();
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "9";
            showContinue();
        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text += "0";
            showContinue();
        }

        private void button13_Click(object sender, RoutedEventArgs e)
        {
            textBox1.Text = "";
            button1.Visibility = System.Windows.Visibility.Hidden;

        }

        private void showContinue()
        {
            button1.Visibility = System.Windows.Visibility.Visible;
        }

        private void image1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            image2.Visibility = System.Windows.Visibility.Visible;
          
            image2.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.Parlenol), UriKind.Relative));
            
        }

    

        private void image3_MouseUp(object sender, MouseButtonEventArgs e)
        {
            image2.Visibility = System.Windows.Visibility.Visible;

            image2.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.BRB), UriKind.Relative));
        }

        private void image4_MouseUp(object sender, MouseButtonEventArgs e)
        {
            image2.Visibility = System.Windows.Visibility.Visible;

            image2.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.Linophen), UriKind.Relative));
        }

        private void image5_MouseUp(object sender, MouseButtonEventArgs e)
        {
            image2.Visibility = System.Windows.Visibility.Visible;
           
            image2.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.Anaanx), UriKind.Relative));
        }

        private void image6_MouseUp(object sender, MouseButtonEventArgs e)
        {
            image2.Visibility = System.Windows.Visibility.Visible;

            image2.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.Cyclomeovan), UriKind.Relative));
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            button1.Visibility = System.Windows.Visibility.Visible;
        }

        private void closeDriTab_Click(object sender, RoutedEventArgs e)
        {
            closeDriTab.Visibility = System.Windows.Visibility.Hidden;
            image7.Visibility = System.Windows.Visibility.Hidden;
        }

        private void showDriTab_Click(object sender, RoutedEventArgs e)
        {
            
            //closeDriTab.Visibility = System.Windows.Visibility.Visible;
            //image7.Visibility = System.Windows.Visibility.Visible;
            Window w = new Window();
            w.Topmost = true;
            w.WindowState = WindowState.Maximized;
            w.ResizeMode = ResizeMode.NoResize;
            w.WindowStyle = WindowStyle.None;
            w.Content = driTabInsert;
            driTabInsert.CloseDriTabInsertButton.Click += delegate(object _sender, RoutedEventArgs _e) {
                if (w != null)
                {
                    w.Close();
                    w.Content = null; 
                }
            };
            w.Show();
        }


        private void button14_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(button14.ActualWidth.ToString());
            Console.WriteLine(label7.ActualWidth.ToString());
            Console.WriteLine(label7.FontSize.ToString());
            button14.Background = green;
            button15.Background = color;
            button16.Background = color;
            button17.Background = color;
            button18.Background = color;
            button19.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button15_Click(object sender, RoutedEventArgs e)
        {
            button14.Background = color;
            button15.Background = green;
            button16.Background = color;
            button17.Background = color;
            button18.Background = color;
            button19.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button16_Click(object sender, RoutedEventArgs e)
        {
            button14.Background = color;
            button15.Background = color;
            button16.Background = green;
            button17.Background = color;
            button18.Background = color;
            button19.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button17_Click(object sender, RoutedEventArgs e)
        {
            button14.Background = color;
            button15.Background = color;
            button16.Background = color;
            button17.Background = green;
            button18.Background = color;
            button19.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button18_Click(object sender, RoutedEventArgs e)
        {
            button14.Background = color;
            button15.Background = color;
            button16.Background = color;
            button17.Background = color;
            button18.Background = green;
            button19.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button19_Click(object sender, RoutedEventArgs e)
        {
            button14.Background = color;
            button15.Background = color;
            button16.Background = color;
            button17.Background = color;
            button18.Background = color;
            button19.Background = green;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button20_Click(object sender, RoutedEventArgs e)
        {
            button20.Background = green;
            button21.Background = color;
            button22.Background = color;
            button23.Background = color;
            button24.Background = color;
            button25.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button21_Click(object sender, RoutedEventArgs e)
        {
            button20.Background = color;
            button21.Background = green;
            button22.Background = color;
            button23.Background = color;
            button24.Background = color;
            button25.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button22_Click(object sender, RoutedEventArgs e)
        {
            button20.Background = color;
            button21.Background = color;
            button22.Background = green;
            button23.Background = color;
            button24.Background = color;
            button25.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button23_Click(object sender, RoutedEventArgs e)
        {
            button20.Background = color;
            button21.Background = color;
            button22.Background = color;
            button23.Background = green;
            button24.Background = color;
            button25.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button24_Click(object sender, RoutedEventArgs e)
        {
            button20.Background = color;
            button21.Background = color;
            button22.Background = color;
            button23.Background = color;
            button24.Background = green;
            button25.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button25_Click(object sender, RoutedEventArgs e)
        {
            button20.Background = color;
            button21.Background = color;
            button22.Background = color;
            button23.Background = color;
            button24.Background = color;
            button25.Background = green;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button26_Click(object sender, RoutedEventArgs e)
        {
            button26.Background = green;
            button27.Background = color;
            button28.Background = color;
            button29.Background = color;
            button30.Background = color;
            button31.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button27_Click(object sender, RoutedEventArgs e)
        {
            button26.Background = color;
            button27.Background = green;
            button28.Background = color;
            button29.Background = color;
            button30.Background = color;
            button31.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void button28_Click(object sender, RoutedEventArgs e)
        {
            button26.Background = color;
            button27.Background = color;
            button28.Background = green;
            button29.Background = color;
            button30.Background = color;
            button31.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button29_Click(object sender, RoutedEventArgs e)
        {
            button26.Background = color;
            button27.Background = color;
            button28.Background = color;
            button29.Background = green;
            button30.Background = color;
            button31.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button30_Click(object sender, RoutedEventArgs e)
        {
            button26.Background = color;
            button27.Background = color;
            button28.Background = color;
            button29.Background = color;
            button30.Background = green;
            button31.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }


        private void button31_Click(object sender, RoutedEventArgs e)
        {
            button26.Background = color;
            button27.Background = color;
            button28.Background = color;
            button29.Background = color;
            button30.Background = color;
            button31.Background = green;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button32_Click(object sender, RoutedEventArgs e)
        {
            button32.Background = green;
            button33.Background = color;
            button34.Background = color;
            button35.Background = color;
            button36.Background = color;
            button37.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button33_Click(object sender, RoutedEventArgs e)
        {
            button32.Background = color;
            button33.Background = green;
            button34.Background = color;
            button35.Background = color;
            button36.Background = color;
            button37.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button34_Click(object sender, RoutedEventArgs e)
        {
            button32.Background = color;
            button33.Background = color;
            button34.Background = green;
            button35.Background = color;
            button36.Background = color;
            button37.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button35_Click(object sender, RoutedEventArgs e)
        {
            button32.Background = color;
            button33.Background = color;
            button34.Background = color;
            button35.Background = green;
            button36.Background = color;
            button37.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button36_Click(object sender, RoutedEventArgs e)
        {
            button32.Background = color;
            button33.Background = color;
            button34.Background = color;
            button35.Background = color;
            button36.Background = green;
            button37.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button37_Click(object sender, RoutedEventArgs e)
        {
            button32.Background = color;
            button33.Background = color;
            button34.Background = color;
            button35.Background = color;
            button36.Background = color;
            button37.Background = green;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button38_Click(object sender, RoutedEventArgs e)
        {
            button38.Background = green;
            button39.Background = color;
            button40.Background = color;
            button41.Background = color;
            button42.Background = color;
            button43.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button39_Click(object sender, RoutedEventArgs e)
        {
            button38.Background = color;
            button39.Background = green;
            button40.Background = color;
            button41.Background = color;
            button42.Background = color;
            button43.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button40_Click(object sender, RoutedEventArgs e)
        {
            button38.Background = color;
            button39.Background = color;
            button40.Background = green;
            button41.Background = color;
            button42.Background = color;
            button43.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button41_Click(object sender, RoutedEventArgs e)
        {
            button38.Background = color;
            button39.Background = color;
            button40.Background = color;
            button41.Background = green;
            button42.Background = color;
            button43.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button42_Click(object sender, RoutedEventArgs e)
        {
            button38.Background = color;
            button39.Background = color;
            button40.Background = color;
            button41.Background = color;
            button42.Background = green;
            button43.Background = color;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void button43_Click(object sender, RoutedEventArgs e)
        {
            button38.Background = color;
            button39.Background = color;
            button40.Background = color;
            button41.Background = color;
            button42.Background = color;
            button43.Background = green;
            if ((button14.Background == green || button15.Background == green || button16.Background == green || button17.Background == green || button18.Background == green || button19.Background == green) && (button20.Background == green || button21.Background == green || button22.Background == green || button23.Background == green || button24.Background == green || button25.Background == green) && (button26.Background == green || button27.Background == green || button28.Background == green || button29.Background == green || button30.Background == green || button31.Background == green) && (button32.Background == green || button33.Background == green || button34.Background == green || button35.Background == green || button36.Background == green || button37.Background == green) && (button38.Background == green || button39.Background == green || button40.Background == green || button41.Background == green || button42.Background == green || button43.Background == green))
            {
                button1.Visibility = System.Windows.Visibility.Visible;
            }
        }

        //
        //
        private void skipQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            skipCurrentQuestion();
        }

        private void skipCurrentQuestion()
        {
            processAnswer(_SKIPPED_QUESTION);
        }
       


    }
}
