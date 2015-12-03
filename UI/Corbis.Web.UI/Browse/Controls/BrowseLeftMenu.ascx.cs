using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.Web.UI.Controls;
using Corbis.Web.Utilities;
using Corbis.Web.Entities;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using Corbis.Framework.Globalization;
using Corbis.Web.UI.MasterPages;
using Corbis.Web.Content;
using Corbis.Web.UI.Presenters;
using System.Diagnostics.CodeAnalysis;
using Corbis.Web.UI.Navigation;

namespace Corbis.Web.UI.Browse
{
    public partial class BrowseLeftMenu : CorbisBaseUserControl{
        private DropDownMenuPresenter dropDownMenuPresenter;
        protected override void OnInit(EventArgs e)
        {
            

            #region Browse Images

            dropDownMenuPresenter = new DropDownMenuPresenter();
            dropDownMenuLeft.DropDownItems = dropDownMenuPresenter.GetDropDownMenuData();
            dropDownMenuLeft.ItemCommand += new EventHandler<DropDownMenuChangedEventArgs>(DropDown_ItemCommandChange);

            #endregion

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            

            
        }
        protected void DropDown_ItemCommandChange(object sender, DropDownMenuChangedEventArgs e)
        {
            if (e == null || String.IsNullOrEmpty(e.NavigateUrl))
            {
                Response.Redirect(SiteUrls.PageNotFound);
                return;
            }

            Response.Redirect(e.NavigateUrl);
        }        

    }



}
