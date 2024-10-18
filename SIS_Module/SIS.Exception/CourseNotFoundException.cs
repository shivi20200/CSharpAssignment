using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS_Module.SIS.Exception
{
    public class CourseNotFoundException : ApplicationException
    {
        public CourseNotFoundException(string message) : base(message) { }
    }
}
