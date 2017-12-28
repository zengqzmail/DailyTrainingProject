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
using System.Threading;
using System.Globalization;
using System.Timers;
using ATM;
using WMPLib;


namespace ReactionTest
{
    /// <summary>
    /// Interaction logic for ReactionTestWindow.xaml
    /// </summary>
    /// 
    

    public partial class ReactionTestWindow : Window
    {
        string taskLanguage="EN";

        public ReactionTestWindow()
        {

           InitializeComponent();
           this.KeyDown +=new KeyEventHandler(ReactionTestWindow_KeyDown);
            
        }

        public ReactionTestWindow(string taskLanguage) 
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
            }

            textBlockInstructions1.Text = InstructionStrings.InstructionsPart1;
            textBlockInstructions2.Text = InstructionStrings.InstructionsPart2;
            textBlockInstructions3.Text = InstructionStrings.InstructionsPart3;
            textBlockInstructions4.Text = InstructionStrings.InstructionsPart4;
            textBlockInstructions5.Text = InstructionStrings.InstructionsPart5;
        }

        public void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*MessageBoxResult result = MessageBox.Show("Closing called, are you sure you want to close it?", "Closing", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
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
            else if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }*/
        }
       
        new Model1Container context = new ReactionTest.Model1Container();
        System.Timers.Timer myTimer, myTimer1 ;
        int life, delay, exitKeysCount;
        int[] intArray1;
        int i = 0;
        bool   flaggSimple = false, flaggComplex = false, done = false, simpleEarly = false, complexEarly = true, firstSimple = true, firstComplex = true;
        string location;
        Form1 messagee = new Form1();
        private List<Dictionary<String, String>> taskDataList;
        DateTime timeStart, timeDone;
        long timeSpan;
        System.Data.Objects.ObjectSet<SimpleReaction> simpleReactions;
        System.Data.Objects.ObjectSet<ComplexReaction> complexReactions;


        private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
            dictionary1.Add("time", DateTime.Now.ToString());
            dictionary1.Add("eventType", "TaskClosed");
            dictionary1.Add("eventData", "");
            dictionary1.Add("eventSummary", "EscapeEntered");
            taskDataList.Add(dictionary1);
            this.Close();
        }


        private void ReactionTestWindow_KeyDown(Object sender, KeyEventArgs e)
        {
            
            if (textBlockInstructions1.Visibility == System.Windows.Visibility.Visible && e.Key == Key.Space) //first simple trial instruction page
            {
                textBlockInstructions1.Visibility = System.Windows.Visibility.Hidden;
                textBlockInstructions2.Visibility = System.Windows.Visibility.Visible;
                textBoxDemoSquare.Visibility = System.Windows.Visibility.Visible;
            }
            else if (textBlockInstructions2.Visibility == System.Windows.Visibility.Visible && e.Key == Key.Space) //second simple trial instruction page
            {
                textBlockInstructions2.Visibility = System.Windows.Visibility.Hidden;
                textBoxDemoSquare.Visibility = System.Windows.Visibility.Hidden;
                textBlockInstructions3.Visibility = System.Windows.Visibility.Visible;
            }
            else if (textBlockInstructions3.Visibility == System.Windows.Visibility.Visible && e.Key == Key.Space) //last instruction page before simple trial
            {
                textBlockInstructions3.Visibility = System.Windows.Visibility.Hidden;
                textBoxPlus.Visibility = System.Windows.Visibility.Visible;
                firstSimple = true;
                SimpleReactions(); //begin simple trial
            }
            else if (textBlockInstructions4.Visibility == System.Windows.Visibility.Visible && e.Key == Key.Space) //instructions for complex trial
            {
                i = 0;
                textBlockInstructions4.Visibility = System.Windows.Visibility.Hidden;
                textBoxPlus.Visibility = System.Windows.Visibility.Visible;
                ComplexReactions(); //begin complex trial
            }
            else if (textBlockInstructions5.Visibility == System.Windows.Visibility.Visible && e.Key == Key.Space) //after complex trial, give instructions for simple trial
            {
                textBlockInstructions5.Visibility = System.Windows.Visibility.Hidden;
                textBoxPlus.Visibility = System.Windows.Visibility.Visible;
                firstSimple = true;
                SimpleReactions(); //begin simple trial
            }
            
            if (flaggSimple == true && e.Key == Key.B)
            {
                timeDone = DateTime.Now;
                timeSpan = (long)(timeDone - timeStart).TotalMilliseconds;
                Dictionary<String, String> dictionary = new Dictionary<String, String>();
                dictionary.Add("time", DateTime.Now.ToString());
                dictionary.Add("eventType", "KeyPress");
                dictionary.Add("eventSummary", "ReactionType:S_RowId:" + simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Id.ToString()+"_Delay:"+delay.ToString());
                dictionary.Add("eventData", timeSpan.ToString());
                taskDataList.Add(dictionary);
                flaggSimple = false;
                try
                {
                    myTimer.Stop();
                    myTimer1.Stop();
                } catch {

                }
                textBoxCenter.Visibility = System.Windows.Visibility.Hidden;
                textBoxPlus.Visibility = System.Windows.Visibility.Visible;
                if (i < intArray1.Count() -1)
                {
                    i++;
                    SimpleReactions();
                } 
                else if (i + 1 == intArray1.Count() && done == false)
                {
                    done = true;
                    i = 0;
                    textBlockInstructions4.Visibility = System.Windows.Visibility.Visible;
                    textBoxPlus.Visibility = System.Windows.Visibility.Hidden;
                }
                else if (i + 1 == intArray1.Count() && done == true)
                {
                    messagee.ShowBox(InstructionStrings.ThankYouMessage, InstructionStrings.Exit);
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventSummary", "");
                    dictionary1.Add("eventData", "");
                    taskDataList.Add(dictionary1);
                    this.Close();

                }
            } 
            else if (flaggComplex == true && (e.Key == Key.Z || e.Key == Key.OemQuestion))
            {
                timeDone = DateTime.Now;
                timeSpan = (long)(timeDone - timeStart).TotalMilliseconds;
                flaggComplex = false;
               
                textBoxLeft.Visibility = System.Windows.Visibility.Hidden;
                textBoxRight.Visibility = System.Windows.Visibility.Hidden;
                try
                {
                    myTimer.Stop();
                    myTimer1.Stop();
                }
                catch
                {

                }
                if ((e.Key == Key.Z && location.Contains('L')) || (e.Key == Key.OemQuestion && location.Contains('R')))
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "KeyPress");
                    dictionary.Add("eventSummary", "ReactionType:C_RowId:" + complexReactions.AsEnumerable().ElementAt(intArray1[i]).Id.ToString()+"_Delay:"+delay.ToString() + "_Correct:T");
                    dictionary.Add("eventData", timeSpan.ToString());
                    taskDataList.Add(dictionary);
                }
                else
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "KeyPress");
                    dictionary.Add("eventSummary", "ReactionType:C_RowId:" + complexReactions.AsEnumerable().ElementAt(intArray1[i]).Id.ToString()+"_Delay:"+delay.ToString() + "_Correct:F");
                    dictionary.Add("eventData", timeSpan.ToString());
                    taskDataList.Add(dictionary);
                }

                if (i < intArray1.Count() - 1)
                {
                    i++;
                    ComplexReactions();
                }
                else if (i + 1 == intArray1.Count())
                {
                    i = 0;
                    textBoxPlus.Visibility = System.Windows.Visibility.Hidden;
                    textBlockInstructions5.Visibility = System.Windows.Visibility.Visible;
                    
                }
            }
            else if (simpleEarly == true && e.Key == Key.B)
            {
                try
                {
                    myTimer.Stop();
                    myTimer1.Stop();
                }
                catch
                {

                }
                simpleEarly = false;
                //mediaElement1.Source = new Uri(@"Resources/error sound.wav", UriKind.RelativeOrAbsolute);
                mediaElement1.Play();
                messagee.ShowBox(InstructionStrings.EarlyPressWarning, InstructionStrings.Continue);
                textBoxCenter.Visibility = System.Windows.Visibility.Hidden;
                textBoxPlus.Visibility = System.Windows.Visibility.Visible;
                SimpleReactions();
            }
            else if (
                complexReactions != null 
                && textBlockInstructions5.Visibility == System.Windows.Visibility.Hidden 
                && firstSimple == false
                && (complexEarly == true && (e.Key == Key.Z || e.Key == Key.OemQuestion) )
                ) 
                //rblanco2, Nov 2 2015: adding non-null check for complexReactions field, 
                //visibility check for textBlockInstructions5.Visibility, and firstSimple == false.
                //seems to be the only (current) way to make sure the user is in a complexReactions trial.
            {
                try
                {
                    myTimer.Stop();
                    myTimer1.Stop();
                }
                catch
                {

                }
                complexEarly = false;

                //mediaElement1.Source = new Uri(@"Resources/error sound.wav", UriKind.RelativeOrAbsolute);
                mediaElement1.Play();
                messagee.ShowBox(InstructionStrings.EarlyPressWarning, InstructionStrings.Continue);
                textBoxLeft.Visibility = System.Windows.Visibility.Hidden;
                textBoxRight.Visibility = System.Windows.Visibility.Hidden;
                ComplexReactions();
            }
             
        }
       
        private void SimpleReactions()
        {

            simpleReactions = context.SimpleReactions;
            
            var converter = new System.Windows.Media.BrushConverter();

            if (firstSimple == true)
            {
                Random rng = new Random();
                int thisArraySize = simpleReactions.AsEnumerable().Count();
                intArray1 = new int[thisArraySize];
                for (int i = 0; i < thisArraySize; i++)
                    intArray1[i] = i;

                //randomize stimulus order
                for (int i = intArray1.Count() - 1; i > 0; i--)
                {
                    int swapper = rng.Next(i + 1);
                    if (swapper != i)
                    {
                        int tmp = intArray1[swapper];
                        intArray1[swapper] = intArray1[i];
                        intArray1[i] = tmp;
                    }
                }
            }
            
            //intArray1 defines the order of the elements to be ....

           // for (int i = 0; i < intArray1.Count(); i++)
            {
                textBoxCenter.Background = (Brush)converter.ConvertFromString(simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Color);
                textBoxCenter.Width = simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Width;
                textBoxCenter.Height = simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Height;
                textBoxPlus.Width = simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Width;
                textBoxPlus.Height = simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Height;
                
                delay = simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Delay;
                myTimer = new System.Timers.Timer(simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Delay);
                myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Elapsed);
                myTimer.AutoReset = false;
                myTimer.Start();
                life = simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Life;
                
            }

            simpleEarly = true;
            firstSimple = false;
        }
        private void ComplexReactions()
        {


            complexReactions = context.ComplexReactions;
            var converter = new System.Windows.Media.BrushConverter();


            if (firstComplex == true)
            {
                Random rng = new Random();
                int thisArraySize = complexReactions.AsEnumerable().Count();
                intArray1 = new int[thisArraySize];
                for (int i = 0; i < thisArraySize; i++)
                    intArray1[i] = i;

                //randomize stimulus order
                for (int i = intArray1.Count() - 1; i > 0; i--)
                {
                    int swapper = rng.Next(i + 1);
                    if (swapper != i)
                    {
                        int tmp = intArray1[swapper];
                        intArray1[swapper] = intArray1[i];
                        intArray1[i] = tmp;
                    }
                }
            }

            {
                textBoxRight.Background = (Brush)converter.ConvertFromString(complexReactions.AsEnumerable().ElementAt(intArray1[i]).Color);
                textBoxRight.Width = complexReactions.AsEnumerable().ElementAt(intArray1[i]).Width;
                textBoxRight.Height = complexReactions.AsEnumerable().ElementAt(intArray1[i]).Height;
                textBoxLeft.Background = (Brush)converter.ConvertFromString(complexReactions.AsEnumerable().ElementAt(intArray1[i]).Color);
                textBoxLeft.Width = complexReactions.AsEnumerable().ElementAt(intArray1[i]).Width;
                textBoxLeft.Height = complexReactions.AsEnumerable().ElementAt(intArray1[i]).Height;
                delay = complexReactions.AsEnumerable().ElementAt(intArray1[i]).Delay;
                myTimer = new System.Timers.Timer(complexReactions.AsEnumerable().ElementAt(intArray1[i]).Delay);
                myTimer.Elapsed += new System.Timers.ElapsedEventHandler(myTimerComplex_Elapsed);
                myTimer.AutoReset = false;
                myTimer.Start();
                life = complexReactions.AsEnumerable().ElementAt(intArray1[i]).Life;
                location = complexReactions.AsEnumerable().ElementAt(intArray1[i]).Location;
            }

            complexEarly = true;
            firstComplex = false;

        }

            private void myTimer_Elapsed(Object obj, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => //added to fix threading issues causing InvalidOperationException
                {

                    textBoxCenter.Visibility = System.Windows.Visibility.Visible;
                    textBoxPlus.Visibility = System.Windows.Visibility.Hidden;
                    boxShown();
                }));
        }

            private void myTimerComplex_Elapsed(Object obj, ElapsedEventArgs e)
            {
                this.Dispatcher.Invoke((Action)(() => //added to fix threading issues causing InvalidOperationException
                {

                    if (location.Contains('L'))
                    {
                        textBoxLeft.Visibility = System.Windows.Visibility.Visible;
                        boxShown();
                    } else if (location.Contains('R'))
                    {
                        textBoxRight.Visibility = System.Windows.Visibility.Visible;
                        boxShown();
                    }
                    
                }));
            }

        private void boxShown()
        {
            
            myTimer1 = new System.Timers.Timer(life);
            myTimer1.Elapsed += new System.Timers.ElapsedEventHandler(myTimer1_Elapsed);
            myTimer1.AutoReset = false;
            myTimer1.Start();
            timeStart = DateTime.Now;
            if (textBoxCenter.Visibility == System.Windows.Visibility.Visible) // simple trial - one box in the center
            {
                flaggSimple = true;
                simpleEarly = false;
            }
            else if (textBoxRight.Visibility == System.Windows.Visibility.Visible || textBoxLeft.Visibility == System.Windows.Visibility.Visible) //complex trial - two boxes
            {
                flaggComplex = true;
                complexEarly = false;
            }
        }

        
        private void myTimer1_Elapsed(Object obj, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => //added to fix threading issues causing InvalidOperationException
            {
                textBoxRight.Visibility = System.Windows.Visibility.Hidden;
                textBoxLeft.Visibility = System.Windows.Visibility.Hidden;
                textBoxCenter.Visibility = System.Windows.Visibility.Hidden;
                textBoxPlus.Visibility = System.Windows.Visibility.Visible;
                
                if (flaggSimple == true && i + 1 == intArray1.Count() && done == false)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "KeyPressTimeOut");
                    dictionary.Add("eventSummary", "ReactionType:S_RowId:" + simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Id.ToString()+"_Delay:"+delay.ToString()+"");
                    dictionary.Add("eventData", "");
                    taskDataList.Add(dictionary);
                    
                    done = true;
                    textBlockInstructions4.Visibility = System.Windows.Visibility.Visible;
                    textBoxPlus.Visibility = System.Windows.Visibility.Hidden;

                }
                else if (i + 1 == intArray1.Count() && done == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "KeyPressTimeOut");
                    dictionary.Add("eventSummary", "ReactionType:S_RowId:" + simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Id.ToString()+"_Delay:"+delay.ToString() +"");
                    dictionary.Add("eventData", "");
                    taskDataList.Add(dictionary);
                    messagee.ShowBox(InstructionStrings.ThankYouMessage, InstructionStrings.Exit);
                    Dictionary<String, String> dictionary1 = new Dictionary<String, String>();
                    dictionary1.Add("time", DateTime.Now.ToString());
                    dictionary1.Add("eventType", "TaskClosed");
                    dictionary1.Add("eventSummary", "");
                    dictionary1.Add("eventData", "");
                    taskDataList.Add(dictionary1);
                    this.Close();

                }
                else if (flaggComplex == true && i + 1 == intArray1.Count())
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "KeyPressTimeOut");
                    dictionary.Add("eventSummary", "ReactionType:C_RowId:" + complexReactions.AsEnumerable().ElementAt(intArray1[i]).Id.ToString()+"_Delay:"+delay.ToString()+"");
                    dictionary.Add("eventData", "");
                    taskDataList.Add(dictionary);
                    i = 0;
                    textBlockInstructions5.Visibility = System.Windows.Visibility.Visible;
                    textBoxPlus.Visibility = System.Windows.Visibility.Hidden;
                    
                }
             
                if (i < intArray1.Count() -1 && flaggSimple == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "KeyPressTimeOut");
                    dictionary.Add("eventSummary", "ReactionType:S_RowId:" + simpleReactions.AsEnumerable().ElementAt(intArray1[i]).Id.ToString()+"_Delay:"+delay.ToString()+"");
                    dictionary.Add("eventData", "");
                    taskDataList.Add(dictionary);
                    i++;
                    SimpleReactions();
                }
                else if (i < intArray1.Count() - 1 && flaggComplex == true)
                {
                    Dictionary<String, String> dictionary = new Dictionary<String, String>();
                    dictionary.Add("time", DateTime.Now.ToString());
                    dictionary.Add("eventType", "KeyPressTimeOut");
                    dictionary.Add("eventSummary", "ReactionType:C_RowId:" + complexReactions.AsEnumerable().ElementAt(intArray1[i]).Id.ToString()+"_Delay:"+delay.ToString()+"");
                    dictionary.Add("eventData", "");
                    taskDataList.Add(dictionary);
                    i++;
                    ComplexReactions();
                }

                flaggSimple = false;
                flaggComplex = false;
            }));
        }
       

        

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBlockInstructions1.Visibility = System.Windows.Visibility.Visible;
            this.Focus();
            //
            taskDataList = new List<Dictionary<String, String>>();
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            dictionary.Add("time", DateTime.Now.ToString());
            dictionary.Add("eventType", "TaskStarted");
            dictionary.Add("eventSummary", "");
            dictionary.Add("eventData", "");
            taskDataList.Add(dictionary);
        }

        public List<Dictionary<String, String>> getEventData()
        {
            return taskDataList;
        }

        //
        //this method allows us to play error sound.wav more than once
        private void mediaElement1_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement1.Stop();
            mediaElement1.Position = TimeSpan.Zero;
        }
       
        
    }
}
