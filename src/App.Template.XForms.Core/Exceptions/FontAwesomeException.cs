using System;
using System.Collections.Generic;
using System.Text;

namespace App.Template.XForms.Core.Exceptions
{
    public class FontAwesomeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:FontAwesomeException"/> class
        /// </summary>
        public FontAwesomeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FontAwesomeException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public FontAwesomeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FontAwesomeException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public FontAwesomeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
