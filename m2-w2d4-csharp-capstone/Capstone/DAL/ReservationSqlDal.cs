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
    }
}
