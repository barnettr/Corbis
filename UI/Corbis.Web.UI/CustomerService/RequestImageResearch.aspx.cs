using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Web.UI.CustomerService.ViewInterfaces;
using Corbis.Web.UI.Presenters.CustomerService;
using Corbis.Web.Entities;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.CustomerService
{
    public partial class RequestImageResearch : CorbisBasePage, IRequestImageResearch
    {
        private CustomerServicePresenter presenter;
        
        protected override void OnInit(EventArgs e)
        {
            this.RequiresSSL = null;
            base.OnInit(e);
            presenter = new CustomerServicePresenter();
            presenter.RequestImageResearchView = this;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.PopulateModelRelease();
                this.PopulateNatureOfBusiness();
            }
        }
        protected void Submit_Click(object sender, EventArgs e)
        {
            presenter.SubmitResearchRequest();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RIRsuccess", "doSuccess();", true);
        }

        private void PopulateModelRelease()
        {
            List<DisplayValue<ModelReleaseForResearch>> originalModelReleaseList = new List<DisplayValue<ModelReleaseForResearch>>();
            List<DisplayValue<ModelReleaseForResearch>> modelReleaseList = new List<DisplayValue<ModelReleaseForResearch>>();

            originalModelReleaseList = CorbisBasePage.GetEnumDisplayValues<ModelReleaseForResearch>();

            foreach (DisplayValue<ModelReleaseForResearch> item in originalModelReleaseList)
            {
                if (!string.IsNullOrEmpty(item.Text))
                {
                    modelReleaseList.Add(item);
                }
            }

            this.modelRelease.DataSource = modelReleaseList;
            this.modelRelease.DataValueField = "Id";
            this.modelRelease.DataTextField = "Text";
            this.modelRelease.DataBind();
        }

        private void PopulateNatureOfBusiness()
        {
            List<DisplayValue<NatureOfYourBusiness>> originalNatureOfYourBusinessList = new List<DisplayValue<NatureOfYourBusiness>>();
            List<DisplayValue<NatureOfYourBusiness>> natureOfYourBusinessList = new List<DisplayValue<NatureOfYourBusiness>>();

            originalNatureOfYourBusinessList = CorbisBasePage.GetEnumDisplayValues<NatureOfYourBusiness>();

            foreach (DisplayValue<NatureOfYourBusiness> item in originalNatureOfYourBusinessList)
            {
                if (!string.IsNullOrEmpty(item.Text))
                {
                    natureOfYourBusinessList.Add(item);
                }
            }

            this.natureOfBusiness.DataSource = natureOfYourBusinessList;
            this.natureOfBusiness.DataValueField = "Id";
            this.natureOfBusiness.DataTextField = "Text";
            this.natureOfBusiness.DataBind();
        }

        public string ImageDescription
        {
            get
            {
                return this.imageDescription.Text;
            }
        }
        public string ProjectDeadline
        {
            get
            {
                return this.projectDeadline.Text;
            }
        }
        public string ModelRelease
        {
            get
            {
                return this.modelRelease.SelectedValue;
            }
        }
        public string ProjectClient
        {
            get
            {
                return this.projectClient.Text;
            }
        }
        public string JobNumber
        {
            get
            {
                return this.jobNumber.Text;
            }
        }
        public string NatureOfBusiness
        {
            get
            {
                return this.natureOfBusiness.SelectedValue;
            }
        }
        public string OtherDescription
        {
            get
            {
                return this.otherDescription.Text;
            }
        }
    }
}
