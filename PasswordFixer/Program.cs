using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarpoolPlanner.Controllers;
using CarpoolPlanner.Model;

namespace PasswordFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: cppasswd email password");
                return;
            }
            var email = args[0];
            var password = args[1];
            using (var context = ApplicationDbContext.Create())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return;
                }
                UserController.SetPassword(user, password);
                context.SaveChanges();
            }
            Console.WriteLine("Password updated.");
        }
    }
}
