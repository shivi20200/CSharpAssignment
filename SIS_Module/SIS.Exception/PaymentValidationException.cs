using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIS_Module.SIS.Exception
{
    public class PaymentValidationException : ApplicationException
    {
        public PaymentValidationException(string message) : base(message) { }
    }
}
