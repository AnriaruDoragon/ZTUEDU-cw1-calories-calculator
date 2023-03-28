using System;

namespace CCLibrary.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        private const string message = "Продукт не знайдено!";
        public ProductNotFoundException(string message = message) : base(message) { }
    }
}
