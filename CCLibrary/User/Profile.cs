using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCLibrary.Exceptions;

namespace CCLibrary.User
{
    [Table("Profiles")]
    public class Profile
    {
        internal string _login;
        private string _password;
        private string _secretWord;
        public bool IsRemembered = false;

        protected float _height = 0;
        protected float _weight = 0;

        [Key]
        public long Id { get; protected set; }
        public string Name { get; set; } = "N/D";
        public byte[]? Image { get; set; } = null;
        public DateTime? BirthDay { get; set; }
        public Genders Gender { get; set; }
        public double CaloriesGoal { get; set; }

        // EntityFramework
        public Profile()
        {
            _login = string.Empty;
            _password = string.Empty;
            _secretWord = string.Empty;
        }

        public Profile(string login, string password, string secretWord)
        {
            _login = login;
            _password = password;
            _secretWord = secretWord;
        }
        
        public int Age
        {
            get
            {
                if (BirthDay is null)
                    return 0;
                int age = DateTime.Today.Year - ((DateTime)BirthDay).Year;
                if (DateTime.Today < ((DateTime)BirthDay).AddYears(age)) age--;
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

        /// <summary>
        /// Verify password.
        /// </summary>
        public bool CheckPassword(string password)
        {
            return _password.Equals(password, StringComparison.Ordinal);
        }

        /// <summary>
        /// Verify secret word.
        /// </summary>
        public bool CheckSecretWord(string secretWord)
        {
            return secretWord.Equals(_secretWord, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Set new password.
        /// </summary>
        internal void SetPassword(string newPassword)
        {
            _password = newPassword;
        }
    }
}
