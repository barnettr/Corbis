using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Corbis.Web.UI.Controls
{
	public class COFFModalPopup : ModalPopup, INamingContainer
	{
        private string _itemCountText;
        public string ItemCountText
        {
            get { return _itemCountText; }
            set { _itemCountText = value; }
        }
        /**
         * Add the item selection count to the title bar
         **/
        public override void CustomizeTitleBarControl(HtmlGenericControl titleBar)
        {
            HtmlGenericControl countDiv = new HtmlGenericControl("div");
            countDiv.Attributes.Add("class", "coffSelectedItemCount");

            HtmlGenericControl coffSelectedItemCount = new HtmlGenericControl("span");
            coffSelectedItemCount.InnerText = ItemCountText;
            coffSelectedItemCount.ID = "CoffSelectedItemCountSpanLayer";
            countDiv.Controls.Add(coffSelectedItemCount);
            titleBar.Controls.Add(countDiv);
        }
	}
}