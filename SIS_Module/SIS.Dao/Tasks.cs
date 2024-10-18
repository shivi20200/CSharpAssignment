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
    public static class EnrollmentSystem
    {
        public static void EnrollStudent(Student student, List<string> courses)
        {
            using (var connection = DBConnectionUtil.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Create new student
                    var studentId = CreateStudent(connection, transaction, student);

                    // Enroll student in courses
                    foreach (var courseName in courses)
                    {
                        var courseId = GetCourseId(connection, transaction, courseName);
                        if (courseId > 0)
                        {
                            EnrollStudentInCourse(connection, transaction, studentId, courseId);
                        }
                    }

                    transaction.Commit();
                    Console.WriteLine($"{student.FirstName} {student.LastName} has been successfully enrolled.");
                }
                catch (ApplicationException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"An error occurred while enrolling student: {ex.Message}");
                }
            }
        }

        public static int CreateStudent(SqlConnection connection, SqlTransaction transaction, Student student)
        {
            string query = "INSERT INTO Students (FirstName, LastName, DateOfBirth, Email, PhoneNumber) OUTPUT INSERTED.StudentID VALUES (@FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber)";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);

                return (int)command.ExecuteScalar();
            }
        }

        public static int GetCourseId(SqlConnection connection, SqlTransaction transaction, string courseName)
        {
            string query = "SELECT CourseID FROM Courses WHERE CourseName = @CourseName";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@CourseName", courseName);
                object result = command.ExecuteScalar();
                return result != null ? (int)result : -1;
            }
        }

        public static void EnrollStudentInCourse(SqlConnection connection, SqlTransaction transaction, int studentId, int courseId)
        {
            string query = "INSERT INTO Enrollments (StudentID, CourseID, EnrollmentDate) VALUES (@StudentID, @CourseID, @EnrollmentDate)";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@StudentID", studentId);
                command.Parameters.AddWithValue("@CourseID", courseId);
                command.Parameters.AddWithValue("@EnrollmentDate", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }

        public static void AssignTeacherToCourse(Teacher teacher, string courseCode)
        {
            using (var connection = DBConnectionUtil.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Create teacher record
                    CreateTeacher(connection, transaction, teacher);

                    // Assign teacher to course
                    string updateQuery = "UPDATE Courses SET InstructorName = @InstructorName WHERE CourseCode = @CourseCode";
                    using (SqlCommand command = new SqlCommand(updateQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@InstructorName", teacher.FirstName + " " + teacher.LastName);
                        command.Parameters.AddWithValue("@CourseCode", courseCode);
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    Console.WriteLine($"{teacher.FirstName} {teacher.LastName} has been assigned to course {courseCode}.");
                }
                catch (ApplicationException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"An error occurred while assigning teacher: {ex.Message}");
                }
            }
        }

        public static void CreateTeacher(SqlConnection connection, SqlTransaction transaction, Teacher teacher)
        {
            string query = "INSERT INTO Teachers (FirstName, LastName, Email) VALUES (@FirstName, @LastName, @Email)";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@FirstName", teacher.FirstName);
                command.Parameters.AddWithValue("@LastName", teacher.LastName);
                command.Parameters.AddWithValue("@Email", teacher.Email);
                command.ExecuteNonQuery();
            }
        }

        public static void RecordPayment(Payment payment)
        {
            using (var connection = DBConnectionUtil.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Record the payment
                    RecordPaymentInDatabase(connection, transaction, payment);

                    // Assuming there is a balance column, you can update it here
                    Console.WriteLine($"Payment of ${payment.Amount} recorded for student ID: {payment.StudentId}.");

                    transaction.Commit();
                }
                catch (ApplicationException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"An error occurred while recording payment: {ex.Message}");
                }
            }
        }

        public static void RecordPaymentWithStudent(int studentId, Payment payment)
        {
            using (var connection = DBConnectionUtil.GetConnection())
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // Ensure student exists
                    // You can also include student details in case the student needs to be created
                    string firstName = "John"; // You may want to pass or retrieve this info
                    string lastName = "Doe";    // You may want to pass or retrieve this info
                    DateTime dateOfBirth = new DateTime(2000, 1, 1); // Example data
                    string email = "johndoe@example.com"; // Example data
                    string phoneNumber = "123-456-7890"; // Example data

                    InsertStudentIdIfNotExists(studentId, firstName, lastName, dateOfBirth, email, phoneNumber, connection, transaction);

                    // Record the payment
                    RecordPaymentInDatabase(connection, transaction, payment);

                    transaction.Commit();
                    Console.WriteLine($"Payment of ${payment.Amount} recorded for student ID: {studentId}.");
                }
                catch (ApplicationException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static void InsertStudentIdIfNotExists(int studentId, string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber, SqlConnection connection, SqlTransaction transaction)
        {
            // Check if the StudentID already exists
            if (DoesStudentExist(studentId, connection, transaction))
            {
                Console.WriteLine($"Student with ID {studentId} already exists.");
                return; // Exit the method if the student already exists
            }

            // Enable IDENTITY_INSERT
            string enableIdentityInsert = "SET IDENTITY_INSERT Students ON;";
            using (var enableCommand = new SqlCommand(enableIdentityInsert, connection, transaction))
            {
                enableCommand.ExecuteNonQuery();
            }

            try
            {
                string query = "INSERT INTO Students (StudentID, FirstName, LastName, DateOfBirth, Email, PhoneNumber) " +
                               "VALUES (@StudentID, @FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber)";

                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

                    command.ExecuteNonQuery();
                    Console.WriteLine($"Student with ID {studentId} has been inserted successfully.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            finally
            {
                // Disable IDENTITY_INSERT
                string disableIdentityInsert = "SET IDENTITY_INSERT Students OFF;";
                using (var disableCommand = new SqlCommand(disableIdentityInsert, connection, transaction))
                {
                    disableCommand.ExecuteNonQuery();
                }
            }
        }

        public static bool DoesStudentExist(int studentId, SqlConnection connection, SqlTransaction transaction)
        {
            string query = "SELECT COUNT(1) FROM Students WHERE StudentID = @StudentID";
            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@StudentID", studentId);
                return (int)command.ExecuteScalar() > 0; // Returns true if student exists
            }
        }


        public static void RecordPaymentInDatabase(SqlConnection connection, SqlTransaction transaction, Payment payment)
        {
            string query = "INSERT INTO Payments (StudentID, Amount, PaymentDate) VALUES (@StudentID, @Amount, @PaymentDate)";

            using (SqlCommand command = new SqlCommand(query, connection, transaction))
            {
                command.Parameters.AddWithValue("@StudentID", payment.StudentId);
                command.Parameters.AddWithValue("@Amount", payment.Amount);
                command.Parameters.AddWithValue("@PaymentDate", payment.PaymentDate);
                command.ExecuteNonQuery();
            }
        }

        public static void GenerateEnrollmentReport(string courseName)
        {
            using (var connection = DBConnectionUtil.GetConnection())
            {
                connection.Open();

                string query = @"
                SELECT s.FirstName, s.LastName, e.EnrollmentDate
                FROM Enrollments e
                JOIN Students s ON e.StudentID = s.StudentID
                JOIN Courses c ON e.CourseID = c.CourseID
                WHERE c.CourseName = @CourseName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CourseName", courseName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.WriteLine($"Enrollment report for {courseName}:");
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]} - Enrolled on {reader["EnrollmentDate"]}");
                        }
                    }
                }
            }
        }
    }
}
