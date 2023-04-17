using System;

namespace CCLibrary.Exceptions;

public class SettingsException : Exception
{
    public SettingsException(string message) : base(message) { }
}
