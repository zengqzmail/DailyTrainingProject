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
    /// Interaction logic for PillOrganizer.xaml
    /// </summary>
    public partial class PillOrganizer : UserControl
    {

        public PillOrganizer()
        {
            InitializeComponent();
        }

        private void InitMedicationLabels()
        {
            this.Parlenol.MedicationName = "Parlenol";
            this.BRB.MedicationName = "BRB";
            this.Linophen.MedicationName = "Linophonen";
            this.Anaanx.MedicationName = "Anaanx";
            this.Cyclomeovan.MedicationName = "Cyclomeovan";
        }

        private void PillOrganizer_Loaded(object sender, RoutedEventArgs e)
        {
            InitMedicationLabels();
        }

        /*
         * returns amount of each medication selected by user
         * gets amount from PillOrganizerNumberPicker.NumValue
         */
        public Dictionary<String, Int32> GetMedicationAmounts()
        {
            //throw new NotImplementedException();
            //
            Dictionary<String, Int32> medAmounts = new Dictionary<string,int>{
                {"Parlenol",    Parlenol.NumValue},
                {"BRB",         BRB.NumValue},
                {"Linophen",    Linophen.NumValue},
                {"Anaanx",      Anaanx.NumValue},
                {"Cyclomeovan", Cyclomeovan.NumValue },
            };

            return medAmounts;
        }
    }
}
