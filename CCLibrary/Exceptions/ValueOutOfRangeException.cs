using System;

namespace CCLibrary.Exceptions
{
    internal class ValueOutOfRangeException : Exception
    {
        public ValueOutOfRangeException(string message) : base(message) { }
    }
}
