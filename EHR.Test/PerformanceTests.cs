using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EHR.Test
{
    [TestClass]
    public class PerformanceTests
    {
        private readonly string connectionString = "Server = . ; Database = EHR ; Trusted_Connection = true ; TrustServerCertificate = true;MultipleActiveResultSets= true";
        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void MeasureSelectQueryPerformance()
        {
            int patientId = 1;
            int iterations = 1000; // Number of queries to simulate load
            double totalTimeMs = 0;
            double averageTimeMs = 0;

            TestContext.WriteLine($"Starting performance test at {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            // Insert sample data to ensure there are records to query
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                for (int i = 0; i < 100; i++)
                {
                    var cmd = new SqlCommand("INSERT INTO Visits (PatientId, DoctorId, Date, ReasonForVisit, VisitType, Diagnosis, TreatmentPlan) VALUES (@PatientId, 1, '2025-05-15', 'Checkup', 'In-person', 'Flu', 'Rest')", conn);
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    cmd.ExecuteNonQuery();
                }
                TestContext.WriteLine("Sample data inserted successfully.");
            }

            // Measure performance of SELECT query
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var stopwatch = new Stopwatch();

                for (int i = 0; i < iterations; i++)
                {
                    stopwatch.Restart();
                    var cmd = new SqlCommand("SELECT * FROM Visits WHERE PatientId = @PatientId AND Diagnosis IS NOT NULL AND TreatmentPlan IS NOT NULL", conn);
                    cmd.Parameters.AddWithValue("@PatientId", patientId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read()) { /* Simulate reading data */ }
                    }
                    stopwatch.Stop();
                    totalTimeMs += stopwatch.ElapsedMilliseconds;
                }
            }

            averageTimeMs = totalTimeMs / iterations;
            TestContext.WriteLine($"Total queries executed: {iterations}");
            TestContext.WriteLine($"Total time taken: {totalTimeMs:F2} ms");
            TestContext.WriteLine($"Average time per query: {averageTimeMs:F2} ms");

            // Define a performance threshold (e.g., average query should take less than 10ms)
            double thresholdMs = 10.0;
            TestContext.WriteLine($"Performance threshold: {thresholdMs} ms per query");
            if (averageTimeMs > thresholdMs)
            {
                TestContext.WriteLine("Performance test failed: Average query time exceeded threshold.");
            }
            else
            {
                TestContext.WriteLine("Performance test passed: Average query time within threshold.");
            }

            Assert.IsTrue(averageTimeMs <= thresholdMs, $"Average query time ({averageTimeMs:F2} ms) exceeded threshold ({thresholdMs} ms)");
        }
    }
}
