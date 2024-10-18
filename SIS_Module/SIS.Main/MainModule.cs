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

namespace SIS_Module.SIS.Main
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

            // Create a new Student object for John Doe
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

            // Create a new Teacher object for Sarah Smith
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

            // Call the method to insert Student ID
            int studentId = 101; // Specify the student ID you want to insert
            var payment = new Payment
            {
                StudentId = studentId, // Use the same student ID for payment
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
/*
public static void Main(string[] args)
{
    using (SqlConnection connection = DBConnectionUtil.GetConnection())
    {
        try
        {
            connection.Open();
            Console.WriteLine("Connection successful!");
        }
        catch (ApplicationException ex)
        {
            Console.WriteLine("Failed to connect: " + ex.Message);
        }
        // Build the configuration
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        // Now you can access the configuration values
        string connectionString = configuration.GetConnectionString("DefaultConnection");
        Console.WriteLine($"Connection string: {connectionString}");

        Console.WriteLine("Initializing SQL Database...");

        // Step 1: Database Initialization
        DatabaseInitializer.InitializeDatabase();

        var studentDAO = new StudentDAO();
        var courseDAO = new CourseDAO();
        var teacherDAO = new TeacherDAO();
        var enrollmentDAO = new EnrollmentDAO();
        var paymentDAO = new PaymentDAO();

        // Task 8: Enroll John Doe
        Console.WriteLine("Enrolling John Doe...");
        var johnCourses = new List<string> { "Introduction to Programming", "Mathematics 101" };
        EnrollmentSystem.EnrollStudent("John", "Doe", new DateTime(1995, 8, 15), "john.doe@example.com", "123-456-7890", johnCourses);
        Console.WriteLine("John Doe has been successfully enrolled.\n");

        // Task 9: Assign Sarah Smith to a course
        Console.WriteLine("Assigning Sarah Smith to a course...");
        EnrollmentSystem.AssignTeacherToCourse("Sarah Smith", "sarah.smith@example.com", "Computer Science", "CS302");
        Console.WriteLine("Sarah Smith has been assigned to the course.\n");

        // Task 10: Record Jane Johnson's payment
        Console.WriteLine("Recording payment for Jane Johnson...");
        EnrollmentSystem.RecordPayment(101, 500.00m, new DateTime(2023, 4, 10));
        Console.WriteLine("Payment for Jane Johnson has been recorded.\n");

        // Task 11: Generate enrollment report for Computer Science 101
        Console.WriteLine("Generating enrollment report for Computer Science 101...");
        EnrollmentSystem.GenerateEnrollmentReport("Computer Science 101");
        Console.WriteLine("Enrollment report generated successfully.\n");
    }
}
}
}
public static void Main()
{
    using (SqlConnection connection = DBConnectionUtil.GetConnection())
    {
        try
        {
            connection.Open();
            Console.WriteLine("Connection successful!");
        }
        catch (ApplicationException ex)
        {
            Console.WriteLine("Failed to connect: " + ex.Message);
        }
    }
}
}
}
static void Main(string[] args)
{
    Console.WriteLine("Initializing Student Information System...");
    // Create an instance of SIS
    SIS sis = new SIS();

    // Create sample students
    Student student1 = new Student(1, "John", "Doe", new DateTime(1995, 5, 15), "john.doe@example.com", "123-456-7890");
    Student student2 = new Student(2, "Jane", "Smith", new DateTime(1996, 8, 20), "jane.smith@example.com", "987-654-3210");

    // Add students to SIS
    sis.Students.Add(student1);
    sis.Students.Add(student2);

    // Create sample courses
    Course course1 = new Course(1, "Mathematics", "MATH101", "");
    Course course2 = new Course(2, "Science", "SCI101", "");

    // Add courses to SIS
    sis.Courses.Add(course1);
    sis.Courses.Add(course2);

    // Create a teacher
    Teacher teacher1 = new Teacher(1, "Mr.", "Anderson", "mr.anderson@example.com");
    sis.Teachers.Add(teacher1);

    // Assign teacher to a course
    sis.AssignTeacherToCourse(teacher1, course1);
    sis.AssignTeacherToCourse(teacher1, course2);

    // Enroll students in courses
    sis.EnrollStudentInCourse(student1, course1);
    sis.EnrollStudentInCourse(student2, course1);
    sis.EnrollStudentInCourse(student2, course2);

    // Record payments
    student1.MakePayment(100, DateTime.Now);
    student2.MakePayment(150, DateTime.Now);

    // Generate reports
    sis.GenerateEnrollmentReport(course1);
    sis.GeneratePaymentReport(student2);
    sis.CalculateCourseStatistics(course1);

    // Display information
    student1.DisplayStudentInfo();
    course1.DisplayCourseInfo();
    teacher1.DisplayTeacherInfo();

    // Build the configuration
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

    IConfiguration configuration = builder.Build();

    // Now you can access the configuration values
    string connectionString = configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"Connection string: {connectionString}");

    Console.WriteLine("Initializing SQL Database...");

    // Step 1: Database Initialization
    DatabaseInitializer.InitializeDatabase();

    var studentDAO = new StudentDAO();
    var courseDAO = new CourseDAO();
    var teacherDAO = new TeacherDAO();
    var enrollmentDAO = new EnrollmentDAO();
    var paymentDAO = new PaymentDAO();

    // Example 1: Adding a new student
    var newStudent = new Student
    {
        FirstName = "John",
        LastName = "Doe",
        DateOfBirth = new DateTime(2000, 1, 1),
        Email = "john.doe@example.com",
        PhoneNumber = "123-456-7890"
    };

    // Call AddStudent method
    StudentDAO.AddStudent(newStudent); // Ensure this is called once and no connection is reused here

    // Example 2: Adding a new course
    var newCourse = new Course
    {
        CourseName = "Introduction to Programming",
        CourseCode = "CS101",
        InstructorName = "Jane Smith"
    };
    CourseDAO.AddCourse(newCourse);

    // Example 3: Enrolling a student in a course (transaction management)
    try
    {
        EnrollmentDAO.AddEnrollment(newStudent.StudentId, newCourse.CourseId); // Use the class name
        Console.WriteLine("Enrollment successful.");
    }
    catch (ApplicationException ex)
    {
        Console.WriteLine($"Failed to enroll student: {ex.Message}");
    }


    // Example 4: Adding a new payment
    var payment = new Payment
    {
        StudentId = newStudent.StudentId,
        Amount = 500.00m,
        PaymentDate = DateTime.Now
    };
    PaymentDAO.AddPayment(payment);

    // Example 5: Retrieving all students
    var students = StudentDAO.GetAllStudents();
    Console.WriteLine("Students in the system:");
    foreach (var student in students)
    {
        Console.WriteLine($"{student.FirstName} {student.LastName} - {student.Email}");
    }

    // Example 6: Updating student details
    newStudent.Email = "john.new@example.com";
    StudentDAO.UpdateStudent(newStudent);

    // Example 7: Dynamic Query Builder Example
    var columns = new List<string> { "FirstName", "LastName", "Email" };
    var conditions = new Dictionary<string, object> { { "FirstName", "John" } };
    var dynamicResults = DBQueryBuilderUtil.ExecuteDynamicQuery("Students", columns, conditions, "LastName", "ASC");
    Console.WriteLine("Dynamic Query Results:");
    foreach (var row in dynamicResults)
    {
        Console.WriteLine($"{row["FirstName"]} {row["LastName"]} - {row["Email"]}");
    }
}
}
*/





