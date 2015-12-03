using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.Rfcd;
using Corbis.Web.Authentication;
using Corbis.RFCD.Contracts.V1;
//using Corbis.Web.Entities;
//using System.Collections.Generic;

namespace Corbis.Web.UI.RfCd
{
    public partial class RfCdResults : CorbisBasePage, IRFCDResultsView
    {
        RfcdPresenter presenter;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            presenter = new RfcdPresenter(this);
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                presenter.DisplyRFCDResults();
            }
            browseRFCD.NavigateUrl = SiteUrls.RfcdCategory;
        }

        protected void InterestedRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RepeaterItem item = e.Item;
                Corbis.Web.UI.Controls.HyperLink hyperLink = (Corbis.Web.UI.Controls.HyperLink)item.FindControl("categoryLink");
                hyperLink.NavigateUrl = GetRFCDDetailsPageUrl(hyperLink.CommandArgument);
            }
        }

        private string GetRFCDDetailsPageUrl(string volumeNumber)
        {
            string url = string.Empty;
            url = string.Format("{0}?volumeNumber={1}", SiteUrls.RfcdResults, volumeNumber);
            return url;
        }

        protected void AddToCartClick(object sender, CommandEventArgs e)
        {
            presenter.AddToCart(Profile.Current.MemberUid);
        }

        #region IRFCDResultsView Members

        public string CdName
        {
            set { cdName.Text = value; }
        }

        public string NumberOfImages
        {
            set { numberOfImages.Text = value; }
        }

        public string RfcdID
        {
            set { rfcdID.Text = value; }
        }

        public string RfcdImage
        {
            set { rfcdImage.ImageUrl = value; }
        }

        public string ImagePrice
        {
            set { imagePrice.Text = value; }
        }

        public string ImageSize
        {
            set { imageSize.Text = value; }
        }

        public string RfcdID2
        {
            set { rfcdID2.Text = value; }
        }

        public string NumberOfImages2
        {
            set { numberOfImages2.Text = value; }
        }

        public string Copyright
        {
            set { copyright.Text = value; }
        }

        public string RfcdText
        {
            set { rfcdText.Text = value; }
        }

        public System.Collections.Generic.List<RFCDEntity> InterestedRepeater
        {
            set 
            { 
                interestedRepeater.DataSource = value;
                interestedRepeater.DataBind();
            }
        }

        public System.Collections.Generic.List<RfcdDisplayImage> WebProductList
        {
            set { products.WebProductList = value; }
        }

        //public List<SearchResultProduct> SearchResultProducts
        //{
        //    set
        //    {
        //        products.WebProductList = value;
        //    }
        //}

        public string VolumeNumber
        {
            get { return Request.QueryString["volumeNumber"]; }
        }

        private Guid volumeGuid;

        public Guid VolumeGuid
        {
            get { return volumeGuid; }
            set { volumeGuid = value; }
        }
	

        #endregion


    }
}
