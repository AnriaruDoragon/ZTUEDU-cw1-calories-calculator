using System;

namespace CCLibrary.Exceptions
{
    public class WrongSecretWordException : Exception
    {
        private const string message = "Невірне тайне слово!";
        public WrongSecretWordException(string message = message) : base(message) { }
    }
}
