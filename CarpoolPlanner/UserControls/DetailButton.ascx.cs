using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CarpoolPlanner.UserControls
{
    public partial class DetailButton : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            EnsureChildControls();
            Page.PreLoad += Page_PreLoad;
            base.OnInit(e);
        }

        private void Page_PreLoad(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                DetailsVisible = hdnVisible.Value == "true";
            }
        }

        protected override void CreateChildControls()
        {
            if (DetailTemplate == null)
                return;
            DetailTemplate.InstantiateIn(ph);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (DetailsVisible)
            {
                details.Style["display"] = "block";
                hdnVisible.Value = "true";
            }
            else
            {
                details.Style["display"] = "none";
                hdnVisible.Value = "false";
            }
            base.OnPreRender(e);
        }

        public bool DetailsVisible
        {
            get { return (bool?)ViewState["DetailsVisible"] ?? false; }
            set { ViewState["DetailsVisible"] = value; }
        }

        public string PopupClientId
        {
            get { return details.ClientID; }
        }

        private ITemplate _detailTemplate;

        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate DetailTemplate
        {
            get
            {
                return _detailTemplate;
            }
            set
            {
                _detailTemplate = value;
            }
        }
    }
}