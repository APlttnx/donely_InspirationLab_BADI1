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
using System.Windows.Shapes;
using System.Xml.Linq;

namespace donely_Inspilab.Pages.Group
{
    /// <summary>
    /// Interaction logic for JoinGroupWindow.xaml
    /// </summary>
    public partial class JoinGroupWindow : Window
    {
        public JoinGroupWindow()
        {
            InitializeComponent();
        }

        public string Code { get; private set; }
        private void JoinGroup_click(object sender, RoutedEventArgs e)
        {
            try
            {
                Code = txtGroupCode.Text;
                if (Code.Length != 8) throw new ArgumentException("Invalid input");


                this.DialogResult = true;
                this.Close();
            }
            
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong - {ex}", "Fail", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

    }
}
