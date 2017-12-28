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

//testing tfs email notification
namespace MainMenu_bran
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        string password;
        new MainMenuContainer context = new MainMenu_bran.MainMenuContainer();

        public Assesor loggedInAssesor;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private System.Data.Objects.ObjectQuery<Assesor> GetAssesorsQuery(MainMenuContainer mainMenuContainer)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<MainMenu_bran.Assesor> assesorsQuery = mainMenuContainer.Assesors;
            // Returns an ObjectQuery.
            return assesorsQuery;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

           // MainMenu_bran.MainMenuContainer mainMenuContainer = new MainMenu_bran.MainMenuContainer();
            // Load data into Assesors. You can modify this code as needed.
           var assesorsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("assesorsViewSource")));
            var assesorsQuery = this.GetAssesorsQuery(context);
            assesorsViewSource.Source = assesorsQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
           // var assesor = from assesors in context.Assesors where assesors.AssesorName == userNameComboBox.SelectionBoxItem.ToString() select assesors;
            var assesor = ((Assesor)userNameComboBox.SelectedItem);
            password = passwordTextBox.Text;
            if (assesor.Password == password)
            {
                MessageBox.Show("Correct!!");
                this.loggedInAssesor = assesor; //pass this to public field so MainWindow can pick it up
                this.Close();
            }
            else
            {
                MessageBox.Show("Incorrect!!");
            }
        }
        
        
    }
}
