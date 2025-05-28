using donely_Inspilab.Classes;
using donely_Inspilab.Pages.Group;
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
using static donely_Inspilab.Classes.Database;

namespace donely_Inspilab.Pages.Shop
{
    /// <summary>
    /// Interaction logic for OwnerManageShop.xaml
    /// </summary>
    public partial class OwnerManageShop : Page
    {
        private List<BoughtItemOverview> BoughtItemsList;
        private List<ShopItem> ShopItems;
        public OwnerManageShop()

        {
            InitializeComponent();
            LoadItems();
            LoadBoughtItems();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void LoadItems()
        {
            // Adjust groupId as needed (from current group context)
            int groupId = GroupState.LoadedGroup.Id; // Or however you get the current group
            Database db = new Database();
            var shopItems = db.GetShopItems(groupId);
            lsvShopItems.ItemsSource = shopItems;
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            // Open AddRewardWindow for a new item
            var addWindow = new AddRewardWindow();
            if (addWindow.ShowDialog() == true)
            {
                // Get the newly created item from the window
                var newItem = addWindow.NewShopItem;
                if (newItem != null)
                {
                    var db = new Database();
                    db.InsertShopItems(new List<ShopItem> { newItem }, GroupState.LoadedGroup.Id);
                    LoadItems(); // Refresh list
                }
            }
        }

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lsvShopItems.SelectedItem as ShopItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to edit.", "No Item Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Open EditItemWindow with a copy of the item to avoid editing the list directly before confirmation
            ShopItem itemCopy = new ShopItem(selectedItem.Name, selectedItem.Description, selectedItem.Price, selectedItem.Limit)
            {
                Id = selectedItem.Id // You need to preserve the Id for updating
            };

            var editWindow = new EditItemWindow(itemCopy);
            if (editWindow.ShowDialog() == true)
            {
                // Save changes to the database
                Database db = new Database();
                db.UpdateShopItem(itemCopy);

                // Update item in list and refresh ListView
                selectedItem.Name = itemCopy.Name;
                selectedItem.Description = itemCopy.Description;
                selectedItem.Price = itemCopy.Price;
                selectedItem.Limit = itemCopy.Limit;
                lsvShopItems.Items.Refresh();
            }
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lsvShopItems.SelectedItem as ShopItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to delete.", "No Item Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Confirm deletion
            var result = MessageBox.Show($"Are you sure you want to delete '{selectedItem.Name}'?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Database db = new Database();
                db.DeleteShopItem(selectedItem.Id);
                LoadItems(); // Refresh list
            }
        }

        private void LoadBoughtItems()
        {
            Database db = new Database();
            BoughtItemsList = db.GetBoughtItemsOverview(GroupState.LoadedGroup.Id);
            lsvBoughtItems.ItemsSource = BoughtItemsList;
        }
    }
}
