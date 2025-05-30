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
using donely_Inspilab.Pages;

namespace donely_Inspilab.Pages.Group
{
    /// <summary>
    /// Interaction logic for AddRewardWindow.xaml
    /// </summary>
    public partial class AddRewardWindow : Window
    {
        public AddRewardWindow()
        {
            InitializeComponent();
        }

        public ShopItem NewShopItem { get; private set; } //Dit is het item dat de GroupCreationPage toevoegd aan de lijst, daarom public

        private void AddShopItem_Click(object sender, RoutedEventArgs e)
        {
            try{
                NewShopItem = new ShopItem(txtName.Text, txtDescription.Text, Convert.ToInt32(txtPrice.Text), Convert.ToInt32(txtLimit.Text));
                this.DialogResult = true;
                this.Close();
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input: Please make sure that Price and Limit are numbers", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong - {ex}", "Item not added", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
