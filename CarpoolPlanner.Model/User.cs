using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using log4net;

namespace CarpoolPlanner.Model
{
    public enum UserStatus
    {
        Active,
        Unapproved,
        Disabled
    }

    /// <summary>
    /// Specifies how the user plans to commute.
    /// </summary>
    public enum CommuteMethod
    {
        /// <summary>
        /// Indicates that the user will need a ride from someone else.
        /// </summary>
        NeedRide,
        /// <summary>
        /// Indicates that the user can drive other people.
        /// </summary>
        Driver,
        /// <summary>
        /// Indicates that the user will get their own ride (separate from the carpool system).
        /// </summary>
        HaveRide
    }

    public class User
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(User));

        private const int saltSize = 24;
        private const int hashSize = 24;
        private const int defaultIterationCount = 1000;

        public User()
        {
            UserTrips = new List<UserTrip>();
            Seats = 5;
            EmailNotify = false;
            EmailVisible = true;
            PhoneNotify = true;
            PhoneVisible = true;
        }

        [Key, MaxLength(500), Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Sets the current user's password.
        /// </summary>
        /// <param name="password">The plain-text password.</param>
        public void SetPassword(string password)
        {
            if (password == null)
            {
                Salt = null;
                Password = null;
                Iterations = 0;
                log.Info(string.Concat("User '", Id, "' removed his/her password"));
            }
            else
            {
                var rng = new RNGCryptoServiceProvider();
                Salt = new byte[saltSize];
                rng.GetBytes(Salt);
                Iterations = defaultIterationCount;
                Password = Crypto.PBKDF2(password, Salt, Iterations, hashSize);
                log.Info(string.Concat("User '", Id, "' changed his/her password"));
            }
        }

        /// <summary>
        /// Checks whether the specified plain-text password is the correct password for the current user.
        /// </summary>
        /// <param name="password">The plain-text password to check.</param>
        public bool IsPasswordCorrect(string password)
        {
            if (password == null)
                return Password == null;
            if (Password == null || Salt == null)
                return false;
            var hash = Crypto.PBKDF2(password, Salt, Iterations, hashSize);
            return Crypto.SlowEquals(hash, Password);
        }

        /// <summary>
        /// Performs operations that should take the same amount of time as verifying the password.
        /// </summary>
        /// <param name="password">The plain-text password to "check".</param>
        public static void FakeIsPasswordCorrect(string password)
        {
            if (password == null)
                return;
            var buffer = new byte[saltSize];
            var hash = Crypto.PBKDF2(password, buffer, defaultIterationCount, hashSize);
            Crypto.SlowEquals(hash, buffer);
        }

        /// <summary>
        /// Gets or sets the hashed password.
        /// </summary>
        [ScriptIgnore]
        public byte[] Password { get; set; }

        /// <summary>
        /// Gets or sets the salt that was used to hash the password.
        /// </summary>
        [ScriptIgnore]
        public byte[] Salt { get; set; }

        /// <summary>
        /// Gets or sets the number of PBKDF2 iterations used to hash the password.
        /// </summary>
        [ScriptIgnore]
        public int Iterations { get; set; }

        /// <summary>
        /// Gets or sets the user's e-mail address.
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets whether the user wishes to receive notifications via e-mail.
        /// </summary>
        public bool EmailNotify { get; set; }

        /// <summary>
        /// Gets or sets whether the users e-mail is visible to other users.
        /// </summary>
        public bool EmailVisible { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets whether the user wishes to receive notifications via SMS.
        /// </summary>
        public bool PhoneNotify { get; set; }

        /// <summary>
        /// Gets or sets whether the users phone number is visible to other users.
        /// </summary>
        public bool PhoneVisible { get; set; }

        [ScriptIgnore]
        public UserStatus Status { get; set; }

        public CommuteMethod CommuteMethod { get; set; }

        [ScriptIgnore]
        public long? LastTextMessageId { get; set; }

        /// <summary>
        /// Indicates that the user can drive other people if necessary, but would prefer not to. Does not apply if CommuteMethod is Driver.
        /// </summary>
        [Display(Name = "")]
        public bool CanDriveIfNeeded { get; set; }

        public int Seats { get; set; }

        [ScriptIgnore]
        public bool IsAdmin { get; set; }

        public ICollection<UserTrip> UserTrips { get; private set; }

        public override string ToString()
        {
            return Id;
        }
    }
}