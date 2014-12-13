using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using CarpoolPlanner.Model;

namespace CarpoolPlanner.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx";
            // Enable this once you have account confirmation enabled for password reset functionality
            // ForgotPasswordHyperLink.NavigateUrl = "Forgot";
            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (!IsValid)
                return;
            using (var context = ApplicationDbContext.Create())
            {
                var user = context.FindUser(txtUserName.Text, txtPassword.Text);
                if (user != null)
                {
                    App.UpdateCachedUser(user);
                    switch (user.Status)
                    {
                        case UserStatus.Disabled:
                            ShowError("Your account has has been disabled.");
                            break;
                        default:
                            // Note: allow unapproved accounts to log in, but give them limited access.
                            // Log the user in, redirect to the proper page.
                            FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, RememberMe.Checked);
                            break;
                    }
                }
                else
                {
                    ShowError("Invalid username or password.");
                }
            }
        }

        private void ShowError(string error)
        {
            FailureText.Text = error;
            ErrorMessage.Visible = true;
        }
    }
}