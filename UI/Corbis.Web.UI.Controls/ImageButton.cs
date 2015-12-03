using System;
using System.Globalization;
using System.IO;
using System.Web;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI.Controls
{
    public class ImageButton : System.Web.UI.WebControls.ImageButton
    {
        private bool enableViewState = false;
        private string cultureName = string.Empty;
        private const string defaultCultureName = "en-US";
        private bool isThemeImage = false;
        private bool localize = false;

        public bool IsThemeImage
        {
            get { return isThemeImage; }
            set { isThemeImage = value; }
        }

        public bool Localize
        {
            get { return localize; }
            set { localize = value; }
        }

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack)
            {
                if (isThemeImage)
                {
                    ImageUrl = "~/App_Themes/" + Page.Theme + ImageUrl;
                }
                else
                {
                    ImageUrl = "~" + ImageUrl;
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
    }
}
