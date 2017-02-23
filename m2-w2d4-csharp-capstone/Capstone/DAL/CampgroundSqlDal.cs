using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    class CampgroundSqlDal
    {
        private string connectionString;
        private const string SQL_SelectAllCampsByParkId = 
                                                    @"select * from campground
                                                    where park_id = @parkId";
        public CampgroundSqlDal(string dbConnection)
        {
            connectionString = dbConnection;
        }

        public List<Campground> GetAllCampsByParkId(int parkId)
        {
            List<Campground> listOfCamps = new List<Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SelectAllCampsByParkId, conn);
                    cmd.Parameters.AddWithValue("@parkId", parkId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Campground camps = new Campground();

                        camps.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        camps.ParkId = Convert.ToInt32(reader["park_id"]);
                        camps.Name = Convert.ToString(reader["name"]);
                        camps.OpenFromMonth = Convert.ToInt32(reader["open_from_mm"]);
                        camps.OpenToMonth = Convert.ToInt32(reader["open_to_mm"]);
                        camps.DailyFee = Convert.ToDouble(reader["daily_fee"]);


                        listOfCamps.Add(camps);

                    }
                    return listOfCamps;
                }

                } catch (SqlException e)
                {
                    throw e;
                }

        }
    }
}
