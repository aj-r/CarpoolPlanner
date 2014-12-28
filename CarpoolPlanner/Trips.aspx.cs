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

namespace CarpoolPlanner
{
    public partial class Trips : System.Web.UI.Page
    {
        private List<Trip> trips;
        private List<UserTrip> userTrips;
        private Trip newTrip; 

        protected void Page_Load(object sender, EventArgs e)
        {
            var user = App.CurrentUser;
            using (var context = ApplicationDbContext.Create())
            {
                trips = context.Trips.Include(t => t.Recurrences).ToList();
                userTrips = context.GetUserTrips(App.CurrentUser.Id).Include(ut => ut.Recurrences).ToList();
                if (!IsPostBack)
                {
                    newTrip = new Trip();
                    newTrip.Recurrences.Add(new TripRecurrence());
                }
                else
                {
                    LoadDataFromPage(context);
                }
            }
        }

        public string SuccessMessage { get; set; }

        private void LoadDataFromPage(ApplicationDbContext context)
        {
            bool changed = false;
            var user = App.CurrentUser;
            foreach (RepeaterItem item in repTrips.Items)
            {
                var hdnTripId = (HiddenField)item.FindControl("hdnTripId");
                var chkAttending = (CheckBox)item.FindControl("chkAttending");
                var repTripRecurrences = (Repeater)item.FindControl("repTripRecurrences");
                var tripId = int.Parse(hdnTripId.Value);
                // Add/remove the UserTrip if necessary
                var userTrip = userTrips.FirstOrDefault(ut => ut.TripId == tripId);
                if (chkAttending.Checked && userTrip == null)
                {
                    userTrip = new UserTrip { UserId = user.Id, TripId = tripId };
                    userTrips.Add(userTrip);
                    context.UserTrips.Add(userTrip);
                    changed = true;
                }
                else if (!chkAttending.Checked && userTrip != null)
                {
                    userTrips.Remove(userTrip);
                    context.UserTrips.Remove(userTrip);
                    changed = true;
                    // TODO: automatically stop attending the next instances for all recurrences?
                }
                if (chkAttending.Checked)
                {
                    foreach (RepeaterItem subItem in repTripRecurrences.Items)
                    {
                        var hdnTripRecurrenceId = (HiddenField)subItem.FindControl("hdnTripRecurrenceId");
                        var chkAttendingRecurrence = (CheckBox)subItem.FindControl("chkAttendingRecurrence");
                        var tripRecurrenceId = int.Parse(hdnTripRecurrenceId.Value);
                        var userTripRecurrence = userTrip.Recurrences.FirstOrDefault(ut => ut.TripRecurrenceId == tripRecurrenceId);
                        if (chkAttendingRecurrence.Checked && userTripRecurrence == null)
                        {
                            userTripRecurrence = new UserTripRecurrence { UserId = user.Id, TripRecurrenceId = tripRecurrenceId, TripId = tripId };
                            userTrip.Recurrences.Add(userTripRecurrence);
                            context.UserTripRecurrences.Add(userTripRecurrence);
                            // Automatically set attendance status to indeterminate.
                            var tripRecurrence = context.TripRecurrences.Find(tripRecurrenceId);
                            var userTripInstance = context.GetNextUserTripInstance(tripRecurrence, App.CurrentUser);
                            if (userTripInstance != null)
                                userTripInstance.Attending = null;
                            changed = true;
                        }
                        else if (!chkAttendingRecurrence.Checked && userTripRecurrence != null)
                        {
                            userTrip.Recurrences.Remove(userTripRecurrence);
                            context.UserTripRecurrences.Remove(userTripRecurrence);
                            // Automatically stop attending the next instance of this recurrence.
                            var tripRecurrence = context.TripRecurrences.Find(tripRecurrenceId);
                            var userTripInstance = context.GetNextUserTripInstance(tripRecurrence, App.CurrentUser);
                            if (userTripInstance != null)
                                userTripInstance.Attending = false;
                            changed = true;
                        }
                    }
                }
            }
            if (changed)
            {
                context.SaveChanges();
                SuccessMessage = "Preferences saved.";
            }

            newTrip = new Trip
            {
                Name = txtNewTripName.Text,
                Location = txtLocation.Text
            };
            foreach (RepeaterItem item in repNewTripRecurrences.Items)
            {
                newTrip.Recurrences.Add(GetTripRecurrence(item));
            }
        }

        private static TripRecurrence GetTripRecurrence(RepeaterItem item)
        {
            var nudEvery = (NumericUpDown)item.FindControl("nudEvery");
            var ddlRecurrenceType = (DropDownList)item.FindControl("ddlRecurrenceType");
            var dtpStartDate = (DateTimePicker)item.FindControl("dtpStartDate");
            var dtpEndDate = (DateTimePicker)item.FindControl("dtpEndDate");
            var recurrence = new TripRecurrence
            {
                Every = (int)(nudEvery.Value ?? 1),
                Type = (RecurrenceType)Enum.Parse(typeof(RecurrenceType), ddlRecurrenceType.SelectedValue),
                Start = dtpStartDate.Value,
                End = dtpEndDate.Value
            };
            return recurrence;
        }

        protected void btnCreateTrip_Click(object sender, EventArgs e)
        {
            using (var context = ApplicationDbContext.Create())
            {
                context.Trips.Add(newTrip);
                context.SaveChanges();
            }
            trips.Add(newTrip);
            newTrip = new Trip();
            newTrip.Recurrences.Add(new TripRecurrence());
        }

        protected void btnAddTripRecurrence_Click(object sender, EventArgs e)
        {
            newTrip.Recurrences.Add(new TripRecurrence());
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            // Populate the values in the form controls
            repTrips.DataSource = trips;
            repTrips.DataBind();
            lblEmptyTrips.Visible = trips.Count == 0;

            txtNewTripName.Text = newTrip.Name;
            txtLocation.Text = newTrip.Location;
            repNewTripRecurrences.DataSource = newTrip.Recurrences;
            repNewTripRecurrences.DataBind();
        }

        protected void repTrips_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var trip = e.Item.DataItem as Trip;
            if (trip == null)
                return;
            var item = e.Item;
            var hdnTripId = (HiddenField)item.FindControl("hdnTripId");
            var chkAttending = (CheckBox)item.FindControl("chkAttending");
            var repTripRecurrences = (Repeater)item.FindControl("repTripRecurrences");
            hdnTripId.Value = trip.Id.ToString();
            repTripRecurrences.DataSource = trip.Recurrences.OrderBy(tr => tr.Start).ToList();
            repTripRecurrences.DataBind();
            var userTrip = userTrips.FirstOrDefault(ut => ut.TripId == trip.Id);
            chkAttending.Checked = userTrip != null;
        }

        protected void repTripRecurrences_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var recurrence = e.Item.DataItem as TripRecurrence;
            if (recurrence == null)
                return;
            var item = e.Item;
            var hdnTripRecurrenceId = (HiddenField)item.FindControl("hdnTripRecurrenceId");
            var lblRecurrence = (Label)item.FindControl("lblRecurrence");
            var chkAttendingRecurrence = (CheckBox)item.FindControl("chkAttendingRecurrence");
            hdnTripRecurrenceId.Value = recurrence.Id.ToString();
            lblRecurrence.Text = recurrence.ToString();
            var userTrip = userTrips.FirstOrDefault(ut => ut.TripId == recurrence.TripId);
            if (userTrip == null)
                return;
            var userTripRecurrence = userTrip.Recurrences.FirstOrDefault(utr => utr.TripRecurrenceId == recurrence.Id);
            chkAttendingRecurrence.Checked = userTripRecurrence != null;
        }

        protected void repNewTripRecurrences_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var recurrence = e.Item.DataItem as TripRecurrence;
            if (recurrence == null)
                return;
            var item = e.Item;
            var nudEvery = (NumericUpDown)item.FindControl("nudEvery");
            var ddlRecurrenceType = (DropDownList)item.FindControl("ddlRecurrenceType");
            var dtpStartDate = (DateTimePicker)item.FindControl("dtpStartDate");
            var dtpEndDate = (DateTimePicker)item.FindControl("dtpEndDate");
            nudEvery.Value = recurrence.Every;
            ddlRecurrenceType.SelectedValue = recurrence.Type.ToString();
            dtpStartDate.Value = recurrence.Start;
            dtpEndDate.Value = recurrence.End;
        }
    }
}