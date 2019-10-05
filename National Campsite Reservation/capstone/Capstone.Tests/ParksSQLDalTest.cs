using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capstone;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using System.Collections.Generic;
using Capstone.Models;

namespace Capstone.Tests
{
    class ParksSQLDalTest
    {
        [TestClass()]
        public class ParkTests
        {
            private TransactionScope tran;
            private string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=NationalParkReservation;Integrated Security=True";

            [TestInitialize]
            public void Initialize()
            {
                tran = new TransactionScope();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd;
                    connection.Open();

                    cmd = new SqlCommand("INSERT INTO park VALUES ('test park', 'ohio', '1990-02-02', 35333, 250832, 'A beautiful park');", connection);
                    cmd.ExecuteNonQuery();
                }
            }

            [TestCleanup()]
            public void Cleanup()
            {
                tran.Dispose();
            }

            [TestMethod()]
            public void InsertIntoParksTest()
            {
                ParkSqlDAL testPark = new ParkSqlDAL(connectionString);
                List<Park> testList = testPark.ChoosePark(4);

                foreach (Park parks in testList)
                {
                    Assert.AreEqual("test park", parks.Name);
                    Assert.AreEqual("ohio", parks.Location);
                    Assert.AreEqual("A beautiful park", parks.Description);
                }

            }

            [TestMethod()]
            public void ReturnAllParksInData()
            {
                ParkSqlDAL testPark = new ParkSqlDAL(connectionString);
                List<Park> testList = testPark.AllParks();

                Assert.AreEqual(4, testList.Count);
            }

            [TestMethod()]
            public void ReturnChosenParkTest()
            {
                ParkSqlDAL testPark = new ParkSqlDAL(connectionString);
                List<Park> testList = testPark.ChoosePark(2);

                foreach (Park parks in testList)
                {
                    Assert.AreEqual("Arches", parks.Name);
                    Assert.AreEqual(76518, parks.Area);
                    Assert.AreEqual(1284767, parks.Visitors);
                }
            }
        }
    }
}
