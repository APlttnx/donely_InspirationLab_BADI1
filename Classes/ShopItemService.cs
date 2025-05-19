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
        public static void InsertShopItems(List<ShopItem> shopItems, int groupID)
        {
            Database db = new();
            bool result = db.InsertShopItems(shopItems, groupID);
            if (!result)
                throw new ArgumentException("Something went wrong adding shop items");
        }
    }
}
