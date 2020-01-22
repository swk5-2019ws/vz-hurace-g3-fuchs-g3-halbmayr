using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

#pragma warning disable CA1032 // Implement standard exception constructors
namespace Hurace.Core.Debugging.Exceptions
{
    public class HuraceException : Exception
    {
        public HuraceException(string message) : base(message)
        {
        }

        public HuraceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
