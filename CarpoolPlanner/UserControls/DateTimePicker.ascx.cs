using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarpoolPlanner.UserControls
{
    [ValidationProperty("Value")]
    public partial class DateTimePicker : System.Web.UI.UserControl
    {
        public DateTimePicker()
        {
            IsTimeVisible = true;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.PreLoad += Page_PreLoad;
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            DateTime parsedValue;
            if (DateTime.TryParse(txtDate.Text, out parsedValue))
            {
                if (nudHour.Value.HasValue)
                    parsedValue = parsedValue.AddHours((double)nudHour.Value.Value);
                if (nudMinute.Value.HasValue)
                    parsedValue = parsedValue.AddMinutes((double)nudMinute.Value.Value);
                Value = parsedValue;
            }
            else
            {
                Value = null;
            }
            IsTimeVisible = Time.Visible;
        }

        public DateTime? Value { get; set; }

        public bool IsRequired { get; set; }

        protected override void OnPreRender(EventArgs e)
        {
            if (Value.HasValue)
            {
                txtDate.Text = Value.Value.ToString(App.DateFormat);
                nudHour.Value = Value.Value.Hour;
                nudMinute.Value = Value.Value.Minute;
            }
            else
            {
                txtDate.Text = string.Empty;
                nudHour.Value = 0;
                nudMinute.Value = 0;
            }
            if (IsRequired)
                txtDate.Attributes["required"] = "required";
            else
                txtDate.Attributes.Remove("required");
            Time.Visible = IsTimeVisible;
        }

        public bool IsTimeVisible { get; set; }
    }
}