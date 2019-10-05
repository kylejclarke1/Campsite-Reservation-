using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.Tests
{
    class TestCampGroundSQLDAL
    {
        [TestClass]
        public class CampgroundTests
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

                    cmd = new SqlCommand("INSERT INTO campground VALUES (2,'test campground', 2, 12, 27.00);", connection);
                    cmd.ExecuteNonQuery();
                }
            }

            [TestCleanup()]
            public void Cleanup()
            {
                tran.Dispose();
            }

            [TestMethod()]
            public void InsertNewCampgroundsIntoCampgroundsTest()
            {
                CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
                List<Campground> testList = dal.GetCampgrounds(8);

                foreach (Campground camps in testList)
                {
                    Assert.AreEqual(2, camps.ParkId);
                    Assert.AreEqual("test campground", camps.Name);
                    Assert.AreEqual("Feb", camps.OpenMonth);
                    Assert.AreEqual("Dec", camps.CloseMonth);
                    Assert.AreEqual(27.00, camps.DailyFee);
                }
            }

            [TestMethod()]
            public void ReturnSelectedParkCampgroundsTest()
            {
                CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
                List<Campground> testList = dal.GetCampgrounds(4);

                foreach (Campground camps in testList)
                {
                    Assert.AreEqual(2, camps.ParkId);
                    Assert.AreEqual("Devil's Garden", camps.Name);
                    Assert.AreEqual("Jan", camps.OpenMonth);
                    Assert.AreEqual("Dec", camps.CloseMonth);
                    Assert.AreEqual(25.00, camps.DailyFee);
                }
            }
        }
    }
}
