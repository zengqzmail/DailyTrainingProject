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
    /// Interaction logic for PillOrganizerNumberPicker.xaml
    /// TODO: reduce button width, make them square
    /// TODO: place number textbox BETWEEN buttons
    /// TODO: increase number textbox width by ~50%
    /// TODO: increase size of plus, minus icons
    /// </summary>
    public partial class PillOrganizerNumberPicker : UserControl
    {

        private string _medicationName;

        public string MedicationName
        {
            get { return _medicationName; }
            set 
            { 
                _medicationName = value;
                this.medLabel.Content = value;
            }
        }

        private int _numValue = 0;
        public int NumValue
        {
            get { return _numValue; }
            set
            {
                _numValue = value;
                txtNum.Text = value.ToString();
            }
        }

        private int lowerBound = 0;
        private int upperBound = 6;
        //

        public PillOrganizerNumberPicker()
        {
            InitializeComponent();  
        }

        public PillOrganizerNumberPicker(string medName)
            : this()
        {
            this._medicationName = medName;
        }

        private void cmdPlus_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue < upperBound)
            {
                NumValue++;
            }    
        }

        private void cmdMinus_Click(object sender, RoutedEventArgs e)
        {
            if (NumValue > lowerBound)
            {
                NumValue--;
            }
        }

        private void txtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(txtNum.Text, out _numValue))
                txtNum.Text = _numValue.ToString();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //nothing for now.

        }
    }
}
