using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.Utilities;
using Corbis.Web.Authentication;

namespace Corbis.Web.UI
{
    public partial class BetaFeedback : CorbisBasePage
    {
        public string myBetaUsername = "";
        public string myUsername = "";
        public string myEmail = "";
        protected override void OnInit(EventArgs e)
        {
            HtmlHelper.CreateStylesheetControl(this.Page, Stylesheets.Beta, "Beta");
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            eulaModal.Title = Resources.Legal.BetaSiteUsageAgreementTitle;
            lblEulaText.Text = Resources.Legal.BetaSiteUsageAgreementText;

            //this.SetAnalytics("LND:CustomerService:Beta:Feedback");

            myBetaUsername = BetaSiteAuthentication.BetaAccessUsername;
            myUsername = Profile.UserName;
            myEmail = Profile.Email;

            ReadContent(Server.MapPath(@"../Content/BetaFAQ.xml"));
        }

        public void ReadContent(string filename)
        {
            XDocument content = XDocument.Load(filename);

            var updates = from c in content.Descendants("Update")
                          select new
                          {
                              Update = c.Value
                          };

            this.UpdateMessages = updates;

            var faqs = from c in content.Descendants("FAQ")
                       select new
                       {
                           Question = c.Element("Question").Value,
                           Answer = c.Element("Answer").Value
                       };
            this.Faqs = faqs;
        }

        public object UpdateMessages
        {
            set
            {

                updatesRepeater.DataSource = value;
                updatesRepeater.DataBind();
            }
        }

        public object Faqs
        {
            set
            {
                faqAccordion.DataSource = value;
                faqAccordion.DataBind();
            }
        }
    }
}
