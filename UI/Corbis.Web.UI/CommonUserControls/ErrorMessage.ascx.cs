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

namespace Corbis.Web.UI.CommonUserControls
{
    public partial class ErrorMessage : CorbisBaseUserControl
    {
        private string validationGroupText;
       
        public string ValidationGroup
        {

            get
            {

                return validationGroupText;
            }
            set
            {

                validationGroupText = value;
            }

        }

        public bool DisplayError
        {

            get
            {
                return errorControlDiv.Visible;
            }

            set
            {
                errorControlDiv.Visible = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            validationSummaryGroup.ValidationGroup = validationGroupText;           

        }

    }
}