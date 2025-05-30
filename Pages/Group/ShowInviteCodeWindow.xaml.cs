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
using System.Windows.Shapes;
using donely_Inspilab.Classes;

namespace donely_Inspilab.Pages.Group
{
    /// <summary>
    /// Interaction logic for ShowInviteCode.xaml
    /// </summary>
    public partial class ShowInviteCodeWindow : Window
    {
        public ShowInviteCodeWindow()
        {
            InitializeComponent();
            txtGroupCode.Text = GroupState.LoadedGroup.InviteCode;
        }

        private void CopyCode_click(object sender, RoutedEventArgs e) //Kopieer invite code to clipboard
        {
            Clipboard.SetText(txtGroupCode.Text);
            MessageBox.Show("Code Copied", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
