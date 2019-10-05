using System;
using System.Collections.Generic;
using System.Text;
using Capstone.DAL;
using Capstone.Models;
using System.Collections;
using System.Threading.Tasks;
using System.Globalization;
using Capstone.DAL.Capstone.DAL;

namespace Capstone
{
    class NationParkReservationUI
    {
        string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True";

        public void ReservationSQLDal(string dataConnection)
        {
            connectionString = dataConnection;
        }

        public void Run()
        {
            bool isDone = false;

            do
            {
                Console.WriteLine();
                Console.WriteLine("Select a Park for Further Details");

                ParkSqlDAL dal = new ParkSqlDAL(connectionString);
                List<Park> park = dal.AllParks();

                int i = 1;
                foreach (Park parks in park)
                {
                    Console.WriteLine(i.ToString() + ")".PadRight(3) + parks.Name);
                    i++;
                }

                DisplayParksInterface();
                string input = Console.ReadLine();

                if (int.Parse(input) == 9)
                {
                    Console.WriteLine("Please come back soon!");
                    isDone = true;
                }
                else if (int.Parse(input) > 0 && int.Parse(input) < i)
                {
                    ParksInformation(input);
                }
                else
                {
                    Console.WriteLine("Make a valid selection");
                }
            }
            while (!isDone);
        }

        //Displays the parks to pick from

        void DisplayParksInterface()
        {
            Console.WriteLine("9)".PadRight(4) + "Quit");
            Console.WriteLine("");
        }

        //Displays Information about the park selected

        void ParksInformation(string input)
        {
            bool isDone = false;
            int intInput = Int32.Parse(input);
            do
            {
                ParkChoice(input);
                DisplayParksInformation();
                Console.WriteLine("");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ParkCampGrounds(intInput);
                        break;
                    case "2":
                        CampGroundReservation(intInput);
                        break;
                    case "3":
                        isDone = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid selection.");
                        break;
                }

            } while (!isDone);

            return;

        }

        void DisplayParksInformation()
        {
            Console.WriteLine();
            Console.WriteLine("1) View Campgrounds");
            Console.WriteLine("2) Search for Reservation");
            Console.WriteLine("3) Return to Previous Screen");
            Console.WriteLine("");
        }
        void ParkCampGrounds(int intInput)
        {
            bool isDone = false;

            do
            {
                DisplayCampgrounds(intInput);
                DisplayParkCampGrounds();
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        CampGroundReservation(intInput);
                        break;
                    case "2":
                        isDone = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid selection.");
                        Console.WriteLine();
                        break;
                }

            } while (!isDone);

            return;

        }

        void DisplayParkCampGrounds()
        {
            Console.WriteLine();
            Console.WriteLine("1) Search for Available Reservation");
            Console.WriteLine("2) Return to Previous Screen");

            Console.WriteLine("");
        }

        public bool CampGroundReservation(int intInput)
        {
            DisplayCampgrounds(intInput);
            CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
            List<Campground> camps = dal.GetCampgrounds(intInput);

            DateTime arrivalResult = DateTime.MinValue;
            DateTime departureResult = DateTime.MinValue;
            DateTime TodayDate = DateTime.Today;

            Console.WriteLine();
            int campgroundID = CLIHelper.GetInteger("Which campground? (enter 0 to cancel)");
            if (campgroundID == 0)
            {
                Console.WriteLine("Hope to have you stay next time!");
                return false;
            }
            else if (campgroundID < 0 && campgroundID > camps.Count)
            {
                Console.WriteLine("Please select valid campground");
                Console.WriteLine();
                return true;
            }
            string arrivalDate = CLIHelper.GetString("What is the arrival date? (mm/dd/yyyy)");
            if (!DateTime.TryParse(arrivalDate, out arrivalResult) || arrivalResult < TodayDate)
            {
                Console.WriteLine("Please provide a valid date of arrival for the reservation.");
                Console.ReadLine();
                return true;
            }
            string departureDate = CLIHelper.GetString("What is the departure date? (mm/dd/yyyy)");
            if (!DateTime.TryParse(departureDate, out departureResult) || departureResult <= arrivalResult) 
            {
                Console.WriteLine("Please provide a valid date of depature for the reservation.");
                Console.ReadLine();
                return true;
            }
            else if (departureResult < arrivalResult)
            {
                Console.WriteLine();
                Console.WriteLine("Departure date cannot be before the arrival date. Pleases try again with a valid date of departure.");
                return true;
            }

            AvailableSitesToReserve(campgroundID, arrivalDate, departureDate);
            return true;
        }

        //Checks with current reservations to see if site is available 

        public bool AvailableSitesToReserve(int campgroundID, string arrivalDate, string departureDate)
        {
            int reservationId = 0;

            DateTime arrivalResult = DateTime.MinValue;
            DateTime departureResult = DateTime.MinValue;

            DateTime.TryParse(arrivalDate, out arrivalResult);
            DateTime.TryParse(departureDate, out departureResult);

            SiteSqlDAL dal = new SiteSqlDAL(connectionString);
            List<Site> site = dal.GetTopFiveSites(campgroundID, arrivalResult, departureResult);

            if (site.Count == 0)
            {
                Console.WriteLine();
                Console.WriteLine("No availiable sites to reserve");
                return true;
            }

            Console.WriteLine();
            Console.WriteLine("Results Matching Your Search Criteria");
            Console.WriteLine("Site No.   Max Occup.   Accessible?   Max RV Length   Utility   Cost");

            foreach (Site sites in site)
            {
                string result;
                result = sites.Id.ToString().PadRight(11);
                result += sites.MaxOcc.ToString().PadRight(13);

                if (sites.Accessible == false)
                {
                    result += "No".PadRight(14);
                }
                else
                {
                    result += "Yes".PadRight(14);
                }

                if (sites.RVLength > 0)
                {
                    result += sites.RVLength.ToString().PadRight(16);
                }
                else
                {
                    result += "N/A".PadRight(16);
                }

                if (sites.Utilities == false)
                {
                    result += "No".PadRight(10);
                }
                else
                {
                    result += "Yes".PadRight(10);
                }
                result += "$" + sites.totalCost;
                Console.WriteLine(result);
            }

            int siteNumber = CLIHelper.GetInteger("Which site should be reserved? (enter 0 to cancel)");
            if (siteNumber == 0)
            {
                Console.WriteLine("Hope to have you stay next time!");
                return false;
            }
            else if (siteNumber < 0)
            {
                Console.WriteLine("Please select valid site number");
                Console.WriteLine();
                return true;
            }

            string nameToBook = CLIHelper.GetString("What name should the reservation be made under?  ");

            ReservationSQLDal newDAL = new ReservationSQLDal(connectionString);
            reservationId = newDAL.MakeReservation(siteNumber, nameToBook, arrivalDate, departureDate);

            Console.WriteLine($"{nameToBook} The reservation has been made and the confirmation id is {reservationId}");
            Console.WriteLine();
            Console.WriteLine("Would you like to make another reservation? If so press enter to return to park menu");
            Console.ReadLine();
         
            return true;

        }





        public void ParkChoice(string input)
        {
            int intInput = Int32.Parse(input);
            ParkSqlDAL dal = new ParkSqlDAL(connectionString);
            List<Park> park = dal.ChoosePark(intInput);
            Console.WriteLine();
            Console.WriteLine("Park Information Screen");

            foreach (Park parks in park)
            {
                Console.WriteLine("Name:".PadRight(20) + parks.Name + " National Park");
                Console.WriteLine("Location:".PadRight(20) + parks.Location.ToString());
                Console.WriteLine("Established:".PadRight(20) + parks.EstablishDate.ToShortDateString());
                Console.WriteLine("Area:".PadRight(20) + string.Format("{0:n0}", parks.Area) + " sq km");
                Console.WriteLine("Vistors:".PadRight(20) + string.Format("{0:n0}", parks.Visitors));
                Console.WriteLine("");
                Console.WriteLine(parks.Description);
            }
        }

        void DisplayCampgrounds(int intInput)
        {
            CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
            List<Campground> camps = dal.GetCampgrounds(intInput);

            DateTimeFormatInfo dateTimeInfo = new DateTimeFormatInfo();
            Console.WriteLine();
            Console.WriteLine("National Park Campgrounds");
            Console.WriteLine();
            Console.WriteLine("    Name                               Open  Close  Daily Fee");

            foreach (Campground camp in camps)
            {
                Console.WriteLine("#" + camp.CampgroundId.ToString().PadRight(3) + camp.Name.ToString().PadRight(35) + dateTimeInfo.GetAbbreviatedMonthName(camp.OpenMonth).PadRight(6) + dateTimeInfo.GetAbbreviatedMonthName(camp.CloseMonth).PadRight(7) + string.Format("{0:C}", camp.DailyFee));
            }
        }

        public void DisplayNameOfParks()
        {
            ParkSqlDAL dal = new ParkSqlDAL(connectionString);
            List<Park> park = dal.AllParks();

            int i = 1;
            foreach (Park parks in park)
            {
                Console.WriteLine(i.ToString() + ")".PadRight(3) + parks.Name);
                i++;
            }
        }
    }
}