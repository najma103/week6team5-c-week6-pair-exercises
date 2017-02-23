using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    class SiteSqlDal
    {
        string dbConnection;
        string SQL_Available_Sites = @"select * from site where campground_id = @campId and  site_id not IN
                                        (select site.site_id from site
                                        join campground camp on camp.campground_id = site.campground_id
                                        join reservation r on r.site_id = site.site_id
                                        where (r.to_date > @fromDate AND r.from_date < @toDate)
                                        );";
                                      
        public SiteSqlDal(string strConnection)
        {
            dbConnection = strConnection;
        }
        public List<Site> GetAllAvailableSitesForCamp(int campId, DateTime fromDate, DateTime toDate)
        {
            List<Site> listOfSites = new List<Site>();
            try
            {
                using (SqlConnection conn = new SqlConnection(dbConnection))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_Available_Sites, conn);
                    cmd.Parameters.AddWithValue("@campId", campId);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate);
                    cmd.Parameters.AddWithValue("@toDate", toDate);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = new Site();

                        site.SiteId = Convert.ToInt32(reader["site_id"]);
                        site.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        site.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        site.MaxOccupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.Accessible = Convert.ToBoolean(reader["accessible"]);
                        site.MaxRVLength = Convert.ToInt32(reader["max_rv_length"]);
                        site.Utilities = Convert.ToBoolean(reader["utilities"]);


                        listOfSites.Add(site);

                    }
                    return listOfSites;
                }

            }
            catch (SqlException e)
            {
                throw e;
            }

        }

    }
}
