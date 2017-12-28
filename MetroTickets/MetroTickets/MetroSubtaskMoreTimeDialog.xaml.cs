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

//following pattern outlined in http://stackoverflow.com/questions/4593449/wpf-querying-user-with-modal-dialog-box-inputdialog
namespace MetroTickets
{
    /// <summary>
    /// Interaction logic for MetroSubtaskMoreTimeDialog.xaml
    /// </summary>
    public partial class MetroSubtaskMoreTimeDialog : Window
    {
        
        public MetroSubtaskMoreTimeDialog()
        {
            InitializeComponent();
            this.stmtdTextBlock.Text = MetroInstructionStrings.SubtaskMoreTimePrompt;
            this.stmtdYesButton.Content = MetroInstructionStrings.Yes;
            this.stmtdNoButton.Content = MetroInstructionStrings.No;
        }

        private void Window_Loaded()
        {
            this.DialogResult = null; //this will throw an exception if placed in constructor.
        }

        public void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Closing called, are you sure you want to close it?", "Closing", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
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
