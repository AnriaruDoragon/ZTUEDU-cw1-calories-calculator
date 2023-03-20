using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CCLibrary.User;
using CCLibrary.Exceptions;

namespace CCLibrary.Data
{
    public class ProfileContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(Data.Database.DbSource);

        /// <summary>
        /// Create a new profile.
        /// </summary>
        /// <exception cref="ProfileAlreadyExistsException"></exception>
        public Profile CreateProfile(string login, string password)
        {
            if (Profiles.Any(profile => profile._login.Equals(login, StringComparison.OrdinalIgnoreCase)))
                throw new ProfileAlreadyExistsException();

            Profile newProfile = new(login, password);
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
    }
}
