using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    class ParkSqlDal
    {
        private string connectionString;
        private const string SQL_SelectAllParks = @"select * from park order by name;";
        public ParkSqlDal(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Dictionary<int,Park> GetAllParks()
        {
           
            Dictionary<int, Park> dictOfParks = new Dictionary<int, Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_SelectAllParks, conn);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.ParkId = Convert.ToInt32(reader["park_id"]);
                        park.Name = Convert.ToString(reader["name"]);
                        park.Location = Convert.ToString(reader["location"]);
                        park.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        park.Area = Convert.ToInt32(reader["area"]);
                        park.Visitors = Convert.ToInt32(reader["visitors"]);
                        park.Description = Convert.ToString(reader["description"]);

                        //key for dictionary is park id
                        dictOfParks[park.ParkId] = park;

                    }
                    return dictOfParks;
                }
            }
            catch (SqlException ex)
            {
                //Log and throw the exception
                throw ex;
            }
        }
    }
}
