using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarpoolPlanner.Model;

namespace CarpoolPlanner
{
    public partial class Users : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (App.CurrentUser.Status != UserStatus.Active)
                return;
            if (!IsPostBack)
            {
                using (var context = ApplicationDbContext.Create())
                {
                    repUsers.DataSource = context.Users.ToList();
                    repUsers.DataBind();
                }
            }
        }

        protected void repUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var user = e.Item.DataItem as User;
            if (user == null)
                return;
            var ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
            var chkIsAdmin = (CheckBox)e.Item.FindControl("chkIsAdmin");
            var lblDriver = (Label)e.Item.FindControl("lblDriver");
            ddlStatus.SelectedValue = user.Status.ToString();
            chkIsAdmin.Checked = user.IsAdmin;
            switch (user.CommuteMethod)
            {
                case CommuteMethod.Driver:
                    lblDriver.Text = string.Format("Driver ({0} seats)", user.Seats);
                    break;
                case CommuteMethod.HaveRide:
                    lblDriver.Text = "Will get my own ride. ";
                    break;
            }
            if (user.CommuteMethod != CommuteMethod.Driver && user.CanDriveIfNeeded)
                lblDriver.Text += string.Format("Can drive if needed ({0} seats)", user.Seats);
        }

        [WebMethod, ScriptMethod]
        public static void UpdateUserStatus(string userId, string status)
        {
            if (!App.CurrentUser.IsAdmin)
                throw new SecurityException();
            var parsedStatus = (UserStatus)Enum.Parse(typeof(UserStatus), status);
            if (userId == App.CurrentUser.Id && parsedStatus == UserStatus.Disabled)
                throw new Exception("You cannot disable yourself.");
            using (var context = ApplicationDbContext.Create())
            {
                var user = context.Users.Find(userId);
                user.Status = parsedStatus;
                context.SaveChanges();
                App.UpdateCachedUser(user);
            }
        }

        [WebMethod, ScriptMethod]
        public static void UpdateUserIsAdmin(string userId, bool isAdmin)
        {
            if (!App.CurrentUser.IsAdmin)
                throw new SecurityException();
            if (userId == App.CurrentUser.Id)
                throw new Exception("You cannot remove admin rights from yourself.");
            using (var context = ApplicationDbContext.Create())
            {
                var user = context.Users.Find(userId);
                user.IsAdmin = isAdmin;
                context.SaveChanges();
                App.UpdateCachedUser(user);
            }
        }
    }
}