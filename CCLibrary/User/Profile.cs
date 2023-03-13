using System;
using CCLibrary.Exceptions;

namespace CCLibrary.User
{
    public class Profile
    {
        internal string _login;
        private string _password;

        public ulong Id { get; set; }
        public string Name { get; set; } = "N/D";
        public byte[]? Image { get; set; }

        public DateTime BirthDay { get; set; }
        protected float _height;
        protected float _weight;

        public int Age
        {
            get
            {
                int age = DateTime.Today.Year - BirthDay.Year;
                if (DateTime.Today < BirthDay.AddYears(age)) age--;
                return age;
            }
        }

        public float HeightInCm
        {
            get { return _height; }
            set
            {
                if (value <= 0)
                    throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
                _height = value;
            }
        }

        public float WeightInKg
        {
            get { return _weight; }
            set
            {
                if (value <= 0)
                    throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
                _weight = value;
            }
        }

        public Profile(string login, string password)
        {
            _login = login;
            _password = password;
        }

        /// <summary>
        /// Перевірити пароль на дійсність.
        /// </summary>
        public bool CheckPassword(string password)
        {
            return _password.Equals(password, StringComparison.Ordinal);
        }

        /// <summary>
        /// Оновити поточний пароль.
        /// </summary>
        /// <exception cref="WrongPasswordException"></exception>
        public void UpdatePassword(string oldPassword, string newPassword)
        {
            if (CheckPassword(oldPassword))
                _password = newPassword;
            else
                throw new WrongPasswordException();
        }
    }
}
