using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;
using Capstone.DAL;
using System.Collections.Generic;
using Capstone.Models;
using Capstone.DAL.Capstone.DAL;

namespace Capstone.Tests
{
    class ReservationSQLDalTest
    {
        [TestClass()]
        public class ReservationTest
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

                    cmd = new SqlCommand("INSERT INTO reservation VALUES (1, 'Kyle', '2017-01-01', '2017-01-10', '2017-06-28');", connection);
                    cmd.ExecuteNonQuery();
                }
            }

            [TestCleanup()]
            public void Cleanup()
            {
                tran.Dispose();
            }

            [TestMethod()]
            public void FindReservationsTest()
            {
                ReservationSQLDal testRes = new ReservationSQLDal(connectionString);
                List<Reservation> RevTest = testRes.ReservationsDuringDatesSelected(1);

                foreach (Reservation res in RevTest)
                {
                    Assert.AreEqual(1, res.Id);
                    Assert.AreEqual("Smith Family Reservation", res.Name);
                    Assert.AreEqual(new System.DateTime(2018, 10, 23), res.FromDate);
                }
            }

            [TestMethod()]
            public void MakeReservationTest()
            {
                ReservationSQLDal newTest = new ReservationSQLDal(connectionString);

            }
        }
    }
}
