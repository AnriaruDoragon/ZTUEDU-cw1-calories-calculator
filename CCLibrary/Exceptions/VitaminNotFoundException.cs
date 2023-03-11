using System;

namespace CCLibrary.Exceptions
{
    public class VitaminNotFoundException : Exception
    {
        private const string message = "Вітамін з такою назвою не знайдено!";
        public VitaminNotFoundException(string message = message) : base(message) { }
    }
}
