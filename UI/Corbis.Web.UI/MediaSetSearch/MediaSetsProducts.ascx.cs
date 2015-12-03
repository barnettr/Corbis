using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.Presenters.MediaSetSearch;
using Corbis.Image.Contracts.V1;
using System.Collections.Specialized;
using Corbis.Web.UI.Controls;
using Corbis.Web.Utilities;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI.MediaSetSearch
{
    public partial class MediaSetsProducts : CorbisBaseUserControl, IMediaSetsProducts
    {
        #region Private Memebrs

        //private MediasetSearchPresenter presenter;
        protected CenteredImageContainer thumbWrap;

        public List<MediaSet> MediasetList
        {
            set
            {
                this.MediaSetRepeater.DataSource = value;
                MediaSetRepeater.DataBind();


                if (null != value && value.Count > 1 && null != value[0])
                {
                    //TODO:- this is a temporary code remove it once final visuals are implemented
                    this.count.Text = "Total number of results = " + value[0].MediaSetType;
                }
            }
        }

        #endregion

       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //presenter = new MediasetSearchPresenter(this);
            if (Profile.IsChinaUser)
            {
                DownloadingProhibitedDiv.Visible = true;
            }
            else
            {
                DownloadingProhibitedDiv.Visible = false;
            }
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
          
        }


        protected void Result_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            // Execute the following logic for Items and Alternating Items.
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MediaSet currentItem = e.Item.DataItem as MediaSet;
               Corbis.Web.UI.Controls.Label lblMediaSetType = (Corbis.Web.UI.Controls.Label)e.Item.FindControl("MediaSetType");
               Corbis.Web.UI.Controls.Label lblMediaSetTitle = (Corbis.Web.UI.Controls.Label)e.Item.FindControl("MediaSetTitle");
               Corbis.Web.UI.Controls.Label lblMediaSetCreditLine = (Corbis.Web.UI.Controls.Label)e.Item.FindControl("MediaSetCreditLine");
               Corbis.Web.UI.Controls.Label lblDatePhotographed = (Corbis.Web.UI.Controls.Label)e.Item.FindControl("DatePhotographed");
                
                switch (currentItem.MediaSetTypeId)
                {
                    case (int)ImageMediaSetType.StorySet:
                        lblMediaSetType.Text = GetLocalResourceObject("storyset.Text").ToString();
                        lblMediaSetType.Visible = true;
                        lblMediaSetTitle.Text = StringHelper.Truncate(currentItem.Title, 45);
                        lblMediaSetTitle.Visible = true;
                        if (currentItem.DatePhotographed == null && currentItem.ApproximateDatePhotographed == null)
                        {
                            lblDatePhotographed.Visible = false;
                        }
                        else
                        {
                            lblDatePhotographed.Text = (currentItem.DatePhotographed == null) 
                                ?
                                DateTime.Parse(currentItem.DatePhotographed, Language.EnglishUS.CultureInfo).ToShortDateString().ToString(Language.CurrentCulture) : 
                                (currentItem.DatePhotographed.IndexOf("/") > 0 
                                ? 
                                DateTime.Parse(currentItem.DatePhotographed, Language.EnglishUS.CultureInfo).ToShortDateString().ToString(Language.CurrentCulture)
                                : currentItem.DatePhotographed);
                            lblDatePhotographed.Visible = true;
                        }
                        lblMediaSetCreditLine.Visible = false;
                        break;
                    case (int)ImageMediaSetType.PhotoShoot:

                        lblMediaSetType.Text = GetLocalResourceObject("sameshoot.Text").ToString();
                        lblMediaSetCreditLine.Text = currentItem.CreditLine;
                        lblMediaSetType.Visible = true;
                        lblMediaSetCreditLine.Visible = true;
                        lblDatePhotographed.Visible = false;
                        lblMediaSetTitle.Visible = false;
                        break;
                    case (int)ImageMediaSetType.SameModel:
                        lblMediaSetType.Text = GetLocalResourceObject("samemodel.Text").ToString();
                        lblMediaSetType.Visible = true;
                        lblMediaSetCreditLine.Visible = false;
                        lblDatePhotographed.Visible = false;
                        lblMediaSetTitle.Visible = false;
                        break;
                    case (int)ImageMediaSetType.Album:
                        lblMediaSetType.Text = GetLocalResourceObject("album.Text").ToString();
                        lblMediaSetTitle.Text = StringHelper.Truncate(currentItem.Title, 45);
                        lblMediaSetTitle.Visible = true;
                        lblMediaSetType.Visible = true;
                        lblMediaSetCreditLine.Visible = false;
                        lblDatePhotographed.Visible = false;
                        break;
                    case (int)ImageMediaSetType.Promotional:
                        lblMediaSetType.Text = GetLocalResourceObject("promotional.Text").ToString();
                        lblMediaSetTitle.Text = StringHelper.Truncate(currentItem.Title, 45);
                        lblMediaSetType.Visible = true;
                        lblMediaSetTitle.Visible = true;
                        lblMediaSetCreditLine.Visible = false;
                        lblDatePhotographed.Visible = false;
                        break;
                }
                thumbWrap = (CenteredImageContainer)e.Item.FindControl("thumbWrap");
                thumbWrap.Attributes["onclick"] = string.Format(@"window.location.href = '../imagegroups/imagegroups.aspx?typ={0}&id={1}'", currentItem.MediaSetTypeId, DataBinder.Eval(e.Item.DataItem, "MediaSetId"));

                // DO SOME THUMB SETUP
                thumbWrap.ImgUrl = currentItem.LeadImage.Url170;
                thumbWrap.Size = 170;
                thumbWrap.Ratio = currentItem.LeadImage.AspectRatio;
                thumbWrap.AltText = GetLocalResourceObject("viewMediaSet.Tooltip").ToString();
            }
        }
        public bool ShowAddToLightboxPopup
        {
            set { throw new System.NotImplementedException(); }
        }

        public List<Lightbox> LightboxList
        {
            set { throw new System.NotImplementedException(); }
        }

        public string ActiveLightbox
        {
            set { throw new System.NotImplementedException(); }
        }

        public List<LightboxDisplayImage> LightboxItems
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool ShowZeroResults
        {
            set { throw new System.NotImplementedException(); }
        }

        public int ItemsPerPage
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int CurrentPageNumber
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int TotalRecords
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public int CurrentPageHitCount
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public bool ShowQuickPicTab
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public void AdjustStatusForUser()
        {
            throw new System.NotImplementedException();
        }
    }
}
