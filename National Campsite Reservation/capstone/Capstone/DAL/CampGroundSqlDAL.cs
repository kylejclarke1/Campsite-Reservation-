using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;


namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private string connectionString;

        private const string SQL_GetAllCampgrounds = "SELECT * FROM campground WHERE park_id = @intInput;";
        
        public CampgroundSqlDAL(string dataConnection)
        {
            connectionString = dataConnection;
        }

        //Gets a list of all the Campgrounds from a specific parkId
        
        public List<Campground> GetCampgrounds(int intInput)
        {
            List<Campground> output = new List<Campground>();

            try
            {
                using (SqlConnection connections = new SqlConnection(connectionString))
                {
                    connections.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetAllCampgrounds, connections);
                    cmd.Parameters.AddWithValue("@intInput", intInput);


                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Campground c = new Campground();

                        c.CampgroundId = Convert.ToInt32(reader["campground_id"]);
                        c.ParkId = Convert.ToInt32(reader["park_id"]);
                        c.Name = Convert.ToString(reader["name"]);
                        c.OpenMonth = Convert.ToInt32(reader["open_from_mm"]);
                        c.CloseMonth = Convert.ToInt32(reader["open_to_mm"]);
                        c.DailyFee = Convert.ToDouble(reader["daily_fee"]);
                        output.Add(c);
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

