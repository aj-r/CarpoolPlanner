using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using CarpoolPlanner.Model;
using System.Web.Security;

namespace CarpoolPlanner.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            using (var context = ApplicationDbContext.Create())
            {
                var loginName = txtUserId.Text;
                var existingUser = context.Users.FirstOrDefault(u => u.LoginName == loginName);
                if (existingUser != null)
                {
                    ErrorMessage.Text = "A user with the same name already exists.";
                    return;
                }
                var user = new User
                {
                    LoginName = loginName,
                    Name = txtName.Text != string.Empty ? txtName.Text : null,
                    Email = txtEmail.Text,
                    EmailNotify = chkEmailNotify.Checked,
                    EmailVisible = chkEmailVisible.Checked,
                    Phone = txtPhone.Text,
                    PhoneNotify = chkPhoneNotify.Checked,
                    PhoneVisible = chkPhoneVisible.Checked,
                    CommuteMethod = (CommuteMethod)Enum.Parse(typeof(CommuteMethod), rblCommuteMethod.SelectedValue),
                    CanDriveIfNeeded = chkCanDriveIfNeeded.Checked,
                    Status = UserStatus.Unapproved
                };
                if (nudSeats.Value.HasValue)
                    user.Seats = (int)nudSeats.Value.Value;
                user.SetPassword(Password.Text);
                context.Users.Add(user);
                int updateCount = context.SaveChanges();
                if (updateCount == 0)
                {
                    ErrorMessage.Text = "Failed to create user - unknown reason.";
                    return;
                }
                // Log the user in
                // Note: allow unapproved accounts to log in, but give them limited access.
                FormsAuthentication.SetAuthCookie(user.LoginName, false);
                // Put them on the trips page to start, since that is the only thing unapproved users have access to.
                Response.Redirect("~/Trips.aspx");
            }
        }
    }
}