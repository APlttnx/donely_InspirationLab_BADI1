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
using donely_Inspilab.Classes;

namespace donely_Inspilab.Pages.Group
{
    /// <summary>
    /// Interaction logic for GroupOwnerPage.xaml
    /// </summary>
    public partial class GroupOwnerPage : Page
    {
        public GroupOwnerPage()
        {
            InitializeComponent();
        }

        private void ToEditGroupWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToInviteCodeWindow_Click(object sender, RoutedEventArgs e)
        {
            ShowInviteCodeWindow codeWindow = new();
                codeWindow.Show();
        }

        private void ToMemberDetailPage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ToHomePage_Click(object sender, RoutedEventArgs e)
        {
            NavService.ToHomePage();
        }
    }
}
