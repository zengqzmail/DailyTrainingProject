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
    /// <summary>
    /// Interaction logic for LanguageSelectScreen.xaml
    /// </summary>
    public partial class LanguageSelectScreen : UserControl
    {
        internal string selectedLanguage;

        public LanguageSelectScreen()
        {
            InitializeComponent();
        }


        private void selectEnglish(object sender, RoutedEventArgs e)
        {
            this.selectedLanguage = "EN";
        }

        private void selectSpanish(object sender, RoutedEventArgs e)
        {
            this.selectedLanguage = "ES";
        }
    }
}
