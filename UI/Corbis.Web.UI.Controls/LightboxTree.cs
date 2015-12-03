using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Web.Utilities.StateManagement;
using System.Text.RegularExpressions;

namespace Corbis.Web.UI.Controls
{    
    public partial class LightboxTree : UserControl
    {
        private string countsSeparator = string.Empty;
		private string oneChildCountFormat = string.Empty;
		private string childrenCountFormat = string.Empty;
		private string oneItemCountFormat = string.Empty;
		private string itemCountFormat = string.Empty;
		private string sharedByFormat = string.Empty;
		private string updatedFormat = string.Empty;
		private int selectedLightboxId;
		private string selectedLightboxName;

        public string CountsSeparator
        {
            get { return countsSeparator; }
            set { countsSeparator = value; }
        }

		public string OneItemCountFormat
		{
			get { return oneItemCountFormat; }
			set { oneItemCountFormat = value; }
		}

		public string ItemCountFormat
		{
			get { return itemCountFormat; }
			set { itemCountFormat = value; }
		}
        
        public string OneChildCountFormat
        {
            get { return oneChildCountFormat; }
            set { oneChildCountFormat = value; }
        }

        public string ChildrenCountFormat
        {
            get { return childrenCountFormat; }
            set { childrenCountFormat = value; }
        }

        public string SharedByFormat
        {
            get { return sharedByFormat; }
            set { sharedByFormat = value; }
        }

		public string UpdatedFormat
		{
			get { return updatedFormat; }
			set { updatedFormat = value; }
		}

		public int SelectedLightboxId
		{
			get { return selectedLightboxId; }
			set { selectedLightboxId = value; }
		}

		public string SelectedLightboxName
		{
			get { return selectedLightboxName; }
			set { selectedLightboxName = value; }
		}

        public bool SelectedLightboxShareBy
        {
            get;
            set;
        }

		private Repeater lightboxes;
        public Repeater Lightboxes
        {
            get
            {
                EnsureChildControls();
                return lightboxes;
            }
            set
            {
                EnsureChildControls();
                lightboxes = value;
            }
        }

		private List<Lightbox> dataSource;
		public List<Lightbox> DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        private LightboxTreeItemTemplate itemTemplate;
        public LightboxTreeItemTemplate ItemTemplate
        {
            get
            {
                if (itemTemplate == null)
                {
                    itemTemplate = new LightboxTreeItemTemplate();
                }
                return itemTemplate;
            }
            set { itemTemplate = value; }
        }

        public Dictionary<int, Lightbox> LightboxDirectory { get; set; }

        private RepeaterCommandEventHandler itemCommand;
		public event RepeaterCommandEventHandler ItemCommand
        {
            add { itemCommand += value; }
            remove { itemCommand -= value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Lightboxes.ItemDataBound += new RepeaterItemEventHandler(Lightboxes_ItemDataBound);
            
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            lightboxes = new Repeater();
            lightboxes.ID = "lightboxes";
            lightboxes.ItemTemplate = ItemTemplate;
			lightboxes.ItemCommand += new RepeaterCommandEventHandler(Lightboxes_ItemCommand);
            Controls.Add(lightboxes);
        }

        public override void DataBind()
        {
            EnsureChildControls();
            base.DataBind();
			Lightboxes.DataSource = DataSource;
            Lightboxes.DataBind();
        }

		protected void Lightboxes_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			Lightbox lightbox = e.Item.DataItem as Lightbox;
			int lightboxCount = (lightbox.LightboxChildren == null ? 0 : lightbox.LightboxChildren.Count);
			Localize lightboxName = e.Item.FindControl("lightboxName") as Localize;
            Localize counts = e.Item.FindControl("counts") as Localize;
			Localize childCount = e.Item.FindControl("childCount") as Localize;
			Localize changedAt = e.Item.FindControl("changedAt") as Localize;
			Localize sharedBy = e.Item.FindControl("sharedBy") as Localize;

			//setting up sorting properties
			HtmlGenericControl lightboxItem = e.Item.Controls[0] as HtmlGenericControl;
            // Encode the lightbox name before display.
            string lightboxNameDisplay = System.Web.HttpUtility.HtmlEncode(lightbox.LightboxName);

			foreach (Control control in lightboxItem.Controls)
			{
				switch (((HtmlGenericControl)control).Attributes["class"])
				{
					case "Arrow":
						control.Visible = (lightbox.LightboxChildren != null && lightbox.LightboxChildren.Count > 0);

						break;
					case "LightboxRow":
						HtmlGenericControl lightboxRow = control as HtmlGenericControl;
						lightboxRow.Attributes["title"] = lightbox.LightboxName;
                        lightboxRow.Attributes["shared"] = (!String.IsNullOrEmpty(lightbox.SharedBy) && !lightbox.Writtable).ToString();
						lightboxRow.Attributes["id"] = lightbox.LightboxId.ToString();
						HtmlGenericControl deleteDiv = (HtmlGenericControl)lightboxRow.Controls[1];
                        bool IsSharedFromParent = CheckForSharedParent(lightbox.LightboxId);
                        bool IsLightboxOwnedAndShared = IsLightboxOwnedAndSharedOut(lightbox);

                        ((HtmlGenericControl)deleteDiv.Controls[0]).Attributes["onclick"] = String.Format("ShowDelete({0},{1},'{2}','{3}');", lightbox.LightboxId.ToString(), lightboxCount.ToString(), IsSharedFromParent, IsLightboxOwnedAndShared);

						if (lightbox.LightboxId == SelectedLightboxId)
						{
							//Showing the delete light box icon and seting the style class of active lightbox 
							deleteDiv.Attributes["style"] = "display:block;";
							lightboxRow.Attributes.Add("class", "Active");
                            
							SelectedLightboxName = lightboxNameDisplay;
						}

                        //Check if ligthbox is shared and not owner
                        if (IsSharedFromParent)
                        {
                            //Have to add the icon because IE7 does not give opacity to background images
                            HtmlImage deleteIcon = new HtmlImage();
                            deleteIcon.Src = "../Images/iconDelete.gif";
                            deleteIcon.Attributes["class"] += "disabled";
                            HtmlGenericControl deleteDivChild = (HtmlGenericControl)deleteDiv.Controls[0];
                            deleteDivChild.Style["background-image"] = "none;";
                            deleteDivChild.Controls.Add(deleteIcon);
                        }
                        
                        break;
					case "Children":
						// Add children & Expand/collapse image
						HtmlGenericControl children = control as HtmlGenericControl;
						if (lightboxCount > 0)
						{
							LightboxTree childrenTree = children.Controls[0] as LightboxTree;
							childrenTree.CountsSeparator = countsSeparator;
							childrenTree.OneItemCountFormat = oneItemCountFormat;
							childrenTree.ItemCountFormat = itemCountFormat;
							childrenTree.OneChildCountFormat = oneChildCountFormat;
							childrenTree.ChildrenCountFormat = childrenCountFormat;
							childrenTree.SharedByFormat = sharedByFormat;
							childrenTree.UpdatedFormat = updatedFormat;
							childrenTree.ItemTemplate = ItemTemplate;
							childrenTree.SelectedLightboxId = SelectedLightboxId;
							childrenTree.ItemCommand += new RepeaterCommandEventHandler(Lightboxes_ItemCommand);
                            childrenTree.LightboxDirectory = this.LightboxDirectory;
							childrenTree.DataSource = lightbox.LightboxChildren;
							childrenTree.DataBind();

							children.Attributes["parentid"] = lightbox.LightboxId.ToString();
							children.Controls.Add(childrenTree);
						}
						else
						{
							children.Visible = false;
						}

						break;
				}
			}

			// Lightbox name
			lightboxName.Text = lightboxNameDisplay;

			// Images count
			string countText = "";
			if (lightbox.ProductCount == 1)
			{
				countText = string.Format(OneItemCountFormat, 1);
			}
			else
			{
				countText = string.Format(ItemCountFormat, lightbox.ProductCount);
			}

			if (lightboxCount > 0)
			{
				countText += CountsSeparator;
			}
			counts.Text = countText;

			// Children lightboxes count
			string childCountText = "";
			if (lightboxCount > 0)
			{
				if (lightbox.LightboxChildren.Count == 1)
				{
					childCountText += string.Format(OneChildCountFormat, 1);
				}
				else
				{
					childCountText += string.Format(ChildrenCountFormat, lightboxCount);
				}
			}
			childCount.Text = childCountText;

			// Date modified
			changedAt.Text = string.Format(UpdatedFormat, lightbox.ChangedAt.ToShortDateString());

			// Shared
            if (!string.IsNullOrEmpty(lightbox.SharedBy) || lightbox.SharedOut)
			{
				sharedBy.Text = string.Format(SharedByFormat, lightbox.SharedBy);
			}
            else
            {
                sharedBy.Parent.Visible = false;
            }
		}

		protected void Lightboxes_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (itemCommand != null)
			{
				itemCommand(source, e);
			}
		}

        private bool CheckForSharedParent(int lightboxId)
        {
            if (LightboxDirectory != null && //we do have a dictionary
                LightboxDirectory.ContainsKey(lightboxId) && //and it has the lightbox we are looking for
                LightboxDirectory[lightboxId].ParentLightboxId != null && //and it does have a parent
                LightboxDirectory.ContainsKey((int)LightboxDirectory[lightboxId].ParentLightboxId) && //and we have the parent in the list
                !String.IsNullOrEmpty(LightboxDirectory[(int)LightboxDirectory[lightboxId].ParentLightboxId].SharedBy) //and the parent is a shared lightbox
                )
            {
                return true;              
            }
            else
            {
                return false;
            }
        }

        private bool IsLightboxOwnedAndSharedOut(Lightbox lb)
        {
            if(!string.IsNullOrEmpty(lb.SharedBy ))
            {
                return false;
            }

            return lb.SharedOut;
        }
    }
}
