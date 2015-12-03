using System;

namespace Corbis.Web.UI.Navigation
{
	// Summary:
	//     Provides data for the Pager event.
	public class PagerEventArgs : EventArgs
	{
		private int pageIndex = 1;

		public PagerEventArgs(PagerEventArgs e)
		{
			this.PageIndex = e.PageIndex;
		}

		public PagerEventArgs(int pageIndex)
		{
			this.PageIndex = pageIndex;
		}

		#region Event properties

		public int PageIndex
		{
			get { return pageIndex; }
			set { pageIndex = value; }
		}

		#endregion
	}
}
