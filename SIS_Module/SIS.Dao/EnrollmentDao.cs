using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SIS_Module.SIS.Entity;
using SIS_Module.SIS.Util;
using SIS_Module.SIS.Dao;
using SIS_Module.SIS.Exception;
using SIS_Module.SIS.Entity;


namespace SIS_Module.SIS.Dao
{
    public class EnrollmentDAO
    {
        public static List<Enrollment> GetAllEnrollments()
        {
            List<Enrollment> enrollments = new List<Enrollment>();
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Ensure the connection is opened
                    string query = "SELECT * FROM Enrollments";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            enrollments.Add(new Enrollment
                            {
                                EnrollmentId = (int)reader["EnrollmentID"],
                                StudentId = (int)reader["StudentID"],
                                CourseId = (int)reader["CourseID"],
                                EnrollmentDate = (DateTime)reader["EnrollmentDate"]
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while retrieving enrollments: " + ex.Message);
            }
            return enrollments;
        }

        public static void AddEnrollment(int studentId, int courseId)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "INSERT INTO Enrollments (StudentID, CourseID, EnrollmentDate) VALUES (@StudentID, @CourseID, @EnrollmentDate)";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@StudentID", studentId);
                            command.Parameters.AddWithValue("@CourseID", courseId);
                            command.Parameters.AddWithValue("@EnrollmentDate", DateTime.Now);
                            command.ExecuteNonQuery(); // Execute the command
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while adding an enrollment: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }

        public static void UpdateEnrollment(Enrollment enrollment)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "UPDATE Enrollments SET StudentID = @StudentID, CourseID = @CourseID, EnrollmentDate = @EnrollmentDate WHERE EnrollmentID = @EnrollmentID";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@StudentID", enrollment.StudentId);
                            command.Parameters.AddWithValue("@CourseID", enrollment.CourseId);
                            command.Parameters.AddWithValue("@EnrollmentDate", enrollment.EnrollmentDate);
                            command.Parameters.AddWithValue("@EnrollmentID", enrollment.EnrollmentId);
                            command.ExecuteNonQuery(); // Execute the command
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while updating the enrollment: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }
    }
}


