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
using Corbis.Web.Utilities;
using Corbis.Web.UI.Lightboxes.ViewInterfaces;
using Corbis.Web.UI.Presenters.Lightboxes;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Lightboxes
{
	public partial class EmailLightbox : CorbisBaseUserControl, ISendEmailLightboxView 
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Page.ClientScript.RegisterClientScriptInclude("EmailLightboxScript", SiteUrls.EmailLightboxScript);
			HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.EmailLightbox, "EmailLightboxCSS");

			fromEmail.Text = Profile.Email;
		}

		protected void sendButton_Click(object sender, EventArgs e)
		{
			to.Text = to.Text.Trim();
			subject.Text = subject.Text.Trim();
			message.Text = message.Text.Trim();

			Page.Validate("emailLightboxValidationGroup");
			if (Page.IsValid)
			{
                ((CorbisBasePage)Page).AnalyticsData["events"] = AnalyticsEvents.LightboxEmail;

                LightboxesPresenter presenter = new LightboxesPresenter(this);
				presenter.EmailLightbox();
				if (ValidationErrors != null)
				{
					((CorbisBasePage)Page).SetValidationErrors(ValidationErrors);
					lightboxLinkDisplay.Text = lightboxLink.Text;
					ScriptManager.RegisterStartupScript(this.emailLightboxUpdatePanel, this.emailLightboxUpdatePanel.GetType(), "resizeEmailLightboxModal", "ResizeModal('emailLightbox');", true);
				}
				else
				{
					ScriptManager.RegisterStartupScript(this.emailLightboxUpdatePanel, this.emailLightboxUpdatePanel.GetType(), "emailLightboxSuccess", "CorbisUI.EmailLightbox.Handler.showSuccessModal();", true);
				}
			}
			else
			{
				lightboxLinkDisplay.Text = lightboxLink.Text;
				ScriptManager.RegisterStartupScript(this.emailLightboxUpdatePanel, this.emailLightboxUpdatePanel.GetType(), "resizeEmailLightboxModal", "ResizeModal('emailLightbox');", true);
			}
		}

		public string LightboxLinkTemplate
		{
			get { return SiteUrls.EmailLightboxView + "?uid={0}"; }
		}

		#region ISendEmailLightboxView Members

		public int LightboxId
		{
			get 
			{	
				int parsedLightboxId;
				return int.TryParse(lightboxId.Text, out parsedLightboxId) ? parsedLightboxId : 0;
			}
		}

		public string LightboxLink
		{
			get { return lightboxLink.Text; }
		}

		public string FromEmail
		{
			get { return Profile.Email; }
		}

		public string ToEmails
		{
			get { return to.Text; }
		}

		public string Subject
		{
			get { return subject.Text; }
		}

		public string Message
		{
			get { return message.Text; }
		}

		#endregion
	}
}