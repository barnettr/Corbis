using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Lightboxes.ViewInterfaces;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Utilities;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Lightboxes
{
    public partial class CreateLightbox : Corbis.Web.UI.CorbisBaseUserControl
    {
		private EventHandler createLightbox;

		public event EventHandler CreateLightboxHandler
		{
			add { createLightbox += value; }
			remove { createLightbox -= value; }
		}

		protected void Create_Click(object sender, EventArgs e)
		{
			Page.Validate("CreateLightbox");
            
			if (Page.IsValid)
			{
				Corbis.Web.UI.Presenters.Lightboxes.LightboxesPresenter presenter = new Corbis.Web.UI.Presenters.Lightboxes.LightboxesPresenter((IView)Page);
				int newLightboxId = presenter.CreateLightbox(Profile.UserName, this.NewLightboxName);
            
				if (ValidationErrors != null && ValidationErrors.Count > 0)
				{
					ValidatorCollection groupValidators = new ValidatorCollection();
					foreach (BaseValidator validator in Page.Validators)
					{
						if (validator.ValidationGroup == "CreateLightbox")
						{
							groupValidators.Add(validator);
						}
					}

					((CorbisBasePage)Page).SetValidationErrors(ValidationErrors, groupValidators);
					ScriptManager.RegisterStartupScript(Page, this.GetType(), "openCreateLightboxModalPopup", "ResizeModal('createLightboxModalPopup');", true);
				}
				else
				{
					StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
					stateItems.SetStateItem<string>(new StateItem<string>(LightboxCartKeys.Name, LightboxCartKeys.LightboxIdKey, newLightboxId.ToString(), StateItemStore.Cookie));
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "closeCreateLightboxModalPopup", "HideModal('createLightboxModalPopup');createLightbox_success();", true);

					if (createLightbox != null)
					{
                        ((CorbisBasePage)Page).AnalyticsData["events"] = AnalyticsEvents.LightboxCreate;
                        createLightbox(sender, new NewLightboxEventArgs(newLightboxId, this.NewLightboxName));
					}

					//clear textbox for next entry
					this.NewLightboxName = "";
				}
			}
			else
			{
				//need to decode new name to display properly
				lightboxName.Text = Server.HtmlDecode(lightboxName.Text);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "openCreateLightboxModalPopup", "ResizeModal('createLightboxModalPopup');", true);
			}
		}

        protected void setValidLink_Click(object sender, EventArgs e)
        {
            Page.GetValidators(createLightboxValidationSummary.ValidationGroup)[0].IsValid = true;
        }

		public string NewLightboxName
		{
			get { return lightboxName.Text; }
			set { lightboxName.Text = value; }
		}
    }

	public class NewLightboxEventArgs : EventArgs
	{
		public NewLightboxEventArgs(int newLightboxId, string newLightboxName)
		{
			NewLightboxId = newLightboxId;
			NewLightboxName = newLightboxName;
		}

		#region Event properties

		private int _newLightboxId;
		public int NewLightboxId
		{
			get { return _newLightboxId; }
			set { _newLightboxId = value; }
		}

		private string _newLightboxName;
		public string NewLightboxName
		{
			get { return _newLightboxName; }
			set { _newLightboxName = value; }
		}

		#endregion
	}
}