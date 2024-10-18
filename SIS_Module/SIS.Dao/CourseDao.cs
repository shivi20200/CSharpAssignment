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
    public class CourseDAO
    {
        public static List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Ensure the connection is opened
                    string query = "SELECT * FROM Courses";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course
                            {
                                CourseId = (int)reader["CourseID"],
                                CourseName = reader["CourseName"].ToString(),
                                CourseCode = reader["CourseCode"].ToString(),
                                InstructorName = reader["InstructorName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while retrieving courses: " + ex.Message);
            }
            return courses;
        }

        public static void AddCourse(Course course)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "INSERT INTO Courses (CourseName, CourseCode, InstructorName) VALUES (@CourseName, @CourseCode, @InstructorName); SELECT SCOPE_IDENTITY();";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@CourseName", course.CourseName);
                            command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                            command.Parameters.AddWithValue("@InstructorName", course.InstructorName);

                            // Use ExecuteScalar to get the new Course ID
                            course.CourseId = Convert.ToInt32(command.ExecuteScalar());
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while adding a course: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }

        public static void UpdateCourse(Course course)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "UPDATE Courses SET CourseName = @CourseName, CourseCode = @CourseCode, InstructorName = @InstructorName WHERE CourseID = @CourseID";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@CourseName", course.CourseName);
                            command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                            command.Parameters.AddWithValue("@InstructorName", course.InstructorName);
                            command.Parameters.AddWithValue("@CourseID", course.CourseId);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while updating the course: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }
    }
}
