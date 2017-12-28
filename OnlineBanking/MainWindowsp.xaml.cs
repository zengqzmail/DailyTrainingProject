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

namespace OnlineBanking
{
    /// <summary>
    /// Interaction logic for MainWindowsp.xaml
    /// </summary>
    public partial class MainWindowsp : UserControl
    {
        public MainWindowsp()
        {
            InitializeComponent();
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            Window parent = Window.GetWindow(this);
            parent.Close();
        }
    }
}
