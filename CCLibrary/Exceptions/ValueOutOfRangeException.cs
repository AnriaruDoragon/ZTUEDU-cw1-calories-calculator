using System;

namespace CCLibrary.Exceptions
{
    public class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException(string message) : base(message) { }
    }
}
