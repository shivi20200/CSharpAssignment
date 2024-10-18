using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS_Module.SIS.Entity
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<Enrollment> Enrollments { get; set; }
        public List<Payment> Payments { get; set; } // Added for payment history

        public Student(int studentId, string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber)
        {
            StudentId = studentId;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Email = email;
            PhoneNumber = phoneNumber;
            Enrollments = new List<Enrollment>();
            Payments = new List<Payment>(); // Initialize payments
        }

        public Student() { }

        public void EnrollInCourse(Course course)
        {
            Enrollment enrollment = new Enrollment(Enrollments.Count + 1, StudentId, course.CourseId, DateTime.Now, this, course);
            Enrollments.Add(enrollment);
            course.Enrollments.Add(enrollment);
        }

        public void UpdateStudentInfo(string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public void MakePayment(decimal amount, DateTime paymentDate)
        {
            Payment payment = new Payment(Payments.Count + 1, StudentId, amount, paymentDate, this);
            Payments.Add(payment);
        }

        public void DisplayStudentInfo()
        {
            Console.WriteLine($"Student ID: {StudentId}, Name: {FirstName} {LastName}, DOB: {DateOfBirth.ToShortDateString()}, Email: {Email}, Phone: {PhoneNumber}");
        }

        public List<Course> GetEnrolledCourses()
        {
            List<Course> courses = new List<Course>();
            foreach (var enrollment in Enrollments)
            {
                courses.Add(enrollment.Course);
            }
            return courses;
        }

        public List<Payment> GetPaymentHistory()
        {
            return Payments;
        }
    }
}
