using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Reservation
    {

            public int ReservationId { get; set; }
            public int SiteId { get; set; }
            public string Name { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public DateTime CreateDate { get; set; }


            public override string ToString()
            {
                return ReservationId.ToString().PadRight(9) + SiteId.ToString().PadRight(24) + Name.ToString().PadRight(32) + FromDate.ToShortDateString().PadRight(15) + ToDate.ToShortDateString().PadRight(17) + CreateDate.ToShortDateString().PadRight(10);
            }
        }
}
