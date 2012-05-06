using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Arpar
{
    /// <summary>
    /// Exception used when duplicity in definition occures.
    /// </summary>
    public class DuplicateArgumentException : System.Exception
    {
        /// <summary>
        /// Initialize exception.
        /// </summary>
        public DuplicateArgumentException()
        {
        }

        /// <summary>
        /// Initialize exception with custom message.
        /// </summary>
        /// <param name="message">Custom message.</param>
        public DuplicateArgumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initialize exception with custom message and inner exception.
        /// </summary>
        /// <param name="message">Custom message.</param>
        /// <param name="innerException">Inner exception to pass.</param>
        public DuplicateArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}