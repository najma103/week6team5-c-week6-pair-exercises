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

        private const string SQL_GetCurrentReservation = @"SELECT r.site_id, r.name, r.from_date, r.to_date, create_date, r.reservation_id FROM park p
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

        public Dictionary<int, Reservation> GetCurrentReservations(int parkId)
        {
            Dictionary<int, Reservation> dictReservations = new Dictionary<int, Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetCurrentReservation, conn);
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Reservation reservation = new Reservation();

                        reservation.ReservationId = Convert.ToInt32(reader["reservation_id"]);
                        reservation.SiteId = Convert.ToInt32(reader["site_id"]);
                        reservation.Name = Convert.ToString(reader["name"]);
                        reservation.FromDate = Convert.ToDateTime(reader["from_date"]);
                        reservation.ToDate = Convert.ToDateTime(reader["to_date"]);
                        reservation.CreateDate = Convert.ToDateTime(reader["create_date"]);

                        dictReservations[reservation.ReservationId] = reservation;
                    }
                    return dictReservations;
                }

            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}
