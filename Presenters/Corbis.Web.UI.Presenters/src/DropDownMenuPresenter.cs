using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Presenters
{
    public class DropDownMenuPresenter:BasePresenter
    {

        public DropDownMenuPresenter()
        {

        }



        #region DropDownMenu Sample Data
        private   List<DropDownMenuData> getSampleDropData()
        {
            List<DropDownMenuData> listDrop = new List<DropDownMenuData>();

            DropDownMenuData dropDownMenu1 = new DropDownMenuData();
            dropDownMenu1.ParentText = "browseImagesTitleCreative";
            dropDownMenu1.ParentTooltip = "browseImagesTitleCreative";
            dropDownMenu1.ParentPageUrl = String.Empty;

            List<DropDownMenuItemData> childMenu = new List<DropDownMenuItemData>();

            DropDownMenuItemData itemChildData1 = new DropDownMenuItemData();
            itemChildData1.Text = "browseImagesTitleRightsManaged";
            itemChildData1.PageUrl = "/Browse/RightsManaged.aspx";
            itemChildData1.Tooltip = "browseImagesTitleRightsManaged";
            childMenu.Add(itemChildData1);
            
            DropDownMenuItemData itemChildData2 = new DropDownMenuItemData();
            itemChildData2.Text = "browseImagesTitleRoyaltyFree";
            itemChildData2.PageUrl = "/Browse/RoyaltyFree.aspx";
            itemChildData2.Tooltip = "browseImagesTitleRoyaltyFree";
            childMenu.Add(itemChildData2);

            DropDownMenuItemData itemChildData3 = new DropDownMenuItemData();
            itemChildData3.Text = "browseImagesTitleIllustration";
            itemChildData3.PageUrl = "/Browse/Illustration.aspx";
            itemChildData3.Tooltip = "browseImagesTitleIllustration";
            childMenu.Add(itemChildData3);

            dropDownMenu1.childMenus = childMenu;
            listDrop.Add(dropDownMenu1);

            DropDownMenuData dropDownMenu2 = new DropDownMenuData();
            dropDownMenu2.ParentText = "browseImagesTitleEditorial";
            dropDownMenu2.ParentTooltip = "browseImagesTitleEditorial";
            dropDownMenu2.ParentPageUrl = String.Empty;

            childMenu = new List<DropDownMenuItemData>();

            DropDownMenuItemData itemChildData4 = new DropDownMenuItemData();
            itemChildData4.Text = "browseImagesTitleDocumentary";
            itemChildData4.PageUrl = "/Browse/Documentary.aspx";
            itemChildData4.Tooltip = "browseImagesTitleDocumentary";
            childMenu.Add(itemChildData4);

            DropDownMenuItemData itemChildData6 = new DropDownMenuItemData();
            itemChildData6.Text = "browseImagesTitleFineArt";
            itemChildData6.PageUrl = "/Browse/FineArt.aspx";
            itemChildData6.Tooltip = "browseImagesTitleFineArt";
            childMenu.Add(itemChildData6);

            DropDownMenuItemData itemChildData7 = new DropDownMenuItemData();
            itemChildData7.Text = "browseImagesTitleArchive";
            itemChildData7.PageUrl = "/Browse/Archival.aspx";
            itemChildData7.Tooltip = "browseImagesTitleArchive";
            childMenu.Add(itemChildData7);

            DropDownMenuItemData itemChildData5 = new DropDownMenuItemData();
            itemChildData5.Text = "browseImagesTitleCurrentEvents";
            itemChildData5.PageUrl = "/Browse/CurrentEvents.aspx";
            itemChildData5.Tooltip = "browseImagesTitleCurrentEvents";
            childMenu.Add(itemChildData5);

            DropDownMenuItemData itemChildData8 = new DropDownMenuItemData();
            itemChildData8.Text = "browseImagesTitleEntertainment";
            itemChildData8.PageUrl = "/Browse/Entertainment.aspx";
            itemChildData8.Tooltip = "browseImagesTitleEntertainment";
            childMenu.Add(itemChildData8);            
            
            dropDownMenu2.childMenus = childMenu;
            listDrop.Add(dropDownMenu2);

            DropDownMenuData dropDownMenu7 = new DropDownMenuData();
            dropDownMenu7.ParentText = "browseImagesTitleMotion";
            dropDownMenu7.ParentTooltip = "browseImagesTitleMotion";
			dropDownMenu7.ParentPageUrl = string.Empty;
            listDrop.Add(dropDownMenu7);

            return listDrop;
        }
        #endregion



        public List<DropDownMenuData> GetDropDownMenuData()
        {
            List<DropDownMenuData> data = new List<DropDownMenuData>();
            List<DropDownMenuData> cacheData = CachePersistenceHelper.RetrieveFromCache(CacheItem.DropDownMenuList) as List<DropDownMenuData>;                     
           
            if (cacheData != null)
            {
                data = cacheData;
            }
            else
            {
                try
                {
                    data = getSampleDropData();

                    CachePersistenceHelper.SaveToCache(CacheItem.DropDownMenuList, data);
                }
                catch (Exception)
                {

                }
                
            }

            return data;
        }
    }


    #region DropDownMenuData
    public class DropDownMenuData
    {
        private string parentText;
        private string parentTooltip;
        private string parentPageUrl;
        public List<DropDownMenuItemData> childMenus;

        public DropDownMenuData()
        {
        }
        public DropDownMenuData(string parentText, string parentPageUrl, List<DropDownMenuItemData> childMenus)
        {
            this.ParentText = parentText;
            this.ParentTooltip = parentTooltip;
            this.parentPageUrl = parentPageUrl;
        }

        public string ParentText
        {
            get { return parentText; }
            set { parentText = value; }
        }



        public string ParentPageUrl
        {
            get { return parentPageUrl; }
            set { parentPageUrl = value; }
        }



        public string ParentTooltip
        {
            get { return parentTooltip; }
            set { parentTooltip = value; }
        }



    }

    #endregion

    #region DropDownMenu Item Data
    public class DropDownMenuItemData
    {
        #region Constructor(s)
        public DropDownMenuItemData()
        {
        }
        public DropDownMenuItemData(string text, string pageUrl)
        {
            this.text = text;
            this.pageUrl = pageUrl;
        }

        #endregion

        #region Private members
        private string text;
        private string pageUrl;
        private string tooltip;
        #endregion

        #region Properties
       
        public string Text
        {
            get { return text; }
            set { text = value; }
        }


      
        public string PageUrl
        {
            get { return pageUrl; }
            set { pageUrl = value; }
        }


      
        public string Tooltip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }

        #endregion
    }

    #endregion
}

    
