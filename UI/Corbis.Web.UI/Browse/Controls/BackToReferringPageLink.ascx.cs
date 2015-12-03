using System;
using System.IO;
using System.Web;

namespace Corbis.Web.UI.Browse.Controls
{
	public partial class BackToReferringPageLink : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			ConfigureBackLink();
		}

		/// <summary>
		/// Checks to see if the user came from a page on the corbis website
		/// if so, creates a link back to the page that they came from
		/// </summary>
		private void ConfigureBackLink()
		{
			string referringUrl = string.Empty;
			string referringPage = string.Empty;
			string referringPageName = string.Empty;
			string currentPage = string.Empty;

			if (Request.UrlReferrer != null)
			{
				referringUrl = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
				currentPage = HttpContext.Current.Request.Url.ToString().ToLower();

				// Make sure they came from the corbis domain and from the browse section
				if (referringUrl.IndexOf("corbis") >= 0 && referringUrl.IndexOf("browse") >= 0)
				{
					referringPage = Path.GetFileNameWithoutExtension(referringUrl);

					// if the resource doesn't exist, we don't want to throw an error,
					// just don't show the back link
					try
					{
						referringPageName = GetLocalResourceObject(referringPage).ToString();
						currentPage = Path.GetFileNameWithoutExtension(currentPage);

						// Make sure it's not the page we're currently on
						if (referringPage != currentPage)
						{
							BackLink.Text = "<div class=\"BackLink\"><p><a href=\"" + 
								referringPage + ".aspx\" title=\"" +
								referringPageName + "\">" + referringPageName + "</a></p></div>";
						}
					}
					catch { }
				}
			}

		}
	}
}