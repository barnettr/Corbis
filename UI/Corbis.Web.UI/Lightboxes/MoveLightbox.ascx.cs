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
using Corbis.Web.Utilities.StateManagement;
using Corbis.LightboxCart.Contracts.V1;
using System.Collections.Generic;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Lightboxes
{
    public partial class MoveLightbox : CorbisBaseUserControl, IMoveLightboxView
    {
        private LightboxesPresenter lightboxPresenter = null;
        private IMyLightboxesView myLightboxView = null;  
     
        protected void Page_Load(object sender, EventArgs e)
        {            
            Page.ClientScript.RegisterClientScriptInclude("MoveLightboxScript", SiteUrls.MoveLightboxScript);
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.MyLightboxes, "MoveLightboxCSS");
            lightboxPresenter = new LightboxesPresenter(this);
            myLightboxView = Page as IMyLightboxesView;
            lightboxPresenter.LoadLightboxesToMoveTo();
            ddlLightbox.SelectedValue = Request.Form[ddlLightbox.ClientID.Replace('_', '$')];
            _newParentLightboxId = int.Parse(ddlLightbox.SelectedValue);
        }

        #region Event Handlers

        protected void moveButton_Click(object sender, EventArgs e)
        {           
            lightboxPresenter.MoveLightbox();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "closeModal", "reloadParentWindow();", true);
            ddlLightbox.SelectedIndex = 0;    
        }
        # endregion
        
        #region IMoveLightboxView Members

       
        public int LightboxIdToMove
        {
            get
            {
                return myLightboxView.LightboxId;
            }
        }

        int _newParentLightboxId;
        public int? NewParentLightboxId
        {
            get
            {
                return _newParentLightboxId > 1 ? (int?)_newParentLightboxId : null;
            }
        }

        public LightboxTreeSort LightboxTreeSortBy
        {
            get
            {
                return myLightboxView.LightboxTreeSortBy;
            }
        }

        public List<Lightbox> Lightboxes
        {
            set
            {                
                if (value != null && value.Count > 0)
                {                    
                    ddlLightbox.DataSource = value;
                    ddlLightbox.DataValueField = "LightboxId";
                    ddlLightbox.DataTextField = "LightboxName";
                    ddlLightbox.DataBind();
                }
                ddlLightbox.Items.Insert(0, new ListItem((string)GetLocalResourceObject("Selectone.Text"), "0"));                
                ddlLightbox.Items.Insert(1, new ListItem((string)GetLocalResourceObject("MovetoTop.Text"), "1"));
                ddlLightbox.Items[1].Attributes.Add("style", "font-style:italic;" + ddlLightbox.Items[1].Text);                
            }
        }
        #endregion
    }

	
}