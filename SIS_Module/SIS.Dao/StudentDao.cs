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
using SIS_Module.SIS.Main;
using System.Transactions;
using System.Data.Common;
using SIS_Module.SIS.Entity;

namespace SIS_Module.SIS.Dao
{
    public class StudentDAO
    {
        public static List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Ensure the connection is opened
                    string query = "SELECT StudentID, FirstName, LastName, DateOfBirth, Email, PhoneNumber FROM Students"; // Check this line
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Print column names for debugging
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine(reader.GetName(i)); // Print each column name
                        }

                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                StudentId = (int)reader["StudentID"], // Ensure this matches the printed names
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString(),
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                Email = reader["Email"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString()
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while retrieving students: " + ex.Message);
            }
            return students;
        }

        public static void AddStudent(Student student)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "INSERT INTO Students (FirstName, LastName, DateOfBirth, Email, PhoneNumber) VALUES (@FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber); SELECT SCOPE_IDENTITY();";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@FirstName", student.FirstName);
                            command.Parameters.AddWithValue("@LastName", student.LastName);
                            command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                            command.Parameters.AddWithValue("@Email", student.Email);
                            command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);

                            // Use ExecuteScalar to get the new Student ID
                            student.StudentId = Convert.ToInt32(command.ExecuteScalar());
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while adding a student: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }

        public static void UpdateStudent(Student student)
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open(); // Open the connection
                    using (SqlTransaction transaction = connection.BeginTransaction()) // Start the transaction
                    {
                        string query = "UPDATE Students SET FirstName = @FirstName, LastName = @LastName, DateOfBirth = @DateOfBirth, Email = @Email, PhoneNumber = @PhoneNumber WHERE StudentID = @StudentID";
                        using (SqlCommand command = new SqlCommand(query, connection, transaction)) // Associate the transaction with the command
                        {
                            command.Parameters.AddWithValue("@FirstName", student.FirstName);
                            command.Parameters.AddWithValue("@LastName", student.LastName);
                            command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                            command.Parameters.AddWithValue("@Email", student.Email);
                            command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                            command.Parameters.AddWithValue("@StudentID", student.StudentId);
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit(); // Commit the transaction
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while updating the student: " + ex.Message);
                // Rollback if an error occurs (though the transaction is scoped to the using block)
            }
        }
    }
}

