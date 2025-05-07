using donely_Inspilab.Pages.auth;
using donely_Inspilab.Classes;
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
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            InitializeComponent();
        }
        private void ToRegisterPage(object sender, RoutedEventArgs e)
        {
            NavService.ToRegisterPage();
        }
        private void ToLoginPage(object sender, RoutedEventArgs e)
        {
            NavService.ToLoginPage();
        }
    }
}
