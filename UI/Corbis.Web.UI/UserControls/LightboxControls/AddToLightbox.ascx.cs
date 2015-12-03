using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Corbis.Framework.Globalization;
using Corbis.Web.UI.Presenters.ImageGroups;
using System.Web.UI.HtmlControls;
using System.Threading;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.UI.Lightboxes.ViewInterfaces;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Image.Contracts.V1;

namespace Corbis.Web.UI.Lightboxes
{
	/// <summary>
	/// Stand alone control for adding media to lightbox
	/// 
	/// To use:
	///		1. Register control on page
    ///			<%@ Register TagPrefix="Corbis" TagName="AddToCart" Src="~/UserControls/CheckoutControls/AddToCart.ascx" %>
	///		2. Add control on page
	///			<corbis:AddToCart ID="addToCartControl" runat="server" OnAddToLightboxHandler="" OnAddToNewLightboxHandler="" PopulateDropdown="" />
	///			set handlers as needed, set PopulateDropdown to true if you want the control to populate the lightbox drop down as well, otherwise you'll have to do it.
	///		3. Hook up control to show add to lightbox modal
	///			onclick="javascript:ShowAddToLightboxModal('<%# Eval("MediaUid") %>', false); return false;"
	///			set ActiveLightbox, LightboxList properties as needed.
	/// </summary>
    public partial class AddToLightbox : CorbisBaseUserControl
    {
        private EventHandler addToNewLightboxHandler;
        private EventHandler addToLightboxHandler;
		private bool _useDefaultHandler = true;
		private bool _populateDropdown = false;
        private int lightBoxId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (!string.IsNullOrEmpty(lightboxDropDownList.SelectedValue))
                {
                    LightboxId = int.Parse(lightboxDropDownList.SelectedValue);
                }
            }
            if (PopulateDropdown)
			{
				LightboxesPresenter lightboxPresenter = new LightboxesPresenter((IView)Page);
				LightboxList = lightboxPresenter.GetLightboxList(Profile.UserName, false);
				ActiveLightbox = (new StateItemCollection(System.Web.HttpContext.Current)).GetStateItemValue<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, StateItemStore.Cookie);
			}
        }

        public event EventHandler AddToNewLightboxHandler
        {
            add { addToNewLightboxHandler += value; }
            remove { addToNewLightboxHandler -= value; }
        }
        public event EventHandler AddToLightboxHandler
        {
            add { addToLightboxHandler += value; }
            remove { addToLightboxHandler -= value; }
        }
        private bool eventExecuted = false;

        public bool EventExecuted
        {
            get { return this.eventExecuted; }
            set { this.eventExecuted = value; }
        }
        
        protected void addToNewLightboxBtn_Click(object sender, EventArgs e)
        {
			Page.Validate("AddToNewLightbox");
			if (Page.IsValid)
			{
				if (UseDefalutHandler)
				{
					LightboxesPresenter lightboxPresenter = new LightboxesPresenter((IView)Page);
					int lightboxId;
					int mediaSetId;
					bool isMediaSetFlag = int.TryParse(this.offeringUidHidden.Value, out mediaSetId);
					ImageGroupsPresenter imagegroupsPresenter = new ImageGroupsPresenter((IView)Page);
					if (Request.ServerVariables["URL"].ToLower().IndexOf("imagegroups.aspx") > 0 && isMediaSetFlag)
					{
						lightboxId = lightboxPresenter.CreateLightbox(Profile.UserName, LightboxName);
						int imageGroupTypeInt;
						ImageMediaSetType imageGroupType = int.TryParse(Request.QueryString["typ"], out imageGroupTypeInt) ? (Enum.IsDefined(typeof(ImageMediaSetType), imageGroupTypeInt) ? (ImageMediaSetType)(imageGroupTypeInt) : ImageMediaSetType.Unknown) : ImageMediaSetType.Unknown;
						if (!string.IsNullOrEmpty(MediaSetId) && !string.IsNullOrEmpty(Request.QueryString["typ"].ToString()) && (int)(ImageMediaSetType.OutlineSession) != int.Parse(Request.QueryString["typ"]) && (int)(ImageMediaSetType.RFCD) != int.Parse(Request.QueryString["typ"]))
						{
							imagegroupsPresenter.AddAllMediaSetImagesToLightbox(this.MediaSetId, lightboxId);
						}
						else if (!IsGuid(OutlineSessionId) && (int)(ImageMediaSetType.OutlineSession) == int.Parse(Request.QueryString["typ"]))
						{
							imagegroupsPresenter.AddAllOutlineSessionSetImagesToLightbox(int.Parse(this.OutlineSessionId), lightboxId);
						}
						else if ((int)(ImageMediaSetType.RFCD) == int.Parse(Request.QueryString["typ"]))
						{
							if (IsGuid(RfcdOfferingUid))
							{
								imagegroupsPresenter.AddRfcdToLightbox(new Guid(this.RfcdOfferingUid), lightboxId);
							}
							else if (Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionCoff) && offeringUidHidden.Value.IndexOf("|") > 0)
							{
								imagegroupsPresenter.AddAllRfcdImageToLightbox(this.RfcdvolumeNumber, lightboxId);
							}
						}
						else if ((int)(ImageMediaSetType.RFCD) == int.Parse(Request.QueryString["typ"]) && !Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionCoff))
						{
							imagegroupsPresenter.AddRfcdToLightbox(new Guid(this.RfcdOfferingUid), lightboxId);
						}
					}
					else
					{
						if (offeringUidHidden.Value.IndexOf("|") > 0)
						{
							bool addAllItems = false;
							bool.TryParse(this.addAllItemsToLightboxHidden.Value, out addAllItems);
							Guid OffUid = new Guid(offeringUidHidden.Value.Split('|')[1]);
							if (addAllItems)
							{
								lightboxId = imagegroupsPresenter.AddAllRfcdImageToNewLightbox(Profile.UserName, this.LightboxName, this.RfcdvolumeNumber);
							}
							else
							{
								lightboxId =
									lightboxPresenter.AddToNewLightBox(Profile.UserName, OffUid, Profile.CountryCode,
																	   Language.CurrentLanguage.LanguageCode,
																	   this.LightboxName);
							}
						}
						else
						{
							lightboxId =
								lightboxPresenter.AddToNewLightBox(Profile.UserName, this.OfferingUid,
																   Profile.CountryCode,
																   Language.CurrentLanguage.LanguageCode,
																   this.LightboxName);
						}
						if (ValidationErrors != null && ValidationErrors.Count > 0)
						{
							((CorbisBasePage)Page).SetValidationErrors(ValidationErrors);
							ScriptManager.RegisterStartupScript(Page, this.GetType(), "openAddToLightboxModalPopup",
																String.Format("ShowAddToLightboxModal('{0}', this, true);",
																			  this.OfferingUid), true);
						}
						else
						{

						}
					}
					(new StateItemCollection(System.Web.HttpContext.Current)).SetStateItem<string>(
								new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey,
													  lightboxId.ToString(), StateItemStore.Cookie));
					this.LightboxList = lightboxPresenter.GetLightboxList(Profile.UserName, false);
					this.LightboxId = lightboxId;
					this.ActiveLightbox = lightboxId.ToString();
					this.NewLightboxName = LightboxName;
					this.LightboxName = "";
					ScriptManager.RegisterStartupScript(Page, this.GetType(), "closeAddToLightboxModalPopup",
														"HideModal('addToLightboxModalPopup'); $(CorbisUI.GlobalVars.AddToLightbox.createLightboxSection).addClass('displayNone');",
														true);
				}

				if (addToNewLightboxHandler != null)
				{
					addToNewLightboxHandler(sender, e);
				}
			}
			else
			{
				string openModalScript = String.Format("ShowAddToLightboxModal('{0}', this, true);", (Page is Corbis.Web.UI.ImageGroups.ImageGroups? this.offeringUidHidden.Value: this.OfferingUid.ToString()));
				ScriptManager.RegisterStartupScript(Page, this.GetType(), "openAddToLightboxModalPopup", openModalScript, true);
			}
        }

        protected void addToLightboxBtn_Click(object sender, EventArgs e)
        {
           
            //if (Request.ServerVariables["URL"].ToLower().IndexOf("imagegroups.aspx") > 0)
            //{
                
            //}
            if (commandButton != null && commandButton.Value != null && commandButton.Value == "addToNewLightboxBtn")
            {
                addToNewLightboxBtn_Click(sender, e);
                return;
            }
            //UseDefalutHandler = true;
            if (UseDefalutHandler)
            {
                string selectedLightbox = this.LightboxId.ToString();
                LightboxesPresenter lightboxPresenter = new LightboxesPresenter((IView)Page);

                int mediaSetId;
                bool isMediaSetFlag = int.TryParse(this.offeringUidHidden.Value, out mediaSetId);

                ImageGroupsPresenter imagegroupsPresenter = new ImageGroupsPresenter((IView)Page);
                
                if (Request.ServerVariables["URL"].ToLower().IndexOf("imagegroups.aspx") > 0 && isMediaSetFlag)
                {
					int imageGroupTypeInt;
					ImageMediaSetType imageGroupType = int.TryParse(Request.QueryString["typ"], out imageGroupTypeInt)? (Enum.IsDefined(typeof(ImageMediaSetType), imageGroupTypeInt) ? (ImageMediaSetType)(imageGroupTypeInt) : ImageMediaSetType.Unknown) : ImageMediaSetType.Unknown;
                    if (!string.IsNullOrEmpty(MediaSetId) && !string.IsNullOrEmpty(Request.QueryString["typ"].ToString()) && (int)(ImageMediaSetType.OutlineSession) != int.Parse(Request.QueryString["typ"]) && (int)(ImageMediaSetType.RFCD) != int.Parse(Request.QueryString["typ"]))
                    {
                        imagegroupsPresenter.AddAllMediaSetImagesToLightbox(this.MediaSetId, this.LightboxId);

                    }
                    else if (!IsGuid(OutlineSessionId) && (int)(ImageMediaSetType.OutlineSession) == int.Parse(Request.QueryString["typ"]))
                    {
                        imagegroupsPresenter.AddAllOutlineSessionSetImagesToLightbox(int.Parse(this.OutlineSessionId), this.LightboxId);
                    }
                    else if ((int)(ImageMediaSetType.RFCD) == int.Parse(Request.QueryString["typ"]))
                    {
                        
                        if (IsGuid(RfcdOfferingUid))
                        {
                            imagegroupsPresenter.AddRfcdToLightbox(new Guid(this.RfcdOfferingUid), this.LightboxId);
                        }
                        else if(Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionCoff) && offeringUidHidden.Value.IndexOf("|") > 0)
                        {
                            imagegroupsPresenter.AddAllRfcdImageToLightbox(this.RfcdvolumeNumber, this.LightboxId);
                        }
                    }
                    else if ((int)(ImageMediaSetType.RFCD) == int.Parse(Request.QueryString["typ"]) && !Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionCoff))
                    {
                        imagegroupsPresenter.AddRfcdToLightbox(new Guid(this.RfcdOfferingUid), this.LightboxId);
                    }
                }
                else
                {
                    bool addAllItems = false;
                    bool.TryParse(this.addAllItemsToLightboxHidden.Value , out addAllItems);
                    if (addAllItems)
                    {
                        if (Profile.Permissions.Contains(Corbis.Membership.Contracts.V1.Permission.HasPermissionCoff) && offeringUidHidden.Value.IndexOf("|") > 0)
                        {
                            imagegroupsPresenter.AddAllRfcdImageToLightbox(this.RfcdvolumeNumber, this.LightboxId);
                        }
                    }
                    else
                    {
                        lightboxPresenter.AddToLightBox(Profile.UserName, this.OfferingUid, Profile.CountryCode, Language.CurrentLanguage.LanguageCode, this.LightboxId);
                    }
                }
                this.LightboxList = lightboxPresenter.GetLightboxList(Profile.UserName, false);
                (new StateItemCollection(System.Web.HttpContext.Current)).SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, selectedLightbox, StateItemStore.Cookie));
                this.ActiveLightbox = selectedLightbox;
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "closeAddToLightboxModalPopup", "HideModal('addToLightboxModalPopup');", true);
            }
           if (!string.IsNullOrEmpty(Request.QueryString["ParentPage"]))
            {
                if (Request.QueryString["ParentPage"].IndexOf("ImageGroups") >= 0 && !UseDefalutHandler)
                {
                    string selectedLightbox = this.LightboxId.ToString();
                    LightboxesPresenter lightboxPresenter = new LightboxesPresenter((IView)Page);
                    lightboxPresenter.AddToLightBox(Profile.UserName, this.OfferingUid, Profile.CountryCode, Language.CurrentLanguage.LanguageCode, this.LightboxId);
                    this.LightboxList = lightboxPresenter.GetLightboxList(Profile.UserName, false);
                    (new StateItemCollection(System.Web.HttpContext.Current)).SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, selectedLightbox, StateItemStore.Cookie));
                    this.ActiveLightbox = selectedLightbox;
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "closeAddToLightboxModalPopup", "ParentPageRedirect();", true);
                }            }
           
            if (addToLightboxHandler != null)
            {
               
                addToLightboxHandler(sender, e);
            }
        }

       public static bool IsGuid(string guidString)
        {
        bool bResult=false;
        try
        {
        Guid g = new Guid(guidString);
        bResult=true;
        }
        catch
        {
        bResult=false;
        }

        return bResult;
        }



        public Guid OfferingUid
        {
            get {
                if (this.offeringUidHidden.Value.IndexOf("|") > 0)
                {
                    return new Guid(this.offeringUidHidden.Value.Split('|')[1]);
                }
                return new Guid(this.offeringUidHidden.Value);
            }
        }

        public string RfcdOfferingUid
        {
            get { return this.offeringUidHidden.Value; }
        }
        public string MediaSetId
        {
            get { return this.offeringUidHidden.Value; }
        }

        public string RfcdvolumeNumber
        {
            get {
                if (this.offeringUidHidden.Value.IndexOf("|") > 0)
                {
                    return offeringUidHidden.Value.Split('|')[0];;
                }
                else
                {
                    return this.offeringUidHidden.Value;
                }
            }
        }
        private bool addIndividualItems;

        public bool AddIndividualItems {
            get { return this.addIndividualItems; }
            set { this.addIndividualItems = value; }
        }

        public string OutlineSessionId
        {
            get { return this.offeringUidHidden.Value; }
        }

        public int LightboxId
        {
            get { 
                //Do not return the seleced value because the list is populated everytime during pageload .
                //The reason its populated is because , when anonymous users click on "Add To Lighbox" , the 
                //lightbox dropdown needs to be refreshed and loaded with the actual logged in users lightbox.
                //return int.Parse(lightboxDropDownList.SelectedValue);
                return this.lightBoxId;
            }
            set
            {
                this.lightBoxId = value;
            }
        }

        public string LightboxName
        {
            get { return lightboxName.Text; }
			set { lightboxName.Text = value; }
        }
	    public string NewLightboxName
	    {
            get { return newLightboxName.Value; }
            set { newLightboxName.Value = value; }
	    }
        public List<Lightbox> LightboxList
        {
            get { return (List<Lightbox>)ViewState[this.UniqueID + "_LightboxList"]; }
			set 
			{ 
				ViewState[this.UniqueID + "_LightboxList"] = value;
				if (value != null && value.Count > 0 )
				{
                    AddToLightboxSection.Visible = true;
					lightboxDropDownList.DataSource = LightboxesPresenter.GetLightboxesDropdownSource(value, 1);
					lightboxDropDownList.DataValueField = "Value";
					lightboxDropDownList.DataTextField = "Key";
					lightboxDropDownList.DataBind();
				}
				else
				{
					addToLightboxTitle.Text = GetLocalResourceObject("createNewLightboxLabel.Text").ToString();
					AddToLightboxSection.Visible = false;
				}
			}
        }

		public bool UseDefalutHandler
		{
			get { return _useDefaultHandler; }
			set { _useDefaultHandler = value; }
		}

		public bool PopulateDropdown
		{
			get { return _populateDropdown; }
			set { _populateDropdown = value; }
		}
		
		public string ActiveLightbox
        {
            set
            {
				if (!string.IsNullOrEmpty(value) && lightboxDropDownList.Items.FindByValue(value) != null)
                {
                    lightboxDropDownList.SelectedValue = value;

                }
            }
        }

    }
}

