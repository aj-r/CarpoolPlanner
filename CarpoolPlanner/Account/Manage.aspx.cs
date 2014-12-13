using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarpoolPlanner.Model;
using log4net;

namespace CarpoolPlanner.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected static ILog log = LogManager.GetLogger(typeof(Manage));

        protected string UpdateSuccessMessage { get; private set; }
        protected string UpdateErrorMessage { get; private set; }
        protected string ChangePasswordSuccessMessage { get; private set; }
        protected string ChangePasswordErrorMessage { get; private set; }

        protected void Page_Load()
        {
            if (!IsPostBack)
            {
                // Fill form fields
                var user = App.CurrentUser;
                txtName.Text = user.Name;
                rblCommuteMethod.SelectedValue = user.CommuteMethod.ToString();
                chkCanDriveIfNeeded.Checked = user.CanDriveIfNeeded;
                nudSeats.Value = user.Seats;
                txtEmail.Text = user.Email;
                chkEmailNotify.Checked = user.EmailNotify;
                chkEmailVisible.Checked = user.EmailVisible;
                txtPhone.Text = user.Phone;
                chkPhoneNotify.Checked = user.PhoneNotify;
                chkPhoneVisible.Checked = user.PhoneVisible;
            }
            ConfirmNewPassword.Attributes["equalto"] = "#" + NewPassword.ClientID;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = ApplicationDbContext.Create())
                {
                    var user = context.Users.Find(App.CurrentUserId);
                    user.Name = txtName.Text != string.Empty ? txtName.Text : null;
                    user.CommuteMethod = (CommuteMethod)Enum.Parse(typeof(CommuteMethod), rblCommuteMethod.SelectedValue);
                    user.CanDriveIfNeeded = chkCanDriveIfNeeded.Checked;
                    user.Seats = (int)(nudSeats.Value ?? 0);
                    user.Email = txtEmail.Text;
                    user.EmailNotify = chkEmailNotify.Checked;
                    user.EmailVisible = chkEmailVisible.Checked;
                    user.Phone = txtPhone.Text;
                    user.PhoneNotify = chkPhoneNotify.Checked;
                    user.PhoneVisible = chkPhoneVisible.Checked;
                    context.SaveChanges();
                    App.UpdateCachedUser(user);
                }
                UpdateSuccessMessage = "Updated successfully.";
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                UpdateErrorMessage = ex.Message;
            }
        }
        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (NewPassword.Text != ConfirmNewPassword.Text)
                {
                    ChangePasswordErrorMessage = "New passwords do not match.";
                    return;
                }
                using (var context = ApplicationDbContext.Create())
                {
                    var user = context.FindUser(App.CurrentUserId, CurrentPassword.Text);
                    if (user == null)
                    {
                        ChangePasswordErrorMessage = "Current password is incorrect.";
                        return;
                    }
                    user.SetPassword(NewPassword.Text);
                    context.SaveChanges();
                    App.UpdateCachedUser(user);
                }
                ChangePasswordSuccessMessage = "Password changed successfully.";
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
                ChangePasswordErrorMessage = ex.Message;
            }
        }
    }
}