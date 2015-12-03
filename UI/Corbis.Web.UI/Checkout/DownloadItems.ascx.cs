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

namespace Corbis.Web.UI.Checkout
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

				List<string> downloadableSizes = new List<string>();

				List<KeyValuePair<string, string>> dropdownDataSource = downloadableImage.AvailableSizes.ConvertAll<KeyValuePair<string, string>>(
					new Converter<AvailableFileSize,KeyValuePair<string,string>>(
						delegate(AvailableFileSize availableFileSize) {
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

				fileSizeDropdown.Attributes["onchange"] = String.Format("CorbisUI.Order.SetFileSizeAvailability(this, new Array({0}))", String.Join(",", downloadableSizes.ToArray()));

				
				if (fileSizeDropdown.SelectedIndex >= 0 && downloadableImage.AvailableSizes[fileSizeDropdown.SelectedIndex].Availability != Corbis.Image.Contracts.V1.ImageFileAvailability.Immediate)
				{
					Corbis.Web.UI.Controls.GlassButton downloadButton = (Corbis.Web.UI.Controls.GlassButton)e.Item.FindControl("downloadButton");
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