using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Reflection;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.Web.Entities;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.Utilities;
using Corbis.Framework.Globalization;


namespace Corbis.Web.UI.Checkout
{
    public partial class ItemSummary :CorbisBaseUserControl
    {
       // private CheckoutPresenter checkoutPresenter;
        protected void Page_Load(object sender, EventArgs e)
        {
           // checkoutPresenter.CheckoutCartItems();
        }


        protected override void OnInit(EventArgs e)
        {
            ///TODO: set Tax label based in billing country here
            base.OnInit(e);

            //checkoutPresenter = new CheckoutPresenter(this);
        }
        private string GetDimension(float width, float height)
        {
            string result, dataType;
            if (Profile.CountryCode.Equals("US", StringComparison.InvariantCultureIgnoreCase))
            {
                dataType = "in";
            }
            else
                dataType = "cm";
            result = string.Format("{0}{1} x {2}{3}", width, dataType, height, dataType);
            return result;
        }

        public string SubTotal
        {
            set { 
                
                //the following line was breaking the build
                subTotalAmount.InnerText = value;
            }
        }
        public void PriceUpdate()
        {
            priceUpdateBlock.Update();

        }

        protected void summaryItemsRepeater_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            bool coffWorkflow = WorkflowHelper.IsCOFFWorkflow(Request);
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Type itemType = e.Item.DataItem.GetType();
                if (itemType == typeof(CheckoutProduct))
                {
                    CenteredImageContainer wrap = (CenteredImageContainer) e.Item.FindControl("thumbWrap");
                    HtmlGenericControl restrictionDiv =
                        (HtmlGenericControl) e.Item.FindControl("checkoutItemRestrictions");

                    string rel = string.Empty;
                    CheckoutProduct product = (CheckoutProduct) e.Item.DataItem;
                    HtmlGenericControl itemTypeText = (HtmlGenericControl) e.Item.FindControl("checkoutItemType");
                    String localizedLicenseModel = GetLocalResourceObject(product.LicenseModel+"ShortText").ToString();
                    itemTypeText.InnerText = product.PriceTier + " " + localizedLicenseModel;
                    itemTypeText.Attributes["class"] = "checkoutItemType " + product.LicenseModel + "_color";
                    if (product.IsRFCD)
                    {
                        restrictionDiv.Visible = false;
                        itemTypeText.InnerText += " CD";
                    }

                    if (product.LicenseModel == LicenseModel.RF)
                    {
                        if (product.IsRFCD)
                        {

                            FileSize fileSize = Enum.IsDefined(typeof(FileSize), int.Parse(product.RfcdEntity.FileSize)) ? (FileSize)(Enum.Parse(typeof(FileSize), product.RfcdEntity.FileSize.ToString())) : FileSize.Unknown;
                            string fileSizeDisplay = CorbisBasePage.GetEnumDisplayText<FileSize>((FileSize)Enum.Parse(typeof(FileSize), fileSize.ToString()));
                            FileSizeText fileSizeText = Enum.IsDefined(typeof(FileSizeText), int.Parse(product.RfcdEntity.FileSize)) ? (FileSizeText)(Enum.Parse(typeof(FileSizeText), product.RfcdEntity.FileSize.ToString())) : FileSizeText.Unknown;
                           string fileSizeTextDisplay=CorbisBasePage.GetEnumDisplayText<FileSizeText>((FileSizeText)Enum.Parse(typeof(FileSizeText), fileSizeText.ToString()));
                            rel = string.Format(
                                @"
                                <div class='rf-info'>{0} ({1}*), {2} {3}</div>
                                <div class='deEmphasize'> {4} </div>
                                ",
                                 fileSizeDisplay,//Small product.RFLicenseDetail.ImageSize.ToString(),
                                 fileSizeTextDisplay,//"32 MB", //product.RFLicenseDetail.UncompressedFileSize,
                                 product.RfcdEntity.ImageCount,
                                 "images", 
                                 "&lowast; " + (string)GetLocalResourceObject("rfWarning")
                                );
                        }
                        else
                        {
                            string dimentionInfo;

                            // for non-rfcd case
                            if (product.RFLicenseDetail != null)
                            {
                                dimentionInfo = GetDimension(product.RFLicenseDetail.ImageWidth,
                                                             product.RFLicenseDetail.ImageHeight);

                                rel =
                                    string.Format(
                                        @"
                                    <div class='rf-info'>{0} ({1}*)</div>
                                    <div class='rf-info'>{2} </div>
                                    <div class='rf-info'>{3} </div>
                                    <div class='deEmphasize'>{4} </div>
                                    ",
                                        CorbisBasePage.GetKeyedEnumDisplayText(product.RFLicenseDetail.ImageSize, "RF"),
                                        product.RFLicenseDetail.UncompressedFileSize,
                                        product.RFLicenseDetail.PixelWidth.ToString() + "px x " +
                                        product.RFLicenseDetail.PixelHeight + "px",
                                        dimentionInfo + " @ " + product.RFLicenseDetail.Resolution + "ppi",
                                        "&lowast; " + (string) GetLocalResourceObject("rfWarning")
                                        );
                            }
                        }
                    }
                    else if (product.LicenseModel == LicenseModel.RM)
                    {
                        string details = string.Empty;
                        if (coffWorkflow && product.UsageAttributes == null)
                        {
                            details = string.Format(
                                @"<div class='deEmphasizeNoUsage'>{0}</div>", GetLocalResourceObject("RMCustomUseNotDefined").ToString());
                            rel = details;
                        }
                        else
                        {
                            foreach (KeyValuePair<string, string> kvp in product.UsageAttributes)
                            {
                                details += string.Format(
                                    @"
                                    <div class='floatLeft'>
                                    <div class='rm-label'>{0}</div>
                                    <div class='rm-info'>{1}</div>
                                    </div>
                                    ",
                                                                   kvp.Key + ":", (kvp.Value == null) ? "&nbsp;" : kvp.Value);
                            }
                            rel =
                                string.Format(
                                    @"
                                    <div class='rm-parent-block'>
                                        <div class='floatLeft'>
                                            <div class='rm-label'>{0}</div>
                                            <div class='rm-info'>{1}</div>
                                        </div>
                                        <div class='floatLeft'>
                                            <div class='rm-label'>{2}</div>
                                            <div class='rm-info'>{3}</div>
                                        </div>
                                    </div>
                                    <div class='rm-block'>
                                        {4}
                                        <div class='clr'></div>
                                    </div>
                                ",
                                    GetLocalResourceObject("UseCategory") as string,
                                    product.UseCatagoryText,
                                    GetLocalResourceObject("SpecificUse") as string,
                                    product.UseTypeText,
                                    details
                                    );
                        }
                    }
                    rel = HttpUtility.HtmlEncode(rel);
                    wrap.Attributes["rel"] = rel;
                    wrap.Attributes["title"] = (string)GetLocalResourceObject(product.IsRFCD ? "rfcdDetails" : "licenseDetails");
                }
            }

        }

        #region ICheckoutView Members

        public List<CheckoutProduct> CheckoutProducts
        {           
          
            set
            {
                summaryItemsRepeater.DataSource = value;
                summaryItemsRepeater.DataBind();
            }
        }

        public string PromotionAmount
        {
            get
            {
                return promotionAmount.InnerText;
            }
            set
            {
                promotionAmount.InnerText = value;
                promotionZone.Visible = !string.IsNullOrEmpty(value);
            }
        }

        public string Tax
        {
            get
            {
                return taxDiv.InnerText;
            }
            set
            {
                //taxDiv.Visible = true;
                taxDiv.InnerText = value;
            }
        }
        public string TaxLabel
        {
            get
            {
                return Tax2.Text;
            }
            set
            {
                Tax2.Text = value;
            }
        }
        public string KsaTax
        {
            get
            {
                return this.KSAtaxDiv.InnerText;
            }
            set
            {
                ksaBlock.Visible = true;
                this.KSAtaxDiv.InnerText = value;
            }
        }
        public string Shipping
        {
            get
            {
                return shippingDiv.InnerText;
            }
            set
            {
                shippingDiv.InnerText = value;
            }
        }
       

        public string Total
        {
            get
            {
                return totalDiv.InnerText;
            }
            set
            {
                totalDiv.InnerText = value;
            }
        }

        #endregion
    }
}
