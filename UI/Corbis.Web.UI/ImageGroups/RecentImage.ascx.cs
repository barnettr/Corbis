using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Corbis.Web.UI.ImageGroups
{
    public partial class recentImage : System.Web.UI.UserControl
    {
        private string recentImageURL = string.Empty;
        private string recentImageId = string.Empty;
        private decimal recentImageRadio = 0;

        public string RecentImageURL
        {
            get { return recentImageURL; }
            set
            {
				string fullURL = String.Format("javascript:EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id={0}&caller=imagegroups');return false;", RecentImageId);
                
                thumbWrap.ImgUrl = value;
                thumbWrap.ToolTip = GetLocalResourceObject("BackImage").ToString();
                thumbWrap.Attributes.Add("onclick", fullURL);
                thumbWrap.Visible = true;
                arrow.Attributes.Add("onclick", fullURL);
                
            }
        }
        public Decimal RecentImageRadio
        {
            set
            {
                Decimal ratio = value;
                thumbWrap.Ratio = ratio;
                Corbis.Web.UI.Controls.Image img = (Corbis.Web.UI.Controls.Image)thumbWrap.FindControl(thumbWrap.ImageID);
                if (ratio > 1)
                {
                    img.Width = Unit.Parse("50px");
                    //img.Attributes.CssStyle.Add("margin-top", "10px");//Commented becoz margin-top is taken care by OnPreRender in CenteredImageContainer.cs                     
                }
                else
                {
                    img.Height = Unit.Parse("50px");
                    //img.Attributes.CssStyle.Add("margin-left", "10px");// Taken care by styles in css

                }
            }
            get
            {
                return recentImageRadio;
            }
        }
        public string RecentImageId
        {
            get { return recentImageId; }
            set { recentImageId = value; }
        }
        public void ChangeStyle(bool addAllLinkVisible)
        {
            this.greyImageContainer.Attributes["class"] = "greyImageContainer " + ((addAllLinkVisible) ? "RFCD" : "RFCDNoAddAllLink");
            this.ImageAndArrowContainer.Attributes["class"] = "imageAndArrowContainer_AddLink";
        }
    }
}