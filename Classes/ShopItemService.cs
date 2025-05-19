using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using donely_Inspilab.Classes;

namespace donely_Inspilab.Classes
{
    public class ShopItemService
    {
        public static bool InsertShopItems(List<ShopItem> shopItems, int groupID)
        {
            Database db = new();
            int result = db.InsertShopItems(shopItems, groupID);
            if (result == shopItems.Count)
                throw new ArgumentException("Something went wrong adding shop items");
            return true;
        }
    }
}
