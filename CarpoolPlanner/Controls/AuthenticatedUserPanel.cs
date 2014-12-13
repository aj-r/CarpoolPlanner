using System;
using System.Web.UI;

namespace CarpoolPlanner.Controls
{
    /// <summary>
    /// Provides authorization control over specific sections of a page's content. 
    /// This control is made to behave like the authorization element in web.config.
    /// </summary>
    [ParseChildren(ChildrenAsProperties = true)]
    [ToolboxData("<{0}:AuthenticatedUserPanel runat=\"server\"></{0}:AuthenticatedUserPanel>")]
    public partial class AuthenticatedUserPanel : Control
    {
        public AuthenticatedUserPanel()
        {
            IsAuthenticated = true;
        }

        protected override void OnInit(EventArgs e)
        {
            // Force the template to be instantiated on init so that child controls can be referenced during Page_Load.
            EnsureChildControls();
            base.OnInit(e);
        }

        // This method is called by ASP.NET when the child controls are to be created.
        protected override void CreateChildControls()
        {
            if (ContentTemplate == null)
                return;
            Control container = new Control();
            ContentTemplate.InstantiateIn(container);
            Controls.Add(container);
            container.Visible = IsAccessGranted;
        }

        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Gets whether the current user has access to the contents of the panel or not.
        /// </summary>
        /// <returns>True if the user is in one of the specified roles or usernames, false if not.</returns>
        public bool IsAccessGranted
        {
            get
            {
                return string.IsNullOrEmpty(App.CurrentUserId) != IsAuthenticated;
            }
        }

        private ITemplate _contentTemplate;

        // Using TemplateInstance.Single allows child controls of the ContentTemplate to be exposed to the page that this control is contained in.
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateInstance(TemplateInstance.Single)]
        public ITemplate ContentTemplate
        {
            get
            {
                return _contentTemplate;
            }
            set
            {
                _contentTemplate = value;
            }
        }
    }
}