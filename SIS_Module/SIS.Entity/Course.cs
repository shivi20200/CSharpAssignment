using SIS_Module.SIS.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS_Module.SIS.Entity
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string InstructorName { get; set; }
        public List<Enrollment> Enrollments { get; set; }

        public Course(int courseId, string courseName, string courseCode, string instructorName)
        {
            CourseId = courseId;
            CourseName = courseName;
            CourseCode = courseCode;
            InstructorName = instructorName;
            Enrollments = new List<Enrollment>();
        }
        public Course() { }
        public void AssignTeacher(Teacher teacher)
        {
            InstructorName = teacher.FirstName + " " + teacher.LastName;
        }

        public void UpdateCourseInfo(string courseCode, string courseName, string instructor)
        {
            CourseCode = courseCode;
            CourseName = courseName;
            InstructorName = instructor;
        }

        public void DisplayCourseInfo()
        {
            Console.WriteLine($"Course ID: {CourseId}, Name: {CourseName}, Code: {CourseCode}, Instructor: {InstructorName}");
        }

        public List<Enrollment> GetEnrollments()
        {
            return Enrollments;
        }

        public string GetTeacher()
        {
            return InstructorName;
        }
    }
}