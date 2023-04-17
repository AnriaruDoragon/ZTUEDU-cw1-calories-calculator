using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CCLibrary.User;
using CCLibrary.Exceptions;

namespace CCLibrary.Data;

public partial class DataContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }

    /// <summary>
    /// Create a new profile.
    /// </summary>
    /// <exception cref="ProfileAlreadyExistsException"></exception>
    public Profile CreateProfile(string login, string password, string secretWord)
    {
        if (Profiles.AsEnumerable().Any(profile => profile.Login.Equals(login, StringComparison.OrdinalIgnoreCase)))
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
        Profile? profile = Profiles.AsEnumerable()
            .FirstOrDefault(profile => profile.Login.Equals(login, StringComparison.OrdinalIgnoreCase))
                ?? throw new ProfiletNotFoundException();

        if (!profile.CheckPassword(password))
            throw new WrongPasswordException();

        return profile;
    }

    /// <summary>
    /// Delete user profile.
    /// </summary>
    /// <exception cref="ProfiletNotFoundException"></exception>
    /// <exception cref="WrongPasswordException"></exception>
    public void DeleteProfile(string login, string password)
    {
        Profile? profile = Profiles.AsEnumerable()
            .FirstOrDefault(profile => profile.Login.Equals(login, StringComparison.OrdinalIgnoreCase))
                ?? throw new ProfiletNotFoundException();

        if (!profile.CheckPassword(password))
            throw new WrongPasswordException();

        Profiles.Remove(profile);
        SaveChanges();
    }

    /// <summary>
    /// Set new password using old password.
    /// </summary>
    /// <exception cref="ProfiletNotFoundException"></exception>
    /// <exception cref="WrongPasswordException"></exception>
    public void UpdatePassword(long id, string oldPassword, string newPassword)
    {
        Profile? profile = Profiles.AsEnumerable()
            .FirstOrDefault(profile => profile.Id.Equals(id))
                ?? throw new ProfiletNotFoundException();

        if (!profile.CheckPassword(oldPassword))
            throw new WrongPasswordException();

        profile.SetPassword(newPassword);
        SaveChanges();
    }

    /// <summary>
    /// Set new password using secret word.
    /// </summary>
    /// <exception cref="ProfiletNotFoundException"></exception>
    /// <exception cref="WrongSecretWordException"></exception>
    public void ResetPassword(string login, string secretWord, string newPassword)
    {
        Profile? profile = Profiles.AsEnumerable()
            .FirstOrDefault(profile => profile.Login.Equals(login, StringComparison.OrdinalIgnoreCase))
                ?? throw new ProfiletNotFoundException();

        if (!profile.CheckSecretWord(secretWord))
            throw new WrongSecretWordException();

        profile.SetPassword(newPassword);
        SaveChanges();
    }

    public void ForgetRememberedProfile()
    {
        Profile? remembered = GetRememberedProfile();
        if (remembered is null)
            return;
        remembered.IsRemembered = false;
        SaveChanges();
    }

    /// <summary>
    /// RememberProfile profile for auto login.
    /// </summary>
    /// <exception cref="ProfiletNotFoundException"></exception>
    public void RememberProfile(long id)
    {
        Profile? profile = Profiles.AsEnumerable()
            .FirstOrDefault(profile => profile.Id == id)
                ?? throw new ProfiletNotFoundException();

        ForgetRememberedProfile();

        profile.IsRemembered = true;
        SaveChanges();
    }

    /// <summary>
    /// Get remembered profile.
    /// </summary>
    public Profile? GetRememberedProfile()
        => Profiles.AsEnumerable().FirstOrDefault(profile => profile.IsRemembered);
}
