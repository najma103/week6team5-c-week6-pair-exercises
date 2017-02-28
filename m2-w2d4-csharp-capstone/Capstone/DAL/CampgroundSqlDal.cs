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
        private const string SQL_SelectAllCampsByParkId = @"select * from campground where park_id = @parkId";
        private const string SQL_SelectCampByCampId = @"select * from campground where campground_id = @campId";
        public CampgroundSqlDal(string dbConnection)
        {
            connectionString = dbConnection;
        }

        public Dictionary<int,Campground> GetAllCampsByParkId(int parkId)
        {
            Dictionary<int,Campground> listOfCamps = new Dictionary<int,Campground>();
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


                        listOfCamps[camps.CampgroundId] = camps;

                    }
                    return listOfCamps;
                }

                } catch (SqlException e)
                {
                    throw e;
                }

        }

        public Dictionary<int,Campground> GetNameByCampId(int campId)
        {
            Dictionary<int,Campground> listOfCamps = new Dictionary <int,Campground>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SelectCampByCampId, conn);
                    cmd.Parameters.AddWithValue("@campId", campId);

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


                        listOfCamps[camps.CampgroundId] = camps;

                    }
                    return listOfCamps;
                }

            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}
