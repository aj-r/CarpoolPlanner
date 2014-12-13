using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarpoolPlanner.Controls
{
    /// <summary>
    /// A check box with a third indeterminate state.
    /// </summary>
    public class TriStateCheckBox : WebControl, IPostBackDataHandler
    {
        public event EventHandler CheckedChanged;

        protected override void OnInit(EventArgs e)
        {
            // Tell that page that this control requires ControlState to work.
            // Otherwise, LoadControlState and SaveControlState would not get called!
            Page.RegisterRequiresControlState(this);
            Page.RegisterRequiresPostBack(this);
            base.OnInit(e);
        }

        private bool? _checked;

        public bool? Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                RegisterIndeterminate();
            }
        }

        public string OnClientCheckedChanged
        {
            get { return (string)ViewState["OnClientCheckedChanged"]; }
            set
            {
                ViewState["OnClientCheckedChanged"] = value;
                RegisterOnChange();
            }
        }

        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (CheckedChanged != null)
                CheckedChanged(this, e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            var sm = new ScriptManager();
            RegisterOnChange();
            RegisterIndeterminate();
        }

        private void RegisterOnChange()
        {
            if (Page == null) // We can't register a script until the control has been added to the page.
                return;
            string script =
                "$('#" + ClientID + "').change(function(e) {" +
                    "$('#" + ClientID + " input[type=checkbox]').prop('indeterminate', false);" +
                    "$('#" + ClientID + "_indeterminate').val('false');" +
                    (OnClientCheckedChanged ?? "") + 
                " });";
            ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_change", script, true);
        }

        private void RegisterIndeterminate()
        {
            if (Page == null)
                return;
            string script = Checked == null ? "$('#" + ClientID + " input[type=checkbox]').prop('indeterminate', true);" : "";
            ScriptManager.RegisterStartupScript(this, GetType(), ClientID + "_indeterminate", script, true);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
            if (Checked == true)
                writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            // Use a hidden field to post back the indeterminate state (since the checkbox indeterminate property won't post back to the server)
            writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID + "_indeterminate");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID + "_indeterminate");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, Checked == null ? "true" : "false");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        protected override void LoadControlState(object savedState)
        {
            _checked = savedState as bool?;
        }

        protected override object SaveControlState()
        {
            // Save value in control state so we know if it changed on the next post back.
            return _checked;
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string newValueString = postCollection[postDataKey];
            string newIndeterminateValueString = postCollection[postDataKey + "_indeterminate"];
            bool? prevValue = _checked;
            if (newValueString == "on")
                _checked = true;
            else if (newIndeterminateValueString == "true")
                _checked = null;
            else
                _checked = false;
            return _checked != prevValue;
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            OnCheckedChanged(new EventArgs());
        }
        
    }
}