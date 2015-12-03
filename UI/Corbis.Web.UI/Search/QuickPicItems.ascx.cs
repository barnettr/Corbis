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
using System.Collections.Generic;
using Corbis.Web.UI.Presenters.Search;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.Presenters.QuickPic;
using Corbis.Image.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;


namespace Corbis.Web.UI.Search
{
    public partial class QuickPicItems : CorbisBaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadQuicPicView();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
           
        }

        private void LoadQuicPicView()
        {
            QuickPicPresenter quickPicPresenter = new QuickPicPresenter();
            List<QuickPicItem> quickPicList = quickPicPresenter.QuickPicList;

            if (quickPicList != null && quickPicList.Count > 0)
            {
				LoadQuickView = quickPicList;                
                emptyQuickpicView.Visible = true;
                centerMe.Attributes["class"] += " hdn";
            }

            else
            {
                emptyQuickpicView.Visible = true;
                LoadQuickView = null;
                if (Profile.IsAnonymous)
                {
                    signInFromQuickPic.Visible = true;
                    signInFromQuickPic.NavigateUrl = SiteUrls.Authenticate;
                }

            }

        }

        public int Count
        {
            get
            {
                return searchBuddyQuickPic.Items.Count;
            }
        }

        private List<QuickPicItem> LoadQuickView
        {  
            set
            {
                searchBuddyQuickPic.DataSource = value;
                searchBuddyQuickPic.DataBind();
                
            }


        }

        protected void searchBuddyQuickPic_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {           
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl lboxBlock = (HtmlGenericControl)e.Item.FindControl("lboxBlock");
                Corbis.Web.UI.Controls.CenteredImageContainer imageThumb = (Corbis.Web.UI.Controls.CenteredImageContainer)e.Item.FindControl("imageThumb");
                Corbis.Web.UI.Controls.Image img = (Corbis.Web.UI.Controls.Image)imageThumb.FindControl(imageThumb.ImageID);
               
                imageThumb.Attributes["onclick"] = string.Format(@"EnlargeImagePopUp('../Enlargement/Enlargement.aspx?id={0}&caller=quickpic');return false;", DataBinder.Eval(e.Item.DataItem, "CorbisId"));
                imageThumb.Attributes.CssStyle.Add("background", "#262626");
                imageThumb.Attributes.CssStyle.Add("cursor", "pointer");           
                lboxBlock.Attributes.Add("class", "quickPicBlock inCart");

                Decimal ratio = (Decimal)DataBinder.Eval(e.Item.DataItem, "AspectRatio");
                if (ratio > 1)
                     img.Width = Unit.Parse("90px");
                else
                     img.Height = Unit.Parse("90px");

                Corbis.Web.UI.Controls.HoverButton btnClose = (Corbis.Web.UI.Controls.HoverButton)e.Item.FindControl("btnClose");
                QuickPicItem dataItem = (QuickPicItem)e.Item.DataItem;
               
                Corbis.Web.UI.Controls.Label licenseModel = (Corbis.Web.UI.Controls.Label)e.Item.FindControl("licenseModel");



                LicenseModel licenseModelEnum = (LicenseModel)Enum.Parse(typeof(LicenseModel), dataItem.LicenseModel);
                licenseModel.Text = CorbisBasePage.GetEnumDisplayText<Corbis.CommonSchema.Contracts.V1.LicenseModel>((Corbis.CommonSchema.Contracts.V1.LicenseModel)licenseModelEnum);

                //LicenseModel licenseModelEnum = Enum.IsDefined(typeof(LicenseModel), (LicenseModel)Enum.Parse(typeof(LicenseModel), dataItem.LicenseModel)) ? (LicenseModel)Enum.Parse(typeof(LicenseModel), dataItem.LicenseModel) : LicenseModel.Unknown;
                
                
                lboxBlock.Attributes.Add("corbisid", dataItem.CorbisID);
                if (Page is Corbis.Web.UI.Lightboxes.MyLightboxes)
			    {
				    btnClose.OnClientClick = string.Format("CorbisUI.Handlers.Quickpic.deleteItem('{0}'); return false;", dataItem.CorbisID);
			    }
			    else
			    {
				    btnClose.OnClientClick = string.Format("CorbisUI.Handlers.Quickpic.deleteItem('{0}'); return false;", dataItem.CorbisID);
			    }
		    }


        }
        


}
}