using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using Capstone.Models;

namespace Capstone.DAL
{
    public class ParkSqlDAL
    {
        private string connectionString;
        private const string AllParksSQL = "SELECT * FROM park";
        private const string ParkChosenSQL = "SELECT * FROM park WHERE park_id = @intInput";

        public ParkSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }
        //Gets a park based on user input
        public List<Park> ChoosePark(int intInput)
        {
            List<Park> choosePark = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(ParkChosenSQL, conn);
                    cmd.Parameters.AddWithValue("@intInput", intInput);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Park p = new Park();
                        p.Id = Convert.ToInt32(reader["park_id"]);
                        p.Name = Convert.ToString(reader["name"]);
                        p.Location = Convert.ToString(reader["location"]);
                        p.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        p.Area = Convert.ToInt32(reader["area"]);
                        p.Visitors = Convert.ToInt32(reader["visitors"]);
                        p.Description = Convert.ToString(reader["description"]);

                        choosePark.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return choosePark;
        }
        // Gets a list of all the parks available. 
        public List<Park> AllParks()
        {
            List<Park> AllParkNames = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(AllParksSQL, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park p = new Park();
                        p.Id = Convert.ToInt32(reader["park_id"]);
                        p.Name = Convert.ToString(reader["name"]);
                        p.Location = Convert.ToString(reader["location"]);
                        p.EstablishDate = Convert.ToDateTime(reader["establish_date"]);
                        p.Area = Convert.ToInt32(reader["area"]);
                        p.Visitors = Convert.ToInt32(reader["visitors"]);
                        p.Description = Convert.ToString(reader["description"]);

                        AllParkNames.Add(p);
                    }

                }
            }
            catch (Exception)
            {

            }
            return AllParkNames;
        }
    }
}