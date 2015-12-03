using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace Corbis.Web.Utilities
{
    public static class HtmlHelper
    {

        public static void CreateStylesheetControl(Page page, string stylesheet, string stylesheetId)
        {
            CreateStylesheetControl(page, stylesheet, stylesheetId, null);
        }

        public static void CreateStylesheetControl(Page page, string stylesheet, string stylesheetId, string theme)
        {
            if(page.Header == null)
            {
                return;
            }

             
             if (StringHelper.IsNullOrTrimEmpty(stylesheet))
                 return;

             if (StringHelper.IsNullOrTrimEmpty(stylesheetId))
                 return;


            if(StringHelper.IsNullOrTrimEmpty(theme))
            {
                stylesheet = "~" + stylesheet;
            }
            else
            {
                stylesheet = "~/App_Themes/" + theme + stylesheet;
            }
        
            HtmlLink link = new HtmlLink();

            stylesheet = stylesheet.Contains("?") ? stylesheet + "&" : stylesheet + "?";
            stylesheet = stylesheet + "v=" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            link.Href = stylesheet;
            link.ID = stylesheetId;
            link.Attributes.Add("type", "text/css");
            link.Attributes.Add("rel", "stylesheet");

            page.Header.Controls.Add(link);
		}

        public static Control FindControlIterative(Control root, string id)
        {
            Control ctl = root;
            LinkedList<Control> ctls = new LinkedList<Control>();

            while (ctl != null)
            {
                if (ctl.ID == id)
                    return ctl;
                foreach (Control child in ctl.Controls)
                {
                    if (child.ID == id)
                        return child;
                    if (child.HasControls())
                        ctls.AddLast(child);
                }
                ctl = ctls.First.Value;
                ctls.Remove(ctl);
            }
            return null;
		}

		/// <summary>
		/// Checks to see if a javascript file has already been included in a page
		/// before adding it
		/// </summary>
		/// <param name="page">The page context</param>
		/// <param name="javascript">The path and file name of the javascript file to add</param>
		/// <param name="javascriptId">The unique identifier of the javascript file</param>
		public static void AddJavascriptToPage(Page page, string javascript, string javascriptId)
		{
			bool exists = false;

			foreach (Control HeaderControl in page.Header.Controls)
			{
				if (HeaderControl.ID == javascriptId)
					exists = true;
			}

			if (!exists)
				CreateJavascriptControl(page, javascript, javascriptId);
		}

		/// <summary>
		/// Adds a javascript file to a page
		/// </summary>
		/// <param name="page">The page context</param>
		/// <param name="javascript">The javascript file to add</param>
		/// <param name="javascriptId">The unique identifier for the javascript file</param>
		private static void CreateJavascriptControl(Page page, string javascript, string javascriptId)
		{
			HtmlGenericControl js = new HtmlGenericControl("script");
			js.Attributes["type"] = "text/javascript";
			js.Attributes["src"] = javascript;
			js.ID = javascriptId;
			page.Header.Controls.Add(js);
		}

		/// <summary>
		/// Checks the page's header controls for a stylesheet
		/// before attempting to add it to avoid duplication
		/// </summary>
		/// <param name="page">The page context</param>
		/// <param name="stylesheet">The path to the stylesheet</param>
		/// <param name="stylesheetId">The unique name/id of the stylesheet</param>
		public static void AddStylesheetToPage(Page page, string stylesheet, string stylesheetId)
		{
			bool exists = false;
			foreach (Control HeaderControl in page.Header.Controls)
			{
				if (HeaderControl.ID == stylesheetId)
					exists = true;
			}
			if (!exists)
				CreateStylesheetControl(page, stylesheet, stylesheetId);
		}

		/// <summary>
		/// Checks the page's header controls for a meta tag
		/// before attempting to add it to avoid duplication.
		/// If another meta tag with the same Name is found, it
		/// uses the boolean value to remove it if set to true.
		/// </summary>
		/// <param name="page">The page context</param>
		/// <param name="Key">The name of the meta tag (ie: Keywords, Description)</param>
		/// <param name="Value">The content of the meta tag</param>
		/// <param name="RemoveExisting">If set to true, a matching meta tag will be removed if found, otherwise the new meta tag will not be added if an existing one is found</param>
		public static void AddMetaTagToPage(Page page, string Key, string Value, bool RemoveExisting)
		{
			bool exists = false;

			if (Key == "PageTitle" && RemoveExisting)
			{
				page.Header.Title = Value;
			}
			else
			{
				foreach (Control HeaderControl in page.Header.Controls)
				{
					if (HeaderControl.GetType().ToString() == "System.Web.UI.HtmlControls.HtmlMeta")
					{
						if (((HtmlMeta)HeaderControl).Name == Key)
						{
							if (RemoveExisting)
								page.Header.Controls.Remove(HeaderControl);
							else
								exists = true;
						}
					}
				}
				if (!exists)
					CreateMetaTagControl(page, Key, Value);
			}
		}

		/// <summary>
		/// Adds a meta tag to the page
		/// </summary>
		/// <param name="page">The page context</param>
		/// <param name="Key">The name of the meta tag (ie: Keywords, Description)</param>
		/// <param name="Value">The content of the meta tag</param>
		public static void CreateMetaTagControl(Page page, string Key, string Value)
		{
			HtmlMeta hm = new HtmlMeta();
			HtmlHead head = (HtmlHead)page.Header;

			hm.Name = Key;
			hm.Content = Value;

			head.Controls.Add(hm);
		}
	}
}
