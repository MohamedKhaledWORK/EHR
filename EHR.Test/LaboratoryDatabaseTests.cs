using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHR.Test
{
    [TestClass]
    public class LaboratoryDatabaseTests
    {
        private readonly string connectionString = "Server = . ; Database = EHR ; Trusted_Connection = true ; TrustServerCertificate = true;MultipleActiveResultSets= true";
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void VerifyUpdateLabTestResult()
        {
            int patientId = 1;
            string desiredResult = "Normal";
            bool foundResult = false;

            TestContext.WriteLine($"Starting test at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            // Insert a sample lab test
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("INSERT INTO LabTests (PatientId, LabTestType, TestDate, NormalRange ,TestStatus) VALUES (@PatientId, 'Blood Glucose (Fasting)','2025-05-15','70-100 mg/dL', 'Pending')", conn);
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                cmd.ExecuteNonQuery();
                TestContext.WriteLine("Lab test inserted successfully.");
            }

            // Update the test result
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE LabTests SET TestResult = @TestResult WHERE PatientId = @PatientId AND LabTestType = 'Blood Test'", conn);
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                cmd.Parameters.AddWithValue("@TestResult", desiredResult);
                cmd.ExecuteNonQuery();
                TestContext.WriteLine("Lab test result updated successfully.");
            }

            // Verify the updated result
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT TestResult FROM LabTests WHERE PatientId = @PatientId AND LabTestType = 'Blood Test'", conn);
                cmd.Parameters.AddWithValue("@PatientId", patientId);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string result = reader["TestResult"].ToString();
                        TestContext.WriteLine($"Found lab test: TestResult = {result}");
                        if (result == desiredResult)
                        {
                            foundResult = true;
                            TestContext.WriteLine($"Desired TestResult '{desiredResult}' found for PatientId {patientId}!");
                        }
                    }
                }
            }

            if (foundResult)
            {
                TestContext.WriteLine("Test completed successfully: Lab test result updated.");
            }
            Assert.IsTrue(foundResult, $"Desired TestResult '{desiredResult}' not found for PatientId {patientId}");
        }
    }
}
