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
    /// Interaction logic for GroupDashboardPage.xaml
    /// </summary>
    public partial class GroupDashboardPage : Page
    {
        public GroupDashboardPage()
        {
            InitializeComponent();
            lblTest.Content = GroupState.LoadedGroup.Name;
        } 
    }
}
