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
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }

        //testing purposes
        //Questions2 test = new Questions2();
        //Questions1 test3 = new Questions1();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        //List<Dictionary<String, String>> taskDataList;
        
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            VideoPrescriptions test = new VideoPrescriptions("ES");
            this.Content = test;
        }
    }
}
