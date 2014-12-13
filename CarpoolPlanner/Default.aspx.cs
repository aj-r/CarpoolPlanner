using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using CarpoolPlanner.Controls;
using CarpoolPlanner.Model;
using CarpoolPlanner.UserControls;
using log4net;

namespace CarpoolPlanner
{
    public partial class _Default : Page
    {
        private static ILog log = LogManager.GetLogger(typeof(_Default));

        private List<UserTrip> userTrips;

        protected void Page_Load(object sender, EventArgs e)
        {
            var user = App.CurrentUser;
            using (var context = ApplicationDbContext.Create())
            {
                userTrips = context.GetUserTrips(App.CurrentUser).Include(ut => ut.Instances).Include(ut => ut.Trip.Recurrences).ToList();
                foreach (var tripRecurrence in userTrips.SelectMany(ut => ut.Trip.Recurrences))
                {
                    // With EF magic, this method automatically adds the next TripInstance for each recurrence to the Trip
                    context.GetNextTripInstance(tripRecurrence, ApplicationDbContext.TripInstanceRemovalDelay);
                }
                if (IsPostBack)
                {
                    LoadDataFromPage(context);
                }
            }
        }

        public string SuccessMessage { get; set; }

        public string ErrorMessage { get; set; }
        
        private void LoadDataFromPage(ApplicationDbContext context)
        {
            // Ensure all users are loaded from the db (needed for tripInstance.GetRequiredSeats())
            context.Users.ToList();

            var user = App.CurrentUser;
            bool changed = false;
            foreach (RepeaterItem item in repTrips.Items)
            {
                var hdnTripId = (HiddenField)item.FindControl("hdnTripId");
                var repTripInstances = (Repeater)item.FindControl("repTripInstances");
                var tripId = int.Parse(hdnTripId.Value);
                var userTrip = userTrips.FirstOrDefault(ut => ut.TripId == tripId);
                if (userTrip == null)
                    continue;
                foreach (RepeaterItem subItem in repTripInstances.Items)
                {
                    var hdnTripInstanceId = (HiddenField)subItem.FindControl("hdnTripInstanceId");
                    var chkAttending = (TriStateCheckBox)subItem.FindControl("chkAttending");
                    var btnDetail = (DetailButton)subItem.FindControl("btnDetail");
                    var rblCommuteMethod = (RadioButtonList)btnDetail.FindControl("rblCommuteMethod");
                    var chkCanDriveIfNeeded = (CheckBox)btnDetail.FindControl("chkCanDriveIfNeeded");
                    var nudSeats = (NumericUpDown)btnDetail.FindControl("nudSeats");
                    var tripInstanceId = int.Parse(hdnTripInstanceId.Value);
                    var userTripInstance = userTrip.Instances.FirstOrDefault(ut => ut.TripInstanceId == tripInstanceId);
                    if (userTripInstance == null)
                    {
                        // If an instance does not exist, it is probably because the user is not attending it. Create one anyways with Attending = false
                        userTripInstance = UserTripInstance.Create(user, tripInstanceId, tripId);
                        userTripInstance.Attending = false;
                        userTrip.Instances.Add(userTripInstance);
                        context.UserTripInstances.Add(userTripInstance);
                        changed = true;
                    }
                    if (chkAttending.Checked == true)
                    {
                        if (userTripInstance.Attending != true)
                        {
                            // User wants to attend
                            if (userTripInstance.ConfirmTime == null)
                                userTripInstance.ConfirmTime = DateTime.Now;
                            userTripInstance.Attending = true;
                            if (user.CommuteMethod == CommuteMethod.NeedRide)
                            {
                                var tripInstance = userTrip.Trip.Instances.FirstOrDefault(ti => ti.Id == tripInstanceId);
                                if (tripInstance.DriversPicked)
                                {
                                    // Drivers have already been picked. Make sure there is enough room for this user.
                                    // Ensure all user trip instances are loaded from the db (needed for tripInstance.GetRequiredSeats())
                                    context.UserTripInstances.Where(uti => uti.TripInstanceId == tripInstanceId).ToList();
                                    var requiredSeats = tripInstance.GetRequiredSeats();
                                    var availableSeats = tripInstance.GetAvailableSeats();
                                    if (requiredSeats > availableSeats)
                                    {
                                        userTripInstance.Attending = false;
                                        userTripInstance.NoRoom = true;
                                        ErrorMessage = "You cannot attend because there are not enough seats. You have been added to the waiting list.";
                                    }
                                }
                            }
                        }
                        userTripInstance.CommuteMethod = (CommuteMethod)Enum.Parse(typeof(CommuteMethod), rblCommuteMethod.SelectedValue);
                        userTripInstance.CanDriveIfNeeded = chkCanDriveIfNeeded.Checked;
                        userTripInstance.Seats = (int?)nudSeats.Value ?? 0;
                        changed = true;
                    }
                    else if (chkAttending.Checked == false && userTripInstance.Attending != false)
                    {
                        userTripInstance.Attending = false;
                        userTripInstance.ConfirmTime = null;
                        changed = true;
                    }
                    if (btnDetail.DetailsVisible)
                        visibleDetailId = tripInstanceId;
                    // TODO: if user checks "I can drive" without checking chkAttending, automatically add a UserTripInstance / set Attending to true.
                }
            }
            if (changed)
            {
                context.SaveChanges();
                if (string.IsNullOrEmpty(ErrorMessage))
                    SuccessMessage = "Changes saved.";
            }
        }

        int visibleDetailId;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Populate the values in the form controls
            repTrips.DataSource = userTrips;
            repTrips.DataBind();
            lblEmptyTrips.Visible = userTrips.Count == 0;
        }

        protected void repTrips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var userTrip = e.Item.DataItem as UserTrip;
            if (userTrip == null)
                return;
            var item = e.Item;
            var hdnTripId = (HiddenField)item.FindControl("hdnTripId");
            var repTripInstances = (Repeater)item.FindControl("repTripInstances");
            hdnTripId.Value = userTrip.TripId.ToString();
            // Trip.Instances was manually populated in Page_Load and only contains the next instance for each recurrence
            repTripInstances.DataSource = userTrip.Trip.Instances.OrderBy(ti => ti.Date).ToList();
            repTripInstances.DataBind();
        }

        protected void repTripInstances_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var instance = e.Item.DataItem as TripInstance;
            if (instance == null)
                return;
            var item = e.Item;
            var hdnTripInstanceId = (HiddenField)item.FindControl("hdnTripInstanceId");
            var lblInstance = (Label)item.FindControl("lblInstance");
            var chkAttending = (TriStateCheckBox)item.FindControl("chkAttending");
            var btnDetail = (DetailButton)item.FindControl("btnDetail");
            var rblCommuteMethod = (RadioButtonList)btnDetail.FindControl("rblCommuteMethod");
            var chkCanDriveIfNeeded = (CheckBox)btnDetail.FindControl("chkCanDriveIfNeeded");
            var nudSeats = (NumericUpDown)btnDetail.FindControl("nudSeats");
            var lblSeatCount = (Label)btnDetail.FindControl("lblSeatCount");
            var lblAttendanceCount = (Label)btnDetail.FindControl("lblAttendanceCount");
            var lblAttendees = (Label)btnDetail.FindControl("lblAttendees");
            hdnTripInstanceId.Value = instance.Id.ToString();
            lblInstance.Text = instance.ToString();
            var userTrip = userTrips.FirstOrDefault(ut => ut.TripId == instance.TripId);
            if (userTrip == null)
                return;
            var userTripInstance = userTrip.Instances.FirstOrDefault(uti => uti.TripInstanceId == instance.Id);
            chkAttending.Checked = userTripInstance != null ? userTripInstance.Attending : false;
            bool attending = userTripInstance != null && userTripInstance.Attending == true;
            if (attending)
                rblCommuteMethod.SelectedValue = userTripInstance.CommuteMethod.ToString();
            chkCanDriveIfNeeded.Checked = userTripInstance != null && userTripInstance.CanDriveIfNeeded;
            nudSeats.Value = userTripInstance != null ? (decimal?)userTripInstance.Seats : null;

            using (var context = ApplicationDbContext.Create())
            {
                // Reload the trip instance with the required eager loading. (Maybe there is a more efficient way to do this. The current method uses a lot of DB connections/queries)
                var tripInstance = context.GetTripInstanceById(instance.Id);
                int availableSeats = tripInstance.GetAvailableSeats();
                lblSeatCount.Text = availableSeats.ToString();
                int requiredSeats = tripInstance.GetRequiredSeats();
                lblAttendanceCount.Text = requiredSeats.ToString();
                lblSeatCount.CssClass = (availableSeats < requiredSeats) ? "text-danger" : "";
                lblAttendees.Text = tripInstance.GetStatusReport().Replace("\n", "<br />");
            }

            if (visibleDetailId > 0 && visibleDetailId == instance.Id)
                btnDetail.DetailsVisible = true;

            var script =
                "registerPageLoad(function() {\n" +
                "    var prevIsDriver = $('[id^=" + rblCommuteMethod.ClientID + "_1]').prop('checked');\n" +
                "    function setSeatsEnabled() {\n" +
                "        var isAttending = $('#" + chkAttending.ClientID + "').prop('checked') || $('#" + chkAttending.ClientID + " input[type=checkbox]').prop('checked');\n" +
                "        var isDriver = $('[id^=" + rblCommuteMethod.ClientID + "_1]').prop('checked');\n" +
                "        var chkCanDriveIfNeeded = $('#" + chkCanDriveIfNeeded.ClientID + "');\n" +
                "        chkCanDriveIfNeeded.prop('disabled', !isAttending || isDriver);\n" +
                "        if (isDriver != prevIsDriver) {\n" +
                "            chkCanDriveIfNeeded.prop('checked', isDriver);\n" +
                "            prevIsDriver = isDriver;\n" +
                "        }\n" +
                "        var canDrive = chkCanDriveIfNeeded.prop('checked');\n" +
                "        $('[id^=" + rblCommuteMethod.ClientID + "]').prop('disabled', !isAttending);\n" +
                "        $('#" + nudSeats.ClientID + "').prop('disabled', !isAttending || (!isDriver && !canDrive));\n" +
                "    }\n" +
                "    $('#" + chkCanDriveIfNeeded.ClientID + ", [id^=" + rblCommuteMethod.ClientID + "_], #" + chkAttending.ClientID + "').change(function() {\n" +
                "        setSeatsEnabled();\n" +
                "    });\n" +
                "    setSeatsEnabled();\n" +
                "});";
            ScriptManager.RegisterStartupScript(this, GetType(), item.UniqueID, script, true);
        }

        [WebMethod]
        public static string Version()
        {
            return App.Version.ToString();
        }
    }
}