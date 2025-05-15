using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace EHR.Test
{


    [TestClass]
    public class AdminDatabaseTests
    {
        private readonly string connectionString = "Server = . ; Database = EHR ; Trusted_Connection = true ; TrustServerCertificate = true;MultipleActiveResultSets= true";
        private TestContext testContextInstance;

        // Property to access TestContext
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void VerifyAddDoctorAvailability()
        {
            int doctorId = 2; // DoctorId to test
            string desiredTimeSlot = "10:00-13:00"; // Desired TimeSlot to find
            bool foundDesiredTimeSlot = false;

            TestContext.WriteLine($"Starting test at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            // Insert sample data
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO DoctorAvailabilities (DoctorId, DayOfWeek, TimeSlot) VALUES (@DoctorId, 'Tuesday', '10:00-13:00')", conn);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                cmd.ExecuteNonQuery();
                TestContext.WriteLine("Insert operation completed successfully.");
            }

            // Read all records for the DoctorId and check for desired TimeSlot
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT DayOfWeek, TimeSlot FROM DoctorAvailabilities WHERE DoctorId = @DoctorId", conn);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string timeSlot = reader["TimeSlot"].ToString();
                        TestContext.WriteLine($"Found record: DayOfWeek = {reader["DayOfWeek"]}, TimeSlot = {timeSlot}");
                        if (timeSlot == desiredTimeSlot)
                        {
                            foundDesiredTimeSlot = true;
                            TestContext.WriteLine($"Desired TimeSlot '{desiredTimeSlot}' found for DoctorId {doctorId}!");
                        }
                    }
                }
            }

            // Assert and final output
            if (foundDesiredTimeSlot)
            {
                TestContext.WriteLine("Test completed successfully: Operation done and TimeSlot found.");
            }
            Assert.IsTrue(foundDesiredTimeSlot, $"Desired TimeSlot '{desiredTimeSlot}' not found for DoctorId {doctorId}");
        }
    }
}