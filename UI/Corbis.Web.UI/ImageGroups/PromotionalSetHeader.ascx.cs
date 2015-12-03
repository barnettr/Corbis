using System;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters.ImageGroups;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.ImageGroups
{
    public partial class PromotionalSetHeader : CorbisBaseUserControl, IPromotionalSetHeaderView
    {
        public ImageGroupsPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            presenter = ((IImageGroupsView)Page).Presenter;
            presenter.ImageGroupHeaderView = this;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((int)(ImageMediaSetType.Promotional) == int.Parse(Request.QueryString["typ"]))
            {
                presenter.GetImageGroupHeader();
                this.addalltolightbox.OnClientClick = "ShowAddToLightboxModal('" + Request.QueryString["id"].ToString() +
                                                      "', this, false);return false;";
                this.button.OnClientClick = "ShowAddToLightboxModal('" + Request.QueryString["id"].ToString() +
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
                if (string.IsNullOrEmpty(value))
                {
                    this.recentImage.Visible = false;
                    this.PS.Visible = false;
                    this.ALButton.Visible = false;
                    this.ALButton2.Visible = true;
                    this.recentImageId = string.Empty;
                }
                else
                {
                    recentImageId = value;
                    this.recentImage.Visible = true;
                    this.recentImage.RecentImageId = value;
                    this.ALButton.Visible = true;
                    this.ALButton2.Visible = false;
                }
            }
        }

        private string recentImageURL;
        public string RecentImageURL
        {
            get { return recentImageURL; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.recentImage.Visible = false;
                    this.PS.Visible = false;
                    this.ALButton.Visible = false;
                    this.ALButton2.Visible = true;
                    this.recentImageURL = string.Empty;
                }
                else
                {
                    this.recentImage.Visible = true;
                    this.PS.Visible = true;
                    this.ALButton.Visible = true;
                    this.ALButton2.Visible = false;
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

        #region Implementation of IPromotionalSetHeaderView

        public string Title
        {
            set { title.Text = value; }
        }

        public string Id
        {
            set { id.Text = value; }
        }

        public string ImageCount
        {
            set { imageCount.Text = value; }
        }


        #endregion
    }
}
