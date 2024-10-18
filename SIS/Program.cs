using Microsoft.Extensions.Configuration;
using SIS_Module.SIS.Dao;
using SIS_Module.SIS.Entity;
using SIS_Module.SIS.Exception;
using SIS_Module.SIS.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace SIS
{
    class Program
    {
        static void Main(string[] args)
        {

            // Ensure database is initialized
            DatabaseInitializer.InitializeDatabase();

            // Task 8: Enroll John Doe
            Console.WriteLine("Enrolling John Doe...");
            var johnCourses = new List<string> { "Introduction to Programming", "Mathematics 101" };

            var john = new Student
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1995, 8, 15),
                Email = "john.doe@example.com",
                PhoneNumber = "123-456-7890"
            };

            EnrollmentSystem.EnrollStudent(john, johnCourses);
            Console.WriteLine("John Doe has been successfully enrolled.\n");

            // Task 9: Assign Sarah Smith to a course
            Console.WriteLine("Assigning Sarah Smith to a course...");

            var sarah = new Teacher
            {
                FirstName = "Sarah",
                LastName = "Smith",
                Email = "sarah.smith@example.com"
            };

            EnrollmentSystem.AssignTeacherToCourse(sarah, "CS302");
            Console.WriteLine("Sarah Smith has been assigned to the course.\n");

            // Task 10: Record Jane Johnson's payment
            Console.WriteLine("Recording payment for Jane Johnson...");

            int studentId = 101;
            var payment = new Payment
            {
                StudentId = studentId,
                Amount = 500.00m,
                PaymentDate = new DateTime(2023, 4, 10)
            };

            // Insert the student ID and record the payment
            EnrollmentSystem.RecordPaymentWithStudent(studentId, payment);
            Console.WriteLine("Payment recorded successfully.");

            Console.WriteLine("Payment for Jane Johnson has been recorded.\n");

            // Task 11: Generate enrollment report for Computer Science 101
            Console.WriteLine("Generating enrollment report for Computer Science 101...");
            EnrollmentSystem.GenerateEnrollmentReport("Computer Science 101");
            Console.WriteLine("Enrollment report generated successfully.\n");
        }
    }
}

