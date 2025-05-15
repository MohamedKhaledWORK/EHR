using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHR.Test
{
    [TestClass]
    public class PatientDatabaseTests
    {
        private readonly string connectionString = "Server = . ; Database = EHR ; Trusted_Connection = true ; TrustServerCertificate = true;MultipleActiveResultSets= true";

            private TestContext testContextInstance;

            public TestContext TestContext
            {
                get { return testContextInstance; }
                set { testContextInstance = value; }
            }

            [TestMethod]
            public void VerifyViewMedicalHistory()
            {
                int patientId = 1;
                string desiredReason = "Checkup";
                string desiredVisitType = "In-person";
                bool foundVisit = false;

                TestContext.WriteLine($"Starting test at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

                // Insert sample visit without Diagnosis and TreatmentPlan
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("INSERT INTO Visits (PatientId, DoctorId, Date, ReasonForVisit, VisitType) VALUES (@PatientId, 1, '2025-05-15', @Reason, @VisitType)", conn);
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    cmd.Parameters.AddWithValue("@Reason", desiredReason);
                    cmd.Parameters.AddWithValue("@VisitType", desiredVisitType);
                    cmd.ExecuteNonQuery();
                    TestContext.WriteLine("Visit inserted successfully.");
                }

             

                // Retrieve and display visits with non-NULL Diagnosis and TreatmentPlan
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("SELECT * FROM Visits WHERE PatientId = @PatientId AND Diagnosis IS NOT NULL AND TreatmentPlan IS NOT NULL", conn);
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Build a string with all column names and values
                            string visitData = "Found visit: ";
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader[i];
                                visitData += $"{columnName} = {(value == DBNull.Value ? "NULL" : value.ToString())}, ";
                            }
                            TestContext.WriteLine(visitData.TrimEnd(',', ' '));
                            foundVisit = true; // Mark as found if we retrieved any matching visit
                        }
                    }
                }

                if (foundVisit)
                {
                    TestContext.WriteLine("Test completed successfully: Visits with Diagnosis and TreatmentPlan retrieved.");
                }
                else
                {
                    TestContext.WriteLine("No visits with Diagnosis and TreatmentPlan found.");
                }
                Assert.IsTrue(foundVisit, "No visits with Diagnosis and TreatmentPlan found for PatientId {patientId}");
            }
        }
    }
