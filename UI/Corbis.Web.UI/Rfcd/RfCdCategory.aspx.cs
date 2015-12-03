using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Corbis.Web.UI.MasterPages;
using Languages = Corbis.Framework.Globalization.Language;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.Presenters.Rfcd;
using Corbis.Web.UI.Presenters.Rfcd.ViewInterfaces;
using Corbis.Web.Content;
using Corbis.Web.Entities;
using Corbis.Web.Utilities;
using AjaxControlToolkit;
using Corbis.RFCD.Contracts.V1;
using System.Xml;
using System.Collections.Specialized;

namespace Corbis.Web.UI.Rfcd
{
    public partial class RfcdCategory : CorbisBasePage, IRfcdCategoryView
    {

        private RfcdPresenter presenter;
        private Guid categoryUID;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            presenter = new RfcdPresenter(this);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                presenter.GetAllRFCDCategories();
            }
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

        protected void RfcdByCategory_SelectedNodeChanged(object sender, EventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            CategoryTitle = GetCatagoryTitle(treeView.SelectedNode);
            CategoryUID = new Guid(treeView.SelectedNode.Value.ToString());

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
                return categoryTitle.Text.Replace("[ ","").Replace(" ]","");
            }
            set
            {
                categoryTitle.Text = "[ " + value + " ]";
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
