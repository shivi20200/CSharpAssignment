﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS_Module.SIS.Exception
{
    public class DuplicateEnrollmentException : ApplicationException
    {
        public DuplicateEnrollmentException(string message) : base(message) { }
    }
}

