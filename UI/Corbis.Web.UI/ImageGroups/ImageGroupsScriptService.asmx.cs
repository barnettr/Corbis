using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using Corbis.Web.UI.Presenters.ImageGroups;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.QuickPic;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.ImageGroups
{
    /// <summary>
    /// Summary description for LightboxScriptService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
    public class ImageGroupScriptService : Corbis.Web.UI.src.CorbisWebService
    {

        private QuickPicPresenter quickPicPresenter;


        public ImageGroupScriptService()
        {
            quickPicPresenter = new QuickPicPresenter();
        }

        [WebMethod(true)]
        public string AddItemToQuickPick(string corbisID, string url128, string licenceModel, string aspectRation, string title)
        {
            QuickPicItem quickPic = new QuickPicItem(corbisID, url128, licenceModel, aspectRation, title);
            if (quickPicPresenter.AddItemToQuickPick(quickPic))
            {
                return corbisID;
            }
            else
            {
                return string.Empty;
            }
        }

        [WebMethod(true)]
        public string RemoveItemFromQuickPick(string corbisID)
        {
            quickPicPresenter.RemoveItemToQuickPick(corbisID);
            return corbisID;
        }

        [WebMethod(true)]
        public bool displayOptionsClarifications(bool showclarificationstatus)
        {
            StateItemCollection stateItems = new StateItemCollection(System.Web.HttpContext.Current);
            stateItems.SetStateItem<string>(new StateItem<string>(SearchKeys.Name, SearchKeys.ShowClarificationCookieKey, showclarificationstatus.ToString(), StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
            return showclarificationstatus;
        }

        [WebMethod(true)]
        public void SavePreviewPreference(int previewState)
        {
            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
            stateItems.SetStateItem<int>(new StateItem<int>(SearchKeys.Name, SearchKeys.SearchPreviewKey, previewState, StateItemStore.Cookie, StatePersistenceDuration.NeverExpire));
        }

    }
}
