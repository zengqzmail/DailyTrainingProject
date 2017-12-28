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
using System.Windows.Shapes;

namespace MetroTickets
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
       
        public DialogWindow()
        {
            InitializeComponent();
        }

        public DialogWindow(string p, string p_2) 
            : this()
        {
            // TODO: Complete member initialization
            this.textBlock.Text = p;
            this.ContinueButton.Content = p_2;
           
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
