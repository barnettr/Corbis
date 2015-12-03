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
using Corbis.Web.UI.Pricing.ViewInterfaces;
using Corbis.Web.Utilities;
using System.Collections.Generic;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.Pricing;
using Corbis.Web.UI.Controls;
using Resources;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.UI.Presenters.Common;
using Corbis.Web.UI.Common.ViewInterfaces;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.Pricing
{
    public partial class RequestPrice : CorbisBasePage, IRequestPriceView, ICountriesAndStatesView
    {
        PricingPresenter pricingPresenter;
        CommonPresenter commonPresenter;

        protected CenteredImageContainer thumbWrap;

        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = null;
            base.OnInit(e);

            pricingPresenter = new PricingPresenter(this);
            commonPresenter = new CommonPresenter();
            commonPresenter.CountriesAndStatesView = this;
            
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MasterBase, "MasterBaseCSS");
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.RequestPrice, "RequestPriceCSS");
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack && !Page.IsCallback)
            {
                pricingPresenter.PopulateRequestPriceForm();

                commonPresenter.GetCountries();
                commonPresenter.GetStates();
            }

            // Get query string parameters.
            int.TryParse(Request.QueryString["lightboxid"], out lightboxId);
            corbisId = Request.QueryString["corbisid"];
            if (!String.IsNullOrEmpty(Request.QueryString["container"]))
            {
                ParentPage container = (Corbis.Web.Entities.ParentPage)Enum.Parse(typeof(Corbis.Web.Entities.ParentPage), Request.QueryString["container"].ToString());
                containerName = CorbisBasePage.GetEnumDisplayText<ParentPage>(container);
            }

            // Get the simgle item flag for lightbox request price items.
            bool isSingleItem = (Request.QueryString["optionselected"] != null && Request.QueryString["optionselected"] == "2") ? 
                false : true;

            if (isSingleItem)
            {
                // Get single image details.
                pricingPresenter.GetImageThumbnailDetail();
            }
            else
            {
                // Get all lightbox images details.
                pricingPresenter.GetAllLightboxItems();
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            pricingPresenter.SendRequestForPrice();

            ScriptManager.RegisterStartupScript(this, this.GetType(), "clearGetInTouchForm", "CorbisUI.Pricing.ContactUs.setupThankYouQueue();CorbisUI.Pricing.ContactUs.clearGetInTouchForm(true);", true);
        }

        protected void Result_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            int height = 0;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LightboxDisplayImage product = new LightboxDisplayImage();
                thumbWrap = (CenteredImageContainer)e.Item.FindControl("thumbWrap");
                

                // centered image
                CenteredImageContainer wrap = (CenteredImageContainer)e.Item.FindControl("thumbWrap");
                
                decimal aspectRatio = thumbWrap.Ratio;
                int Size = thumbWrap.Size;

                Corbis.Web.UI.Controls.Image img = (Corbis.Web.UI.Controls.Image)wrap.FindControl(wrap.ImageID);
                if (aspectRatio > 1)
                {
                    height = (Size - (int)Math.Round(Size / aspectRatio)) >> 1;
                    
                        img.Style.Add("margin-top", height.ToString() + "px");
                        img.Style.Add("width", Size.ToString() + "px");
                }
                else
                {
                    img.Style.Add("height", Size.ToString() + "px");
                }
            
            }
        }


        #region IRequestPriceView Members

        public string SelectOne
        {
            get { return Resource.SelectOne; }
        }
        
        public string Dashes
        {
            get { return Resource.Dashes; }
        }

        public bool StatesEnabled
        {
            get { return stateList.Enabled; }
            set { stateList.Enabled = value; }
        }

        public List<ContentItem> StateList
        {
            get
            {
                return stateList.DataSource as List<ContentItem>;
            }

            set
            {
                List<ContentItem> states = new List<ContentItem>();

                states = value;
                               
                stateList.DataTextField = "ContentValue";
                stateList.DataValueField = "Key";
                stateList.BindDataWithActions(states);

                if (states.Count > 1)
                {
                    stateList.Enabled = true;
                }
            }
        }

        public List<ContentItem> CountryList
        {
            get
            {
                return countryList.DataSource as List<ContentItem>;
            }
            
            set
            {
                countryList.DataTextField = "ContentValue";
                countryList.DataValueField = "Key";
                countryList.BindDataWithActions(value);
            }
        }

        public bool NameDefaultVisibility
        {
            set
            {
                firstNameDefaultDiv.Visible = value;
                lastNameDefaultDiv.Visible = value;
            }

        }

        public bool NameAsianVisibility
        {
            set
            {
                firstNameAsianDiv.Visible = value;
                lastNameAsianDiv.Visible = value;
            }

        }

        public string FirstName 
        { 
            get
            {
                return firstName.Text;
            }
            set
            {
                firstName.Text = value;
            } 
        }

        public string LastName
        {
            get
            {
                return lastName.Text;
            }
            set
            {
                lastName.Text = value;
            }
        }

        public string FirstNameAsian
        {
            get
            {
                return firstNameAsian.Text;
            }
            set
            {
                firstNameAsian.Text = value;
            }
        }

        public string LastNameAsian
        {
            get
            {
                return lastNameAsian.Text;
            }
            set
            {
                lastNameAsian.Text = value;
            }
        }

        public string Email
        {
            get
            {
                return email.Text;
            }
            set
            {
                email.Text = value;
            }
        }

        public string Phone
        {
            get
            {
                return phone.Text;
            }
            set
            {
                phone.Text = value;
            }
        }

        public string CountryCode
        {
            get
            {
                return countryList.SelectedValue;
            }
            set
            {
                countryList.SelectedValue = value;
            }
        }

        public string StateCode
        {
            get
            {
                return provinceCode.Value;
            }
            set
            {
                stateList.SelectedValue = value;
                provinceCode.Value = value;
            }
        }

        public string Comments
        {
            get
            {
                return comments.Text;
            }
        }

        public string LightboxName
        {
            get
            {
                return string.Empty;
            }
        }

        private string containerName = string.Empty;
        public string ContainerName
        {
            get
            {
                return containerName;
            }
        }

        private string corbisId = string.Empty;
        public string CorbisId
        {
            get { return corbisId; }
        }

        private int lightboxId;
        public int LightboxId
        {
            get { return lightboxId; }
        }

        private List<LightboxDisplayImage> lightboxItems = null;
        public List<LightboxDisplayImage> LightboxItems
        {
            get
            {
                return lightboxItems;
            }
            set
            {
                lightboxItems = value;
                results.DataSource = value;
                results.DataBind();
            }
        }

        public string GetLicenceModelLocalizedText(LicenseModel licenseModel)
        {
            return CorbisBasePage.GetEnumDisplayText<LicenseModel>(licenseModel);
        }
        #endregion
    }
}
