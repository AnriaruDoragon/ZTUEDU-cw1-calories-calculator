using System;

namespace CCLibrary.Exceptions;

public class WrongPasswordException : Exception
{
    private const string message = "Невірний пароль!";
    public WrongPasswordException(string message = message) : base(message) { }
}
