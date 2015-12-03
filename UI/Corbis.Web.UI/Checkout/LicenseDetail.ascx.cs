using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Corbis.Framework.Globalization;
using Corbis.WebOrders.Contracts.V1;
using Corbis.LightboxCart.Contracts.V1;
using System.Collections.Generic;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.Image.Contracts.V1;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Checkout
{
	public partial class LicenseDetail : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public MediaLicenseDetail LicenseDetailInfo
		{
			set
			{
				List<KeyValuePair<string, string>> usageDisplay = new List<KeyValuePair<string,string>>();

				imageThumb.Src = value.Url128;
				imageThumb.Attributes["title"] = value.CorbisId + " - " + value.Title;
                if (value.OfferingType == OfferingType.RFCD)
                {
                    imageThumb.Attributes.Add("class","RFCD");
                }
               
				imagePCT.Text = value.OfferingType==OfferingType.RFCD? "": CorbisBasePage.GetEnumDisplayText<Category>(value.Category) + "<br/>";
				imageID.Text = value.CorbisId;
				imageType.Text = String.Format("{0} {1} {2}", value.PriceTierDisplayText,CorbisBasePage.GetEnumDisplayText<LicenseModel>(value.LicenseModel), (value.OfferingType == OfferingType.RFCD? GetLocalResourceObject("cd").ToString(): ""));
				licenseModelText.Attributes["class"] = String.Format("licenseModel {0}_color", value.LicenseModel.ToString());
				imageCaption.Text = value.Title;
				imagePrice.Text = GetPriceString(value.PurchasingContract, value.NetPrice, CurrencyCode) + (value.OfferingType == OfferingType.RFCD ? ", <span class=\"rfcdImageCount\">" + String.Format(GetLocalResourceObject("imageCount").ToString(), value.RfcdImageCount) + "</span>" : "");
				if (value.OfferingType == OfferingType.RFCD)
				{
					imagePCT.Visible = false;
				}

				if (value.LicenseModel == LicenseModel.RM)
				{
					List<CompletedUsageAttributeValuePair> usages = value.CompletedUsage.AttributeValuePairs;

					usageDisplay = usages.ConvertAll<KeyValuePair<string, string>>(
						new Converter<CompletedUsageAttributeValuePair,KeyValuePair<string,string>>(
							delegate(CompletedUsageAttributeValuePair imageUsage)
							{
								return new KeyValuePair<string,string>(imageUsage.UseAttributeDescription, imageUsage.ValueDescription);
							}
						)
					);

                    if (value.LicenseStartDate.HasValue)
                    {
                        usageDisplay.Add(new KeyValuePair<string, string>(GetLocalResourceObject("startDate").ToString(), DateHelper.GetLocalizedLongDateWithoutWeekday(value.LicenseStartDate.Value)));
                    }

					usageDisplay.Insert(0, new KeyValuePair<string, string>(GetLocalResourceObject("specificUse").ToString(), value.CompletedUsage.UseTypeDescription));
					usageDisplay.Insert(0, new KeyValuePair<string, string>(GetLocalResourceObject("useCategory").ToString(), value.CompletedUsage.UseCategoryDescription));
				}
				else
				{
                    string unCompressedFileSize = string.Empty;
                    
                    if (!string.IsNullOrEmpty(value.ImageDimension.UnCompressedFileSize))
                    {
                        unCompressedFileSize = " (" + value.ImageDimension.UnCompressedFileSize + " *)";
                    }
                    else
                    {
                        unCompressedFileSize = " *";
                    }


                    usageDisplay.Add(new KeyValuePair<string, string>(GetLocalResourceObject("fileSize").ToString(), String.Format("{0}", CorbisBasePage.GetEnumDisplayText<FileSize>((FileSize)value.ImageDimension.FileSizeCode) + unCompressedFileSize)));
					if (value.OfferingType != OfferingType.RFCD) usageDisplay.Add(new KeyValuePair<string, string>(GetLocalResourceObject("dimensions").ToString(), 
						String.Format(GetLocalResourceObject("dimensionTemplate").ToString(), 
									  value.ImageDimension.PixelHeight.ToString(), 
									  value.ImageDimension.PixelWidth.ToString(),
									  (((CorbisBasePage)Page).Profile.CountryCode == "US" ? value.ImageDimension.HeightInches.ToString() + "in" : value.ImageDimension.HeightCms.ToString() + "cm"),
									  (((CorbisBasePage)Page).Profile.CountryCode == "US" ? value.ImageDimension.WidthInches.ToString() + "in" : value.ImageDimension.WidthCms.ToString() + "cm"),
									  value.ImageDimension.Resolution.ToString())
						)
					);
				}

				licensingDetailsRepeater.DataSource = usageDisplay;
				licensingDetailsRepeater.DataBind();

				if (value.PricingIconDisplay == Corbis.Image.Contracts.V1.PricingIcon.DoubleDollar || value.PricingIconDisplay == Corbis.Image.Contracts.V1.PricingIcon.TripleDollar)
				{
					pricingIcon.Attributes["class"] = value.PricingIconDisplay.ToString();
				}
				else
				{
					pricingIcon.Visible = false;
				}

				modelReleased.Text = String.Format(GetLocalResourceObject("modelReleased").ToString(), CorbisBasePage.GetEnumDisplayText<ModelRelease>(value.ModelReleaseStatus));
				propertyReleased.Text = String.Format(GetLocalResourceObject("propertyRelease").ToString(), (value.PropertyReleaseStatus ? GetLocalResourceObject("release").ToString() : GetLocalResourceObject("noRelease").ToString()));
				if (value.DomesticEmbargoDate != null && value.DomesticEmbargoDate > DateTime.Now)
				{
                    domEmbargoDate.Text = String.Format(GetLocalResourceObject("domEmbargoDate").ToString(), value.DomesticEmbargoDate.Value.ToString("MMM dd, yyyy", Language.CurrentCulture));
				}
				else
				{
					domEmbargoDate.Visible = false;
				}

				if (value.InternationalEmbargoDate != null && value.InternationalEmbargoDate > DateTime.Now)
				{
                    intEmbargoDate.Text = String.Format(GetLocalResourceObject("intEmbargoDate").ToString(), value.InternationalEmbargoDate.Value.ToString("MMM dd, yyyy", Language.CurrentCulture));
				}
				else
				{
					intEmbargoDate.Visible = false;
				}

				if (value.OfferingType != OfferingType.RFCD && value.Restrictions != null)
				{
					value.Restrictions.RemoveAll(
						new Predicate<Restriction>(
							delegate(Restriction restriction) { 
								return (restriction.RestrictionUser == RestrictionUser.CorbisProducts || restriction.RestrictionUser == RestrictionUser.CorbisAdPromo);
							}
						)
					);

					if (value.Restrictions.Count > 0)
					{
						restrictionsRepeater.DataSource = value.Restrictions;
						restrictionsRepeater.DataBind();
                        
                        if (value.Restrictions.Count > 10)
                        {
                            moreRestrictions.Visible = true;
                            restrictionsHolder.Attributes["class"] = "restrictionDivEx";
                        }
                        else
                        {
                            moreRestrictions.Visible = false;
                        }
						
					}
					else
					{
						moreRestrictions.Visible = false;
					}
				}
				else
				{
					restrictionsRepeater.Visible = false;
					moreRestrictions.Visible = false;

					if (value.OfferingType == OfferingType.RFCD)
					{
						restrictionTitle.Visible = false;
						releaseDetails.Visible = false;
					}
				}
                if (value.LicenseModel == LicenseModel.RF || value.OfferingType == OfferingType.RFCD)
                {
                    sizeFoootNoteDiv.Visible = true;
                }
			}
		}

		private string currencyCode;
		public string CurrencyCode
		{
			get { return currencyCode; }
			set { currencyCode = value; }
		}

		private String GetPriceString(ContractType contractType, double price, string currencyCode)
		{
			if (contractType == ContractType.Purchasing)
			{
				return GetLocalResourceObject("AsPerCotract").ToString();
			}
			else
			{
                return String.Format("{0:C} ({1})", price, currencyCode);
			}
		}
	}
}