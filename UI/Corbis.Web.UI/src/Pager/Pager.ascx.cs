using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Web.UI.HtmlControls;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Navigation
{
	public partial class Pager : CorbisBaseUserControl
	{
		#region Fields

		public delegate void PageCommandDelegate(object sender, PagerEventArgs e);        
		public event PageCommandDelegate PageCommand;

	    private string _prevCssClass;
	    private string _nextCssClass;
        private string _PrevDisabledCssClass;
        private string _NextDisabledCssClass;
	    private string _LabelCssClass;

	    private string _prefixTextFormat;
	    private string _postfixTextFormat;

		#endregion

		#region Properties

	    public Button PreviousButton
	    {
            get { return previous; }
	    }

	    public Button NextButton
	    {
            get { return next; }
	    }

	    public TextBox PageNumberTextBox
	    {
            get { return pageNumber; }
	    }

		public string PrevCssClass
		{
			get { return _prevCssClass; }
			set { _prevCssClass = value; }
		}

		public string NextCssClass
		{
			get { return _nextCssClass; }
			set { _nextCssClass = value; }
		}

		public string PrevDisabledCssClass
		{
			get { return _PrevDisabledCssClass; }
			set { _PrevDisabledCssClass = value; }
		}

		public string NextDisabledCssClass
		{
			get { return _NextDisabledCssClass; }
			set { _NextDisabledCssClass = value; }
		}

		public string LabelCssClass
		{
			get { return _LabelCssClass; }
			set { _LabelCssClass = value; }
		}

		public string PrefixLabelCssClass
		{
			get { return prefixText.CssClass; }
			set { prefixText.CssClass = value; }
		}

		public string PostfixLabelCssClass
		{
			get { return postfixText.CssClass; }
			set { postfixText.CssClass = value; }
		}

		public string PageNumberCssClass
		{
			get { return pageNumber.CssClass; }
			set { pageNumber.CssClass = value; }
		}

		public int PageIndex
		{
			get
			{
			    int page;
			    int.TryParse(pageNumber.Text, out page);
			    return Math.Max(1, page);
			}
			set
			{
                int pageIndex = Math.Max(1, value);

                if (TotalPages > 0)
                    pageIndex = Math.Min(pageIndex, TotalPages);

			    pageNumber.Text = pageIndex.ToString();
                origPageNumber.Value = pageNumber.Text;

                PreviousEnabled = pageIndex > 1;
                NextEnabled = TotalPages > 0 && pageIndex < TotalPages;
			}
		}

        public bool PreviousEnabled
        {
            get { return previous.Enabled; }
            set 
            {
                previous.Enabled = value;
                SetDisablePreviousCss();
            }
        }

        public bool NextEnabled
        {
            get { return next.Enabled; }
            set
            {
                next.Enabled = value;
                SetDisableNextCss();
            }
        }

	    public int PageSize
		{
			get 
			{ 
				int size = 20;
				int.TryParse(pageSize.Value, out size);
				return size;
			}
			set
			{
                ((CorbisBasePage)Page).AnalyticsData["prop6"] = value.ToString();

                this.pageSize.Value = value.ToString();

                // TODO Does this belong here????
                NextEnabled = PageIndex < TotalPages;
			}
		}

        public string PageText
        {
            get
            {
                return pageNumber.Text;
            }
        }

        private void SetDisableNextCss()
        {
            next.CssClass = next.Enabled ? NextCssClass : NextDisabledCssClass;
        }
        
	    private void SetDisablePreviousCss()
	    {
	        previous.CssClass = previous.Enabled ? PrevCssClass : PrevDisabledCssClass;
	    }

	    public int TotalRecords
		{
			get 
			{
				int total = 0;
				int.TryParse(totalItems.Value, out total);
				return total;
			}
			set
			{
				totalItems.Value = value.ToString();
                NextEnabled = PageIndex < TotalPages;
                pageNumber.MaxLength = TotalPages.ToString().Length;
			}
		}

		public int StartingRecord
		{
			get { return ((PageIndex - 1) * PageSize) + 1; }
		}

		public int EndingRecord
		{
			get { return Math.Min(StartingRecord + PageSize - 1, TotalRecords); }
		}

		public int TotalPages
		{
			get { return (PageSize == 0 ? 0: (int)Math.Ceiling((decimal)TotalRecords / (decimal)PageSize)); }
		}

		#endregion

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			Bind();
		}
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            SetDisableNextCss();
            SetDisablePreviousCss();
            if(!string.IsNullOrEmpty(this.pageNumber.Text))
            {
			ScriptManager.RegisterStartupScript(this, this.GetType(), "textChangedVariable", "var textChanged = false;", true);
            }
        }

        public string PrefixTextFormat
        {
            get { return _prefixTextFormat ?? (string)GetLocalResourceObject("prefixTextFormat"); }
            set { _prefixTextFormat = value; }
        }

        public string PostfixTextFormat
        {
            get { return _postfixTextFormat ?? (string)GetLocalResourceObject("postfixTextFormat"); }
            set { _postfixTextFormat = value; }
        }

	    public void Bind()
		{
            int totalPages = Math.Max(1, TotalPages);
            prefixText.Text = PrefixTextFormat.Replace("{0}", totalPages.ToString());
            postfixText.Text = PostfixTextFormat.Replace("{0}", totalPages.ToString());
		}

		protected void ProcessCommand(object sender, EventArgs e)
		{
			PageIndex = int.Parse(((CommandEventArgs)e).CommandArgument.ToString()) + int.Parse(this.pageNumber.Text);
			RaisePageCommand(sender);
		}

		protected void ProcessTextChange(object sender, EventArgs e)
		{
			int newPageNumber;
            pageNumber.Text = string.IsNullOrEmpty(pageNumber.Text) ? "1": pageNumber.Text;
			if (int.TryParse(pageNumber.Text, out newPageNumber))
			{
				PageIndex = Math.Max(1, Math.Min(newPageNumber, TotalPages));
				RaisePageCommand(sender);
			}
			else
			{
				pageNumber.Text = PageIndex.ToString();
			}
		}

		#region Helper methods

		private void RaisePageCommand(object sender)
		{
			if (PageCommand != null)
			{
				PageCommand(sender, new PagerEventArgs(PageIndex));
			}
		}

		#endregion
	}
}
