using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.Controls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:CenteredImageContainer runat=server></{0}:CenteredImageContainer>")]
    public class CenteredImageContainer : CompositeControl
    {
        #region Member Variables
        private Decimal _Ratio;
        private int _ContainerSize;
        private Image _Img = new Image();
        private string _cssClass = "thumbWrap";
        #endregion

        #region Public Properties
        public string ImgUrl
        {
            get { return _Img.ImageUrl; }
            set { _Img.ImageUrl = value; }
        }
        public string AltText
        {
            get { return _Img.AlternateText; }
            set { _Img.AlternateText = value; }
        }
        public string ImageID
        {
            get { return _Img.ID; }
            set { _Img.ID = value; }
        }
        public Decimal Ratio
        {
            get { return _Ratio; }
            set { _Ratio = value; }
        }
        public int Size
        {
            get { return _ContainerSize; }
            set { _ContainerSize = value; }
        }
        public bool IsAbsolute
        {
            get { return _Img.IsAbsolute; }
            set { _Img.IsAbsolute = value; }
        }

        public String ImgCssClass
        {
            get { return _cssClass; }
            set { _cssClass = value; }
        }

        public string OverImageUrl
        {
            get { return _Img.OverImageUrl; }
            set { _Img.OverImageUrl = value; }
        }

        public string DownImageUrl
        {
            get { return _Img.DownImageUrl; }
            set { _Img.DownImageUrl = value; }
        }

        public string UpImageUrl
        {
            get { return _Img.UpImageUrl; }
            set { _Img.UpImageUrl = value; }
        }

        public bool IsThemeImage
        {
            get { return _Img.IsThemeImage; }
            set { _Img.IsThemeImage = value; }
        }
        #endregion

        #region Initialization / Control Rendering
        protected override void CreateChildControls()
        {
            this.Controls.Add(_Img);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Ratio > 1)
            {
                int height = (Size - (int)Math.Round(Size / Ratio)) >> 1;
                if (ImageID != "enlargement")
                {
                    _Img.Style.Add("margin-top", height.ToString() + "px");
                    _Img.Style.Add("width", Size.ToString() + "px");
                }
            }
            else
            {
                _Img.Style.Add("height", Size.ToString() + "px");
            }
            this.CssClass = ImgCssClass;
            
            // See Bug 17737
            // If you have alt text without a title attribute, IE will display the alt text as a tooltip
            // if you put in an empty title attribute, IE will no longer display the alt text. :)
            _Img.Attributes["title"] = "";
        }

        protected override System.Web.UI.HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }
        #endregion
    }
}
