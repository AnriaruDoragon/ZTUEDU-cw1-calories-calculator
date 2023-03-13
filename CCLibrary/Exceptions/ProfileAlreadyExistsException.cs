using System;

namespace CCLibrary.Exceptions
{
    public class ProfileAlreadyExistsException : Exception
    {
        private const string message = "Профіль вже існує!";
        public ProfileAlreadyExistsException(string message = message) : base(message) { }
    }
}
