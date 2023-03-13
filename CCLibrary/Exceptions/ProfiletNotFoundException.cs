using System;

namespace CCLibrary.Exceptions
{
    public class ProfiletNotFoundException : Exception
    {
        private const string message = "Профіль не знайдено!";
        public ProfiletNotFoundException(string message = message) : base(message) { }
    }
}
