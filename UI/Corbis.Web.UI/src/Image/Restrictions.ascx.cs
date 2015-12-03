using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Image.Contracts.V1;
using Corbis.Framework.Globalization;
using Corbis.CommonSchema.Contracts.V1;

namespace Corbis.Web.UI.Image
{
    public partial class Restrictions : CorbisBaseUserControl, Corbis.Web.UI.ViewInterfaces.IImageRestrictionsView
    {
        
        #region Public Properties

        public bool ShowHeader
        {
            set
            {
                this.headerDiv.Visible = value;
            }
        }

        #endregion

        #region IRestrictionsView members
        
        private bool usePopupMode = false;
        public bool UsePopupMode
        {
            set
            {
                usePopupMode = value;
            }
            get
            {
                return usePopupMode; 
            }
        }
        
        public Language LanguageName
        {
            set
            {
                if ((value == Language.ChineseSimplified || value == Language.Japanese) && this.usePopupMode == true)
                {
                    popupMode.Visible = true;
                    inlineMode.Visible = false;
                }
                else
                {
                    popupMode.Visible = false;
                    inlineMode.Visible = true;
                }
                if (!string.IsNullOrEmpty(Request.QueryString["print"]))
                {
                    popupMode.Visible = false;
                    inlineMode.Visible = true;
                }
            }

        }

        public ModelRelease ModelRelease
        {
            set 
            {
                string displayText = CorbisBasePage.GetEnumDisplayText<ModelRelease>(value);
                if (inlineMode.Visible)
                {
                    modelReleaseText.Text = displayText;
                }
                else
                {
                    popupModelReleaseText.Text = displayText;
                }
            }
        }

        public string PropertyReleaseText
        {
            set 
            { 
                string displayText = (string)GetLocalResourceObject(value);
                if (inlineMode.Visible)
                {
                    propertyReleaseText.Text = displayText;
                }
                else
                {
                    popupPropertyReleaseText.Text = displayText;
                }
            }
        }

        public bool ShowDomesticEmbargoDate
        {
            set 
            { 
                domesticEmbargo.Visible = value;
                popupDomesticEmbargo.Visible = value;
            }
        }

        public string DomesticEmbargoDate
        {
            set
            {
                if (inlineMode.Visible)
                {
                    domesticEmbargoDate.Text = value;
                }
                else
                {
                    popupDomesticEmbargoDate.Text = value;
                }
            }
        }
        
        public bool ShowInternationalEmbargoDate
        {
            set
            {
                internationalEmbargo.Visible = value;
                popupInternationalEmbargo.Visible = value;
            }
        }

        public string InternationalEmbargoDate
        {
            set
            {
                if (inlineMode.Visible)
                {
                    internationalEmbargoDate.Text = value;
                }
                else
                {
                    popupInternationalEmbargoDate.Text = value;
                }
            }
        }

        public bool ShowPricingIcon
        {
            set
            {
                pricingIcon.Visible = value;
                popupPricingIcon.Visible = value;
            }
        }

        public string PricingIconUrl
        {
            set
            {
                if (inlineMode.Visible)
                {
                    pricingIcon.ImageUrl = value;
                }
                else
                {
                    popupPricingIcon.ImageUrl = value;
                }
            }
        }

        public List<Restriction> RestrictionsDataSource
        {
            set
            {
                if (inlineMode.Visible)
                {
                    restrictionsList.DataSource = value;
                    restrictionsList.DataTextField = "Notice";
                    restrictionsList.DataBind();
                }
                else
                {
                    popupRestrictionsList.DataSource = value;
                    popupRestrictionsList.DataTextField = "Notice";
                    popupRestrictionsList.DataBind();
                }
            }
        }

        #endregion
    }
}
