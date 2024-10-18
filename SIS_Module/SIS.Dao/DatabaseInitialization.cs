using System;
using SIS_Module.SIS.Entity;
using SIS_Module.SIS.Exception;
using SIS_Module.SIS.Util;
using SIS_Module.SIS.Main;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using static Mysqlx.Expect.Open.Types.Condition.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace SIS_Module.SIS.Dao
{
    public class DatabaseInitializer
    {
        public static void InitializeDatabase()
        {
            try
            {
                using (SqlConnection connection = DBConnectionUtil.GetConnection())
                {
                    connection.Open();


                    /*                   string dropForeignKeys = @"
                               IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Enrollments_Students')
                                   ALTER TABLE Enrollments DROP CONSTRAINT FK_Enrollments_Students;

                               IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Enrollments_Courses')
                                   ALTER TABLE Enrollments DROP CONSTRAINT FK_Enrollments_Courses;

                               IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Payments_Students')
                                   ALTER TABLE Payments DROP CONSTRAINT FK_Payments_Students;";

                                       using (SqlCommand command = new SqlCommand(dropForeignKeys, connection))
                                       {
                                           command.ExecuteNonQuery();
                                       }

                                       // Drop the Payments table first (it references Students)
                                       string dropPaymentsTable = "IF OBJECT_ID('Payments', 'U') IS NOT NULL DROP TABLE Payments;";
                                       using (SqlCommand command = new SqlCommand(dropPaymentsTable, connection))
                                       {
                                           command.ExecuteNonQuery();
                                       }

                                       // Drop the Enrollments table (it references Students and Courses)
                                       string dropEnrollmentsTable = "IF OBJECT_ID('Enrollments', 'U') IS NOT NULL DROP TABLE Enrollments;";
                                       using (SqlCommand command = new SqlCommand(dropEnrollmentsTable, connection))
                                       {
                                           command.ExecuteNonQuery();
                                       }

                                       // Drop the Teachers table (no foreign keys)
                                       string dropTeachersTable = "IF OBJECT_ID('Teachers', 'U') IS NOT NULL DROP TABLE Teachers;";
                                       using (SqlCommand command = new SqlCommand(dropTeachersTable, connection))
                                       {
                                           command.ExecuteNonQuery();
                                       }

                                       // Drop the Courses table (it does not have any foreign keys)
                                       string dropCoursesTable = "IF OBJECT_ID('Courses', 'U') IS NOT NULL DROP TABLE Courses;";
                                       using (SqlCommand command = new SqlCommand(dropCoursesTable, connection))
                                       {
                                           command.ExecuteNonQuery();
                                       }

                                       // Finally, drop the Students table
                                       string dropStudentsTable = "IF OBJECT_ID('Students', 'U') IS NOT NULL DROP TABLE Students;";
                                       using (SqlCommand command = new SqlCommand(dropStudentsTable, connection))
                                       {
                                           command.ExecuteNonQuery();
                                       }
                                   }

                               }
                               catch (SqlException ex)
                               {
                                   Console.WriteLine("An error occurred while initializing the database: " + ex.Message);
                               }
                           }
                       }
                   }
                    */
                    // Create the Students table
                    string createStudentsTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Students' AND xtype='U')
                BEGIN
                    CREATE TABLE Students (
                        StudentID INT PRIMARY KEY IDENTITY(101,1),
                        FirstName NVARCHAR(100),
                        LastName NVARCHAR(100),
                        DateOfBirth DATE,
                        Email NVARCHAR(100),
                        PhoneNumber NVARCHAR(15)
                    )
                END";

                    using (SqlCommand command = new SqlCommand(createStudentsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Create the Courses table
                    string createCoursesTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Courses' AND xtype='U')
                BEGIN
                    CREATE TABLE Courses (
                        CourseID INT PRIMARY KEY IDENTITY(1,1),
                        CourseName NVARCHAR(100),
                        CourseCode NVARCHAR(10),
                        InstructorName NVARCHAR(100)
                    )
                END";

                    using (SqlCommand command = new SqlCommand(createCoursesTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Create the Teachers table
                    string createTeachersTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Teachers' AND xtype='U')
                BEGIN
                    CREATE TABLE Teachers (
                        TeacherID INT PRIMARY KEY IDENTITY(1,1),
                        FirstName NVARCHAR(100),
                        LastName NVARCHAR(100),
                        Email NVARCHAR(100)
                    )
                END";

                    using (SqlCommand command = new SqlCommand(createTeachersTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Create the Enrollments table
                    string createEnrollmentsTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Enrollments' AND xtype='U')
                BEGIN
                    CREATE TABLE Enrollments (
                        EnrollmentID INT PRIMARY KEY IDENTITY(1,1),
                        StudentID INT,
                        CourseID INT,
                        EnrollmentDate DATE,
                        FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
                        FOREIGN KEY (CourseID) REFERENCES Courses(CourseID)
                    )
                END";

                    using (SqlCommand command = new SqlCommand(createEnrollmentsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Create the Payments table
                    string createPaymentsTable = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Payments' AND xtype='U')
                BEGIN
                    CREATE TABLE Payments (
                        PaymentID INT PRIMARY KEY IDENTITY(1,1),
                        StudentID INT,
                        Amount DECIMAL(18,2),
                        PaymentDate DATE,
                        FOREIGN KEY (StudentID) REFERENCES Students(StudentID)
                    )
                END";

                    using (SqlCommand command = new SqlCommand(createPaymentsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                Console.WriteLine("Database initialized and tables created successfully.");
            }

            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred while initializing the database: " + ex.Message);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }
    }
}

