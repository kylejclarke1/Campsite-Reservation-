using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using System.Data.SqlClient;

namespace Capstone.DAL
{
    namespace Capstone.DAL
    {
        public class ReservationSQLDal
        {
            private string connectionString;
            private const string SQL_ReservationsDuringDatesSelected = "SELECT * from reservation WHERE @arrivalDate < to_date and @departureDate > from_date ORDER BY site_id";
            private const string SQL_MakeReservation = "INSERT INTO reservation ([site_id],[name],[from_date],[to_date],[create_date]) VALUES (@siteNumber, @nameToBook, @arrivalDate, @departureDate, @dateCreated)";
            
            public ReservationSQLDal(string dataConnection)
            {
                connectionString = dataConnection;
            }
            //Lets the user make a reservation and creates a reservationId 
            public int MakeReservation(int siteNumber, string nameToBook, string arrivalDate, string departureDate)
            {

                int reservationId = 0;
                DateTime theDate = DateTime.Today;
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(SQL_MakeReservation, conn);

                        cmd.Parameters.AddWithValue("@siteNumber", siteNumber);
                        cmd.Parameters.AddWithValue("@nameToBook", nameToBook);
                        cmd.Parameters.AddWithValue("@arrivalDate", arrivalDate);
                        cmd.Parameters.AddWithValue("@departureDate", departureDate);
                        cmd.Parameters.AddWithValue("@dateCreated", theDate);


                        cmd.ExecuteNonQuery();

                        cmd = new SqlCommand("SELECT MAX(reservation_id) FROM reservation", conn);
                        reservationId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine("An error occurred. " + e.Message);
                    throw;
                }

                return reservationId;
            }



            //Gets a list of reservations during the selected date period. 
            public List<Reservation> ReservationsDuringDatesSelected(int intInput)
            {
                List<Reservation> ReservationsDuringDatesSelected = new List<Reservation>();

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(SQL_ReservationsDuringDatesSelected, conn);
                        cmd.Parameters.AddWithValue("@intInput", intInput);


                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Reservation R = new Reservation();
                            R.ReservationId = Convert.ToInt32(reader["reservation_id"]);
                            R.SiteId = Convert.ToInt32(reader["site_id"]);
                            R.Name = Convert.ToString(reader["name"]);
                            R.FromDate = Convert.ToDateTime(reader["from_date"]);
                            R.ToDate = Convert.ToDateTime(reader["to_date"]);
                            R.CreateDate = Convert.ToDateTime(reader["create_date"]);

                            ReservationsDuringDatesSelected.Add(R);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return ReservationsDuringDatesSelected;
            }


        }
    }
}

