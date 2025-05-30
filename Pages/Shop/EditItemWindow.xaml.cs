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

namespace donely_Inspilab.Pages.Shop
{
    public partial class EditItemWindow : Window
    {
        public ShopItem Item { get; private set; }

        public EditItemWindow(ShopItem item)
        {
            InitializeComponent();
            Item = item;
            TxtName.Text = item.Name;
            TxtDescription.Text = item.Description;
            TxtPrice.Text = item.Price.ToString();
            TxtLimit.Text = item.Limit.ToString();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtName.Text) ||
                //string.IsNullOrWhiteSpace(TxtDescription.Text) ||
                !int.TryParse(TxtPrice.Text, out int price) ||
                !int.TryParse(TxtLimit.Text, out int limit))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Item.Name = TxtName.Text;
            Item.Description = TxtDescription.Text;
            Item.Price = price;
            Item.Limit = limit;

            this.DialogResult = true;
            this.Close();
        }
    }
}