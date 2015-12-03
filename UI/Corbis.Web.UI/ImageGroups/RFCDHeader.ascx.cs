using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.ImageGroups;
using Corbis.Web.Entities;
using Corbis.Web.UI.MasterPages;


namespace Corbis.Web.UI.ImageGroups
{
    public partial class RFCDHeader : CorbisBaseUserControl, IRFCDHeaderView
    {
        public ImageGroupsPresenter presenter;
        protected Corbis.Web.UI.Controls.Label imageSize;
        protected Corbis.Web.UI.Controls.Label uncompressedFileSize;

        protected override void OnInit(EventArgs e)
        {
            presenter = ((IImageGroupsView)Page).Presenter;
            presenter.ImageGroupHeaderView = this;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((int)(ImageMediaSetType.RFCD) == int.Parse(Request.QueryString["typ"]))
            {
                presenter.DisplayRFCDHeader();
                this.addCDtolightbox.OnClientClick = "ShowAddToLightboxModal('" + this.RFCDUid.ToString() + "', this, false);return false;";
                this.AddAllImages.Attributes["onclick"] = string.Format(@"ShowAddAllItemsToLightboxModal('" + id.Text.ToString() + "|" + this.RFCDUid.ToString() + "', this, false,true);return false;");
                this.RFCDAddCDtoCart.Visible = Profile.IsECommerceEnabled;

            }
         
        }


        protected void addtoCart_Click(object sender, EventArgs e)
        {
            try
            {
                presenter.AddToCart(new Guid(this.RFCDUid));
                ((Master)Page.Master).GlobalNav.UpdateCartCount();
            }
            catch (System.FormatException) { }
        }

        #region IRFCDHeaderView Members

        public string RFCDImageURL
        {
            set { rfcdImage.ImageUrl = value; }
        }

        public string Id
        {
            set { id.Text = value; }
        }

        public string Title
        {
            set { title.Text = value; }
        }
        public string RFCDUid
        {
            set { rfcdUid.Value = value; }
            get { return this.rfcdUid.Value; }
        }

        public string FileSize
        {
            set
            {
                string a = CorbisBasePage.GetEnumDisplayText<FileSize>((FileSize)Enum.Parse(typeof (FileSize), value));
                fileSize.Text = a;
            }
        }

        public string Copyright
        {
            set { copyRight.Text = value; }
        }

        public string Price
        {
            set
            {
                price.Text = value;
            }
        }

        public string ImageCount
        {
            set { imageCount.Text = value; }
        }

        public string RecentImageURL
        {
            set
            {
                    this.recentImage.RecentImageURL = value;
                    this.RFCD.Visible = true;
                    this.RFCDButton2.Attributes.Add("class", "RFCDButton2");
                    this.RFCDButton1.Attributes.Add("class", "RFCDButton1");
                    this.buttonBoxLabel.Attributes.Add("class", "buttonBoxLabel");
                    this.rightBlockLink2.Attributes.Add("class", "rightBlockLink");
            }
        }

        public string ImageGroupName
        {
            get { return Request.QueryString["typ"]; }
        }

        public string ImageGroupId
        {
            get { return Request.QueryString["id"]; }
        }

        public string RecentImageId
        {
            get { return this.recentImage.RecentImageId; }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    this.recentImage.Visible = false;
                    this.RFCD.Visible = false;
                    this.RFCDButton2.Attributes.Add("class", "RFCDButton3");
                    this.RFCDButton1.Attributes.Add("class", "RFCDButton1a");
                    this.buttonBoxLabel.Attributes.Add("class", "buttonBoxLabel2");
                }
                else
                {
                    this.recentImage.RecentImageId = value;
                    this.RFCD.Visible = true;
                }
                
            }
        }
        public Decimal RecentImageRadio
        {
            set
            {
                Decimal ratio = value;
                recentImage.RecentImageRadio = value;
            }
        }
        public bool AllLinkVisible
        {
            get { return Boolean.Parse(this.allLinkVisible.Value); }
            set { this.allLinkVisible.Value = value.ToString(); ; }
        }

        public bool AddAllImagetoLightBoxVisible
        {
            get { return AddAllImages.Visible; }
            set { 
                  AddAllImages.Visible = value;
                    // not just the link make the container also non visible
                  rightBlockLink2.Visible = value;
                  AllLinkVisible = value;
                  //ift he link is not visible then we need to change the header control wrapper height , so                      //change it to use different class 
                  HeaderControlWrapper.Attributes["class"] = (value) ? "headerControlWrapper" : "headerControlWrapperNoAddAllLink";
                  RFCD.Attributes["class"] = (value) ? "RFCD" : "RFCDNoAddAllLink";
                  recentImage image = this.recentImage as recentImage;
                  recentImage.ChangeStyle(value);
            }
        }

        public bool ShowAddToCartButton
        {
            set
            {
                this.RFCDAddCDtoCart.Visible = value;
            }
        }

        #endregion
        
    }
}