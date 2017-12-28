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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for InstructionsPart2.xaml
    /// </summary>
    public partial class InstructionsPart2 : UserControl
    {
        public InstructionsPart2()
        {
            InitializeComponent();
            //
            mediaElement1.Source = new Uri(String.Format(@"{0}", Videos.INFO_2A), UriKind.Relative);
            button1.Content = InstructionStrings.Replay;
            button2.Content = InstructionStrings.Continue;
        }



        
        
        
        bool replay = true;
        int replayCount = 0;


        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
        private void mediaOpened1(object sender, RoutedEventArgs e)
        {
            
        }
        private void mediaElement1_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement1.Visibility = System.Windows.Visibility.Hidden;

            mediaElement2.Source = new Uri(String.Format(@"{0}", Videos.INFO_2B), UriKind.Relative);
        }

        private void mediaElement2_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement2.Visibility = System.Windows.Visibility.Hidden;
            
            mediaElement3.Visibility = System.Windows.Visibility.Visible;

            mediaElement3.Source = new Uri(String.Format(@"{0}", Videos.INFO_2C), UriKind.Relative);
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
                button1.Visibility = System.Windows.Visibility.Visible;
            }
            button2.Visibility = System.Windows.Visibility.Visible;

        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {

            replayCount++;
            mediaElement3.Stop() ;
            mediaElement3.Visibility = System.Windows.Visibility.Hidden;
           
            mediaElement2.Visibility = System.Windows.Visibility.Visible;
            mediaElement2.Source = new Uri(String.Format(@"{0}", Videos.INFO_2A), UriKind.Relative);
            MainWindow.recordVideoReplay(mediaElement2.Source.ToString(), replayCount);
            
            button1.Visibility = System.Windows.Visibility.Hidden;
            button2.Visibility = System.Windows.Visibility.Hidden;


            if (replayCount >= MainWindow.allowedNumberReplays)
            {
                replay = false;
            }
            

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Questions2 questions2 = new Questions2();
            this.Content = questions2;
        }
    }
}