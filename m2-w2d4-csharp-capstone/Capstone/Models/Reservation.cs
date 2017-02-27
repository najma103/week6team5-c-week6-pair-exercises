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
                return ReservationId.ToString().PadRight(10) + SiteId.ToString().PadRight(10) + Name.ToString().PadRight(30) + FromDate.ToShortDateString().PadRight(10) + ToDate.ToShortDateString().PadRight(10) + CreateDate.ToShortDateString().PadRight(30);
            }
        }
}
