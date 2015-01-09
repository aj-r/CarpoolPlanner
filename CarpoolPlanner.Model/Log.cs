using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarpoolPlanner.Model
{
    public class Log
    {
        public long Id { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Logger { get; set; }

        public long? UserId { get; set; }

        [Association("User", "user_id", "id")]
        public User User { get; set; }

        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the nested diagnostic context when the log was created.
        /// </summary>
        public string Ndc { get; set; }
    }
}