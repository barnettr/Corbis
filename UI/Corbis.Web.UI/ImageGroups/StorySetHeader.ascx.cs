using System;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters.ImageGroups;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.ImageGroups
{
    public partial class StorySetHeader : CorbisBaseUserControl, IStorySetHeaderView
    {
        public ImageGroupsPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            presenter = ((IImageGroupsView)Page).Presenter;
            presenter.ImageGroupHeaderView = this;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((int)(ImageMediaSetType.StorySet) == int.Parse(Request.QueryString["typ"]))
            {
                presenter.GetImageGroupHeader();
                this.addalltolightbox.OnClientClick = "ShowAddToLightboxModal('" + Request.QueryString["id"].ToString() +
                                                      "', this, false);return false;";
                if (string.IsNullOrEmpty(date.Text) && string.IsNullOrEmpty(location.Text))
                {
                    dateLocation.Visible = false;
                }
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
                    this.RFCD.Visible = false;
                    this.pullDownButton.Attributes.Add("class", "pullDownButtonStory");
                    this.recentImageId = string.Empty;
                }
                else
                {
                    recentImageId = value;
                    this.recentImage.RecentImageId = value;
                    this.recentImage.Visible = true;
                    this.pullDownButton.Attributes.Add("class", "pullDownButtonStory2");
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
                    this.RFCD.Visible = false;
                    this.pullDownButton.Attributes.Add("class", "pullDownButtonStory");
                    this.recentImageURL = string.Empty;
                }
                else
                {
                    this.recentImage.Visible = true;
                    this.RFCD.Visible = true;
                    this.pullDownButton.Attributes.Add("class", "pullDownButtonStory2");
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
        protected void alltolightbox_Click(object sender, EventArgs e)
        {
           
        }

        #region Implementation of IStorySetHeaderView

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

        public string Date
        {
            set {                
                     date.Text = value.Trim();
                }
        }

        public string Location
        {
            set
            {
                if (! string.IsNullOrEmpty(value))
                {
                    location.Text = ", " + value;
                }
            }
        }

        public string Photographer
        {
            set { photographer.Text = value; }
        }

        #endregion
    }
}
