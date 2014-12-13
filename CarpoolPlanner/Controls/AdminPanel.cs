using System;
using System.Web.UI;

namespace CarpoolPlanner.Controls
{
    /// <summary>
    /// Provides authorization control over specific sections of a page's content. 
    /// This control is made to behave like the authorization element in web.config.
    /// </summary>
    [ParseChildren(ChildrenAsProperties = true)]
    [ToolboxData("<{0}:AdminPanel runat=\"server\"></{0}:AdminPanel>")]
    public partial class AdminPanel : Control
    {
        public AdminPanel()
        {
            IsAdmin = true;
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

        /// <summary>
        /// Gets or sets who can see the contents of this panel (admins or non-admins)
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Gets whether the current user has access to the contents of the panel or not.
        /// </summary>
        /// <returns>True if the user is in one of the specified roles or usernames, false if not.</returns>
        public bool IsAccessGranted
        {
            get
            {
                var user = App.CurrentUser;
                return (user != null && user.IsAdmin) == IsAdmin;
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