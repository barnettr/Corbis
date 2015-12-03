using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;

using Corbis.Framework.Globalization;


namespace Corbis.Web.UI.Corporate.Controls
{
	public partial class PressReleaseList : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, EventArgs e)
		{
			PressReleaseYears.DataSource = AvailableYears();
			PressReleaseYears.DataBind();

			PressReleasesByYear.DataSource = AllPressReleases();
			PressReleasesByYear.DataBind();
		}

		#region Data Binding
		protected virtual void PressReleaseYears_OnItemDataBound(Object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Header)
			{
				Literal yearNavTitle = (Literal)e.Item.FindControl("YearNavTitle");
				yearNavTitle.Text = GetLocalResourceObject("YearNavTitle").ToString();
			}
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if (e.Item.DataItem is string)
				{
					string year = e.Item.DataItem.ToString();

					Literal yearLink = (Literal)e.Item.FindControl("Year");
					yearLink.Text = "<a id=\"navYear" + year + "\" href=\"#Year" + year + "\" title=\"" + 
						GetLocalResourceObject("YearLinkTitle").ToString() + year + "\">" + year + "</a>";
				}
			}
		}

		protected virtual void PressReleasesByYear_OnItemDataBound(Object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				if (e.Item.DataItem is XmlNode)
				{
					XmlNode data = (XmlNode)e.Item.DataItem;
					Literal pressRelease = (Literal)e.Item.FindControl("PressRelease");
					string eventDate = string.Empty;
					string eventID = string.Empty;
					string eventTitle = string.Empty;
					string year = string.Empty;

					if (data.SelectSingleNode("EventId") != null)
						eventID = data.SelectSingleNode("EventId").InnerText.ToString();
					if (data.SelectSingleNode("EventDate") != null)
						eventDate = data.SelectSingleNode("EventDate").InnerText.ToString();
					if (data.SelectSingleNode("EventDesc") != null)
						eventTitle = data.SelectSingleNode("EventDesc").InnerText.ToString().Trim();

					year = GetPressReleaseYear(eventDate);

					pressRelease.Text = "<li class=\"tab Year" + year + " hidden\">";
					pressRelease.Text += "<span class=\"eventDate\">" + eventDate + "</span>";
					pressRelease.Text += "<span class=\"eventTitle\">" +
						"<a href=\"/Corporate/Pressroom/PressRelease.aspx?id=" + eventID + 
						"\" title=\"" + GetLocalResourceObject("PressReleaseLinkTitle").ToString() +
						"\" rel=\"600_600_modal\">" + eventTitle + "</a></span></li>";
				}
			}
		}
		#endregion

		#region Data Sources
		/// <summary>
		/// Get all unique years from the PressReleases.xml file
		/// </summary>
		/// <returns>A list of unique years</returns>
		protected List<string> AvailableYears()
		{
			List<string> returnList = new List<string>();
			string eventDate = string.Empty;
			string year = string.Empty;

			XmlDocument doc = CorbisCorporateBasePage.InitConfigDoc(CorbisCorporateBasePage.XmlLanguageFilePath("PressReleases.xml"));
			XmlNode xmlNode = doc.DocumentElement;
			XmlNodeList nodes = xmlNode.SelectNodes("//CorbisCorp/Events/EventDate");

			foreach (XmlNode node in nodes)
			{
				eventDate = node.InnerText;
				year = GetPressReleaseYear(eventDate);

				if (!returnList.Contains(year))
					returnList.Add(year);
			}

			return returnList;
		}

		/// <summary>
		/// Parse the string to find the year of the press release
		/// </summary>
		/// <param name="eventDate">A string representing a date</param>
		/// <returns>Hopefully a year</returns>
		private string GetPressReleaseYear(string eventDate)
		{
			string returnYear = string.Empty;
			string language = Language.CurrentLanguage.LanguageCode;

			if (language != "ja-JP")
			{
				if (eventDate.LastIndexOf(" ") > 0)
					returnYear = eventDate.Substring(eventDate.LastIndexOf(" ") + 1);
			}
			else
			{
				if (eventDate.IndexOf(" ") >= 0)
					returnYear = eventDate.Substring(0, eventDate.IndexOf(" "));
			}

			return returnYear;
		}

		protected List<XmlNode> AllPressReleases()
		{
			List<XmlNode> returnList = new List<XmlNode>();
			XmlDocument doc = CorbisCorporateBasePage.InitConfigDoc(CorbisCorporateBasePage.XmlLanguageFilePath("PressReleases.xml"));
			XmlNode xmlNode = doc.DocumentElement;
			XmlNodeList nodes = xmlNode.SelectNodes("//CorbisCorp/Events");

			foreach (XmlNode node in nodes)
			{
				returnList.Add(node);
			}

			return returnList;
		}
		#endregion
	}
}