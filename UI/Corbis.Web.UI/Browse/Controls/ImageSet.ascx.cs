using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Corbis.Framework.Globalization;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Controls;
using Corbis.Web.UI.Presenters.MediaSetSearch;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Browse
{
	public partial class ImageSet : CorbisBaseUserControl, IMediasetSearchView
	{
		#region Private Fields
		private string _primaryContentTypeID = "682"; //Entertainment as default
		private int _itemsPerPage = 8;
		private int _currentPageNumber = 1;
		private int _totalRecords;
		private int _currentPageHitCount = 0;
		private string _imageSetID = string.Empty;
		private string _imageSetTitle = string.Empty;
		private string _imageSetSubtitle = string.Empty;
		private MediasetSearchPresenter _presenter;
		#endregion

		protected void Page_Prerender(object sender, EventArgs e)
		{
			_presenter = new MediasetSearchPresenter(this);
			if (!Page.IsPostBack)
			{
				Dictionary<string, string> searchParameters = new Dictionary<string, string>();
                searchParameters.Add(MediaSetFilterKeys.PrimaryContentType, _primaryContentTypeID);
				searchParameters.Add(MediaSetFilterKeys.PageNumber, "1");
				searchParameters.Add(MediaSetFilterKeys.RDT, "-60");
				searchParameters.Add(MediaSetFilterKeys.SetType, "3");
				searchParameters.Add(MediaSetFilterKeys.Sort, "2");
				_presenter.GetMediasetSearchResults(searchParameters);
			}
		}
        
		#region Repeater
		protected void ImageSetImages_ItemDataBound(Object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Header)
			{
				Literal imageSetHeader = (Literal)e.Item.FindControl("ImageSetHeader");
				Literal imageSetSubHeader = (Literal)e.Item.FindControl("ImageSetSubHeader");

				imageSetHeader.Text = _imageSetTitle;
				imageSetSubHeader.Text = _imageSetSubtitle;
			}
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				MediaSet data = e.Item.DataItem as MediaSet;
				string imageLink = string.Empty;
				string caption = string.Empty;
				string pubDate = string.Empty;
				string mediaSetID = string.Empty;

				HtmlGenericControl imageContainer = (HtmlGenericControl)e.Item.FindControl("ImageContainer");
				CenteredImageContainer thumbWrap = (CenteredImageContainer)e.Item.FindControl("thumbWrap");
				System.Web.UI.WebControls.HyperLink imageSetLink = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("ImageSetLink");
				Literal imageCaption = (Literal)e.Item.FindControl("ImageCaption");
				Literal publishDate = (Literal)e.Item.FindControl("ImageDate");
				Literal imageSetID = (Literal)e.Item.FindControl("ImageSetID");

				if (e.Item.ItemIndex % 4 == 0)
				{
					imageContainer.Attributes.Add("class", "first");
				}

				if (data != null)
				{
					mediaSetID = data.MediaSetId.ToString();
					imageLink = string.Format("~/ImageGroups/ImageGroups.aspx?typ={0}&id={1}", data.MediaSetTypeId, mediaSetID);
					caption = StringHelper.Truncate(data.Title, 45);

                    if (data.DatePhotographed != null)
						pubDate = GetLocalizedDatePhotographed(data.DatePhotographed, data.ApproximateDatePhotographed);

					thumbWrap.ImgUrl = data.LeadImage.Url170;
					thumbWrap.Size = 170;
					thumbWrap.Ratio = data.LeadImage.AspectRatio;
					thumbWrap.AltText = GetLocalResourceObject("LinkAltText").ToString();
					thumbWrap.IsAbsolute = true;
					thumbWrap.Attributes["onclick"] = string.Format(@"window.location.href = '../imagegroups/imagegroups.aspx?typ={0}&id={1}'", data.MediaSetTypeId, DataBinder.Eval(data, "MediaSetId"));
				}

				imageSetLink.NavigateUrl = imageLink;
				imageSetLink.Text = GetLocalResourceObject("LinkText").ToString();
				imageSetLink.ToolTip = GetLocalResourceObject("LinkAltText").ToString();
				imageCaption.Text = caption;
				publishDate.Text = pubDate;
				imageSetID.Text = mediaSetID;
			}
			if (e.Item.ItemType == ListItemType.Footer)
			{
				System.Web.UI.WebControls.HyperLink imageSetFooter = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("SeeImageSet");

				if (imageSetFooter != null)
				{
					imageSetFooter.NavigateUrl = "~/MediaSetSearch/MediaSetSearch.aspx?pct=" + _primaryContentTypeID;
					imageSetFooter.Text = GetLocalResourceObject("FooterLinkText").ToString();
					imageSetFooter.ToolTip = GetLocalResourceObject("FooterLinkAltText").ToString();
				}
			}
		}

		/// <summary>
		/// If Date Photographed or Aproximate Date Photographed is available,
		/// try to localize it.
		/// </summary>
		/// <param name="datePhotographed">Date Photographed</param>
		/// <param name="aproxDatePhotographed">Approximate Date Photographed</param>
		/// <returns>A localized string representation of Date Photographed</returns>
		private string GetLocalizedDatePhotographed(string datePhotographed, string aproxDatePhotographed)
		{
			string returnDate = datePhotographed;

			if (!string.IsNullOrEmpty(datePhotographed) || !string.IsNullOrEmpty(aproxDatePhotographed))
			{
				if (string.IsNullOrEmpty(datePhotographed))
					returnDate = aproxDatePhotographed;
				
				returnDate = (returnDate == null)
					?
					DateTime.Parse(returnDate, Language.EnglishUS.CultureInfo).ToShortDateString().ToString(Language.CurrentCulture.DateTimeFormat) :
					(returnDate.IndexOf("/") > 0
					?
					DateTime.Parse(returnDate, Language.EnglishUS.CultureInfo).ToShortDateString().ToString(Language.CurrentCulture.DateTimeFormat)
					: returnDate);
			}

			return returnDate;
		}
		#endregion

		#region Public Fields
		/// <summary>
		/// Set the Primary ContentTypeID value:
		/// 660: Commercial-Rights Managed
		/// 661: Editorial
		/// 662: Commercial-Royalty-Free
		/// 664: Sports
		/// 665: News Archive
		/// 666: Historical
		/// 669: Outline
		/// 680: Art
		/// 682: Entertainment
		/// 683: News
		/// </summary>
        /// 

        private DateTime? datePhotographed;

        /// <summary>
        /// Null indicates the usage doesn't have a license start date, 
        /// DateTime.MinValue or before DateTime.Today indicates it's not valid
        /// </summary>
        public DateTime? DatePhotographed
        {
            get { return datePhotographed; }
            set
            {
                datePhotographed = value;
                if (datePhotographed.HasValue)
                {
                    string dateText = datePhotographed.Value.ToString(Language.CurrentCulture.DateTimeFormat.ShortDatePattern);
                   
                }
            }
        }

		public string PrimaryContentTypeID
		{
			get { return _primaryContentTypeID; }
			set { _primaryContentTypeID = value; }
		}

		/// <summary>
		/// Sets the title of the image set.
		/// </summary>
		public string ImageSetTitle
		{
			get { return _imageSetTitle; }
			set { _imageSetTitle = value; }
		}

		/// <summary>
		/// Sets the subtitle of the image set.
		/// </summary>
		public string ImageSetSubtitle
		{
			get { return _imageSetSubtitle; }
			set { _imageSetSubtitle = value; }
		}
		#endregion

		#region IMediasetSearchView Members

		public List<MediaSet> MediasetList
		{
			set
			{
				this.ImageSetImages.DataSource = value;
				ImageSetImages.DataBind();
			}
		}

		public List<Corbis.LightboxCart.Contracts.V1.Lightbox> LightboxList
		{
			set { throw new NotImplementedException(); }
		}

		public string ActiveLightbox
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public List<Corbis.LightboxCart.Contracts.V1.LightboxDisplayImage> LightboxItems
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public bool ShowZeroResults
		{
			set { }
		}

		public int ItemsPerPage
		{
			get { return _itemsPerPage; }
			set { _itemsPerPage = value; }
		}

		public int CurrentPageNumber
		{
			get { return _currentPageNumber; }
			set { _currentPageNumber = value; }
		}

		public int TotalRecords
		{
			get { return _totalRecords; }
			set { _totalRecords = value; }
		}

		public int CurrentPageHitCount
		{
			get { return _currentPageHitCount; }
			set { _currentPageHitCount = value; }
		}

		public bool ShowQuickPicTab
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public void AdjustStatusForUser()
		{
			throw new NotImplementedException();
		}

		#endregion
	}

}