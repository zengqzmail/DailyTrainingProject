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

namespace TasksMainMenu
{
    public partial class PopUp : Window
    {
        
        public PopUp()
        {
            InitializeComponent();
        }

        private void Window_Loaded()
        {
            this.DialogResult = null; //this will throw an exception if placed in constructor.
        }

        // Attach this to the click event of your "OK" button
        private void OnYesButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        // Attach this to the click event of your "Cancel" button
        private void OnNoButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
