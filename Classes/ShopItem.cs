using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace donely_Inspilab.Classes
{
    public class ShopItem
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int Limit { get; set; }

        public ShopItem(string _name, string _description, int _price, int _limit = 0)
        {
            if (string.IsNullOrWhiteSpace(_name)) throw new ArgumentException("Name is required.");
            if (_price < 0) throw new ArgumentException("Price must be greater than 1.");
            if (_limit <= 0) throw new ArgumentException("Limit can't be negative");

            Name = _name;
            Description = _description;
            Price = _price;
            Limit = _limit;
        }



    }
}
