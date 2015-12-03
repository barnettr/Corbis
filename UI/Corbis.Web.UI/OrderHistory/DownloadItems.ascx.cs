using System;
using System.Collections;
using System.Collections.Generic;
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
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.WebOrders.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;


namespace Corbis.Web.UI.OrderHistory
{
    public partial class DownloadItems : Corbis.Web.UI.CorbisBaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void products_ItemDataBound(Object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DownloadableImage downloadableImage = e.Item.DataItem as DownloadableImage;
                DropDownList fileSizeDropdown = (DropDownList)e.Item.FindControl("fileSize");
                HtmlContainerControl licenseStatus = (HtmlContainerControl)e.Item.FindControl("licenseStatus");
                HtmlContainerControl license = (HtmlContainerControl)e.Item.FindControl("license");
                HtmlContainerControl downloadPeriod = (HtmlContainerControl)e.Item.FindControl("downloadPeriod");
                HtmlContainerControl redAlert = (HtmlContainerControl)e.Item.FindControl("redAlert");

                HtmlContainerControl addtoCart = (HtmlContainerControl)e.Item.FindControl("addtoCart");

                HyperLink hyperAddtoCart = (HyperLink)e.Item.FindControl("hyperAddtoCart");
                Corbis.Web.UI.Controls.GlassButton downloadButton = (Corbis.Web.UI.Controls.GlassButton)e.Item.FindControl("downloadButton");
                Corbis.Web.UI.Controls.CenteredImageContainer imageThumb = (Corbis.Web.UI.Controls.CenteredImageContainer)e.Item.FindControl("thumbWrap");
                Corbis.Web.UI.Controls.Image img = (Corbis.Web.UI.Controls.Image)imageThumb.FindControl(imageThumb.ImageID);
               

                Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                switch (downloadableImage.LicenseStatus)
                {
                    case ImageLicenseStatus.ContactUs:

                    case ImageLicenseStatus.Unknown:
                        if (downloadableImage.Url128.IndexOf("not_available") > 0)
                        {
                            img.Attributes.CssStyle.Add(HtmlTextWriterStyle.MarginTop, "0px");
                            //img.Attributes.Add("class", "noImage");
                        }
                        hyperAddtoCart.NavigateUrl = "javascript:void(0)";
                        hyperAddtoCart.Text = GetLocalResourceObject("contactCorbis").ToString();
                        hyperAddtoCart.Attributes["onclick"] = "javascript:CorbisUI.ContactCorbis.ShowContactCorbisModal(this);return false;";
                        addtoCart.Visible = true;
                        fileSizeDropdown.Visible = false;
                        licenseStatus.Visible = false;
                        downloadButton.Visible = false;
                        downloadPeriod.Visible = false;
                        break;
                    case ImageLicenseStatus.Expired:
                        hyperAddtoCart.Attributes["onclick"] = "CorbisUI.Order.addProductToCart('" + downloadableImage.ImageUid + "');return false;";
                        hyperAddtoCart.NavigateUrl = "javascript:void(0)";
                        hyperAddtoCart.Text = GetLocalResourceObject("addtoCart").ToString();
                        lblStatus.Text = GetLocalResourceObject("licenseexpired").ToString();
                        licenseStatus.Attributes["class"] = "redAlert";
                        fileSizeDropdown.Visible = false;
                        downloadButton.Visible = false;
                        addtoCart.Visible = true;
                        licenseStatus.Visible = true;
                        downloadPeriod.Visible = false;
                        break;
                    case ImageLicenseStatus.Expiring:
                        hyperAddtoCart.Attributes["onclick"] = "CorbisUI.Order.addProductToCart('" + downloadableImage.ImageUid + "');return false;";
                        hyperAddtoCart.NavigateUrl = "javascript:void(0)";
                        hyperAddtoCart.Text = GetLocalResourceObject("addtoCart").ToString();
                        if (downloadableImage.LicenseEndDate.Value.Subtract(DateTime.Now).Days > 1)
                            lblStatus.Text = string.Format(GetLocalResourceObject("licenseexpires1").ToString(), downloadableImage.LicenseEndDate.Value.Subtract(DateTime.Now).Days);
                        else
                            lblStatus.Text = GetLocalResourceObject("licenseexpires").ToString();
                        addtoCart.Visible = true;
                        licenseStatus.Visible = true;
                        if (downloadableImage.DownloadPeriodExpired)
                        {
                            fileSizeDropdown.Visible = false;
                            downloadButton.Visible = false;
                            downloadPeriod.Visible = true;
                        }
                        else
                        {
                            fileSizeDropdown.Visible = true;
                            license.Visible = false;
                            downloadButton.Visible = true;
                            downloadPeriod.Visible = false;
                        }
                        break;
                    case ImageLicenseStatus.NotApplicable:
                    case ImageLicenseStatus.Valid:
                        licenseStatus.Visible = false;
                        if (downloadableImage.DownloadPeriodExpired)
                        {
                            hyperAddtoCart.Attributes["onclick"] = "CorbisUI.Order.addProductToCart('" + downloadableImage.ImageUid + "');return false;";
                            hyperAddtoCart.NavigateUrl = "javascript:void(0)";
                            hyperAddtoCart.Text = GetLocalResourceObject("addtoCart").ToString();
                            addtoCart.Visible = true;
                            fileSizeDropdown.Visible = false;
                            downloadButton.Visible = false;
                            downloadPeriod.Visible = true;
                        }
                        else
                        {
                            addtoCart.Visible = false;
                            fileSizeDropdown.Visible = true;
                            license.Visible = false;
                            downloadButton.Visible = true;
                            downloadPeriod.Visible = false;
                        }
                        break;
                    default:
                        break;
                }
                List<string> downloadableSizes = new List<string>();
                List<KeyValuePair<string, string>> dropdownDataSource = new List<KeyValuePair<string, string>>();
                if (downloadableImage.AvailableSizes != null)
                {
                    dropdownDataSource = downloadableImage.AvailableSizes.ConvertAll<KeyValuePair<string, string>>(
                        new Converter<AvailableFileSize, KeyValuePair<string, string>>(
                            delegate(AvailableFileSize availableFileSize)
                            {
                                KeyValuePair<string, string> returnItem = new KeyValuePair<string, string>(availableFileSize.FileSize.GetHashCode().ToString(), CorbisBasePage.GetEnumDisplayText<FileSize>(availableFileSize.FileSize));
                                if (availableFileSize.Availability == Corbis.Image.Contracts.V1.ImageFileAvailability.Immediate) downloadableSizes.Add(String.Format("'{0}'", availableFileSize.FileSize.GetHashCode().ToString()));

                                return returnItem;
                            }
                        )
                    );
                }

                fileSizeDropdown.DataSource = dropdownDataSource;
                fileSizeDropdown.DataTextField = "Value";
                fileSizeDropdown.DataValueField = "Key";
                fileSizeDropdown.DataBind();

                fileSizeDropdown.Attributes["onchange"] = String.Format("CorbisUI.Order.SetFileSizeAvailability(this, new Array({0}))", String.Join(",", downloadableSizes.ToArray()));


                if (fileSizeDropdown.SelectedIndex >= 0
                    && downloadableImage.AvailableSizes != null
                    && downloadableImage.AvailableSizes[fileSizeDropdown.SelectedIndex].Availability != Corbis.Image.Contracts.V1.ImageFileAvailability.Immediate)
                {
                    downloadButton.CssClass += " DisabledGlassButton";
                    downloadButton.Enabled = false;
                    downloadButton.Text = GetLocalResourceObject("pending").ToString();
                    //Looks like the control removes the script when disabled, so setting it in attribute here.
                    ((Button)downloadButton.FindControl("GlassButton")).Attributes["onclick"] = "javascript:CorbisUI.Order.DownloadImage(this); return false;";
                }
            }
        }

        public object Items
        {
            set
            {
                products.DataSource = value;
                products.DataBind();
            }
        }
    }
}