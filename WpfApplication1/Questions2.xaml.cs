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
    /// Interaction logic for Questions2.xaml
    /// </summary>
    public partial class Questions2 : UserControl
    {

        int videoIndex = 0;
        int replayCount = 0;
        bool replay = true;
        string questionNum;
        bool _SKIPPED_QUESTION = true;
        bool _NO_SKIP = false;

        //
        //used to store continueButton's original grid.row and grid.column
        /* so if resolveContinueButtonOverlap() is called, 
         * afterwards continueButton can be placed back in its original location 
         */
        int originalContinueButtonGridRow;
        int originalContinueButtonGridColumn;
        int originalContinueButtonColumnSpan;

        public Questions2()
        {
            InitializeComponent();
            //until I grok WPF localization, set mediaElement1.Source, etc here
            mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q01_02), UriKind.Relative);
            //
            checkBox1.Content = QuestionStrings.YourSSCard;
            checkBox2.Content = QuestionStrings.YourDriversLicense;
            checkBox3.Content = QuestionStrings.YourInsuranceCard;
            checkBox4.Content = QuestionStrings.ListOfMeds;
            //
            rb1TextBlock.Text = QuestionStrings.OWFTMorning;
            rb2TextBlock.Text = QuestionStrings.OWFTAfternoon;
            rb3TextBlock.Text = QuestionStrings.TWFTMorning;
            rb4TextBlock.Text = QuestionStrings.TWFTAfternoon;
            //
            rb10TextBlock.Text = QuestionStrings.OWFTMorning;
            rb11TextBlock.Text = QuestionStrings.OWFTAfternoon;
            rb12TextBlock.Text = QuestionStrings.TWFTMorning;
            rb13TextBlock.Text = QuestionStrings.TWFTAfternoon;
            //
            continueButton.Content = InstructionStrings.Continue;
            button2.Content = InstructionStrings.Replay;
            skipQuestionButton.Content = InstructionStrings.SkipQuestion;
            button13.Content = InstructionStrings.Clear;

            this.originalContinueButtonGridColumn   = Grid.GetColumn(this.continueButton); 
            this.originalContinueButtonGridRow      = Grid.GetRow(this.continueButton);
            //
            this.originalContinueButtonColumnSpan   = Grid.GetColumnSpan(this.continueButton);
        }

        //this is a hack to move the Continue button if stackPanel3 is overlapping it
        void continueButton_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (continueButton.IsVisible && IsContinueButtonOverlapped()) //we only care if it's been set to visible...
            {
                this.resolveContinueButtonOverlap();
            }    
        }

        //this method prevents stackPanel3 from overlapping.
        private void resolveContinueButtonOverlap()
        {
            //throw new NotImplementedException();
            Grid.SetColumnSpan(this.continueButton, 1);
            //
            Grid.SetRow(this.continueButton, 0); // move it to the top
            Grid.SetColumn(this.continueButton, 4); //keep it on the right
            
            this.continueButton.FontSize = 52;
        }

       

        private void mediaElement1_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (replay == true && replayCount < MainWindow.allowedNumberReplays)
            {
               
                button2.Visibility = System.Windows.Visibility.Visible;

            }
            else
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
                stackPanel2.Visibility = System.Windows.Visibility.Visible;
                
            }
            else if (videoIndex == 1)
            {
                stackPanel3.Visibility = System.Windows.Visibility.Visible;
                rb10TextBlock.Text = QuestionStrings.HaveAFullBreakfast;//"Have a full breakfast before you arrive for your visit as you will not be able to \neat or drink anything all morning while you are undergoing the tests.";
                rb11TextBlock.Text = QuestionStrings.DrinkOnlyWater;//"Drink only water after midnight the day before your visit. You cannot eat food \nor drink anything other than water.";
                rb12TextBlock.Text = QuestionStrings.DNEAAMidnight;// "Do not eat anything after midnight the day before your visit. You can drink \nmilk, juice or coffee. ";
                rb13TextBlock.Text = QuestionStrings.DNEAAWakeup;// "Do not eat anything after you wake up on the day of your visit. You can drink \nmilk, juice or coffee.";
                
               
            }
            else if (videoIndex == 2)
            {
                stackPanel1.Visibility = System.Windows.Visibility.Visible;
                checkBox5.Visibility = System.Windows.Visibility.Hidden;


            }
            else if (videoIndex == 3)
            {

                rb1TextBlock.Text = QuestionStrings.CMOMorning;//"Call my office the morning of your appointment.";
                rb2TextBlock.Text = QuestionStrings.CMO24HrsAdvance;//"Call my office at least 24-hours in advance of your \n appointment time.";
                rb3TextBlock.Text = QuestionStrings.CallTheLab;//"Call the lab to cancel your blood work appointment.";
                rb4TextBlock.Text = QuestionStrings.NoneOfTheAbove;// "None of the above.";

                stackPanel1.Visibility = System.Windows.Visibility.Hidden;
                stackPanel2.Visibility = System.Windows.Visibility.Visible;

            }
           
        }


        //radioButton13 seems to be the culprit in this case.
        //TODO: pass any UIElement as a parameter
        private bool IsContinueButtonOverlapped()
        {
            if (!radioButton13.IsVisible)
            {
                return false;
            }
            else
            {
                Point radioButton13_Point = radioButton13.PointToScreen(new Point(0d, 0d));
                Point continueButton_Point = continueButton.PointToScreen(new Point(0d, 0d));
                //
                System.Drawing.Rectangle radioButton13_Rect = new System.Drawing.Rectangle(
                    (int)radioButton13_Point.X,
                    (int)radioButton13_Point.Y,
                    (int)radioButton13.ActualWidth,
                    (int)radioButton13.ActualHeight
                    );



                System.Drawing.Rectangle continueButton_Rect = new System.Drawing.Rectangle(
                    (int)continueButton_Point.X,
                    (int)continueButton_Point.Y,
                    (int)continueButton.ActualWidth,
                    (int)continueButton.ActualHeight
                    );

                return radioButton13_Rect.IntersectsWith(continueButton_Rect);
            }   

        }


        private void processAnswer(bool SKIPPED_QUESTION)
        {
            //
            this.replayCount = 0;
            if (skipQuestionButton.Visibility.Equals(Visibility.Visible))
            {
                skipQuestionButton.Visibility = Visibility.Collapsed;
            }
            if (SKIPPED_QUESTION)
            {
                string questionNum = generateCurrentQuestionNumberString();
                
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
            //
            radioButton1.IsChecked = false;
            radioButton2.IsChecked = false;
            radioButton3.IsChecked = false;
            radioButton4.IsChecked = false;
            radioButton5.IsChecked = false;
            radioButton6.IsChecked = false;
            checkBox1.IsChecked = false;
            checkBox2.IsChecked = false;
            checkBox3.IsChecked = false;
            checkBox4.IsChecked = false;
            checkBox5.IsChecked = false;
            checkBox6.IsChecked = false;

            stackPanel3.Visibility = System.Windows.Visibility.Hidden;
            continueButton.Visibility = System.Windows.Visibility.Hidden;
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

            if (videoIndex == 0)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q02_02), UriKind.Relative);
                videoIndex++;
                replay = true;

            }
            else if (videoIndex == 1)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q03_03), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 2)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q04_04), UriKind.Relative);
                videoIndex++;
                replay = true;
            }
            else if (videoIndex == 3)
            {
                MessageBox.Show(InstructionStrings.ThankYouMessage);
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "TaskComplete");
                dictionary.Add("eventData", "");
                dictionary.Add("eventSummary", "");
                MainWindow.taskDataList.Add(dictionary);
                Application curr = Application.Current;
                curr.MainWindow.Close();
            }

            //reset textbox
            textBox1.Text = "";
        }

        private string generateCurrentQuestionNumberString()
        {
            string questionNum = "Part 2: ";
            if (stackPanel1.Visibility == System.Windows.Visibility.Visible)
            {
                questionNum += "Q3";
            }
		    else if (stackPanel2.Visibility == System.Windows.Visibility.Visible)
            {
			    if(videoIndex == 0)
                {
                    questionNum +="Q1";
                }
			    else if(videoIndex == 3)
                {
                    questionNum +="Q4";
                }
				    
            }
		    else if (stackPanel3.Visibility == System.Windows.Visibility.Visible)
            {
                questionNum += "Q2";
            }
		    //
            return questionNum;
        }

        //continue button
        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            //in case resolveContinueButtonOverlap() was called, set Grid.Row, Grid.Column and Grid.ColumnSpan to original values
            Grid.SetColumn(this.continueButton, this.originalContinueButtonGridColumn);
            Grid.SetRow(this.continueButton, this.originalContinueButtonGridRow);
            Grid.SetColumnSpan(this.continueButton, this.originalContinueButtonColumnSpan);

            processAnswer(_NO_SKIP);
        }

        /*
        * 
        */
        private string GetResxNameByValue(string value, string namespace_name, string resx_filename)
        {

            System.Resources.ResourceManager rm = new System.Resources.ResourceManager(String.Format("{0}.{1}", namespace_name, resx_filename), this.GetType().Assembly);

            var entry =
                rm.GetResourceSet(Thread.CurrentThread.CurrentUICulture, true, true)
                  .OfType<DictionaryEntry>()
                  .FirstOrDefault(e => e.Value.ToString() == value);

            var key = entry.Key.ToString();
            return key;

        }

        /*
         * checks if answer needs to get replaced by Resource Name
         */
        private String getEventSummaryString(string val)
        {
            int question_index = Int32.Parse( questionNum.Substring((questionNum.Length-1),1) );
            Console.WriteLine("question_index : " + question_index);

            bool isAnswerLocaleDependent = (Array.IndexOf(new[] { 1, 2, 3, 4 }, question_index) > -1);
            if (isAnswerLocaleDependent)
            {
                Console.WriteLine("getting resx name string for question " + question_index);
                if (Thread.CurrentThread.CurrentUICulture == CultureInfo.GetCultureInfo("es"))
                {
                    Console.WriteLine("Getting Name value for Spanish string");
                }
                else
                {
                    Console.WriteLine("Getting Name value for English string");
                }
            }

            return isAnswerLocaleDependent ? GetResxNameByValue(val, "WpfApplication1", "QuestionStrings") : val;
        }


        /*
         * gets user's answer and stores it in taskDataList
         */
        private void getData()
        {
            if (stackPanel1.Visibility == System.Windows.Visibility.Visible)
            {
                if (checkBox1.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q3");
                    dictionary.Add("eventData", checkBox1.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox1.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox2.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q3");
                    dictionary.Add("eventData", checkBox2.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox2.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox3.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q3");
                    dictionary.Add("eventData", checkBox3.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox3.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox4.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q3");
                    dictionary.Add("eventData", checkBox4.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox4.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox5.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q3");
                    dictionary.Add("eventData", checkBox5.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox5.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (checkBox6.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q3");
                    dictionary.Add("eventData", checkBox6.Content.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(checkBox6.Content.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
            }
            if (stackPanel2.Visibility == System.Windows.Visibility.Visible)
            {
                if (videoIndex == 0)
                {
                    questionNum = "Part 2: Q1";
                    
                }
                else if (videoIndex == 3)
                {
                    questionNum = "Part 2: Q4";
                }
                if (radioButton1.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb1TextBlock.Text.Replace('\n',' ')); //stripping newline
                    dictionary.Add("eventSummary", getEventSummaryString(rb1TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                } 
                if (radioButton2.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb2TextBlock.Text.Replace('\n', ' '));//stripping newline
                    dictionary.Add("eventSummary", getEventSummaryString(rb2TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (radioButton3.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb3TextBlock.Text.Replace('\n', ' '));//stripping newline
                    dictionary.Add("eventSummary", getEventSummaryString(rb3TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (radioButton4.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", questionNum);
                    dictionary.Add("eventData", rb4TextBlock.Text.ToString());
                    dictionary.Add("eventSummary", getEventSummaryString(rb4TextBlock.Text.ToString()));
                    MainWindow.taskDataList.Add(dictionary);
                }
            }
            if (stackPanel3.Visibility == System.Windows.Visibility.Visible)
            {
                if (radioButton10.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q2");
                    dictionary.Add("eventData", rb10TextBlock.Text.Replace('\n', ' '));//stripping newline
                    dictionary.Add("eventSummary", getEventSummaryString(rb10TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (radioButton11.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q2");
                    dictionary.Add("eventData", rb11TextBlock.Text.Replace('\n', ' '));//stripping newline
                    dictionary.Add("eventSummary", getEventSummaryString(rb11TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                } 
                if (radioButton12.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q2");
                    dictionary.Add("eventData", rb12TextBlock.Text.Replace('\n', ' '));//stripping newline
                    dictionary.Add("eventSummary", getEventSummaryString(rb12TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                }
                if (radioButton13.IsChecked == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "Part 2: Q2");
                    dictionary.Add("eventData", rb13TextBlock.Text.Replace('\n', ' '));//stripping newline
                    dictionary.Add("eventSummary", getEventSummaryString(rb13TextBlock.Text.Replace('\n', ' ')));
                    MainWindow.taskDataList.Add(dictionary);
                }
            }
        }
        private void curr_Exit(object sender, ExitEventArgs e)
        {
           
        }

         
        

    
        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            continueButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox3_Checked(object sender, RoutedEventArgs e)
        {
            continueButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox4_Checked(object sender, RoutedEventArgs e)
        {
            continueButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox5_Checked(object sender, RoutedEventArgs e)
        {
            continueButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox6_Checked(object sender, RoutedEventArgs e)
        {
            continueButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void checkBox6_Checked_1(object sender, RoutedEventArgs e)
        {
            continueButton.Visibility = System.Windows.Visibility.Visible;
        }

        //replay button
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            replayCount++;

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
            continueButton.Visibility = System.Windows.Visibility.Hidden;
            if (videoIndex == 0)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q01_02), UriKind.Relative);
            }
            else if (videoIndex == 1)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q02_02), UriKind.Relative);
            }
            else if (videoIndex == 2)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q03_03), UriKind.Relative);
            }
            else if (videoIndex == 3)
            {
                mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.Q04_04), UriKind.Relative);
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
            continueButton.Visibility = System.Windows.Visibility.Hidden;

        }

        private void showContinue()
        {
            continueButton.Visibility = System.Windows.Visibility.Visible;
        }

       


        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            continueButton.Visibility = System.Windows.Visibility.Visible;
            
        }

        private void skipQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            processAnswer(_SKIPPED_QUESTION);
        }

        


    }
}
