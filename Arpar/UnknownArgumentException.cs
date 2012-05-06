using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arpar
{
    /// <summary>
    /// Exception for situation when passed argument which is not registered to manage (eg. used specifies argument which is not expected.)
    /// </summary>
    public class UnknownArgumentException : System.ArgumentException
    {
        /// <summary>
        /// Initialize exception.
        /// </summary>
        public UnknownArgumentException()
        {
        }

        /// <summary>
        /// Initialize exception with custom message.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public UnknownArgumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialize exception with custom message and inner exception.
        /// </summary>
        /// <param name="message">Custom message.</param>
        /// <param name="innerException">Inner exception to pass.</param>
        public UnknownArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
