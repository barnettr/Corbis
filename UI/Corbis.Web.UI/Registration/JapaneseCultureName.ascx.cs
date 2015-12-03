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
using Corbis.Web.UI.Registration.ViewInterfaces;
using Corbis.Web.UI.Presenters.Registration;
using Corbis.Web.UI.Properties;
using Corbis.Web.Entities;
using System.Collections.Generic;
using Corbis.Web.Content;
using Corbis.DisplayText.Contracts.V1;
using Corbis.DisplayText.ServiceAgents.V1;
using System.Globalization;
using Corbis.Web.Validation;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Registration
{
    public partial class JapaneseCultureName : CorbisBaseUserControl, ICultureSpecificNameView, IViewPropertyValidator
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [PropertyControlMapper("furiganaFirstName")]
        public string FuriganaFirstName
        {
            get { return furiganaFirstName.Text; }
        }

        [PropertyControlMapper("furiganaLastName")]
        public string FuriganaLastName
        {
            get { return furiganaLastName.Text; }
        }

        [PropertyControlMapper("lastName")]
        public string LastName
        {
            get { return lastName.Text; }
        }

        [PropertyControlMapper("firstName")]
        public string FirstName
        {
            get { return firstName.Text; }
        }
    }
}