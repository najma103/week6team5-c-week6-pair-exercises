using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public int CampgroundId { get; set; }
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool Accessible { get; set; }
        public int MaxRVLength { get; set; }
        public bool Utilities { get; set; }


        public override string ToString()
        {
            return SiteId.ToString().PadRight(6) + CampgroundId.ToString().PadRight(30) + SiteNumber.ToString().PadRight(30) + MaxOccupancy.ToString().PadRight(10) + Accessible.ToString().PadRight(10) + MaxRVLength.ToString().PadRight(30) + Utilities.ToString().PadRight(10);
        }
    }
}
