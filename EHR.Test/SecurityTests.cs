using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHR.Test
{
    [TestClass]
    public class SecurityTests
    {
        private readonly string connectionString = "Server = . ; Database = EHR ; Trusted_Connection = true ; TrustServerCertificate = true;MultipleActiveResultSets= true";
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void VerifyLoginSecurity()
        {
            string correctUsername = "doctor1";
            string correctPassword = "Pass123!";
            string incorrectPassword = "WrongPass";
            bool loginSuccess = false;
            bool loginFailed = false;

            TestContext.WriteLine($"Starting test at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            // Insert a test user
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO Staff ( Username, Password, Email, Name, Role) VALUES ( @Username, @Password, 'doctor1@ehr.com', 'Doctor One', 'Doctor')", conn);
                cmd.Parameters.AddWithValue("@Username", correctUsername);
                cmd.Parameters.AddWithValue("@Password", correctPassword);
                cmd.ExecuteNonQuery();
                TestContext.WriteLine("Test user inserted successfully.");
            }

            // Test 1: Correct username and password (should succeed)
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Id FROM Staff WHERE Username = @Username AND Password = @Password", conn);
                cmd.Parameters.AddWithValue("@Username", correctUsername);
                cmd.Parameters.AddWithValue("@Password", correctPassword);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        loginSuccess = true;
                        TestContext.WriteLine($"Login successful with Username: {correctUsername}, Password: {correctPassword}");
                    }
                    else
                    {
                        TestContext.WriteLine($"Login failed with Username: {correctUsername}, Password: {correctPassword}");
                    }
                }
            }

            // Test 2: Correct username, incorrect password (should fail)
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Id FROM Staff WHERE Username = @Username AND Password = @Password", conn);
                cmd.Parameters.AddWithValue("@Username", correctUsername);
                cmd.Parameters.AddWithValue("@Password", incorrectPassword);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        loginFailed = true;
                        TestContext.WriteLine($"Login correctly rejected with Username: {correctUsername}, Password: {incorrectPassword}");
                    }
                    else
                    {
                        TestContext.WriteLine($"Login incorrectly succeeded with Username: {correctUsername}, Password: {incorrectPassword}");
                    }
                }
            }

            if (loginSuccess && loginFailed)
            {
                TestContext.WriteLine("Test completed successfully: Login security validated.");
            }
            Assert.IsTrue(loginSuccess, "Login failed with correct credentials.");
            Assert.IsTrue(loginFailed, "Login succeeded with incorrect credentials, security vulnerability detected!");
        }
    }
}
