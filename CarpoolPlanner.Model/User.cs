﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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
        public User()
        {
            UserTrips = new List<UserTrip>();
            Seats = 5;
            EmailNotify = false;
            EmailVisible = true;
            PhoneNotify = true;
            PhoneVisible = true;
            Status = UserStatus.Unapproved;
        }

        [Key, Required]
        public long Id { get; set; }

        [MaxLength(500), Required]
        public string LoginName { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the hashed password.
        /// </summary>
        [JsonIgnore]
        public byte[] Password { get; set; }

        /// <summary>
        /// Gets or sets the salt that was used to hash the password.
        /// </summary>
        [JsonIgnore]
        public byte[] Salt { get; set; }

        /// <summary>
        /// Gets or sets the number of PBKDF2 iterations used to hash the password.
        /// </summary>
        [JsonIgnore]
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

        public UserStatus Status { get; set; }

        public CommuteMethod CommuteMethod { get; set; }

        [JsonIgnore]
        public long? LastTextMessageId { get; set; }

        /// <summary>
        /// Indicates that the user can drive other people if necessary, but would prefer not to. Does not apply if CommuteMethod is Driver.
        /// </summary>
        public bool CanDriveIfNeeded { get; set; }

        private int seats;

        public int Seats
        {
            get { return seats; }
            set
            {
                if (value < 1)
                    return;
                seats = value;
            }
        }

        public bool IsAdmin { get; set; }

        public ICollection<UserTrip> UserTrips { get; private set; }

        public override string ToString()
        {
            return LoginName;
        }
    }
}