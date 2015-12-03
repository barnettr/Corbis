using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.UI.Presenters.Search;
using Corbis.Web.Utilities.StateManagement;
using Corbis.Web.Utilities;
using Corbis.MarketingCollection.Contracts.V3;
using Corbis.CommonSchema.Contracts.V1;
using Corbis.Web.Entities;
using Corbis.Web.UI;

namespace Corbis.Web.UI.Navigation
{
    public partial class Search : CorbisBaseUserControl, ISearchView
    {
        SearchPresenter searchPresenter;

        private int previousPagingNo;
        private bool noPeopleFilter;
        private bool illustrationFilter;
        private bool photographyFilter;
        private bool colorFilter;
        private bool blackWhiteFilter;
        private bool modelReleasedFilter;
        private string keywordSearchText;
        private string horizontalLabelText;
        private string panoramaLabelText;
        private string verticalLabelText;

        public string DefaultStartDate { get; set; }
        public string DefaultEndDate { get; set; }


        #region Properties

        public SearchPresenter SearchPresenter
        {
            get
            {
                return this.searchPresenter;
            }
        }

        public string MsoSearchParameters
        {
            get
            {
                NameValueCollection qs = new NameValueCollection(Request.QueryString);
                qs.Remove(SearchPresenter.SearchFilterKeys.Keywords);
                qs.Remove(SearchPresenter.SearchFilterKeys.Category);
                qs.Remove(SearchPresenter.SearchFilterKeys.LicenseModel);
                qs.Remove(SearchPresenter.SearchFilterKeys.MediaType);
                qs.Remove(SearchPresenter.SearchFilterKeys.ColorFormat);
                qs.Remove(SearchPresenter.SearchFilterKeys.ModelReleased);

                string numPeople = qs[SearchPresenter.SearchFilterKeys.NumberOfPeople];
                if (numPeople != null && numPeople == ((int)CommonSchema.Contracts.V1.NumberOfPeople.WithoutPeople).ToString())
                {
                    qs.Remove(SearchPresenter.SearchFilterKeys.NumberOfPeople);
                }

                string retval = string.Empty;
                foreach(string key in qs.AllKeys)
                {
                    retval += string.Format("&{0}={1}", key, HttpUtility.UrlEncode(qs[key]));
                }

                return retval.TrimStart('&');
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            search.Click += delegate { this.DoSearch(); };
            extendedSearch.Click += delegate { this.DoSearch(); };
            this.searchPresenter = new SearchPresenter(this);

            this.PopulateMoreSearchOptionsDropdownControls();
            if (!this.IsPostBack)
            {
                searchPresenter.LoadSearchPreferences();
                searchPresenter.PopulateSearchFlyoutAndSearchBuddyFilterState();
            }

            searchPresenter.PopulateMSODatesAvailable();
            this.DefaultStartDate = this.BeginDate;
            this.DefaultEndDate = this.EndDate;

            //SearchPresenter.SetMarketingCollection();
            SearchPresenter.PopulateMoreSearchOptionsCollectionsControls();
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            //SearchPresenter.PopulateMoreSearchOptionsCollectionsControls();
            if (!IsPostBack)
            {
                SearchPresenter.PopulateMoreSearchOptionsStateFromSession();
            }
            InitalizeCheckboxControls();
            SearchPresenter.PopulateOptionsAppliedWithCollectionsData();

            this.SetMSO();

            StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);

            // Get Direclty Manipulated Search Query String from Session and 
            // attach it to PreviousSearch URL.
            string previousSearchQuery = stateItems.GetStateItemValue<string>(SearchSessionKeys.DirectlyManipulatedSearch,null, StateItemStore.Cookie);
            ShowReturnToPreviousSearch = false;
            if (!string.IsNullOrEmpty(previousSearchQuery))
            {
                keywordSearchText = string.Empty;
                PreviousSearchURL = previousSearchQuery;
            }

            if (!IsPostBack)
            {
                searchPresenter.SetFinalMSOState(Request.QueryString);
            }

        }

        private void SetMSO()
        {
            if (string.IsNullOrEmpty(this.hiddenMSOValue.Text))
            {
                this.hiddenMSOValue.Text = MSOState.UnOpened.ToString();
            }

            if (this.hiddenMSOValue.Text == MSOState.Opening.ToString())
            {
                this.hiddenMSOValue.Text = MSOState.Opened.ToString(); // do it once
                this.MSOContentDiv.Visible = true;

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setupMSO", "CorbisUI.ExtendedSearch.setupMSO()", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setNumberOfPeopleControlValue", "CorbisUI.MSOSearch.setNumberOfPeopleControlValue()", true);

                SearchPresenter.SetMoreSearchOptions();

            }
            else
            {
                if (this.hiddenMSOValue.Text == MSOState.UnOpened.ToString())
                {
                    SearchPresenter.SetMoreSearchOptions();
                }
                if (this.hiddenMSOValue.Text == MSOState.Reset.ToString())
                {
                    this.hiddenMSOValue.Text = MSOState.Opened.ToString(); // do it once
                    this.MSOContentDiv.Visible = true;

                }
            }
        }

        protected void deleteMoreSearchOptions_Click(object sender, EventArgs e)
        {
            this.RemoveMoreSearchOptions = true;

            this.DoSearch();
        }

        private void DoSearch()
        {
            SearchPresenter.SetDirectlyManipulatedKeyword(this.KeywordSearch);
            //string query = this.SearchPresenter.BuildQuery();
            //((CorbisBasePage)Page).AnalyticsData["events"] = AnalyticsEvents.Search;
            //((CorbisBasePage)Page).AnalyticsData["prop5"] = query;
            
            //SearchPresenter.SaveSearchPreferences();
            //SearchPresenter.SaveDefaultSearchFilters();
            //Response.Redirect(string.Format("{0}?{1}", SiteUrls.SearchResults, query));
        }


        protected void mcg_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                Repeater marketingCollectionRepeater = (Repeater)item.FindControl("mc");
                DisplayGroupMarketingCollections group = (DisplayGroupMarketingCollections)item.DataItem;

                marketingCollectionRepeater.DataSource = group.MarketingCollections;
                marketingCollectionRepeater.DataBind();
            }
        }

        private void PopulateMoreSearchOptionsDropdownControls()
        {
            this.PopulatePointOfView();
            this.PopulateNumberOfPeople();
            this.PopulateImmediateAvailibility();
        }

        private void PopulatePointOfView()
        {
            var pointOfViewList = CorbisBasePage.GetEnumDisplayValues<PointOfView>();
            pointOfView.DataSource = pointOfViewList;
            pointOfView.DataValueField = "Id";
            pointOfView.DataTextField = "Text";
            pointOfView.DataBind();

            // Find the item that is "all" in the list and make sure it is selected
            var allPov = pointOfViewList.Where(item => item.Value == CommonSchema.Contracts.V1.PointOfView.All).First();
            pointOfView.SelectedIndex = pointOfViewList.IndexOf(allPov);
        }

        private void PopulateNumberOfPeople()
        {
            var numberOfPeopleList = CorbisBasePage.GetEnumDisplayValues<NumberOfPeople>();

            // remove any items that are not localized (i.e. Unknown)
            numberOfPeopleList = new List<DisplayValue<NumberOfPeople>>(numberOfPeopleList
                                                                              .Where(item => !string.IsNullOrEmpty(item.Text))
                                                                              .OrderBy(item => item.Ordinal));
            numberOfPeople.DataSource = numberOfPeopleList;
            numberOfPeople.DataValueField = "Id";
            numberOfPeople.DataTextField = "Text";
            numberOfPeople.DataBind();

            var withPeople = numberOfPeopleList.Where(item => item.Value == CommonSchema.Contracts.V1.NumberOfPeople.WithPeople).First();
            numberOfPeople.SelectedIndex = numberOfPeopleList.IndexOf(withPeople);
        }

        private void PopulateImmediateAvailibility()
        {
            var availResList = CorbisBasePage.GetEnumDisplayValues<ImageAvailability>();

            immediateAvailablility.DataSource = availResList;
            immediateAvailablility.DataValueField = "Id";
            immediateAvailablility.DataTextField = "Text";
            immediateAvailablility.DataBind();

            var allRes = availResList.Where(item => item.Value == ImageAvailability.AllResolutions).First();
            immediateAvailablility.SelectedIndex = availResList.IndexOf(allRes);
        }


        private void InitalizeCheckboxControls()
        {
            this.archival.Text = CorbisBasePage.GetEnumDisplayText<Category>(Category.Archival);
            this.archival.Value = ((int)Category.Archival).ToString();

            this.creative.Text = CorbisBasePage.GetEnumDisplayText<Category>(Category.Creative);
            this.creative.Value = ((int)Category.Creative).ToString();

            this.currentEvents.Text = CorbisBasePage.GetEnumDisplayText<Category>(Category.CurrentEvents);
            this.currentEvents.Value = ((int)Category.CurrentEvents).ToString();

            this.documentary.Text = CorbisBasePage.GetEnumDisplayText<Category>(Category.Documentary);
            this.documentary.Value = ((int)Category.Documentary).ToString();

            this.entertainment.Text = CorbisBasePage.GetEnumDisplayText<Category>(Category.Entertainment);
            this.entertainment.Value = ((int)Category.Entertainment).ToString();

            this.fineArt.Text = CorbisBasePage.GetEnumDisplayText<Category>(Category.FineArt);
            this.fineArt.Value = ((int)Category.FineArt).ToString();

            this.outline.Text = CorbisBasePage.GetEnumDisplayText<Category>(Category.Outline);
            this.outline.Value = ((int)Category.Outline).ToString();
            this.outline.Visible = Profile.CanSeeOutline;

            this.rightsManaged.Value = ((int)LicenseModel.RM).ToString();
            this.royaltyFree.Value = ((int)LicenseModel.RF).ToString();
        }

        protected static string GetMarketingCollectionToolTip(MarketingCollectionGroupType collectionGroupType)
        {
            return CorbisBasePage.GetKeyedEnumDisplayText<MarketingCollectionGroupType>(collectionGroupType, "Tooltip");
        }

        public MoreSearchOptions MoreSearchOptionsSettings
        {
            get
            {
                MoreSearchOptions moreSearchOptions = new MoreSearchOptions();
                moreSearchOptions.DateCreated = DateCreated;
                moreSearchOptions.Days = Days;
                moreSearchOptions.DaysChecked = DaysChecked;
                moreSearchOptions.DateRangeChecked = DateRangeChecked;
                moreSearchOptions.BeginDate = BeginDate;
                moreSearchOptions.EndDate = EndDate;
                moreSearchOptions.Location = Location;
                moreSearchOptions.Photographer = Photographer;
                moreSearchOptions.Provider = Provider;
                moreSearchOptions.PointOfView = PointOfView;
                moreSearchOptions.NumberOfPeople = this.NumberOfPeople;
                moreSearchOptions.ImmediateAvailability = ImmediateAvailability;
                moreSearchOptions.ImageNumbers = ImageNumbers;
                moreSearchOptions.SelectedMarketingCollection = SelectedMarketingCollection;

                //if (HorizontalCheckbox && VerticalCheckbox && PanoramaCheckbox)
                //{
                //    moreSearchOptions.HorizontalCheckbox = false;
                //    moreSearchOptions.PanoramaCheckbox = false;
                //    moreSearchOptions.VerticalCheckbox = false;
                //    this.orientationSummaryDiv.Visible = false;
                //}
                //else
                //{
                moreSearchOptions.HorizontalCheckbox = HorizontalCheckbox;
                moreSearchOptions.PanoramaCheckbox = PanoramaCheckbox;
                moreSearchOptions.VerticalCheckbox = VerticalCheckbox;
                //}

                return moreSearchOptions;
            }
            set
            {
                if (value != null)
                {
                    DateCreated = value.DateCreated;
                    if (!string.IsNullOrEmpty(DateCreated))
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }

                    Days = value.Days;
                    DaysChecked = value.DaysChecked;
                    if (DaysChecked && !string.IsNullOrEmpty(Days))
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }

                    DateRangeChecked = value.DateRangeChecked;
                    if (DateRangeChecked)
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }

                    BeginDate = value.BeginDate;
                    EndDate = value.EndDate;
                    Location = value.Location;
                    if (!string.IsNullOrEmpty(Location))
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }
                    Photographer = value.Photographer;
                    if (!string.IsNullOrEmpty(Photographer))
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }

                    Provider = value.Provider;
                    if (!string.IsNullOrEmpty(Provider))
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }

                    PointOfView = value.PointOfView;
                    if (!PointOfView.Equals("0"))
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }

                    this.NumberOfPeople = value.NumberOfPeople;
                    if (!NumberOfPeople.Equals("5"))
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }

                    ImmediateAvailability = value.ImmediateAvailability;
                    if (!ImmediateAvailability.Equals("1"))
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }

                    ImageNumbers = Server.UrlDecode(value.ImageNumbers);
                    if (!string.IsNullOrEmpty(ImageNumbers) && ImageNumbers != GetLocalResourceObject("imageNumbers.Text").ToString().Trim())
                    {
                        this.ShowOptionsAppliedStyle = true;
                    }

                    SelectedMarketingCollection = value.SelectedMarketingCollection;


                    if (!value.HorizontalCheckbox && !value.VerticalCheckbox && !value.PanoramaCheckbox)
                    {
                        HorizontalCheckbox = true;
                        PanoramaCheckbox = true;
                        VerticalCheckbox = true;
                    }
                    else
                    {
                        HorizontalCheckbox = value.HorizontalCheckbox;
                        PanoramaCheckbox = value.PanoramaCheckbox;
                        VerticalCheckbox = value.VerticalCheckbox;
                        //this.ShowOptionsAppliedStyle = true;
                    }

                    if (!value.DaysChecked && !value.DateRangeChecked)
                    {
                        DaysChecked = true;
                    }

                    if (DaysChecked)
                    {
                        this.searchPresenter.PopulateMSODatesAvailable();

                    }
                }
            }
        }

        #region ISearchView Members

        [StateItemDesc("SearchFilters", "KeywordSearch", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public string KeywordSearch
        {
            // TODO:Security - Need to carfully look at the usage of it. I don't think encoding/decoding needed here.
            get
            {
                return keywordSearch.Text.Trim();
            }
            set
            {
                string trimValue = value == null ? string.Empty : value.Trim();
                keywordSearch.Text = trimValue;
            }
        }

        public string DateCreated
        {
            get
            {
                return this.dateCreated.Text.Trim();
            }
            set
            {
                string trimValue = value == null ? string.Empty : value.Trim();
                this.dateCreated.Text = trimValue;
                if (String.IsNullOrEmpty(trimValue))
                {
                    this.dateCreatedSummaryDiv.Visible = false;
                    this.dateCreatedSummary.Text = string.Empty;
                }
                else
                {
                    this.dateCreatedSummaryDiv.Visible = true;
                    this.dateCreatedSummary.Text = StringHelper.Truncate(trimValue, 35);
                }
            }
        }

        public bool DaysChecked
        {
            get { return this.daysButton.Checked; }
            set
            {
                daysButton.Checked = value;
                daysButtonImg.Attributes["src"] = value ? "/Images/radio_on.png" : "/Images/radio_off.png";
            }
        }

        public string Days
        {
            get
            {
                return this.days.Text.Trim();
            }
            set
            {
                string trimValue = value == null ? string.Empty : value.Trim();
                this.days.Text = trimValue;
                if (String.IsNullOrEmpty(trimValue) || !this.daysButton.Checked)
                {
                    this.daysSummaryDiv.Visible = false;
                    this.daysSummary.Text = string.Empty;
                }
                else
                {
                    this.daysSummaryDiv.Visible = true;
                    this.daysSummary.Text = ((string)GetLocalResourceObject("daysSummary.Text")).Replace("{0}", trimValue);
                }
            }
        }

        public bool DateRangeChecked
        {
            get { return this.betweenButton.Checked; }
            set
            {
                this.betweenButton.Checked = value;
                betweenButtonImg.Attributes["src"] = value ? "/Images/radio_on.png" : "/Images/radio_off.png";
            }
        }

        public string BeginDate
        {
            get
            {
                return this.beginDate.Text.Trim();
            }
            set
            {
                string trimValue = value == null ? string.Empty : value.Trim();
                DateTime testDate;
                bool isValidDate = DateTime.TryParse(trimValue, out testDate);
                if (isValidDate)
                {
                    this.beginDate.Text = trimValue;
                    if (String.IsNullOrEmpty(trimValue) || !this.betweenButton.Checked)
                    {
                        this.dateRangeSummaryDiv.Visible = false;
                        this.beginDateSummary.Text = string.Empty;
                    }
                    else
                    {
                        this.dateRangeSummaryDiv.Visible = true;
                        this.beginDateSummary.Text = trimValue;
                    }
                }
                else
                {
                    this.beginDate.Text = this.DefaultStartDate;
                }
            }
        }

        public string EndDate
        {
            get
            {
                return this.endDate.Text.Trim();
            }
            set
            {
                string trimValue = value == null ? string.Empty : value.Trim();
                DateTime testDate;
                bool isValidDate = DateTime.TryParse(trimValue, out testDate);
                if (isValidDate)
                {
                    this.endDate.Text = trimValue;
                    if (String.IsNullOrEmpty(trimValue) || !this.betweenButton.Checked)
                    {
                        this.dateRangeSummaryDiv.Visible = false;
                        this.endDateSummary.Text = string.Empty;
                    }
                    else
                    {
                        this.dateRangeSummaryDiv.Visible = true;
                        this.endDateSummary.Text = trimValue;
                    }
                }
                else
                {
                    this.endDate.Text = this.DefaultEndDate;
                }
            }
        }

        public string Location
        {
            get
            {
                return this.location.Text.Trim();
            }
            set
            {
                string trimValue = value == null ? string.Empty : value.Trim();
                this.location.Text = trimValue;

                if (String.IsNullOrEmpty(trimValue))
                {
                    this.locationSummaryDiv.Visible = false;
                    this.locationSummary.Text = string.Empty;
                }
                else
                {
                    this.locationSummaryDiv.Visible = true;
                    this.locationSummary.Text = StringHelper.Truncate(Server.HtmlEncode(trimValue), 35);
                }
            }
        }

        public string Photographer
        {
            get
            {
                return this.photographer.Text.Trim();
            }
            set
            {
                string trimValue = value == null ? string.Empty : value.Trim();
                this.photographer.Text = trimValue;
                if (String.IsNullOrEmpty(trimValue))
                {
                    this.photographerSummaryDiv.Visible = false;
                    this.photographerSummary.Text = string.Empty;
                }
                else
                {
                    this.photographerSummaryDiv.Visible = true;
                    this.photographerSummary.Text = StringHelper.Truncate(Server.HtmlEncode(trimValue), 35);
                }
            }
        }

        public string Provider
        {
            get
            {
                return this.provider.Text.Trim();
            }
            set
            {
                string trimValue = value == null ? string.Empty : value.Trim();
                this.provider.Text = trimValue;
                if (String.IsNullOrEmpty(trimValue))
                {
                    this.providerSummaryDiv.Visible = false;
                    this.providerSummary.Text = string.Empty;
                }
                else
                {
                    this.providerSummaryDiv.Visible = true;
                    this.providerSummary.Text = StringHelper.Truncate(trimValue, 35);
                }
            }
        }

        public bool HorizontalCheckbox
        {
            get
            {
                return this.horizontalCheckbox.Checked;
            }
            set
            {
                this.horizontalCheckbox.Checked = value;
            }
        }

        public bool PanoramaCheckbox
        {
            get
            {
                return this.panoramaCheckbox.Checked;
            }
            set
            {
                this.panoramaCheckbox.Checked = value;
            }

        }

        public bool VerticalCheckbox
        {
            get
            {
                return this.verticalCheckbox.Checked;
            }
            set
            {
                this.verticalCheckbox.Checked = value;
            }
        }

        public string HorizontalLabelText
        {
            get
            {
                return horizontalLabelText;
            }
            set
            {
                horizontalLabelText = (String)GetLocalResourceObject(value);
            }
        }

        public string PanoramaLabelText
        {
            get
            {
                return panoramaLabelText;
            }
            set
            {
                panoramaLabelText = (String)GetLocalResourceObject(value);
            }
        }

        public string VerticalLabelText
        {
            get
            {
                return verticalLabelText;
            }
            set
            {
                verticalLabelText = (String)GetLocalResourceObject(value);
            }
        }

        public string PointOfView
        {
            get
            {
                return this.pointOfView.SelectedValue;
            }
            set
            {
                this.pointOfView.SelectedValue = value ?? CommonSchema.Contracts.V1.PointOfView.All.ToString();
                if (String.IsNullOrEmpty(value) || (Int32.Parse(value) == (int)Corbis.CommonSchema.Contracts.V1.PointOfView.All))
                {
                    this.pointOfViewSummaryDiv.Visible = false;
                    this.pointOfViewSummary.Text = string.Empty;
                }
                else
                {
                    this.pointOfViewSummaryDiv.Visible = true;
                    this.pointOfViewSummary.Text = CorbisBasePage.GetEnumDisplayText<PointOfView>((PointOfView)Int32.Parse(value));
                }
            }
        }

        public string NumberOfPeople
        {
            get
            {
                return this.numberOfPeople.SelectedValue;
            }
            set
            {
                this.numberOfPeople.SelectedValue = value;
                if (String.IsNullOrEmpty(value) || (Int32.Parse(value) == (int)CommonSchema.Contracts.V1.NumberOfPeople.WithPeople))
                {
                    this.numberOfPeopleSummaryDiv.Visible = false;
                    this.numberOfPeopleSummary.Text = string.Empty;
                }
                else
                {
                    this.numberOfPeopleSummaryDiv.Visible = true;
                    this.numberOfPeopleSummary.Text = CorbisBasePage.GetEnumDisplayText((NumberOfPeople)Int32.Parse(value));
                }
            }
        }

        public string ImmediateAvailability
        {
            get
            {
                return this.immediateAvailablility.SelectedValue;
            }
            set
            {
                this.immediateAvailablility.SelectedValue = value;
                if (String.IsNullOrEmpty(value) || (Int32.Parse(value) == (int)ImageAvailability.AllResolutions))
                {
                    this.immediateAvailabilitySummaryDiv.Visible = false;
                    this.immediateAvailabilitySummary.Text = string.Empty;
                }
                else
                {
                    this.immediateAvailabilitySummaryDiv.Visible = true;
                    this.immediateAvailabilitySummary.Text = CorbisBasePage.GetEnumDisplayText((ImageAvailability)Int32.Parse(value));
                }
            }
        }

        public string ImageNumbers
        {
            get
            {
                string imageNumberText = string.Empty;

                if (imageNumbers.Text != (string)GetLocalResourceObject("imageNumbers.Text"))
                {
                    imageNumberText = StringHelper.RemoveExtraSpaces(this.imageNumbers.Text);
                }

                return imageNumberText;
            }

            set
            {
                string tempValue = StringHelper.RemoveExtraSpaces(value);
                if (String.IsNullOrEmpty(tempValue))
                {
                    this.imageNumbersSummaryDiv.Visible = false;
                    this.imageNumbers.Text = this.imageNumbersSummary.Text = GetLocalResourceObject("imageNumbers.Text").ToString().Trim();
                }
                else
                {
                    this.imageNumbersSummaryDiv.Visible = true;
                    this.imageNumbers.Text = tempValue;
                    this.imageNumbersSummary.Text = StringHelper.Truncate(tempValue, 35);
                }
            }
        }

        private bool removeMoreSearchOptions = false;
        public bool RemoveMoreSearchOptions
        {
            get
            {
                return removeMoreSearchOptions;
            }
            set
            {
                removeMoreSearchOptions = value;
            }

        }

        public string PremiumCollectionsCountSummary
        {
            get
            {
                return this.premiumCollectionsCountSummary.Text;
            }
            set
            {
                this.premiumCollectionsCountSummary.Text = value;
            }
        }

        public string PremiumCollectionsTotalSummary
        {
            get
            {
                return this.premiumCollectionsTotalSummary.Text;
            }
            set
            {
                this.premiumCollectionsTotalSummary.Text = value;
            }
        }

        public string StandardCollectionsCountSummary
        {
            get
            {
                return this.standardCollectionsCountSummary.Text;
            }
            set
            {
                this.standardCollectionsCountSummary.Text = value;
            }
        }

        public string StandardCollectionsTotalSummary
        {
            get
            {
                return this.standardCollectionsTotalSummary.Text;
            }
            set
            {
                this.standardCollectionsTotalSummary.Text = value;
            }
        }

        public string ValueCollectionsCountSummary
        {
            get
            {
                return this.valueCollectionsCountSummary.Text;
            }
            set
            {
                this.valueCollectionsCountSummary.Text = value;
            }
        }

        public bool ShowReturnToPreviousSearch
        {
            get { return this.returnToPreviousSearchDiv.Visible; }
            set { this.returnToPreviousSearchDiv.Visible = value; }
        }

        /// <summary>
        /// Attaches the Previous (Direclty Manipulated) Search URL and displays or hides it 
        /// based on the business rules.
        /// </summary>
        public string PreviousSearchURL
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.returnPreviousSearchHyperlink.NavigateUrl = SiteUrls.SearchResults + "?" + value;

                    // Url decode both strings to make sure they are equivalent
                    // Dont show it when you are back to the page
                    if (HttpUtility.UrlDecode(Request.RawUrl) == HttpUtility.UrlDecode(returnPreviousSearchHyperlink.NavigateUrl))
//                        && (SearchPresenter.HasDirectlyManipulatedKeyword(Request.QueryString, true))
                       //)
                    {
                        ShowReturnToPreviousSearch = false;
                    }
                    else if (
                                (string.Equals(
                                    Request.CurrentExecutionFilePath,
                                    SiteUrls.SearchResults,
                                    StringComparison.InvariantCultureIgnoreCase
                                    )
                                )
                                || (string.Equals(
                                        Request.CurrentExecutionFilePath,
                                        SiteUrls.ImageGroups,
                                        StringComparison.InvariantCultureIgnoreCase)
                                    )
                                || (string.Equals(
                                        Request.CurrentExecutionFilePath,
                                        SiteUrls.MediaSetSearch,
                                        StringComparison.InvariantCultureIgnoreCase)
                                    )
                            )
                    {
                        ShowReturnToPreviousSearch = true;
                        keywordSearchText = this.keywordSearch.Text;
                    }
                    else
                    {
                        ShowReturnToPreviousSearch = false;
                    }
                }
                else
                {
                    ShowReturnToPreviousSearch = false;
                }
            }
        }

        public string ValueCollectionsTotalSummary
        {
            get
            {
                return this.valueCollectionsTotalSummary.Text;
            }
            set
            {
                this.valueCollectionsTotalSummary.Text = value;
            }
        }

        public string SuperValueCollectionsCountSummary
        {
            get
            {
                return this.superValueCollectionsCountSummary.Text;
            }
            set
            {
                this.superValueCollectionsCountSummary.Text = value;
            }
        }

        public string SuperValueCollectionsTotalSummary
        {
            get
            {
                return this.superValueCollectionsTotalSummary.Text;
            }
            set
            {
                this.superValueCollectionsTotalSummary.Text = value;
            }
        }

        public bool ShowPremiumCollectionsSummary
        {
            set
            {
                premiumCollectionsSummaryDiv.Visible = value;
            }
        }

        public bool ShowStandardCollectionsSummary
        {
            set
            {
                standardCollectionsSummaryDiv.Visible = value;
            }
        }

        public bool ShowValueCollectionsSummary
        {
            set
            {
                this.valueCollectionsSummaryDiv.Visible = value;
            }
        }

        public bool ShowSuperValueCollectionsSummary
        {
            set
            {
                superValueCollectionsSummaryDiv.Visible = value;
            }
        }

        public object MarketingCollection
        {
            get
            {
                return null;
            }
            set
            {
                mcg.DataSource = value;
                mcg.DataBind();
            }
        }

        public List<string> SelectedMarketingCollection
        {
            get
            {
                List<string> selectedCheckboxList = new List<string>();
                bool allSelected = true;
                List<DisplayGroupMarketingCollections> groups = mcg.DataSource as List<DisplayGroupMarketingCollections>;
                //foreach (var group in groups)
                foreach (DisplayGroupMarketingCollections group in groups)
                {
                    List<string> selected = GetSelectedMarketingCollections(group.DisplayGroupId);
                    selectedCheckboxList.AddRange(selected);
                    if (selected.Count != CountMarketingCollections(group.DisplayGroupId))
                    {
                        allSelected = false;
                    }
                }
                return allSelected ? new List<string>() : selectedCheckboxList;
            }
            set
            {
                List<string> selectedCheckboxList = new List<string>();

                List<DisplayGroupMarketingCollections> groups = mcg.DataSource as List<DisplayGroupMarketingCollections>;
                if (groups != null)
                {
                    foreach (DisplayGroupMarketingCollections group in groups)
                    {
                        Repeater rr = FindMarketingCollectionControl(group.DisplayGroupId);
                        if (rr != null)
                        {
                            foreach (RepeaterItem ri in rr.Items)
                            {
                                Corbis.Web.UI.Controls.ImageCheckbox ic = (Corbis.Web.UI.Controls.ImageCheckbox)ri.FindControl("mccb");
                                ic.Checked = value == null || value.Contains(ic.Value) || value.Count == 0;
                            }
                        }
                    }
                }
            }
        }

        public int CountMarketingCollections(MarketingCollectionGroupType groupType)
        {
            Repeater rr = FindMarketingCollectionControl(groupType);
            return rr != null ? rr.Controls.Count : 0;
        }

        private Repeater FindMarketingCollectionControl(MarketingCollectionGroupType groupType)
        {
            Repeater retValue = null;
            List<DisplayGroupMarketingCollections> data = mcg.DataSource as List<DisplayGroupMarketingCollections>;
            if (mcg.DataSource != null)
            {
                for (int i = 0; i < mcg.Controls.Count; i++)
                {
                    if (data[i].DisplayGroupId == groupType)
                    {
                        retValue = mcg.Controls[i].FindControl("mc") as Repeater;
                    }
                }
            }
            return retValue;
        }

        public List<string> GetSelectedMarketingCollections(MarketingCollectionGroupType group)
        {
            List<string> selectedGroupCollections = new List<string>();
            Repeater rr = FindMarketingCollectionControl(group);

            foreach (RepeaterItem ri in rr.Items)
            {
                Corbis.Web.UI.Controls.ImageCheckbox ic = (Corbis.Web.UI.Controls.ImageCheckbox)ri.FindControl("mccb");
                if (ic.Checked)
                {
                    selectedGroupCollections.Add(ic.Value);
                }
            }
            return selectedGroupCollections;
        }

        public bool ShowOptionsAppliedStyle
        {
            get { return bool.Parse(showOptionsAppliedStyle.Value); }
            set { showOptionsAppliedStyle.Value = value.ToString(); }
        }

        #endregion

        #region ISearchBaseView Members

        public bool Creative
        {
            get
            {
                return this.creative.Checked;
            }
            set
            {
                this.creative.Checked = value;
            }
        }

        public bool Editorial
        {
            get
            {
                return this.editorial.Checked;
            }
            set
            {
                this.editorial.Checked = value;
            }
        }

        public bool Documentary
        {
            get
            {
                return this.documentary.Checked;
            }
            set
            {
                this.documentary.Checked = value;
            }
        }

        public bool Archival
        {
            get
            {
                return this.archival.Checked;
            }
            set
            {
                this.archival.Checked = value;
            }
        }

        public bool CurrentEvents
        {
            get
            {
                return this.currentEvents.Checked;
            }
            set
            {
                this.currentEvents.Checked = value;
            }
        }

        public bool Entertainment
        {
            get
            {
                return this.entertainment.Checked;
            }
            set
            {
                this.entertainment.Checked = value;
            }
        }

        public bool FineArt
        {
            get
            {
                return this.fineArt.Checked;
            }
            set
            {
                this.fineArt.Checked = value;
            }
        }

        public bool Outline
        {
            get
            {
                return this.outline.Checked;
            }
            set
            {
                this.outline.Checked = value;
            }
        }

        public bool RightsManaged
        {
            get
            {
                return this.rightsManaged.Checked;
            }
            set
            {
                this.rightsManaged.Checked = value;
            }
        }

        public bool RoyaltyFree
        {
            get
            {
                return this.royaltyFree.Checked;
            }
            set
            {
                this.royaltyFree.Checked = value;
            }
        }

        public string OrientationSummary
        {
            get
            {
                return this.orientationSummary.Text;
            }
            set
            {
                this.orientationSummaryDiv.Visible = true;
                this.orientationSummary.Visible = true;
                this.orientationSummary.Text = value;
            }
        }

        [StateItemDesc("SearchSetting", "PreviousPagingNo", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public int PreviousPagingNo
        {
            get { return this.previousPagingNo; }
            set { this.previousPagingNo = value; }
        }

        [StateItemDesc("SearchFilters", "NoPeople", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public bool NoPeople
        {
            get { return this.noPeopleFilter; }
            set { this.noPeopleFilter = value; }
        }

        [StateItemDesc("SearchFilters", "Photography", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public bool Photography
        {
            get { return photographyFilter; }
            set { photographyFilter = value; }
        }

        [StateItemDesc("SearchFilters", "Illustration", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public bool Illustration
        {
            get { return illustrationFilter; }
            set { illustrationFilter = value; }
        }

        [StateItemDesc("SearchFilters", "Color", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public bool Color
        {
            get { return colorFilter; }
            set { colorFilter = value; }
        }

        [StateItemDesc("SearchFilters", "BlackWhite", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public bool BlackWhite
        {
            get { return blackWhiteFilter; }
            set { blackWhiteFilter = value; }
        }

        [StateItemDesc("SearchFilters", "ModelReleased", StateItemStore.AspSession, StatePersistenceDuration.Session)]
        public bool ModelReleased
        {
            get { return modelReleasedFilter; }
            set { modelReleasedFilter = value; }
        }

        #endregion

        #region ISearchView Members

        public SearchSort SearchSortOption
        {
            get
            {
                SearchSort sortOption;
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                int abc = stateItems.GetStateItemValue<int>(SearchKeys.Name, SearchKeys.SortOptionCookieKey, StateItemStore.Cookie);
                if (abc == (int)SearchSort.Unknown &&
                  Corbis.Web.Authentication.Profile.Current.Permissions.Contains(Permission.HasPermissionSearchOutline))
                {
                    sortOption = SearchSort.DatePublished;
                }
                //else if (abc == (int) SearchSort.Unknown && 
                //         Corbis.Web.Authentication.Profile.Current.Permissions.Contains(Permission.HasPermissionSearchOutline))
                //{
                //    // do nothing
                //}

                else if (abc != (int)SearchSort.Unknown && Enum.IsDefined(typeof(SearchSort), abc))
                {

                    sortOption = (SearchSort)Enum.Parse(typeof(SearchSort), abc.ToString());
                }
                else
                {

                    sortOption = SearchSort.Relevancy;
                }
                return sortOption;
            }
            set
            {
                StateItemCollection stateItems = new StateItemCollection(HttpContext.Current);
                StateItem<int> sortOption = new StateItem<int>(SearchKeys.Name, SearchKeys.SortOptionCookieKey, (int)value,
                                                               StateItemStore.Cookie,
                                                               StatePersistenceDuration.NeverExpire);
                stateItems.SetStateItem<int>(sortOption);
            }
        }

        //public SearchSort SearchSortOption
        //{
        //    get
        //    {
        //        return 2.ToString();
        //    }
        //    set
        //    {
        //        int a;
        //        SearchSort searchSort;
        //        bool parseFlag = int.TryParse(value, out a);
        //        if (parseFlag)
        //        {
        //            a = int.Parse(value);
        //            searchSort = (SearchSort) (Enum.IsDefined(typeof (SearchSort), a)
        //                                        ? Enum.Parse(typeof (SearchSort), value)
        //                                        : SearchSort.Relevancy);

        //        }
        //        else
        //        {
        //            searchSort = SearchSort.Relevancy;
        //        }
        //    }
        //}

        public bool IsMSOVisible
        {
            get
            {
                return this.hiddenMSOValue.Text == MSOState.Opened.ToString();
            }
        }


        #endregion
    }
}
