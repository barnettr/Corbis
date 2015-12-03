using System;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters.ImageGroups;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.ImageGroups
{
    public partial class SamePhotoshootHeader : CorbisBaseUserControl, ISamePhotoshootHeaderView
    {
        public ImageGroupsPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            presenter = ((IImageGroupsView)Page).Presenter;
            presenter.ImageGroupHeaderView = this;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((int)(ImageMediaSetType.PhotoShoot) == int.Parse(Request.QueryString["typ"]))
            {
                presenter.GetImageGroupHeader();
                this.addalltolightbox.OnClientClick = "ShowAddToLightboxModal('" + Request.QueryString["id"].ToString() +
                                     "', this, false);return false;";
 
            }
           
        }

        public string ImageGroupName
        {
            get { return Request.QueryString["typ"]; }
        }

        public string ImageGroupId
        {
            get { return Request.QueryString["id"]; }
        }

        private string recentImageId;
        public string RecentImageId
        {
            get { return recentImageId; }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    this.recentImage.Visible = false;
                    this.PS.Visible = false;
                    this.PSButton2.Attributes.Add("class", "PSButton2");
                    this.recentImageId = string.Empty;
                }
                else
                {
                    recentImageId = value;
                    this.recentImage.Visible = true;
                    this.recentImage.RecentImageId = value;
                    this.PSButton2.Attributes.Add("class", "PSButton");
                }
            }
        }

        private string recentImageURL;
        public string RecentImageURL
        {
            get { return recentImageURL; }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    this.recentImage.Visible = false;
                    this.PS.Visible = false;
                    this.PSButton2.Attributes.Add("class", "PSButton2");
                    this.recentImageURL = string.Empty;
                }
                else
                {
                    this.recentImage.Visible = true;
                    this.PS.Visible = true;
                    this.PSButton2.Attributes.Add("class", "PSButton");
                    this.recentImage.RecentImageURL = value;
                }
            }
        }
        public Decimal RecentImageRadio
        {
            set
            {
                Decimal ratio = value;
                recentImage.RecentImageRadio = value;
            }
        }
        #region Implementation of ISamePhotoshootHeaderView


        public string Id
        {
            set { id.Text = value; }
        }

        public string ImageCount
        {
            set { imageCount.Text = value; }
        }

        public string Photographer
        {
            set { photographer.Text = value; }
        }

        #endregion
    }
}
