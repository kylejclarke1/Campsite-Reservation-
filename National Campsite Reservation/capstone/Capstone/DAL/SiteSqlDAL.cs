using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class SiteSqlDAL
    {
        private string connectionString;
        private const string SQL_TopFiveCampSites = "Select TOP 5 * FROM site s JOIN campground ON s.campground_id = campground.campground_id WHERE s.campground_id = @campground_id AND s.site_id NOT IN (SELECT site_id from reservation WHERE @arrivalDate < to_date AND @departureDate > from_date)";
        

        public SiteSqlDAL(string dataConnection)
        {
            connectionString = dataConnection;
        }

        //Gets a list of the top five campsites at a specific campground. 
        public List<Site> GetTopFiveSites(int campgroundId, DateTime arrivalDate, DateTime departureDate)
        {
            List<Site> output = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_TopFiveCampSites, conn);
                    cmd.Parameters.AddWithValue("@campground_id", campgroundId);
                    cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate);
                    cmd.Parameters.AddWithValue("@departureDate", departureDate);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Site s = new Site();
                        s.Id = Convert.ToInt32(reader["site_id"]);
                        s.SiteNumber = Convert.ToInt32(reader["site_number"]);
                        s.MaxOcc = Convert.ToInt32(reader["max_occupancy"]);
                        s.Accessible = Convert.ToBoolean(reader["accessible"]);
                        s.Utilities = Convert.ToBoolean(reader["utilities"]);
                        s.RVLength = Convert.ToInt32(reader["max_rv_length"]);
                        s.Cost = Convert.ToDouble(reader["daily_fee"]);
                        s.totalCost = s.Cost * (departureDate - arrivalDate).TotalDays;

                        output.Add(s);

                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("There was an error");
                throw;
            }
            return output;
        }
    }
}
