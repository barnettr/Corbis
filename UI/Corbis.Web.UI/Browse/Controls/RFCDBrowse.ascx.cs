using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Corbis.Web.UI.Presenters.Rfcd;
using Corbis.Web.UI.Presenters.Rfcd.ViewInterfaces;
using Corbis.RFCD.Contracts.V1;

namespace Corbis.Web.UI.Browse
{

	public partial class RFCDBrowse : CorbisBaseUserControl, IRfcdCategoryView
	{

		private RfcdPresenter presenter;
		private Guid categoryUID;

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			presenter = new RfcdPresenter(this);
			if (!IsPostBack)
			{
				presenter.GetAllRFCDCategories();
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			presenter.ReloadCategoriesOnLanguageChange();
			presenter.SetPreviousLanguageCode();
		}

		protected void RfcdByAlphabet_Click(object sender, EventArgs e)
		{
			try
			{
				// find which link button is pressed
				// USE CORBIS LINKBUTTONLIST CONTROL
				Corbis.Web.UI.Controls.LinkButton lnkBtn = (Corbis.Web.UI.Controls.LinkButton)sender;
				CategoryTitle = lnkBtn.Text;

				presenter.GetRFCDByFirstLetter();
			}
			catch (Exception ex)
			{
				throw new Exception("RfCdCategory: RfcdByAlphabet_Click()", ex);
			}
		}

		protected void CategoryTreeView_OnSelectedNodeChanged(object sender, EventArgs e)
		{
			TreeView tree = (TreeView)sender;
			CategoryTitle = GetCatagoryTitle(tree.SelectedNode);
			CategoryUID = new Guid(tree.SelectedNode.Value.ToString());

			if (tree.SelectedNode.Expanded == true)
				tree.SelectedNode.Collapse();
			else
				tree.SelectedNode.Expand();

			tree.SelectedNode.Selected = false;
			presenter.GetRFCDByCategory();
		}

		private string GetCatagoryTitle(TreeNode selectedNode)
		{
			string title = string.Empty;
			if (selectedNode.Parent != null && selectedNode.Parent.Text != string.Empty)
			{
				title = string.Format("{0} > {1}", selectedNode.Parent.Text, selectedNode.Text);
			}
			else
			{
				title = selectedNode.Text;
			}
			return title;
		}

		//protected void TreeView_OnTreeNodeDataBound(object sender, TreeNodeEventArgs e)
		//{
		//    if (e.Node.ChildNodes.Count > 0)
		//    {
		//        e.Node.SelectAction = TreeNodeSelectAction.SelectExpand;
		//    }
		//}

		protected void RfCdEntity_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				RepeaterItem item = e.Item;
				Corbis.Web.UI.Controls.LinkButton categoryLink = (Corbis.Web.UI.Controls.LinkButton)item.FindControl("linkRfcdEntity");
				categoryLink.PostBackUrl = GetRFCDDetailsPageUrl(categoryLink.CommandName);
			}
		}

		private string GetRFCDDetailsPageUrl(string volumeNumber)
		{
			string url = string.Empty;
			url = string.Format("{0}?typ=6&id={1}", SiteUrls.RfcdResults, volumeNumber);
			return url;
		}

		#region IRfCdCategoryView Members

		public Guid CategoryUID
		{
			get
			{
				return categoryUID;
			}

			set
			{
				categoryUID = value;
			}
		}

		public string CategoryTitle
		{
			get
			{
				//return categoryTitle.Text.Replace("[ ", "").Replace(" ]", "");
				return categoryTitle.Text.Replace(":", "");
			}
			set
			{
				//categoryTitle.Text = "[ " + value + " ]";
				categoryTitle.Text = value + ":";
			}
		}

		public string PreviousLanguageCode
		{
			get
			{
				return previousLanguageCode.Value;
			}
			set
			{
				previousLanguageCode.Value = value;
			}
		}

		public List<RFCDEntity> RFCDsByFirstLetterOrCategory
		{
			set
			{
				repeaterRfcdEntity.DataSource = value;

				repeaterRfcdEntity.DataBind();
			}
		}

		public string RFCDCategories
		{
			set
			{
				xmlDataSourceRfcdByCategory.Data = value;
			}
		}

		#endregion

	}
}