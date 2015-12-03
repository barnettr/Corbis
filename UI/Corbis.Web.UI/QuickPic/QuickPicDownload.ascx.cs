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
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.Presenters.QuickPic;
using System.Collections.Generic;
using Corbis.Image.Contracts.V1;
using Corbis.WebOrders.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.CommonSchema.Contracts.V1.Image;

namespace Corbis.Web.UI.QuickPic
{
    public partial class QuickPicDownload : CorbisBaseUserControl , IQuickPicDownloadView
    {
        private QuickPicPresenter qpPresenter;
		private bool onlyOneItem;
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            qpPresenter = new QuickPicPresenter(this);            
        }

        protected override void OnLoad(EventArgs e)
        {
            qpPresenter.GetQuickPicImagesToDownload();
            base.OnLoad(e);
        }
   
        #endregion


        public List<DownloadableQuickPicImage> DownloadableQuickPicImages
        {
            set
            {
				if (value.Count == 1)
				{
					onlyOneItem = true;
				}

                this.rptQuickPic.DataSource = value;
                DataBind();
            }
            get
            {
                return this.rptQuickPic.DataSource as List<DownloadableQuickPicImage>;
            }

        }
        protected void rptQuickPic_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DownloadableQuickPicImage quickPicImage = (DownloadableQuickPicImage)e.Item.DataItem;
                DropDownList fileSizeDropdown = (DropDownList)e.Item.FindControl("fileSize");
                Corbis.Web.UI.Controls.HoverButton btndelete = (Corbis.Web.UI.Controls.HoverButton)e.Item.FindControl("btnClose");
                btndelete.ToolTip = GetLocalResourceObject("deleteQuickPic").ToString();
                Corbis.Web.UI.Controls.CenteredImageContainer imageThumb = (Corbis.Web.UI.Controls.CenteredImageContainer)e.Item.FindControl("imageThumb");

				if (onlyOneItem)
				{
					e.Item.FindControl("btnClose").Visible = false;
				}

                imageThumb.ImgUrl = quickPicImage.Url128;
                imageThumb.Ratio = quickPicImage.AspectRatio;
                imageThumb.ToolTip = quickPicImage.CorbisId + " - " + quickPicImage.Title;
                Corbis.Web.UI.Controls.Image img = (Corbis.Web.UI.Controls.Image)imageThumb.FindControl(imageThumb.ImageID);
                
                Decimal ratio = quickPicImage.AspectRatio;
                if (ratio > 1)
                    img.Width = Unit.Parse("128px");
                else
                    img.Height = Unit.Parse("128px");

                Corbis.Web.UI.Image.Restrictions restrictions = (Corbis.Web.UI.Image.Restrictions)e.Item.FindControl("ImageRestrictions");
                ImageRestrictionsPresenter restrictionsPresenter = 
                    new ImageRestrictionsPresenter(restrictions, (DisplayImage)quickPicImage);
                restrictionsPresenter.SetRestrictions();

                List<string> downloadableSizes = new List<string>();

                List<KeyValuePair<string, string>> dropdownDataSource = quickPicImage.AvailableSizes.ConvertAll<KeyValuePair<string, string>>(
                    new Converter<AvailableFileSize, KeyValuePair<string, string>>(
                        delegate(AvailableFileSize availableFileSize)
                        {
                            KeyValuePair<string, string> returnItem = new KeyValuePair<string, string>(availableFileSize.FileSize.GetHashCode().ToString(), CorbisBasePage.GetEnumDisplayText<FileSize>(availableFileSize.FileSize));
                            if (availableFileSize.Availability == Corbis.Image.Contracts.V1.ImageFileAvailability.Immediate) downloadableSizes.Add(String.Format("'{0}'", availableFileSize.FileSize.GetHashCode().ToString()));

                            return returnItem;
                        }
                    )
                );

                fileSizeDropdown.DataSource = dropdownDataSource;
                fileSizeDropdown.DataTextField = "Value";
                fileSizeDropdown.DataValueField = "Key";
                fileSizeDropdown.DataBind();
            }
        }
    }
}