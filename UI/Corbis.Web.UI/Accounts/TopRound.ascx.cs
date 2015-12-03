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
using System.Drawing;
namespace Corbis.Web.UI.Accounts
{
    public partial class TopRound : CorbisBaseUserControl
    {
        private string borderColor = string.Empty;
        private string color = string.Empty;
        private int radius;

        public string BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!string.IsNullOrEmpty(BorderColor))
            {
                this.topRoundCorner_Extender.BorderColor = System.Drawing.ColorTranslator.FromHtml(BorderColor);
            }
            if (!string.IsNullOrEmpty(Color))
            {
                this.topRoundCorner_Extender.Color = System.Drawing.ColorTranslator.FromHtml(Color);
            }
            if (Radius != 0)
            {
                this.topRoundCorner_Extender.Radius = Radius;
            }
        }
    }
}