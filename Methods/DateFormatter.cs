using donely_Inspilab.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace donely_Inspilab.Methods
{
    public static class DateFormatter //formats dates for display in listviews
    {
        public static string FormatDate(DateTime dateTime)
        {
            return (dateTime.ToString("dd-MM-yyyy"));
        }
        public static string FormatDate(DateOnly date)
        {
            return (date.ToString("dd-MM-yyyy"));
        }
    }
}
