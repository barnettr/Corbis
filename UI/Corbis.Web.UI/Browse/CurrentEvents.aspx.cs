using System;

using Corbis.Web.Utilities;


namespace Corbis.Web.UI.Browse
{
	public partial class CurrentEvents : CorbisBrowseBasePage
	{

		protected void Page_Load(object sender, EventArgs e)
		{

			// Hide the News image set if Chinese is the selected language
			if (Profile.IsChinaUser)
			{
				NewsImageSet.Visible = false;

				// Change the subtitle to hide the word "News" from Chinese users
				PageSubtitle.Text = GetLocalResourceObject("zh-CHS_pageSubtitle.Text").ToString();
			}

			SportsImageSet.ImageSetTitle = GetLocalResourceObject("SportsImageSetTitle").ToString();
			SportsImageSet.ImageSetSubtitle = GetLocalResourceObject("SportsImageSetSubtitle").ToString();
			NewsImageSet.ImageSetTitle = GetLocalResourceObject("NewsImageSetTitle").ToString();
			NewsImageSet.ImageSetSubtitle = GetLocalResourceObject("NewsImageSetSubtitle").ToString();
		}

	}
}
