using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System;
using Microsoft.Data.SqlClient;
namespace EHR.Test
{
    

    [TestClass]
    public class DoctorDatabaseTests
    {
        private readonly string connectionString = "Server = . ; Database = EHR ; Trusted_Connection = true ; TrustServerCertificate = true;MultipleActiveResultSets= true";
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void VerifyUpdatePersonalData()
        {
            int doctorId = 1;
            string oldEmail = "", oldName = "", newEmail = "janenew@ehr.com", newName = "Dr Jane Smith";
            bool updatedSuccessfully = false;

            TestContext.WriteLine($"Starting test at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            // Retrieve old Doctor personal info
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT s.Email, s.Name FROM Doctors d JOIN Staff s ON d.StaffId = s.Id WHERE d.Id = @DoctorId", conn);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        oldEmail = reader["Email"].ToString();
                        oldName = reader["Name"].ToString();
                        TestContext.WriteLine($"Old Doctor info: Email = {oldEmail}, Name = {oldName}");
                    }
                    else
                    {
                        TestContext.WriteLine("No Doctor found with the specified DoctorId.");
                        Assert.Fail("Doctor not found in database.");
                    }
                }
            }

            // Update Doctor personal info
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Staff SET Email = @NewEmail, Name = @NewName FROM Doctors d WHERE d.StaffId = Staff.Id AND d.Id = @DoctorId", conn);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                cmd.Parameters.AddWithValue("@NewEmail", newEmail);
                cmd.Parameters.AddWithValue("@NewName", newName);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    TestContext.WriteLine("Doctor personal info updated successfully.");
                    updatedSuccessfully = true;
                }
                else
                {
                    TestContext.WriteLine("Failed to update Doctor personal info.");
                }
            }

            // Retrieve new Doctor personal info
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT s.Email, s.Name FROM Doctors d JOIN Staff s ON d.StaffId = s.Id WHERE d.Id = @DoctorId", conn);
                cmd.Parameters.AddWithValue("@DoctorId", doctorId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string updatedEmail = reader["Email"].ToString();
                        string updatedName = reader["Name"].ToString();
                        TestContext.WriteLine($"New Doctor info: Email = {updatedEmail}, Name = {updatedName}");
                    }
                }
            }

            if (updatedSuccessfully)
            {
                TestContext.WriteLine("Test completed successfully: Doctor personal info updated.");
            }
            Assert.IsTrue(updatedSuccessfully, "Failed to update Doctor personal info.");
        }
    }
}
    

