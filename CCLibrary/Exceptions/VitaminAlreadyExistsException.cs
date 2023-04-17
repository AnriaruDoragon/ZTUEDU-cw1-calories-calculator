using System;

namespace CCLibrary.Exceptions;

public class VitaminAlreadyExistsException : Exception
{
    private const string message = "Вітамін з такою назвою вже існує!";
    public VitaminAlreadyExistsException(string message = message) : base(message) { }
}
