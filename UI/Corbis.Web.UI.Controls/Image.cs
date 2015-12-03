using System;
using System.Globalization;
using System.IO;
using System.Web;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI.Controls
{
    public class Image : System.Web.UI.WebControls.Image
    {
        private bool enableViewState = false;
        private string cultureName = string.Empty;
        private const string defaultCultureName = "en-US";
        private bool isAbsolute = false;
        private string overImageUrl = string.Empty;
        private string downImageUrl = string.Empty;
        private string upImageUrl = string.Empty;
        private bool isThemeImage = false;
        private bool localize = false;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }

        public bool IsAbsolute
        {
            get { return isAbsolute; }
            set { isAbsolute = value; }
        }

        public string OverImageUrl
        {
            get { return overImageUrl; }
            set { overImageUrl = value; }
        }

        public string DownImageUrl
        {
            get { return downImageUrl; }
            set { downImageUrl = value; }
        }

        public string UpImageUrl
        {
            get { return upImageUrl; }
            set { upImageUrl = value; }
        }

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

        public Image()
        {

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (isAbsolute)
            {
                return;
            }
            if (!Page.IsPostBack)
            {
                if (isThemeImage)
                {
                    ImageUrl = "~/App_Themes/" + Page.Theme + ImageUrl;
                    if (!string.IsNullOrEmpty(OverImageUrl))
                    {
                        OverImageUrl = "~/App_Themes/" + Page.Theme + OverImageUrl;
                    }
                    if (!string.IsNullOrEmpty(DownImageUrl))
                    {
                        DownImageUrl = "~/App_Themes/" + Page.Theme + DownImageUrl;
                    }
                    if (!string.IsNullOrEmpty(UpImageUrl))
                    {
                        UpImageUrl = "~/App_Themes/" + Page.Theme + UpImageUrl;
                    }
                }
                else
                {
                    ImageUrl = "~" + ImageUrl;
                    if (!string.IsNullOrEmpty(OverImageUrl))
                    {
                        OverImageUrl = "~" + OverImageUrl;
                    }
                    if (!string.IsNullOrEmpty(DownImageUrl))
                    {
                        DownImageUrl = "~" + DownImageUrl;
                    }
                    if (!string.IsNullOrEmpty(UpImageUrl))
                    {
                        UpImageUrl = "~" + UpImageUrl;
                    }
                }

                if (localize)
                {
                    cultureName = Language.CurrentLanguage.LanguageCode;
                    string filename = HttpContext.Current.Request.MapPath(ImageUrl.Replace(defaultCultureName, cultureName));
                    if (File.Exists(filename))
                    {
                        ImageUrl = ImageUrl.Replace(defaultCultureName, cultureName);
                        if (!string.IsNullOrEmpty(OverImageUrl))
                        {
                            OverImageUrl = OverImageUrl.Replace(defaultCultureName, cultureName);
                        }
                        if (!string.IsNullOrEmpty(DownImageUrl))
                        {
                            DownImageUrl = DownImageUrl.Replace(defaultCultureName, cultureName);
                        }
                        if (!string.IsNullOrEmpty(UpImageUrl))
                        {
                            UpImageUrl = UpImageUrl.Replace(defaultCultureName, cultureName);
                        }
                    }
                    else
                    {
                        Visible = false;
                    }
                }

                if (!string.IsNullOrEmpty(OverImageUrl))
                {
                    Attributes.Add("onmouseover", string.Format("SetImage('{0}', '{1}');", ClientID, ResolveClientUrl(OverImageUrl)));
                    Attributes.Add("onmouseout", string.Format("SetImage('{0}', '{1}');", ClientID, ResolveClientUrl(ImageUrl)));
                }

                if (!string.IsNullOrEmpty(DownImageUrl))
                {
                    Attributes.Add("onmousedown", string.Format("SetImage('{0}', '{1}');", ClientID, ResolveClientUrl(DownImageUrl)));
                }

                if (!string.IsNullOrEmpty(UpImageUrl))
                {
                    Attributes.Add("onmouseup", string.Format("SetImage('{0}', '{1}');", ClientID, ResolveClientUrl(UpImageUrl)));
                }
            }
        }
    }
}
