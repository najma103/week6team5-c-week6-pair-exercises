using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    class ReservationSqlDal
    {
        private string connectionString;
        private const string SQL_SelectLastRow = @"SELECT TOP 1(reservation_id) FROM reservation r
                                                   ORDER BY r.reservation_id DESC;";

        private const string SQL_InsertNewRow = @"insert into reservation values(@siteId, @name, @fromDate, @toDate, @createDate)";

        private const string SQL_GetCurrentReservation = @"SELECT r.name, r.from_date, r.to_date, r.reservation_id FROM park p
                                                        join campground c ON p.park_id = c.park_id
                                                        join site s ON c.campground_id = s.campground_id
                                                        join reservation r ON s.site_id = r.site_id
                                                        WHERE datediff(day, getdate(),r.from_date) <= 30 AND datediff(day, getdate(),r.from_date) >= 0 AND p.park_id = @parkId
                                                        ORDER BY r.from_date;";
        public ReservationSqlDal(string strConnection)
        {
            connectionString = strConnection;
        }
        public bool CreateReservation(Reservation newReservation)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_InsertNewRow, conn);

                    cmd.Parameters.AddWithValue("@siteId", newReservation.SiteId);
                    cmd.Parameters.AddWithValue("@name", newReservation.Name);
                    cmd.Parameters.AddWithValue("@fromDate", newReservation.FromDate);
                    cmd.Parameters.AddWithValue("@toDate", newReservation.ToDate);
                    cmd.Parameters.AddWithValue("@createDate", newReservation.CreateDate);


                    int rowsAffected = cmd.ExecuteNonQuery();

                    return (rowsAffected > 0); //true if one row was affected
                }
            }
            catch (SqlException ex)
            {
                //log
                throw;
            }
        }

        //public List<Reservation> GetCurrentReservations(int parkId)
        //{
        //    List<Reservation> currReservations = new List<Reservation>();
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(connectionString))
        //        {
        //            conn.Open();

        //            SqlCommand cmd = new SqlCommand(SQL_GetCurrentReservation, conn);

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                Campground camps = new Campground();

        //                camps.CampgroundId = Convert.ToInt32(reader["campground_id"]);
        //                camps.ParkId = Convert.ToInt32(reader["park_id"]);
        //                camps.Name = Convert.ToString(reader["name"]);
        //                camps.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
        //                camps.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
        //                camps.DailyFee = Convert.ToDouble(reader["daily_fee"]);


        //                listOfCamps.Add(camps);

        //            }
        //            return listOfCamps;
        //        }

        //    }
        //    catch (SqlException e)
        //    {
        //        throw e;
        //    }
        //}
    }
}
