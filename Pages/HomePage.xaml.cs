using donely_Inspilab.Classes;
using donely_Inspilab.Pages.Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace donely_Inspilab.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            
            InitializeComponent();
        }

        private void GoToSettings(object sender, RoutedEventArgs e)
        {
            NavService.ToSettingsPage();
        }
    }
}
