using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.Membership.Contracts.V1;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Office.Contracts.V1;
using Corbis.Web.UI.Presenters.Checkout;


namespace Corbis.Web.UI.Checkout
{
    public partial class StepDelivery : Corbis.Web.UI.CorbisBaseUserControl, IDelivery
    {
        private Guid _selectedAddress = Guid.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            //addressList.DataSource = addresses;
            //addressList.DataBind();
            //editShippingAddress2.DisplayDeleteSection(false, false, Guid.Empty);
            //for updatepanel triggered postback, we need to reload the address list
        }

        protected void UpdateData()
        {
            List<MemberAddress> addresses = new List<MemberAddress>();
            List<ShippingPriority> priorities = new List<ShippingPriority>();
            for (int i = 0; i < 3; i++)
            {
                MemberAddress address = new MemberAddress();
                address.Address1 = "address1 " + i.ToString();
                address.Address2 = "address2 " + i.ToString();
                address.Address3 = "address3 " + i.ToString();
                address.RegionCode = "RegionCode " + i.ToString();
                address.AddressUid = Guid.NewGuid();
                address.City = "city " + i.ToString();
                address.PostalCode = "PostalCode " + i.ToString();
                address.CountryCode = "countrycode " + i.ToString();
                address.Name = "my name " + i.ToString();
                address.CompanyName = "companyName " + i.ToString();
                address.IsDefaultForType = (i == 0) ? true : false;
                addresses.Add(address);

                ShippingPriority priority = new ShippingPriority();
                priority.ShippingPriorityCode = "code " + i.ToString();
                priority.ShippingPriorityDisplayText = "display Text " + i.ToString();
                priorities.Add(priority);

            }
            addressList.DataSource = addresses;
            addressList.DataBind();
        }

        // TODO: Replace these values with page control values
        #region IDelivery Members

        public bool IsCoffRMResolution1280
        {
            get
            {
                return this.rmResolutionPreference.Checked;
            }
            set
            {
                this.rmResolutionPreference.Checked = value;
            }
        }
        public string SpecialInstruction
        {
            get
            {
                return this.deliverySpecialInstructions.Text;
            }
            set
            {
                this.deliverySpecialInstructions.Text = value;
            }
        }
        public string DeliverySubject
        {
            get
            {
                return this.deliverySubject.Text;
            }
            set
            {
                this.deliverySubject.Text = value;
            }
        }
        private DeliveryMethod rfcdDeliveryMethod = DeliveryMethod.Unknown;

        public DeliveryMethod RfcdDeliveryMethod
        {
            get
            {
                return rfcdDeliveryMethod;
            }
            set
            {
                rfcdDeliveryMethod = value;
            }
        }

        public string DeliveryEmails
        {
            set { this.deliveryEmails.Text = value; }
            get { return this.deliveryEmails.Text; }
        }

        // TODO: ShippingPriorityCodes need to be localized in Resource Files
        private string shippingPriority;
        public string ShippingPriority
        {
            get
            {
                return shippingPriority;
            }
            set
            {
                shippingPriority = value;
            }
        }

        private MemberAddress shippingAddress;
        public MemberAddress ShippingAddress
        {
            get
            {
                return shippingAddress;
            }
            set
            {
                shippingAddress = value;
            }
        }
        private Guid shippingAddressUid;
        public Guid ShippingAddressUid
        {
            get
            {
                return shippingAddressUid;
            }
            set
            {
                shippingAddressUid = value;
            }
        }

        private DeliveryMethod nonRfcdDeliveryMethod = DeliveryMethod.Unknown;
        public DeliveryMethod NonRfcdDeliveryMethod
        {
            get
            {
                foreach (DeliveryMethod method in Enum.GetValues(typeof(DeliveryMethod)))
                {
                    if (this.optionsBlockSelected.Value.Contains(method.ToString()))
                        return method;
                }
                return DeliveryMethod.Unknown;
                //if (this.optionsBlockSelected.Value.Contains("Download"))
                //    return DeliveryMethod.Download;
                //if (this.optionsBlockSelected.Value.Contains("CD"))
                //    return DeliveryMethod.CD;
                //if (this.optionsBlockSelected.Value.Contains("deliveryOptionsFTP"))
                //    return DeliveryMethod.CD;
                //return nonRfcdDeliveryMethod;
            }
            set
            {
                nonRfcdDeliveryMethod = value;
            }
        }
        public List<DeliveryMethod> AvailableNonRfcdDeliveryMethods
        {
            get
            {
                return null;
            }
            set
            {

                List<DeliveryMethod> methods = value;
                //List<DeliveryMethodUI> uiMethods = new List<DeliveryMethodUI>();

                if (methods.Count > 0)
                {
                    optionsBlock.Visible = true;
                }
                else
                {
                    optionsBlock.Visible = false;
                    return;
                }

                methods.ForEach(delegate(DeliveryMethod method)
                                    {
                                        switch (method)
                                        {
                                            case DeliveryMethod.Download:
                                                deliveryOptionsDownload.Visible = true;
                                                break;
                                            case DeliveryMethod.CD:
                                                deliveryOptionsCD.Visible = true;
                                                //deliveryCharges.Visible = true;
                                                break;
                                            case DeliveryMethod.Email:
                                                deliveryOptionsEmail.Visible = true;
                                                //deliveryCharges.Visible = true;
                                                break;
                                            case DeliveryMethod.FTP:
                                                deliveryOptionsFTP.Visible = true;
                                                break;
                                            default:
                                                break;
                                        }

                                    });
             }
        }
        //private List<DeliveryMethod> availablerfcdDeliveryMethods;
        public List<DeliveryMethod> AvailableRfcdDeliveryMethods
        {
            get
            {
                return null;
            }
            set
            {
                List<DeliveryMethod> methods = value;
                //List<DeliveryMethodUI> uiMethods = new List<DeliveryMethodUI>();

                if (methods.Count > 0)
                {
                    optionsBlockRFCD.Visible = true;
                }
                else
                {
                    optionsBlockRFCD.Visible = false;
                    return;
                }

                methods.ForEach(delegate(DeliveryMethod method)
                {
                    switch (method)
                    {
                        case DeliveryMethod.Download:
                            deliveryOptionsDownloadRFCD.Visible = true;
                            break;
                        case DeliveryMethod.CD:
                            deliveryOptionsCDRFCD.Visible = true;
                            //deliveryCharges.Visible = true;
                            break;
                        case DeliveryMethod.Email:
                            deliveryOptionsEmailRFCD.Visible = true;
                            //deliveryCharges.Visible = true;
                            break;
                        case DeliveryMethod.FTP:
                            deliveryOptionsFTPRFCD.Visible = true;
                            break;
                        default:
                            break;
                    }

                });
            }
        }
        //private List<MemberAddress> savedShippingAddresses;
        public List<MemberAddress> SavedShippingAddresses
        {
            get
            {
                return null;
            }
            set
            {
                addressList.DataSource = value;
                addressList.DataBind();
                if (value != null && value.Count > 0)
                {
                    noSavedAddress.Visible = false;
                }
                //savedShippingAddresses = value;
            }
        }
        public Guid SelectedAddress
        {
            set
            {
                _selectedAddress = value;
            }
        }
        //private List<ShippingPriority> shippingPriorities;
        public List<ShippingPriority> ShippingPriorities
        {
            get
            {
                return null;
            }
            set
            {
                shippingPriorityCtrl.DataTextField = "ShippingPriorityDisplayText";
                shippingPriorityCtrl.DataValueField = "ShippingPriorityCode";
                shippingPriorityCtrl.DataSource = value;
                shippingPriorityCtrl.DataBind();
                shippingPriorityCtrl.SelectedIndex = 0;

            }
            
        }
        #endregion

        protected void testBtn_OnClick(object sender, EventArgs e)
        {
            //Debug.WriteLine("selected index=" + optionsList.SelectedIndex.ToString()); 
        }
        protected void editShippingAddress2_ShippingAddressAdded(object sender, CommandEventArgs e)
        {
            //shipping priority popup need to do updatepanel.update here
            Debug.WriteLine("the event is triggered");
            //UpdateData();
            //CheckoutPresenter presenter = new CheckoutPresenter(null);
            ((MainCheckout) this.Page).CheckoutPresenter.GetShippingAddresses((Guid)e.CommandArgument);

            shippingSelectPopup.Update();
        }
        protected void optionsList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            ListViewDataItem item = (ListViewDataItem)e.Item;
            DeliveryMethodUI data = (DeliveryMethodUI)item.DataItem;
            //if (data.Key == DeliveryMethod.Email || data.Key == DeliveryMethod.CD)
            //{
            //    deliveryCharges.Visible = true;
            //}

        }

        protected void addressList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) 
                return;
            RadioButton rb = (RadioButton)e.Item.FindControl("btnKey");
            if (this._selectedAddress != Guid.Empty)
            {
                rb.Checked = (((MemberAddress)e.Item.DataItem).AddressUid == _selectedAddress);
            }
            else
            {
                rb.Checked = (e.Item.ItemIndex == 0);
            }
            string script = "SetUniqueRadioButton('addressList.*addressGroup',this)";
            rb.Attributes.Add("onclick", script);
        }
        
    }
    

    /// <summary>
    /// this is a helper class for databind
    /// </summary>
    class DeliveryMethodUI
    {
        public DeliveryMethod Key { get; set; }
        public string Value { get; set; }
    }
}
