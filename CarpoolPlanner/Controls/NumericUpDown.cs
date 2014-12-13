using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarpoolPlanner.Controls
{
    [ValidationProperty("Value")]
    public class NumericUpDown : WebControl, IPostBackDataHandler
    {
        public event EventHandler ValueChanged;

        protected override void OnInit(EventArgs e)
        {
            // Tell that page that this control requires ControlState to work.
            // Otherwise, LoadControlState and SaveControlState would not get called!
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        private decimal? value;

        public decimal? Value
        {
            get { return value; }
            set
            {
                if (value.HasValue)
                {
                    if (Minimum.HasValue && value.Value < Minimum)
                        value = Minimum;
                    else if (Maximum.HasValue && value > Maximum)
                        value = Maximum;
                }
                this.value = value;
            }
        }

        public decimal? Minimum
        {
            get { return (decimal?)ViewState["Minimum"]; }
            set
            {
                ViewState["Minimum"] = value;
                if (Value.HasValue && value.HasValue && Value < value)
                    Value = value;
            }
        }

        public decimal? Maximum
        {
            get { return (decimal?)ViewState["Maximum"]; }
            set
            {
                ViewState["Maximum"] = value;
                if (Value.HasValue && value.HasValue && Value > value)
                    Value = value;
            }
        }

        public int Step
        {
            get { return (int?)ViewState["Step"] ?? 1; }
            set { ViewState["Step"] = value; }
        }
        /*
        public Unit Width
        {
            get { return (Unit?)ViewState["Width"] ?? 0; }
            set { ViewState["Width"] = value; }
        }

        public Unit Height
        {
            get { return (Unit?)ViewState["Height"] ?? 0; }
            set { ViewState["Height"] = value; }
        }

        public bool Enabled
        {
            get { return (bool?)ViewState["Enabled"] ?? true; }
            set { ViewState["Enabled"] = value; }
        }
        public string CssClass
        {
            get { return (string)ViewState["CssClass"]; }
            set { ViewState["CssClass"] = value; }
        }
        */

        public bool AutoPostBack
        {
            get { return (bool?)ViewState["AutoPostBack"] ?? false; }
            set { ViewState["AutoPostBack"] = value; }
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            if (ValueChanged != null)
                ValueChanged(this, e);
        }

        protected override void LoadControlState(object savedState)
        {
            value = savedState as decimal?;
        }

        protected override object SaveControlState()
        {
            // Save Value in control state so we know if it changed on the next post back.
            return value;
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string newValueString = postCollection[postDataKey];
            decimal newValue;
            // The previous value is stored in Value, which was set during LoadControlState.
            decimal? prevValue = Value;
            Value = decimal.TryParse(newValueString, out newValue) ? (decimal?)newValue : null;
            return Value != prevValue;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            OnValueChanged(new EventArgs());
        }
        
        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "number");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            if (Minimum.HasValue)
                writer.AddAttribute("min", Minimum.Value.ToString());
            if (Maximum.HasValue)
                writer.AddAttribute("max", Maximum.Value.ToString());
            if (Step != 1)
                writer.AddAttribute("step", Step.ToString());
            if (Value.HasValue)
                writer.AddAttribute("value", Value.Value.ToString());
            if (!string.IsNullOrEmpty(CssClass))
                writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
            if (!Enabled)
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            foreach (string key in Attributes.Keys)
                writer.AddAttribute(key, Attributes[key]);
            foreach (string key in Attributes.CssStyle.Keys)
                writer.AddStyleAttribute(key, Attributes.CssStyle[key]);
            if (Width.Value > 0.0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, Width.ToString());
            if (Height.Value > 0.0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Height, Height.ToString());
            if (AutoPostBack)
                writer.AddAttribute(HtmlTextWriterAttribute.Onchange, "__doPostBack('" + UniqueID + "', '')");
            writer.RenderBeginTag("input");
            writer.RenderEndTag();
        }
    }
}