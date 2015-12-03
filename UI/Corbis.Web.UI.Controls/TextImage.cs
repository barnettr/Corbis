using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;

namespace Corbis.Web.UI.Controls
{
    public class TextImage : System.Web.UI.UserControl
    {
        HtmlImage image;
        HtmlAnchor imageLink;
        HtmlAnchor hyperLink;
        HtmlAnchor overHyperLink;
        private bool enableViewState = false;
        private string cultureName = string.Empty;
        private const string defaultCultureName = "en-US";
        private string imageUrl = string.Empty;
        private string overImageUrl = string.Empty;
        private string downImageUrl = string.Empty;
        private string upImageUrl = string.Empty;
        private bool isThemeImage = false;
        private bool localize = false;
        private string navigateUrl = string.Empty;
        private string replaceKey = string.Empty;
        private string replaceValue = string.Empty;
        private string overText = string.Empty;
        private string text = string.Empty;

        public override bool EnableViewState
        {
            get { return enableViewState; }
            set { enableViewState = value; }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
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

        public string NavigateUrl
        {
            get { return navigateUrl; }
            set { navigateUrl = value; }
        }

        public string OverText
        {
            get { return overText; }
            set { overText = value; }
        }

        public string ReplaceKey
        {
            get { return replaceKey; }
            set { replaceKey = value; }
        }

        public string ReplaceValue
        {
            get { return replaceValue; }
            set { replaceValue = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public TextImage()
        {
            imageLink = new HtmlAnchor();
            imageLink.ID = "imageLink";
            image = new HtmlImage();
            image.ID = "image";
            imageLink.Controls.Add(image);
            Controls.Add(imageLink);
            hyperLink = new HtmlAnchor();
            hyperLink.ID = "hyperLink";
            Controls.Add(hyperLink);
            overHyperLink = new HtmlAnchor();
            overHyperLink.ID = "overHyperLink";
            Controls.Add(overHyperLink);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!string.IsNullOrEmpty(ReplaceKey))
            {
                Text = Text.Replace(ReplaceKey, ReplaceValue);
            }

            hyperLink.InnerText = Text;
            hyperLink.Attributes.Add("class", "Text");
            overHyperLink.InnerText = overText;
            overHyperLink.Attributes.Add("class", "OverText");
            if (!string.IsNullOrEmpty(NavigateUrl))
            {
                hyperLink.HRef = VirtualPathUtility.ToAbsolute(NavigateUrl);
                imageLink.HRef = VirtualPathUtility.ToAbsolute(NavigateUrl);
                overHyperLink.HRef = VirtualPathUtility.ToAbsolute(NavigateUrl);
            }

            if (!Page.IsPostBack && !string.IsNullOrEmpty(ImageUrl))
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
                    cultureName = CultureInfo.CurrentUICulture.Name;
                    string filename = HttpContext.Current.Request.MapPath("~" + ImageUrl.Replace(defaultCultureName, cultureName));
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
                    image.Attributes.Add("onmouseover", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(OverImageUrl)));
                    image.Attributes.Add("onmouseout", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(ImageUrl)));
                    hyperLink.Attributes.Add("onmouseover", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(OverImageUrl)));
                    hyperLink.Attributes.Add("onmouseout", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(ImageUrl)));
                }

                if (!string.IsNullOrEmpty(OverText))
                {
                    overHyperLink.Style.Add("display", "none");
                    image.Attributes["onmouseover"] += string.Format("Show('{0}');", overHyperLink.ClientID);
                    image.Attributes["onmouseout"] += string.Format("Hide('{0}');", overHyperLink.ClientID);
                    if (!string.IsNullOrEmpty(OverImageUrl))
                    {
                        overHyperLink.Attributes.Add("onmouseover", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(OverImageUrl)));
                    }
                    overHyperLink.Attributes.Add("onmouseout", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(ImageUrl)));
                    overHyperLink.Attributes["onmouseover"] += string.Format("Show('{0}');", overHyperLink.ClientID);
                    overHyperLink.Attributes["onmouseout"] += string.Format("Hide('{0}');", overHyperLink.ClientID);
                }

                if (!string.IsNullOrEmpty(DownImageUrl))
                {
                    image.Attributes.Add("onmousedown", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(DownImageUrl)));
                    hyperLink.Attributes.Add("onmousedown", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(DownImageUrl)));
                }

                if (!string.IsNullOrEmpty(UpImageUrl))
                {
                    image.Attributes.Add("onmouseup", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(UpImageUrl)));
                    hyperLink.Attributes.Add("onmouseup", string.Format("SetImage('{0}', '{1}');", image.ClientID, VirtualPathUtility.ToAbsolute(UpImageUrl)));
                }

                image.Src = VirtualPathUtility.ToAbsolute(ImageUrl);
            }
        }
    }
}
