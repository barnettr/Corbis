using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Corbis.Web.Utilities.StateManagement;


namespace Corbis.Web.UI.Controls
{
    public class LightboxTreeItemTemplate : ITemplate
    {
        private HtmlGenericControl lightboxRow;
        private HtmlGenericControl deleteIconDiv;
        private StateItemCollection stateItems;

        public void InstantiateIn(Control container)
        {
            /* Sample HTML
            <div class="Lightbox">
                <div onclick="javascript:ToggleTree(this.parentNode);" class="Arrow">
                    <img />
                </div>
                <div class="LightboxRow" onclick="javascript:GetLB(this);" title="SomeLightboxName" shared="False" id="7802410">
                    <div class="Details">
                        <span class="LightboxName">SomeLightboxName</span>
                        <div>
                            <span class="imageCount">129 Items,</span>1 Lightbox
                        </div>
                        <div class="modifiedDate">Modified 4/6/2009</div>
                    </div>
                    <div class="DeleteLB">
                        <div onclick="ShowDelete(7802410,1,'False','False');"></div>
                    </div>
                    <div class="clear"></div>
                </div>
                <div class="Children" style="display:none;" parentid="7802410">
                    <div class="Lightbox">
                        <div class="LightboxRow" onclick="javascript:GetLB(this);" title="01a" shared="False" id="7805599">
                            <div class="Details">
                                <span class="LightboxName">01a</span>
                                <div>
                                    <span id="ctl00_mainContent_lightboxTree_lightboxes_ctl69_ctl14_lightboxes_ctl01_ctl21" class="imageCount">2 Items</span>
                                </div>
                                <div class="modifiedDate">Modified 4/6/2009</div>
                            </div>
                            <div class="DeleteLB">
                                <div onclick="ShowDelete(7805599,0,'False','False');"></div>
                            </div>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
            </div>
            */

            HtmlGenericControl lightbox = new HtmlGenericControl("div");
            lightbox.Attributes["class"] = "Lightbox";

            stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            lightbox = new HtmlGenericControl("div");
            lightbox.Attributes["class"] = "Lightbox";            
            lightboxRow = new HtmlGenericControl("div");
			lightboxRow.Attributes["class"] = "LightboxRow";
            lightboxRow.Attributes["onclick"] = "javascript:GetLB(this);";
            HtmlGenericControl div = new HtmlGenericControl("div");
			div.EnableViewState = false;
			div.Attributes["onclick"] = "javascript:ToggleTree(this.parentNode);";
            div.Attributes["class"] = "Arrow";
            HtmlImage toggle = new HtmlImage();
            toggle.Src = "../../Images/iconCaretCollapsed.gif";

            div.Controls.Add(toggle);
            lightbox.Controls.Add(div);

            HtmlGenericControl details = new HtmlGenericControl("div");
			details.EnableViewState = false;
			details.Attributes["class"] = "Details";

            div = new HtmlGenericControl("span");
			div.EnableViewState = false;
			div.Attributes["class"] = "LightboxName";
			Localize localize = new Localize();
			localize.ID = "lightboxName";
			div.Controls.Add(localize);
			details.Controls.Add(div);

            div = new HtmlGenericControl("div");
			div.EnableViewState = false;
			HtmlGenericControl countSpan = new HtmlGenericControl("span");
            countSpan.Attributes["class"] = "imageCount";
            localize = new Localize();
            localize.ID = "counts";
            countSpan.Controls.Add(localize);

			div.Controls.Add(countSpan);
			localize = new Localize();
			localize.ID = "childCount";
			div.Controls.Add(localize);
            details.Controls.Add(div);

            div = new HtmlGenericControl("div");
			div.EnableViewState = false;
            div.Attributes["class"] = "modifiedDate";
            localize = new Localize();
            localize.ID = "changedAt";
            div.Controls.Add(localize);
            details.Controls.Add(div);

            div = new HtmlGenericControl("div");
			div.EnableViewState = false;
            localize = new Localize();
            localize.ID = "sharedBy";
            div.Controls.Add(localize);
            details.Controls.Add(div);

            deleteIconDiv = new HtmlGenericControl("div");
			deleteIconDiv.EnableViewState = false;
			deleteIconDiv.Attributes["class"] = "DeleteLB";

            HtmlGenericControl deleteIcon = new HtmlGenericControl("div");
            deleteIconDiv.Controls.Add(deleteIcon);

            HtmlGenericControl clearDiv = new HtmlGenericControl("div");
			clearDiv.EnableViewState = false;
			clearDiv.Attributes["class"] = "clear";

            lightboxRow.Controls.Add(details);
            lightboxRow.Controls.Add(deleteIconDiv);
            lightboxRow.Controls.Add(clearDiv);

            lightbox.Controls.Add(lightboxRow);

            div = new HtmlGenericControl("div");
            div.EnableViewState = false;
            div.Attributes["class"] = "ChildrenDiv";
            LightboxTree childrenDivTree = new LightboxTree();
            div.Controls.Add(childrenDivTree);

            div = new HtmlGenericControl("div");
			div.EnableViewState = false;
			div.Attributes["class"] = "Children";
            div.Style[HtmlTextWriterStyle.Display] = "none";
			LightboxTree childrenTree = new LightboxTree();
			div.Controls.Add(childrenTree);
            lightbox.Controls.Add(div);
            container.Controls.Add(lightbox);
        }
    }
}
