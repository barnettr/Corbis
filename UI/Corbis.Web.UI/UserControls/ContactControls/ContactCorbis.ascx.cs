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
using Resources;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.CommonUserControls
{
	/// <summary>
	/// Control for displaying Contact Corbis modal
	/// 
	/// To use:
	///		1. Register control on page
	///			<%@ Register TagPrefix="Corbis" TagName="ContactCorbis" Src="~/UserControls/ContactControls/ContactCorbis.ascx" %>
	///		2. Add control on page
	///			<Corbis:ContactCorbis runat="server" />
	///		3. Add javascript to element that should activate modal, and set unique id for element
	///			eg. <a id="rfPricingContactLink" href="javascript:void(0);" onclick="javascript:CorbisUI.ContactCorbis.ShowContactCorbisModal(this); return false;">contact Customer Service</a>
	///		4. In GetContactCorbisOfficeSucceeded in ModalPopup.js set the location of how the modal will be display in the swicth statement.
	/// </summary>
	public partial class ContactCorbis : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			ScriptManager manager = ScriptManager.GetCurrent(Page);
			manager.Services.Add(new ServiceReference("~/CustomerService/CustomerServiceWebService.asmx"));
        }
    }
}