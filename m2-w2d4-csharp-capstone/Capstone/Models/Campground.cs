using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Campground
    {
        public int CampgroundId { get; set; }
        public int ParkId { get; set; }
        public string Name { get; set; }
        public int OpenFromMonth { get; set; }
        public int OpenToMonth { get; set; }
        public double DailyFee { get; set; }



        public override string ToString()
        {
            string[] months = new string[] {"empty","January", "February", "March", "April", "May", "June",
                                    "July", "August", "September", "October", "November", "December"};
            string strFromMonth = months[OpenFromMonth].ToString();
            string strToMonth = months[OpenToMonth].ToString();
            return CampgroundId.ToString().PadLeft(5).PadRight(17) + Name.PadRight(32) + strFromMonth.PadRight(16) + strToMonth.PadRight(16) + DailyFee.ToString().PadRight(5);
        }
    }
}
