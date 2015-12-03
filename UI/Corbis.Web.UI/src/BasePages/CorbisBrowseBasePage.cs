using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.XPath;

using Corbis.Framework.Globalization;
using Corbis.Web.UI.Browse;
using Corbis.Web.Utilities;
using Corbis.Web.UI.Browse.MasterPages;

namespace Corbis.Web.UI
{
	public class CorbisBrowseBasePage : CorbisBasePage
	{
		private static XmlDocument configDoc = null;

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the CorbisBrowseBasePage class.
        /// </summary>
		public CorbisBrowseBasePage()
        {
        }

        #endregion

		#region Overrides

		protected override void OnPreInit(EventArgs e)
		{
			string masterpageFilename = string.Empty;
			string codeFile = Path.GetFileNameWithoutExtension(HttpContext.Current.Request.FilePath);
			XmlDocument doc = InitConfigDoc(XmlFilePath());
			XmlNode xmlNode = doc.DocumentElement;

			if (xmlNode.SelectSingleNode("MasterPage") != null)
			{
				masterpageFilename = xmlNode.SelectSingleNode("//MasterPage").InnerText.ToString();
				string masterpage = "/Browse/MasterPages/" + masterpageFilename + ".master";
				if (masterpage != this.MasterPageFile)
					this.MasterPageFile = masterpage;
			}

			// Add the page name as a css class to the body tag
			switch (masterpageFilename)
			{
				case "GlobalNav":
					((GlobalNav)this.Master).AddClassToBodyTag(codeFile);
					break;
				case "NoGlobalNav":
					((NoGlobalNav)this.Master).AddClassToBodyTag(codeFile);
					break;
				case "MasterBase":
					((MasterBase)this.Master).AddClassToBodyTag(codeFile);
					break;
			}

			AddOmnitureVariables();

			base.OnPreInit(e);
		}

		protected override void OnLoadComplete(EventArgs e)
		{
			AddMetaTags();
			AddJavascriptFiles();
			AddStylesheetFiles();
			AddBodyCopy();
			AddZoneMainModules();
			AddZoneLeftModules();
			AddCustomModules();
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Loads an Xml file based on the path provided
		/// </summary>
		/// <param name="docPath">The path to the Xml file</param>
		/// <returns>An Xml document object</returns>
		public static XmlDocument InitConfigDoc(string docPath)
		{
			configDoc = new XmlDocument();
			configDoc.Load(System.Web.HttpContext.Current.Server.MapPath(docPath));
			return configDoc;
		}

		/// <summary>
		/// Gets the path based on the codefile name
		/// </summary>
		/// <returns></returns>
		public static string BasicFilePath()
		{
			string returnFilePath = string.Empty;
			string fullFilePath = HttpContext.Current.Request.FilePath;
			string codeFile = Path.GetFileNameWithoutExtension(fullFilePath);
			if (codeFile.ToLower() == "default") codeFile = "HomePage";

			returnFilePath = "/Browse/xml/" + codeFile + "/";

			return returnFilePath;
		}

		/// <summary>
		/// Gets the path to the setup file for each page.
		/// Example: ~/Browse/xml/Archival/Archival.xml
		/// </summary>
		/// <returns>A string representing the path to an Xml document</returns>
		public static string XmlFilePath()
		{
			string returnFilePath = BasicFilePath();
			string codeFile = Path.GetFileNameWithoutExtension(HttpContext.Current.Request.FilePath);
			if (codeFile.ToLower() == "default") codeFile = "HomePage";

			returnFilePath += codeFile + ".xml";
			return returnFilePath;
		}

		/// <summary>
		/// Gets the path to the setup file for each language-specific page.
		/// Example: /Browse/xml/Archival/en-US/Default.xml
		/// </summary>
		/// <returns>A string representing the path to an Xml document</returns>
		public static string XmlLanguageFilePath(string docName)
		{
			string returnFilePath = BasicFilePath();
			string englishFilePath = string.Empty;
			string languageFilePath = string.Empty;
			string language = Language.CurrentLanguage.LanguageCode;
			
			englishFilePath = returnFilePath + "en-US/" + docName;
			languageFilePath = returnFilePath + language + "/" + docName;
			
			if (File.Exists(System.Web.HttpContext.Current.Server.MapPath(languageFilePath)))
				returnFilePath = languageFilePath;
			else
				returnFilePath = englishFilePath;

			return returnFilePath;
		}

		protected void AddMetaTags()
		{
			XmlDocument doc = InitConfigDoc(XmlLanguageFilePath("Default.xml"));
			XmlNode xmlNode = doc.DocumentElement;
			string language = Language.CurrentLanguage.LanguageCode;

			if (xmlNode.SelectSingleNode("PageTitle") != null)
			{
				HtmlHelper.AddMetaTagToPage(this, "PageTitle", xmlNode.SelectSingleNode("PageTitle").InnerText, true);
			}

			if (Profile.IsChinaUser && (xmlNode.SelectSingleNode("PageTitleChina") != null))
			{
				HtmlHelper.AddMetaTagToPage(this, "PageTitle", xmlNode.SelectSingleNode("PageTitleChina").InnerText, true);
			}

			if (xmlNode.SelectSingleNode("PageKeywords") != null)
			{
				HtmlHelper.AddMetaTagToPage(this, "Keywords", xmlNode.SelectSingleNode("PageKeywords").InnerText, true);
			}

			if (xmlNode.SelectSingleNode("PageDescription") != null)
			{
				HtmlHelper.AddMetaTagToPage(this, "Description", xmlNode.SelectSingleNode("PageDescription").InnerText, true);
			}

			HtmlHelper.AddMetaTagToPage(this, "Language", language, true);
		}

		protected void AddJavascriptFiles()
		{
			XmlDocument doc = InitConfigDoc(XmlFilePath());
			XmlNode xmlNode = doc.DocumentElement;

			if (xmlNode.SelectSingleNode("Scripts") != null)
			{
				string path = string.Empty;
				XmlNodeList nodes = xmlNode.SelectNodes("//Scripts/Script");

				foreach (XmlNode node in nodes)
				{
					path = BasicFilePath() + "Scripts/" + node.InnerText;
					HtmlHelper.AddJavascriptToPage(this, path, node.InnerText + "Script");
				}
			}
		}

		protected void AddStylesheetFiles()
		{
			XmlDocument doc = InitConfigDoc(XmlFilePath());
			XmlNode xmlNode = doc.DocumentElement;

			if (xmlNode.SelectSingleNode("StyleSheets") != null)
			{
				string path = string.Empty;
				XmlNodeList nodes = xmlNode.SelectNodes("//StyleSheets/StyleSheet");

				foreach (XmlNode node in nodes)
				{
					path = BasicFilePath() + "StyleSheets/" + node.InnerText;
					HtmlHelper.AddStylesheetToPage(this, path, node.InnerText + "Styles");
				}
			}
		}

		protected void AddOmnitureVariables()
		{
			XmlDocument doc = InitConfigDoc(XmlFilePath());
			XmlNode xmlNode = doc.DocumentElement;

			if (xmlNode.SelectSingleNode("OmnitureVars") != null)
			{
				string path = string.Empty;
				string nodeName = string.Empty;
				XmlNodeList nodes = xmlNode.SelectNodes("//OmnitureVars/OmnitureVar");

				foreach (XmlNode node in nodes)
				{
					nodeName = node.Attributes["name"].Value.ToString();
					AnalyticsData[nodeName] = node.InnerText.ToString();
				}
			}

			AnalyticsData["eVar1"] = Server.HtmlEncode(StringHelper.EncodeToJsString(Profile.UserName));
			AnalyticsData["eVar2"] = Server.HtmlEncode(Profile.Email);
		}

		private void AddBodyCopy()
		{
			ContentPlaceHolder mainContent = (ContentPlaceHolder)Page.Master.FindControl("mainContent");
			HtmlGenericControl Modules = (HtmlGenericControl)mainContent.FindControl("ZoneMainModules");

			XmlDocument doc = InitConfigDoc(XmlLanguageFilePath("Default.xml"));
			XmlNode xmlNode = doc.DocumentElement;

			if (xmlNode.SelectSingleNode("Content") != null)
			{
				HtmlGenericControl bodyCopy = new HtmlGenericControl("div");
				bodyCopy.Attributes["class"] = "MainContent";
				bodyCopy.InnerHtml = xmlNode.SelectSingleNode("Content").InnerXml.ToString();

				Modules.Controls.Add(bodyCopy);
			}
		}

		protected void AddZoneMainModules()
		{
			string language = Language.CurrentLanguage.LanguageCode;
			ContentPlaceHolder mainContent = (ContentPlaceHolder)Page.Master.FindControl("mainContent");
			HtmlGenericControl Modules = (HtmlGenericControl)mainContent.FindControl("ZoneMainModules");

			XPathDocument doc = new XPathDocument(Server.MapPath(XmlFilePath()));
			XPathNavigator nav = doc.CreateNavigator();
			XmlNamespaceManager nm = new XmlNamespaceManager(nav.NameTable);
			XPathExpression expr = nav.Compile("//Modules/Module[@zone='main']");
			expr.AddSort("@order", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);
			XPathNodeIterator iterator = nav.Select(expr);

			while (iterator.MoveNext())
			{
				string xmlSource = string.Empty;

				switch (iterator.Current.Value)
				{
					case "Collection":
						xmlSource = iterator.Current.GetAttribute("xmlSource", nm.DefaultNamespace);
						if (!string.IsNullOrEmpty(xmlSource))
						{
							Collection collection = (Collection)LoadControl("/Browse/Controls/Collection.ascx");
							collection.XmlSource = XmlLanguageFilePath(xmlSource);
							Modules.Controls.Add(collection);
						}
						break;
					case "FeaturedItems":
						xmlSource = iterator.Current.GetAttribute("xmlSource", nm.DefaultNamespace);
						if (!string.IsNullOrEmpty(xmlSource))
						{
							FeaturedItems featured = (FeaturedItems)LoadControl("/Browse/Controls/FeaturedItems.ascx");
							featured.XmlSource = XmlLanguageFilePath(xmlSource);
							Modules.Controls.Add(featured);
						}
						break;
					case "ImageSet":
						string pct = iterator.Current.GetAttribute("primaryContentTypeID", nm.DefaultNamespace);
						if (!string.IsNullOrEmpty(pct))
						{
							ImageSet imageset = (ImageSet)LoadControl("/Browse/Controls/ImageSet.ascx");
							imageset.PrimaryContentTypeID = pct;
							Modules.Controls.Add(imageset);
						}
						break;
					case "ModalSlideshow":
						xmlSource = iterator.Current.GetAttribute("xmlSource", nm.DefaultNamespace);
						if (!string.IsNullOrEmpty(xmlSource))
						{
							ModalSlideshow modalslideshow = (ModalSlideshow)LoadControl("/Browse/Controls/ModalSlideshow.ascx");
							modalslideshow.XmlSource = XmlLanguageFilePath(xmlSource);
							Modules.Controls.Add(modalslideshow);
						}
						break;
					case "Slideshow":
						xmlSource = iterator.Current.GetAttribute("xmlSource", nm.DefaultNamespace);
						if (!string.IsNullOrEmpty(xmlSource))
						{
							Slideshow slideshow = (Slideshow)LoadControl("/Browse/Controls/Slideshow.ascx");
							slideshow.XmlSource = xmlSource;
							Modules.Controls.Add(slideshow);
						}
						break;
				}
			}
		}

		protected void AddZoneLeftModules()
		{
			string language = Language.CurrentLanguage.LanguageCode;
			ContentPlaceHolder leftContent = (ContentPlaceHolder)Page.Master.FindControl("leftContent");
			HtmlGenericControl Modules = (HtmlGenericControl)leftContent.FindControl("ZoneLeftModules");

			XPathDocument doc = new XPathDocument(Server.MapPath(XmlFilePath()));
			XPathNavigator nav = doc.CreateNavigator();
			XmlNamespaceManager nm = new XmlNamespaceManager(nav.NameTable);
			XPathExpression expr = nav.Compile("//Modules/Module[@zone='left']");
			expr.AddSort("@order", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);
			XPathNodeIterator iterator = nav.Select(expr);

			while (iterator.MoveNext())
			{
				string xmlSource = string.Empty;

				switch (iterator.Current.Value)
				{
					case "ModuleImages":
						xmlSource = iterator.Current.GetAttribute("xmlSource", nm.DefaultNamespace);
						if (!string.IsNullOrEmpty(xmlSource))
						{
							ModuleImages moduleimages = (ModuleImages)LoadControl("/Browse/Controls/ModuleImages.ascx");
							moduleimages.XmlSource = XmlLanguageFilePath(xmlSource);
							Modules.Controls.Add(moduleimages);
						}
						break;
					case "ModuleList":
						xmlSource = iterator.Current.GetAttribute("xmlSource", nm.DefaultNamespace);
						if (!string.IsNullOrEmpty(xmlSource))
						{
							ModuleList modulelist = (ModuleList)LoadControl("/Browse/Controls/ModuleList.ascx");
							modulelist.XmlSource = XmlLanguageFilePath(xmlSource);
							Modules.Controls.Add(modulelist);
						}
						break;
					case "ModuleText":
						xmlSource = iterator.Current.GetAttribute("xmlSource", nm.DefaultNamespace);
						if (!string.IsNullOrEmpty(xmlSource))
						{
							ModuleText moduletext = (ModuleText)LoadControl("/Browse/Controls/ModuleText.ascx");
							moduletext.XmlSource = XmlLanguageFilePath(xmlSource);
							Modules.Controls.Add(moduletext);
						}
						break;
					case "ModuleThumbnails":
						xmlSource = iterator.Current.GetAttribute("xmlSource", nm.DefaultNamespace);
						if (!string.IsNullOrEmpty(xmlSource))
						{
							ModuleThumbnails modulethumbs = (ModuleThumbnails)LoadControl("/Browse/Controls/ModuleThumbnails.ascx");
							modulethumbs.XmlSource = XmlLanguageFilePath(xmlSource);
							Modules.Controls.Add(modulethumbs);
						}
						break;
					case "OutlineMembership":
						OutlineMembership outline = (OutlineMembership)LoadControl("/Browse/Controls/OutlineMembership.ascx");
						Modules.Controls.Add(outline);
						break;
				}
			}
		}

		protected void AddCustomModules()
		{
			ContentPlaceHolder leftContent = (ContentPlaceHolder)Page.Master.FindControl("leftContent");
			HtmlGenericControl Modules = (HtmlGenericControl)leftContent.FindControl("ZoneLeftModules");

			XPathDocument doc = new XPathDocument(Server.MapPath(XmlFilePath()));
			XPathNavigator nav = doc.CreateNavigator();
			XmlNamespaceManager nm = new XmlNamespaceManager(nav.NameTable);
			XPathExpression expr = nav.Compile("//CustomModules/CustomModule[@zone='left']");
			expr.AddSort("@order", XmlSortOrder.Ascending, XmlCaseOrder.None, "", XmlDataType.Number);
			XPathNodeIterator iterator = nav.Select(expr);

			while (iterator.MoveNext())
			{
				string xmlSource = iterator.Current.GetAttribute("xmlSource", nm.DefaultNamespace);
				string xslSource = iterator.Current.GetAttribute("xslSource", nm.DefaultNamespace);
				string stylesheet = iterator.Current.GetAttribute("stylesheet", nm.DefaultNamespace);
				string controlPath = iterator.Current.GetAttribute("controlPath", nm.DefaultNamespace);

				if (!string.IsNullOrEmpty(xmlSource) && !string.IsNullOrEmpty(xslSource))
				{
					Control customControl = LoadControl(controlPath, xmlSource, xslSource, stylesheet);
					Modules.Controls.Add(customControl);
				}
			}
		}
		#endregion

		private UserControl LoadControl(string UserControlPath, params object[] constructorParameters)
		{
			List<Type> constParamTypes = new List<Type>();
			foreach (object constParam in constructorParameters)
			{
				constParamTypes.Add(constParam.GetType());
			}

			UserControl control = this.Page.LoadControl(UserControlPath) as UserControl;

			ConstructorInfo constructor = control.GetType().BaseType.GetConstructor(constParamTypes.ToArray());
			if (constructor != null)
			{
				constructor.Invoke(control, constructorParameters);
			}

			return control;
		}
	}
}
