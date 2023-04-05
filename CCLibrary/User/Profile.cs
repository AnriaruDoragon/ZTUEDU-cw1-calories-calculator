using System;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using CCLibrary.Exceptions;

namespace CCLibrary.User
{
    [Table("Profiles")]
    public class Profile
    {
        protected float? _height = null;
        protected float? _weight = null;

        [Key]
        public long Id { get; protected set; }
        public string Login { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string SecretWordHash { get; set; } = string.Empty;
        public bool IsRemembered { get; set; } = false;

        public string Name { get; set; } = "N/D";
        public byte[]? Image { get; set; } = null;
        public DateTime? BirthDay { get; set; } = null;
        public Genders Gender { get; set; } = Genders.Unknown;
        public double CaloriesGoal { get; set; } = 1000;

        public Profile() { }

        public Profile(string login, string password, string secretWord)
        {
            Login = login;
            PasswordHash = HashString(password);
            SecretWordHash = HashString(secretWord);
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

        public float? HeightInCm
        {
            get { return _height; }
            set
            {
                if (value <= 0)
                    throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
                _height = value;
            }
        }

        public float? WeightInKg
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
        /// Hash password or secret word.
        /// </summary>
        public static string HashString(string s)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
            return Convert.ToBase64String(hashedBytes);
        }

        /// <summary>
        /// Verify password.
        /// </summary>
        public bool CheckPassword(string password)
        {
            string hashedPassword = HashString(password);
            return PasswordHash.Equals(hashedPassword, StringComparison.Ordinal);
        }

        /// <summary>
        /// Verify secret word.
        /// </summary>
        public bool CheckSecretWord(string secretWord)
        {
            string hashedSecretword = HashString(secretWord);
            return SecretWordHash.Equals(hashedSecretword, StringComparison.Ordinal);
        }

        /// <summary>
        /// Set new password.
        /// </summary>
        internal void SetPassword(string newPassword)
        {
            PasswordHash = HashString(newPassword);
        }
    }
}
