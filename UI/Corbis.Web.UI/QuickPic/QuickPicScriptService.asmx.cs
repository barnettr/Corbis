using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Corbis.Framework.Globalization;
using Corbis.Web.Entities;
using Corbis.Web.UI.Presenters.QuickPic;
using Corbis.WebOrders.Contracts.V1;

namespace Corbis.Web.UI.QuickPic
{
    /// <summary>
    /// Summary description for QuickPicWScriptService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
    public class QuickPicScriptService :  Corbis.Web.UI.src.CorbisWebService
    {
        private QuickPicPresenter _presenter;

        public QuickPicScriptService()
        {
            _presenter = new QuickPicPresenter();
        }
        
        [WebMethod(true)]
        public QuickPicOrderSummary DownloadQuickPicImages(
            string projectName, 
            List<QuickPicOrderImage> images)
        {
            if (projectName == "") projectName = DateTime.Now.ToString("MM/dd/yyyy", Language.CurrentCulture);
			projectName = HttpContext.GetLocalResourceObject("~/QuickPic/QuickPic.aspx", "projectNamePrefix") + projectName;

            return _presenter.DownloadQuickPicImages(projectName, images);
        }
    }
}
