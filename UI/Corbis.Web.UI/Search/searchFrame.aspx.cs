using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters.MediaSetSearch;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Search
{
    public partial class searchFrame : System.Web.UI.Page, IMediaSetRedirectionView
    {
        public ImageMediaSetType MediaSetType { get; set; }
        public int MediaSetId { get; set; }
        public Guid MediaSetUid { get; set; }
        public Guid SessionSetUid { get; set; }
        public int SessionSetId { get; set; }
        public Guid RfcdUid { get; set; }
        public string RfcdVolume { get; set; }
        public Guid ImageUid { get; set; }
        public string CorbisId { get; set; }

        protected override void OnInit(EventArgs e)
        {
            switch (Request.QueryString[MediaSetFilterKeys.Type])
            {
                case "sets":
                    ProcessMediaSetSearchRequest();
                    break;
                case "set":
                    ProcessMediaSetRequest();
                    break;
                case "sessionset":
                    ProcessSessionSetRequest();
                    break;
                case "enlargement":
                    ProcessImageEnlargmentRequest();
                    break;
                default:
                    if (!string.IsNullOrEmpty(Request.QueryString["rfcduid"]))
                    {
                        ProcessRfcdRequest();
                    }
                    else
                    {
                        ProcessImageSearchRequest();
                    }
                    break;
            }
        }

        /// <summary>
        /// Redirects to the MediaSetSearch page.
        /// </summary>
        /// <remarks>
        /// All QueryString parameters are supported by the destination page, 
        /// so send the QueryString parameters as-is.
        /// </remarks>
        private void ProcessMediaSetSearchRequest()
        {
            Response.Redirect(string.Concat(SiteUrls.MediaSetSearch, "?", Request.QueryString), true);
        }

        /// <summary>
        /// Redirects to the ImageGroups page for the requested MediaSet
        /// </summary>
        /// <remarks>
        /// A media set Uid is passed in the QueryString, we need to look 
        /// up the Id before redirecting.
        /// </remarks>
        private void ProcessMediaSetRequest()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["isuid"]))
            {
                Guid setUid;
                if (GuidHelper.TryParse(HttpUtility.UrlDecode(Request.QueryString["isuid"]), out setUid))
                {
                    MediaSetUid = setUid;
                    MediaSetType = ImageMediaSetType.Unknown;
                    MediasetSearchPresenter presenter = new MediasetSearchPresenter(this);
                    presenter.GetMediaSetId();
                    string redirectTo = SiteUrls.ImageGroups + string.Format("?typ={0}&id={1}", (int)MediaSetType, MediaSetId);
                    Response.Redirect(redirectTo, true);
                    return;
                }
            }
            Response.Redirect(SiteUrls.Home, true);
        }

        /// <summary>
        /// Redirects to the ImageGroups page for the requested SessionSet
        /// </summary>
        /// <remarks>
        /// We will have to look up the session set by uid to redirect to the
        /// ImageGroups page.
        /// </remarks>
        private void ProcessSessionSetRequest()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["ssuid"]))
            {
                Guid setUid;
                if (GuidHelper.TryParse(HttpUtility.UrlDecode(Request.QueryString["ssuid"]), out setUid))
                {
                    SessionSetUid = setUid;
                    MediaSetType = ImageMediaSetType.OutlineSession;
                    MediasetSearchPresenter presenter = new MediasetSearchPresenter(this);
                    presenter.GetSessionSetId();
                    string redirectTo = SiteUrls.ImageGroups + string.Format("?typ={0}&id={1}", (int)MediaSetType, SessionSetId);
                    Response.Redirect(redirectTo, true);
                    return;
                }
            }
            Response.Redirect(SiteUrls.Home, true);
        }

        /// <summary>
        /// Redirects to the ImageGroups page for the requested RFCD
        /// </summary>
        /// <remarks>
        /// We have to look up the RFCD by uid to redirect to the page.
        /// </remarks>
        private void ProcessRfcdRequest()
        {
            string[] rfcdIdParams = HttpUtility.UrlDecode(Request.QueryString["rfcduid"]).Split('|');
            if (!string.IsNullOrEmpty(rfcdIdParams[0]))
            {
                Guid setUid;
                if (GuidHelper.TryParse(rfcdIdParams[0], out setUid))
                {
                    RfcdUid = setUid;
                    MediaSetType = ImageMediaSetType.RFCD;
                    MediasetSearchPresenter presenter = new MediasetSearchPresenter(this);
                    presenter.GetRfcdVolume();
                    string redirectTo = SiteUrls.ImageGroups + string.Format("?typ={0}&id={1}", (int)MediaSetType, RfcdVolume);
                    Response.Redirect(redirectTo, true);
                    return;
                }
            }
            Response.Redirect(SiteUrls.Home, true);
        }

        /// <summary>
        /// Redirects to the Image Enlargement page
        /// </summary>
        /// <remarks>
        /// We have to look up the CorbisID for the uid passed in. Since we 
        /// don't know the context of this redirect, show the global nav.
        /// NOTE: the legacy site url is on /popup/Enlargement.aspx. We are 
        /// redirecting to here in IIRF to localize the redirect logic.
        /// </remarks>
        private void ProcessImageEnlargmentRequest()
        {
            string[] mediauids = HttpUtility.UrlDecode(Request.QueryString["mediauids"]).Split('|');
            if (!string.IsNullOrEmpty(mediauids[0]))
            {
                Guid imageUid;
                if (GuidHelper.TryParse(mediauids[0], out imageUid))
                {
                    ImageUid = imageUid;
                    MediaSetType = ImageMediaSetType.RFCD;
                    MediasetSearchPresenter presenter = new MediasetSearchPresenter(this);
                    presenter.GetCorbisIdForImage();
                    string redirectTo = SiteUrls.Enlargement + "?id=" + CorbisId + "&ext=1";
                    Response.Redirect(redirectTo, true);
                    return;
                }
            }
            Response.Redirect(SiteUrls.Home, true);
        }

        /// <summary>
        /// Redirects to the search results page.
        /// </summary>
        /// <remarks>
        /// Parameters need to be mapped for the new page.
        /// </remarks>
        private void ProcessImageSearchRequest()
        {
            string newQuery = string.Empty;
            string value;

            // NOTE: Not all search filters are supported.
            // The only supported filters are:
            //      Keywords        txt  =>  q
            //      Categories      cat  =>  cat (map old values to new)
            //      Collections     chkColl  =>  mrc
            //      Days available  rdt  =>  ma (convert to positive int)
            //      Query Links     qlnk  =>  qlnk
            //      Sort Order      sort  =>  sort (map old values to new)
            
            // Keywords
            if ((value = Request.QueryString["txt"]) != null)
            {
                newQuery += "&q=" + HttpUtility.UrlEncode(value);
            }

            // Categories need to be converted from the old ids to the new ids.
            if ((value = Request.QueryString["cat"]) != null)
            {
                newQuery += "&cat=" + TranslateCategoryValues(value);
            }
            
            // Marketing Collections
            if ((value = Request.QueryString["chkColl"]) != null)
            {
                newQuery += "&mrc=" + value;
            }

            // Image Numbers
            if ((value = Request.QueryString["img"]) != null)
            {
                newQuery += "&in=" + value;
            }

            // Number of days available -- take the absolute value of the
            // parameter (Monarch requires positive value)
            if ((value = Request.QueryString["rdt"]) != null)
            {
                int days;
                if (int.TryParse(value, out days))
                {
                    newQuery += "&ma=" + Math.Abs(days);
                }
            }

            // Query Links
            if ((value = Request.QueryString["qlnk"]) != null)
            {
                newQuery += "&qlnk=" + value;
            }

            // Sort Order needs to be translated from existing meanings to new values.
            if ((value = Request.QueryString["sort"]) != null)
            {
                newQuery += "&sort=" + TranslateSortValue(value);
            }
            
            Response.Redirect(string.Concat(SiteUrls.SearchResults, "?", newQuery.TrimStart('&')), true);
        }

        /// <remarks>
        /// Categories are mapped as follows:
        ///     Commercial ==> Creative
        ///     Editorial ==> Documentary
        ///     Historical ==> Archival
        ///     Art ==> Fine Art
        ///     News ==> Current Events
        ///     Sports ==> Current Events
        ///     Entertainment ==  Entertainment
        /// </remarks>
        private static string TranslateCategoryValues(string value)
        {
            string[] oldCategories = HttpUtility.UrlDecode(value).Split(',');
            
            if (oldCategories.Length == 0)
            {
                return string.Empty;
            }

            List<string> newCategories = new List<string>();
            foreach(string oldCat in oldCategories)
            {
                switch (oldCat)
                {
                    case "10":  // Commercial ==> Creative
                        newCategories.Add(((int)Category.Creative).ToString());
                        break;
                    case "1":  // Editorial ==> Documentary
                        newCategories.Add(((int)Category.Documentary).ToString());
                        break;
                    case "2":  // Historical ==> Archival
                        newCategories.Add(((int)Category.Archival).ToString());
                        break;
                    case "5":  // Art ==> Fine Art
                        newCategories.Add(((int)Category.FineArt).ToString());
                        break;
                    case "6":  // News ==> Current Events
                        newCategories.Add(((int)Category.CurrentEvents).ToString());
                        break;
                    case "9":  // Sports ==> Current Events
                        newCategories.Add(((int) Category.CurrentEvents).ToString());
                        break;
                    case "7":  // Entertainment ==  Entertainment
                        newCategories.Add(((int)Category.Entertainment).ToString());
                        break;
                }
            }
            return string.Join(",", newCategories.ToArray());
        }

        private static string TranslateSortValue(string value)
        {
            switch(value)
            {
                case "0":  // Relevancy
                    return ((int)SearchSort.Relevancy).ToString();
                case "1":  // Date Photographed
                    return ((int)SearchSort.DatePhotographed).ToString();
                case "2":  // Date Added
                    return ((int)SearchSort.DateAdded).ToString();
                case "3":  // Date Published
                    return ((int)SearchSort.DatePublished).ToString();
            }
            return string.Empty;
        }
    }
}
