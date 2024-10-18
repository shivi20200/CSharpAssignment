using SIS_Module.SIS.Entity;
using SIS_Module.SIS.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS_Module.SIS.Main
{
    public class SIS
    {
        public List<Student> Students { get; set; }
        public List<Course> Courses { get; set; }
        public List<Teacher> Teachers { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Payment> Payments { get; set; }

        public SIS()
        {
            Students = new List<Student>();
            Courses = new List<Course>();
            Teachers = new List<Teacher>();
            Enrollments = new List<Enrollment>();
            Payments = new List<Payment>();
        }

        public void EnrollStudentInCourse(Student student, Course course)
        {
            student.EnrollInCourse(course);
            Enrollment enrollment = new Enrollment(Enrollments.Count + 1, student.StudentId, course.CourseId, DateTime.Now, student, course);
            Enrollments.Add(enrollment);
            course.Enrollments.Add(enrollment);
        }

        public void AssignTeacherToCourse(Teacher teacher, Course course)
        {
            course.AssignTeacher(teacher);
            teacher.AssignedCourses.Add(course);
        }

        public void RecordPayment(Student student, decimal amount, DateTime paymentDate)
        {
            student.MakePayment(amount, paymentDate);
            Payment payment = new Payment(Payments.Count + 1, student.StudentId, amount, paymentDate, student);
            Payments.Add(payment);
        }

        public List<Enrollment> GetEnrollmentsForStudent(Student student)
        {
            return student.Enrollments;
        }

        public List<Course> GetCoursesForTeacher(Teacher teacher)
        {
            return teacher.GetAssignedCourses();
        }

        public void GenerateEnrollmentReport(Course course)
        {
            Console.WriteLine($"Enrollments for {course.CourseName}:");
            foreach (var enrollment in course.GetEnrollments())
            {
                Console.WriteLine($"Student ID: {enrollment.StudentId}, Enrollment Date: {enrollment.EnrollmentDate.ToShortDateString()}");
            }
        }

        public void GeneratePaymentReport(Student student)
        {
            Console.WriteLine($"Payment History for {student.FirstName} {student.LastName}:");
            foreach (var payment in student.GetPaymentHistory())
            {
                Console.WriteLine($"Payment ID: {payment.PaymentId}, Amount: {payment.Amount}, Date: {payment.PaymentDate.ToShortDateString()}");
            }
        }

        public void CalculateCourseStatistics(Course course)
        {
            Console.WriteLine($"Course Statistics for {course.CourseName}:");
            Console.WriteLine($"Number of Enrollments: {course.Enrollments.Count}");
            // Additional statistics can be added here
        }
    }
}

