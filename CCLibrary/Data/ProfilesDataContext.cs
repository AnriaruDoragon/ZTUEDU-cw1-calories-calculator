using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CCLibrary.User;
using CCLibrary.Exceptions;

namespace CCLibrary.Data
{
    public partial class DataContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }

        /// <summary>
        /// Create a new profile.
        /// </summary>
        /// <exception cref="ProfileAlreadyExistsException"></exception>
        public Profile CreateProfile(string login, string password, string secretWord)
        {
            if (Profiles.Any(profile => profile._login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                throw new ProfileAlreadyExistsException();

            Profile newProfile = new(login, password, secretWord);
            Profiles.Add(newProfile);
            SaveChanges();
            return newProfile;
        }

        /// <summary>
        /// Get profile if it exists and the password is correct.
        /// </summary>
        /// <exception cref="ProfiletNotFoundException"></exception>
        /// <exception cref="WrongPasswordException"></exception>
        public Profile GetProfile(string login, string password)
        {
            Profile? profile = Profiles.FirstOrDefault(profile =>
                profile._login.Equals(login, StringComparison.OrdinalIgnoreCase))
                    ?? throw new ProfiletNotFoundException();

            if (!profile.CheckPassword(password))
                throw new WrongPasswordException();

            return profile;
        }

        /// <summary>
        /// RememberProfile profile for auto login.
        /// </summary>
        /// <exception cref="ProfiletNotFoundException"></exception>
        public void RememberProfile(long id)
        {
            Profile? profile = Profiles.FirstOrDefault(profile => profile.Id == id)
                ?? throw new ProfiletNotFoundException();

            Profile? remembered = GetRememberedProfile();
            if (remembered is not null)
                remembered.IsRemembered = false;
            profile.IsRemembered = true;
            SaveChanges();
        }

        /// <summary>
        /// Get remembered profile.
        /// </summary>
        public Profile? GetRememberedProfile()
        {
            return Profiles.AsEnumerable()
                .FirstOrDefault(profile => profile.IsRemembered, null);
        }
    }
}
