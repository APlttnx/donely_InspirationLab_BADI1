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

namespace donely_Inspilab.Pages.Admin
{
    public partial class AdminDashboard : Page
    {
        public AdminDashboard()
        {
            InitializeComponent();
            LoadStats();
        }

        private void GoToUsers(object sender, MouseButtonEventArgs e)
        {
            NavService.ToAdminOverview();
        }


        private void LoadStats()
        {
            Database db = new();
            TxtUserCount.Text = db.GetTotalUserCount().ToString();
            TxtGroupCount.Text = db.GetTotalGroupCount().ToString();
        }

        private void UsersCard_Click(object sender, MouseButtonEventArgs e)
        {
            NavService.ToAdminOverview();
        }
    }
}
