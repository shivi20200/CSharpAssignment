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
    public class TeacherDAO
    {
        public static List<Teacher> GetAllTeachers()
        {
            List<Teacher> teachers = new List<Teacher>();
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Ensure the connection is opened
                    string query = "SELECT * FROM Teachers";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teachers.Add(new Teacher
                            {
                                TeacherId = (int)reader["TeacherID"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                Email = reader["Email"].ToString()
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while retrieving teachers: " + ex.Message);
            }
            return teachers;
        }

        public static void AddTeacher(Teacher teacher)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "INSERT INTO Teachers (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                            command.Parameters.AddWithValue("@LastName", teacher.LastName);
                            command.Parameters.AddWithValue("@Email", teacher.Email);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while adding a teacher: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }

        public static void UpdateTeacher(Teacher teacher)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "UPDATE Teachers SET FirstName = @FirstName, LastName = @LastName, Email = @Email WHERE TeacherID = @TeacherID";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                            command.Parameters.AddWithValue("@LastName", teacher.LastName);
                            command.Parameters.AddWithValue("@Email", teacher.Email);
                            command.Parameters.AddWithValue("@TeacherID", teacher.TeacherId);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while updating the teacher: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }
    }
}


