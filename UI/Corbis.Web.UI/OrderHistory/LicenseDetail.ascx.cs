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
using Corbis.Web.UI.Controls;

namespace Corbis.Web.UI.OrderHistory
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

                /*Fix for Bug-15374 */
                int containerHeight = 90;
                int containerWidth = 90;
                int height = containerHeight;
                int width = containerWidth;

                decimal aspectRatio = thumbWrap.Ratio;
                int Size = thumbWrap.Size;

                Corbis.Web.UI.Controls.Image img = (Corbis.Web.UI.Controls.Image)thumbWrap.FindControl(thumbWrap.ImageID);

                if (aspectRatio > 1)
                {
                    containerHeight = (int)Math.Round(Size / aspectRatio);
                    height = (Size - containerHeight) >> 1;

                    img.Style.Add("margin-top", height.ToString() + "px");
                    img.Style.Add("height", containerHeight.ToString()+"px");
                    img.Style.Add("width", Size.ToString() + "px");
                }
                else
                {

                    containerWidth = (int)Math.Round(Size * aspectRatio);
                    width = (Size - containerWidth);

                    img.Style.Add("height", Size.ToString() + "px");
                    img.Style.Add("width", containerWidth.ToString() + "px");
                    img.Style.Add("margin-top", "0px");
                }

                /* end for Bug- 15374*/

				imagePCT.Text = value.OfferingType==OfferingType.RFCD? "": CorbisBasePage.GetEnumDisplayText<Category>(value.Category) + "<br/>";
				imageID.Text = value.CorbisId;
				imageType.Text = String.Format("{0} {1} {2}", value.PriceTierDisplayText,CorbisBasePage.GetEnumDisplayText<LicenseModel>(value.LicenseModel), (value.OfferingType == OfferingType.RFCD? GetLocalResourceObject("cd").ToString(): ""));
				licenseModelText.Attributes["class"] = String.Format("licenseModel {0}_color", value.LicenseModel.ToString());
				imageCaption.Text = value.Title;
                string rightsAdd=string.Empty;
                if (!string.IsNullOrEmpty(value.RightsAddendumText))
                {


                    //check white space count and insert white space after 80char 
                    System.Text.RegularExpressions.MatchCollection charColl = System.Text.RegularExpressions.Regex.Matches(value.RightsAddendumText, @" ");
                    if (charColl.Count == 0 && !string.IsNullOrEmpty(value.RightsAddendumText))
                    {
                        int y = 80;
                        rightsAdd = value.RightsAddendumText;
                        for (int i = 0; i < value.RightsAddendumText.Length / 80; i++)
                        {
                            rightsAdd = rightsAdd.Insert(y, " ");
                            y += 80;
                        }
                    }
                    else
                    {
                        rightsAdd = value.RightsAddendumText;
                    }
                }
                if (string.IsNullOrEmpty(rightsAdd))
                {
                    rightAdd.Visible = false;
                }
                else if (rightsAdd.Length > 200)
                {
                    rightsAddendumBody.Text = StringHelper.Truncate(rightsAdd, 200);
                    fullrightAdd.Text = rightsAdd;
                    moreRightsAddendum.Visible = true;
                }
                else
                {
                    rightsAddendumBody.Text = rightsAdd;
                    moreRightsAddendum.Visible = false;
                }

                this.Status.Text = value.StatusDisplayText;
				imagePrice.Text = GetPriceString(value.PurchasingContract, value.NetPrice, CurrencyCode) + (value.OfferingType == OfferingType.RFCD ? ", <span class=\"rfcdImageCount\">" + String.Format(GetLocalResourceObject("imageCount").ToString(), value.RfcdImageCount) + "</span>" : "");
                double zero = 0;
                if (value.CreditAmount != zero)
                {
                    this.creditPrice.Text = GetLocalResourceObject("credit").ToString() + GetPriceString(value.PurchasingContract, value.CreditAmount, CurrencyCode);
                }

				if (value.OfferingType == OfferingType.RFCD)
				{
					imagePCT.Visible = false;
				}

				if (value.LicenseModel == LicenseModel.RM && value.CompletedUsage != null)
				{
                    if (value.CompletedUsage.AttributeValuePairs != null)
                    {
                        List<CompletedUsageAttributeValuePair> usages = value.CompletedUsage.AttributeValuePairs;

                        usageDisplay = usages.ConvertAll<KeyValuePair<string, string>>(
                            new Converter<CompletedUsageAttributeValuePair, KeyValuePair<string, string>>(
                                delegate(CompletedUsageAttributeValuePair imageUsage)
                                {
                                    return new KeyValuePair<string, string>(imageUsage.UseAttributeDescription, imageUsage.ValueDescription);
                                }
                            )
                        );
                    }

                    if (value.LicenseStartDate.HasValue)
                    {
                        usageDisplay.Add(new KeyValuePair<string, string>(GetLocalResourceObject("startDate").ToString(), DateHelper.GetLocalizedLongDateWithoutWeekday(value.LicenseStartDate.Value)));
                    }

                    if (!string.IsNullOrEmpty(value.CompletedUsage.UseTypeDescription))
                    {
                        usageDisplay.Insert(0, new KeyValuePair<string, string>(GetLocalResourceObject("specificUse").ToString(), value.CompletedUsage.UseTypeDescription));
                    }
                    if (!String.IsNullOrEmpty(value.CompletedUsage.UseCategoryDescription))
                    {
                        usageDisplay.Insert(0, new KeyValuePair<string, string>(GetLocalResourceObject("useCategory").ToString(), value.CompletedUsage.UseCategoryDescription));
                    }
				}
				else
				{
                    if (value.ImageDimension != null)
                    {
                        string unCompressedFileSize = string.Empty;

                        if (!string.IsNullOrEmpty(value.ImageDimension.UnCompressedFileSize))
                        {
                            unCompressedFileSize = " (" + value.ImageDimension.UnCompressedFileSize + " *) ";
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
                return String.Format("{0} ({1})", CurrencyHelper.GetLocalizedCurrency(price.ToString()), currencyCode);

            }
		}
	}
}