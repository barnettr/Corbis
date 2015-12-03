using System;
using System.Drawing;
using AjaxControlToolkit;

namespace Corbis.Web.UI.Accounts
{
    public partial class RoundCorners : CorbisBaseUserControl
    {
        public Color BorderColor
        {
            get { return roundCornersExtender.BorderColor; }
            set { roundCornersExtender.BorderColor = value; }
        }

        public Color Color
        {
            get { return roundCornersExtender.Color; }
            set { roundCornersExtender.Color = value; }
        }

        public BoxCorners Corners
        {
            get { return roundCornersExtender.Corners; }
            set { roundCornersExtender.Corners = value; }
        }

        public int Radius
        {
            get { return roundCornersExtender.Radius; }
            set { roundCornersExtender.Radius = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}