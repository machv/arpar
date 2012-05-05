using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Arpar
{
    public class DuplicateArgumentException : System.Exception
    {
        public DuplicateArgumentException()
        {
        }

        public DuplicateArgumentException(string message)
            : base(message)
        {
        }

        public DuplicateArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}