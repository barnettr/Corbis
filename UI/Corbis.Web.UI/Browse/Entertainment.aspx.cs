using System;


namespace Corbis.Web.UI.Browse
{
	public partial class Entertainment : CorbisBrowseBasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			EntertainmentImageSet.ImageSetTitle = GetLocalResourceObject("EntertainmentImageSetTitle").ToString();
			EntertainmentImageSet.ImageSetSubtitle = GetLocalResourceObject("EntertainmentImageSetSubtitle").ToString();
		}

	}
}
