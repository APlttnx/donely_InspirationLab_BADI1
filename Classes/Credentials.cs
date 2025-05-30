using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    //Credentials dient als miniklasse specifiek voor de properties die nodig zijn om in te loggen. Kon ook via een kleine constructor in de user klasse, maar maakte het onoverzichtelijk
    //Ook gebruik van primaire constructor --> parameters worden rechtstreeks toegewezen aan properties
    public class Credentials(int _userId, string _hashedPassword, bool _isF2A)
    {
        public int UserID { get; set; } = _userId;
        public string HashedPassword { get; set; } = _hashedPassword;
        public bool IsF2A { get; set; } = _isF2A;

    }
}
