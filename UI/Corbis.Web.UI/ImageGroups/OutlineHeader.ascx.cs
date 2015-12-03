using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters.ImageGroups;
using Corbis.Web.Entities;
using Corbis.Web.UI.ViewInterfaces;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.ImageGroups
{
    public partial class OutlineHeader : CorbisBaseUserControl, IOutlineHeaderView
    {
        public ImageGroupsPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            presenter = ((IImageGroupsView)Page).Presenter;
            presenter.ImageGroupHeaderView = this;
            //InitializeComponent();
            //base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        //private void InitializeComponent()
        //{
        //    this.ltCelebrities.SelectedIndexChanged += new System.EventHandler(this.ltCelebrities_selectedIndexChanged);

        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((int)(ImageMediaSetType.OutlineSession) == int.Parse(Request.QueryString["typ"]))
            {
                presenter.GetImageGroupHeader();
                this.addalltolightbox.OnClientClick = "ShowAddToLightboxModal('" + Request.QueryString["id"].ToString() +
                                                      "',this, false);return false;";
                // just hard code listbox name , don't know how to get name instead Clientid. later may find out.
                string targetName = "ctl00$mainContent$OutlineHeader$ltCelebrities";
                if (Request.Form["__EVENTTARGET"] == targetName &&
                    !string.IsNullOrEmpty(Request.Form[targetName]))
                    Response.Redirect("~/Search/searchresults.aspx?q=" + Request.Form[targetName]);
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
                    this.RFCD.Visible = false;
                    this.scrollBoxOL.Attributes.Add("class", "scrollBoxOL");
                    this.recentImageId = string.Empty;
                }
                else
                {
                    this.recentImage.RecentImageId = value;
                    recentImageId = value;
                    this.recentImage.Visible = true;
                    this.recentImage.RecentImageId = value;
                    this.scrollBoxOL.Attributes.Add("class", "scrollBoxOL2");
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
                    this.RFCD.Visible = false;
                    this.scrollBoxOL.Attributes.Add("class", "scrollBoxOL");
                    this.recentImageURL = string.Empty;
                }
                else
                {
                    this.recentImage.Visible = true;
                    this.RFCD.Visible = true;
                    this.scrollBoxOL.Attributes.Add("class", "scrollBoxOL2");
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

        private void ltCelebrities_selectedIndexChanged(object sender, System.EventArgs e)
        {

            //String lt = ltCelebrities.SelectedValue;
            //Response.Redirect("~/Search/searchresults.aspx?q=" + lt);

        }
        #region Implementation of IOutlineHeaderView

        public string ImageCount
        {
            set 
            { 
                imageCount.Text = value;
                if (int.Parse(value) > 0)
                {
                    string images= (String)GetLocalResourceObject("images");
                    imageCount.Text = String.Format("({0}&nbsp;{1})", value, images);                    
                }
                else
                {
                    this.photographedByOL.Visible = false;
                    this.OL_leftButton.Visible = false;
                    this.RFCD.Visible = false;
                    this.scrollBoxOL.Visible = false;
                    this.imageCount.Visible = false;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "resizeOutlineHeader", "resizeOutlineHeader();", true);
                }
               
            }
        }

        public string Photographer
        {
            set { photographer.Text = value; }
        }

        public string DatePublished
        {
            set { datePublished.Text = DateHelper.GetLocalizedDate(value); }
        }

        public string CreditLine
        {
            set { creditline.Text = value; }
        }

        public List<string> FeaturedCelebrities
        {
            set
            {
                featuringRepeater.DataSource = value;
                featuringRepeater.DataBind();
            }
        }

        protected void featuringRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Corbis.Web.UI.Controls.Label featuringText = (Corbis.Web.UI.Controls.Label)e.Item.FindControl("featuringText");
                HtmlAnchor featuringLink = (HtmlAnchor) e.Item.FindControl("featuringLink");

                featuringText.Text = (string)e.Item.DataItem;
                featuringLink.HRef = SiteUrls.SearchResults + "?q=" + HttpUtility.UrlEncode(featuringText.Text);
            }
        }
            

        #endregion
    }
}
