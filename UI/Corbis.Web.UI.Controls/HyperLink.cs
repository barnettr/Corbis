using System;
using System.Globalization;
using System.IO;
using System.Web;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI.Controls
{
    public class HyperLink : System.Web.UI.WebControls.HyperLink
    {
        #region Fields

        private bool childWindow = false;
        private int childWindowWidth = 800;
        private int childWindowHeight = 600;
        private string cultureName = string.Empty;
        private const string defaultCultureName = "en-US";
        private bool localize = false;
        private string navigateUrl = string.Empty;
        private bool isThemeImage = false;
        private bool enableViewState = false;
        private bool resizable = false;
        #endregion

        #region Properties

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }

        public bool Localize
        {
            get { return localize; }
            set { localize = value; }
        }

        public bool Resizable
        {
            get { return resizable; }
            set { resizable = value; }
        }

        public bool IsThemeImage
        {
            get
            {
                return isThemeImage;
            }
            set
            {
                isThemeImage = value;
            }
        }

        public HyperLink()
        {
            //this.DataBinding += new EventHandler(BindNavigateUrl);
        }

        public bool ChildWindow
        {
            get { return childWindow; }
            set { childWindow = value; }
        }

        public int ChildWindowWidth
        {
            get { return childWindowWidth; }
            set { childWindowWidth = value; }
        }

        public int ChildWindowHeight
        {
            get { return childWindowHeight; }
            set { childWindowHeight = value; }
        }

        #endregion


        #region Methods

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack && ChildWindow == true)
            {
                this.NavigateUrl = "javascript:NewWindow('" + this.NavigateUrl + "'," + this.ChildWindowWidth + "," + this.ChildWindowHeight + ",'" + Resizable.ToString() + "')";
            }
        }

        private string commandArgument;
        public string CommandArgument
        {
            get { return commandArgument; }
            set { commandArgument = value; }
        }
	


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(ImageUrl))
                {
                    if (isThemeImage)
                    {
                        ImageUrl = "~/App_Themes/" + Page.Theme + ImageUrl;
                    }
                    else
                    {
                        ImageUrl = "~" + ImageUrl;
                    }
                }

                if (localize)
                {
                    cultureName = Language.CurrentLanguage.LanguageCode;
                    string filename = HttpContext.Current.Request.MapPath(ImageUrl.Replace(defaultCultureName, cultureName));
                    if (File.Exists(filename))
                    {
                        ImageUrl = ImageUrl.Replace(defaultCultureName, cultureName);
                    }
                    else
                    {
                        Visible = false;
                    }
                }
            }
        }

        #endregion
    }
}
