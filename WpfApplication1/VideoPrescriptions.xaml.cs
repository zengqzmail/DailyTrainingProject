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
using System.Windows.Threading;
using System.Threading;
using System.Globalization;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for VideoPrescriptions.xaml
    /// </summary>
    public partial class VideoPrescriptions : UserControl
    {
        string taskLanguage = "EN"; //english by default

        public VideoPrescriptions()
        {
            InitializeComponent();
            //
           
        }

        public VideoPrescriptions(string taskLanguage)
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

            //until I grok WPF localization, set mediaElement1.Source and image[1-4].Source here
            mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.INFO_1A), UriKind.Relative);
            image1.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.Parlenol), UriKind.Relative));
            image2.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.BRB), UriKind.Relative));
            image3.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.Linophen), UriKind.Relative));
            image4.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.Anaanx), UriKind.Relative));
            image5.Source = new BitmapImage(new Uri(String.Format(@"{0}", Images.Cyclomeovan), UriKind.Relative));
            //
            replayBtn.Content = InstructionStrings.Replay;
            if (taskLanguage.Equals("EN"))
            {
                Grid.SetColumnSpan(replayBtn, 2);
            }
           
            continueBtn.Content = InstructionStrings.Continue;
        }
        
        
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer dispatcherTimer1 = new DispatcherTimer();
        DispatcherTimer dispatcherTimer2 = new DispatcherTimer();
        DispatcherTimer dispatcherTimer3 = new DispatcherTimer();
        DispatcherTimer dispatcherTimer4 = new DispatcherTimer();
        DispatcherTimer dispatcherTimer5 = new DispatcherTimer();
        DispatcherTimer dispatcherTimer6 = new DispatcherTimer();
        DispatcherTimer dispatcherTimer7 = new DispatcherTimer();
        DispatcherTimer dispatcherTimer8 = new DispatcherTimer();
        DispatcherTimer dispatcherTimer9 = new DispatcherTimer();
        
        bool replay = true;
        int replayCount = 0;
        
        bool image1a = true;
        bool image2a = true;
        bool image3a = true;
        bool image4a = true;
        bool image5a = true;



        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (image1a.Equals(true))
            {
                image1.Visibility = System.Windows.Visibility.Visible;
            }
            dispatcherTimer.Stop();
            
            image1a = false;
            
            dispatcherTimer1.Tick += new EventHandler(dispatcherTimer1_Tick);

            dispatcherTimer1.Interval = new TimeSpan(0, 0, 12);

            dispatcherTimer1.Start();
        }

        
        private void dispatcherTimer1_Tick(object sender, EventArgs e)
        {
            if (image2a.Equals(true))
            {
            image2.Visibility = System.Windows.Visibility.Visible;
            }
            image2a = false;
           
            dispatcherTimer1.Stop();
            
            dispatcherTimer2.Tick += new EventHandler(dispatcherTimer2_Tick);

            dispatcherTimer2.Interval = new TimeSpan(0, 0, 14);

            dispatcherTimer2.Start();

        }

        private void dispatcherTimer2_Tick(object sender, EventArgs e)
        {
            if (image3a.Equals(true))
            {
                image3.Visibility = System.Windows.Visibility.Visible;
            }
            image3a = false;
           
            dispatcherTimer2.Stop();
            
            dispatcherTimer3.Tick += new EventHandler(dispatcherTimer3_Tick);

            dispatcherTimer3.Interval = new TimeSpan(0, 0, 14);

            dispatcherTimer3.Start();

        }

        private void dispatcherTimer3_Tick(object sender, EventArgs e)
        {
            if (image4a.Equals(true))
            {
            image4.Visibility = System.Windows.Visibility.Visible;
            }
            image4a = false;
           
            dispatcherTimer3.Stop();
            
            dispatcherTimer4.Tick += new EventHandler(dispatcherTimer4_Tick);

            dispatcherTimer4.Interval = new TimeSpan(0, 0, 14);

            dispatcherTimer4.Start();

        }

        private void dispatcherTimer4_Tick(object sender, EventArgs e)
        {
            if (image5a.Equals(true))
            {
                image5.Visibility = System.Windows.Visibility.Visible;
            }
            image5a = false;
            
            dispatcherTimer4.Stop();
            

        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        private void mediaOpened1(object sender, RoutedEventArgs e)
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 31);

            dispatcherTimer.Start();
            
        }
        private void mediaElement1_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement1.Visibility = System.Windows.Visibility.Hidden;
            mediaElement2.Visibility = System.Windows.Visibility.Visible;
            mediaElement2.Source = new Uri(String.Format(@"{0}",Videos.INFO_1B), UriKind.Relative);
        }

        private void mediaElement2_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement2.Visibility = System.Windows.Visibility.Hidden;
            
            mediaElement3.Visibility = System.Windows.Visibility.Visible;

            mediaElement3.Source = new Uri(String.Format(@"{0}", Videos.INFO_1C), UriKind.Relative);
            mediaElement3.Play();
            mediaStart3(sender, e);
        }

       
        private void mediaElement3_MediaEnded(object sender, RoutedEventArgs e)
        {
        }
        private void mediaStart3(object sender, RoutedEventArgs e)
        {

            if (replay.Equals(true))
            {
                replayBtn.Visibility = System.Windows.Visibility.Visible;
            }
            continueBtn.Visibility = System.Windows.Visibility.Visible;

        }

        private void replayBtn_Click(object sender, RoutedEventArgs e)
        {


            mediaElement3.Stop() ;
            mediaElement3.Visibility = System.Windows.Visibility.Hidden;
           
            mediaElement2.Visibility = System.Windows.Visibility.Visible;
            mediaElement2.Source = new Uri(String.Format(@"{0}",Videos.INFO_1A), UriKind.Relative);

            image1.Visibility = System.Windows.Visibility.Hidden;
            image2.Visibility = System.Windows.Visibility.Hidden;
            image3.Visibility = System.Windows.Visibility.Hidden;
            image4.Visibility = System.Windows.Visibility.Hidden;
            image5.Visibility = System.Windows.Visibility.Hidden;
            replayBtn.Visibility = System.Windows.Visibility.Hidden;
            continueBtn.Visibility = System.Windows.Visibility.Hidden;

            dispatcherTimer5.Interval = new TimeSpan(0, 0, 31);
            dispatcherTimer5.Tick += new EventHandler(dispatcherTimer5_Tick);

           

            dispatcherTimer5.Start();

            replayCount++;
            MainWindow.recordVideoReplay(mediaElement2.Source.ToString(), replayCount);
            //
            if (replayCount >= MainWindow.allowedNumberReplays)
            {
                replay = false;
            }
 
        }

        private void continueBtn_Click(object sender, RoutedEventArgs e)
        {
            Questions1 questions1 = new Questions1();
            
            this.Content = questions1;
        }

        private void dispatcherTimer5_Tick(object sender, EventArgs e)
        {
            
                image1.Visibility = System.Windows.Visibility.Visible;
            
            dispatcherTimer5.Stop();

           

            dispatcherTimer6.Tick += new EventHandler(dispatcherTimer6_Tick);

            dispatcherTimer6.Interval = new TimeSpan(0, 0, 12);

            dispatcherTimer6.Start();
        }

        
        private void dispatcherTimer6_Tick(object sender, EventArgs e)
        {
            
                image2.Visibility = System.Windows.Visibility.Visible;
           
            dispatcherTimer6.Stop();

            dispatcherTimer7.Tick += new EventHandler(dispatcherTimer7_Tick);

            dispatcherTimer7.Interval = new TimeSpan(0, 0, 14);

            dispatcherTimer7.Start();

        }

        private void dispatcherTimer7_Tick(object sender, EventArgs e)
        {
            
                image3.Visibility = System.Windows.Visibility.Visible;
            
            
            dispatcherTimer7.Stop();

            dispatcherTimer8.Tick += new EventHandler(dispatcherTimer8_Tick);

            dispatcherTimer8.Interval = new TimeSpan(0, 0, 14);

            dispatcherTimer8.Start();

        }

        private void dispatcherTimer8_Tick(object sender, EventArgs e)
        {
            
                image4.Visibility = System.Windows.Visibility.Visible;
            
            dispatcherTimer8.Stop();

            dispatcherTimer9.Tick += new EventHandler(dispatcherTimer9_Tick);

            dispatcherTimer9.Interval = new TimeSpan(0, 0, 14);

            dispatcherTimer9.Start();

        }

        private void dispatcherTimer9_Tick(object sender, EventArgs e)
        {
            image5.Visibility = System.Windows.Visibility.Visible;
            dispatcherTimer9.Stop();
        }
    }
}
