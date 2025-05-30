using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    //Specifieke klasse voor overzicht van gekochte items bij de owner --> wat hebben de users gekocht
    public class BoughtItemOverview
    {
            public string UserName { get; set; }
            public string ItemName { get; set; }
            public DateTime Time { get; set; }

        public BoughtItemOverview(string username, string itemname, DateTime time)
        {
            UserName = username;
            ItemName = itemname;
            Time = time;
        }
    }
}
