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

namespace donely_Inspilab.Pages.Shop
{
    public partial class MemberShopPage : Page
    {
        private GroupMember CurrentMember;
        private List<ShopItem> ShopItems;

        public MemberShopPage()
        {
            InitializeComponent();
            LoadItems();
        }

        private void LoadItems()
        {
            CurrentMember = (GroupMember)GroupState.LoadedGroup.Members.FirstOrDefault(t => t.UserId == SessionManager.GetCurrentUserID());
            lblCurrency.Content = CurrentMember.Currency;

            Database db = new Database();
            var allShopItems = db.GetShopItems(GroupState.LoadedGroup.Id);

            // Filter items by their limit
            List<ShopItem> filteredShopItems = new List<ShopItem>();

            foreach (var item in allShopItems)
            {
                if (item.Limit == 0) // No limit, always show
                {
                    filteredShopItems.Add(item);
                }
                else
                {
                    // Check how many times this user bought this item
                    int boughtCount = db.GetBoughtItemCount(CurrentMember.Id, item.Id);

                    if (boughtCount < item.Limit)
                    {
                        filteredShopItems.Add(item);
                    }
                    // else: Don't add, limit reached
                }
            }

            lsvShopItems.ItemsSource = filteredShopItems;
        }


        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = lsvShopItems.SelectedItem as ShopItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Please select an item to buy.", "No Item Selected", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (CurrentMember.Currency < selectedItem.Price)
            {
                MessageBox.Show("Not enough coins!", "Insufficient Funds", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Subtract coins
            CurrentMember.Currency -= selectedItem.Price;
            lblCurrency.Content = CurrentMember.Currency;

            Database db = new Database();

            // Save new currency in DB
            db.UpdateMemberCurrency(CurrentMember.Id, CurrentMember.Currency);

            // Register the bought item in the bought_items table
            db.RegisterBoughtItem(CurrentMember.Id, selectedItem.Id);

            MessageBox.Show($"You bought {selectedItem.Name}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            // Refresh the shop items list
            LoadItems();
        }


    }
}
