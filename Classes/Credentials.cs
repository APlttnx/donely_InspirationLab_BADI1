using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Classes
{
    public class Credentials(int _userId, string _hashedPassword, bool _isF2A)
    {
        public int UserID { get; set; } = _userId;
        public string HashedPassword { get; set; } = _hashedPassword;
        public bool IsF2A { get; set; } = _isF2A;

    }
}
