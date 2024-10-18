using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS_Module.SIS.Exception
{
    public class TeacherNotFoundException : ApplicationException
    {
        public TeacherNotFoundException(string message) : base(message) { }
    }
}
