using System;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using CCLibrary.Exceptions;

namespace CCLibrary.User;

[Table("Profiles")]
public class Profile
{
    protected float? _height = null;
    protected float? _weight = null;

    [Key]
    public long Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string SecretWordHash { get; set; } = string.Empty;
    public bool IsRemembered { get; set; } = false;

    public string Name { get; set; } = "Невідомий";
    public byte[]? Image { get; set; } = null;
    public DateTime? BirthDay { get; set; } = null;
    public Sexes Sex { get; set; } = Sexes.Male;
    public Goals Goal { get; set; } = Goals.Maintaint;
    public double CaloriesGoal { get; set; }

    public Profile() => CalculateCaloriesNorm();

    public Profile(string login, string password, string secretWord) : this()
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

    public float HeightInCm
    {
        get
        {
            if (_height is null)
                return 1;
            return (float)_height;
        }
        set
        {
            if (value <= 0)
                throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
            _height = value;
        }
    }

    public float WeightInKg
    {
        get
        {
            if (_weight is null)
                return 1;
            return (float)_weight * 1000;
        }
        set
        {
            if (value <= 0)
                throw new ValueOutOfRangeException("Значення повинно бути додатнім!");
            _weight = value / 1000;
        }
    }

    /// <summary>
    /// Hash password or secret word.
    /// </summary>
    private static string HashString(string s)
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

    /// <summary>
    /// Calculate new goal based on the preferences.
    /// </summary>
    public void CalculateCaloriesNorm()
    {
        double newGoal = 1700;

        // If Height, Weight, Age and Sex are set, use Harris–Benedict equation
        if (_height is not null && _weight is not null && Age >= 18)
        {
            switch (Sex)
            {
                case Sexes.Female:
                    newGoal = 447.6 + (9.2 * WeightInKg) + (3.1 * HeightInCm) - (4.3 * Age);
                    break;
                case Sexes.Male:
                    newGoal = 88.36 + (13.4 * WeightInKg) + (4.8 * HeightInCm) - (5.7 * Age);
                    break;
            }
        }

        switch (Goal)
        {
            case Goals.Gain:
                newGoal += 750;
                break;
            case Goals.Lose:
                newGoal -= 750;
                break;
        }

        CaloriesGoal = newGoal;
    }
}
