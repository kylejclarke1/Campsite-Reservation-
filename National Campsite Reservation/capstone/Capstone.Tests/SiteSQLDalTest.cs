using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.DAL;
using Capstone.Models;
using System.Collections.Generic;

namespace Capstone.Tests
{
    [TestClass()]
    public class SiteTests
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

                cmd = new SqlCommand("INSERT INTO site (campground_id, site_number, max_occupancy, accessible, utilities, max_rv_length) VALUES ( 1, 1 ,6, 1, 1, 35);", connection);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup()]
        public void Cleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void ReturnTop5SitesTest()
        {
            SiteSqlDAL testSite = new SiteSqlDAL(connectionString);
            List<Site> siteList = testSite.GetTopFiveSites(1, new System.DateTime(2018,10,11), new System.DateTime(2018, 10, 16));

            Assert.AreEqual(5, siteList.Count);
        }

        [TestMethod()]
        public void ReturnNoSitesTest()
        {
            SiteSqlDAL testSite = new SiteSqlDAL(connectionString);
            List<Site> siteList = testSite.GetTopFiveSites(37, new System.DateTime(2018, 10, 15), new System.DateTime(2018, 10, 20));

            Assert.AreEqual(0, siteList.Count);
        }
    }
}
