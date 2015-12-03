using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.Controls
{
	[DefaultProperty("Text")]
	[ToolboxData("<{0}:LinkButtonList runat=server></{0}:LinkButtonList>")]
	public class LinkButtonList : CompositeDataBoundControl
	{
		#region Fields

		public delegate void ControlCommandDelegate(object sender, CommandEventArgs e);
		public event ControlCommandDelegate ControlCommand;

		#endregion

		#region properties

		public List<KeyValuePair<string, string>> Items
		{
			get
			{
				object items = ViewState["Items"];
				if (items == null)
				{
					items = new List<KeyValuePair<string, string>>();
					ViewState["Items"] = items;
				}
				return items as List<KeyValuePair<string, string>>;
			}
			set { ViewState["Items"] = value; }
		}

		private string cssClassField;
		public string CssClassField
		{
			get { return cssClassField; }
			set { cssClassField = value; }
		}

		private string valueField;
		public string ValueField
		{
			get { return valueField; }
			set { valueField = value; }
		}

		private bool valueIsScript;
		public bool ValueIsScript
		{
			get { return valueIsScript; }
			set { valueIsScript = value; }
		}

		private string spacing;
		public string Spacing
		{
			get { return spacing; }
			set { spacing = value; }
		}

		public string SelectedValue
		{
			get
			{
				return (String)ViewState["SelectedValue"] ?? String.Empty;
			}
			set
			{
				ViewState["SelectedValue"] = value;
			}
		}

		public string ClientScriptFunction
		{
			get
			{
				return (String)ViewState["ClientScriptFunction"] ?? String.Empty;
			}
			set
			{
				ViewState["ClientScriptFunction"] = value;
			}
		}

		#endregion

		private void ProcessCommand(object sender, CommandEventArgs e)
		{
			if (ControlCommand != null)
			{
				this.SelectedValue = e.CommandArgument.ToString();
				foreach (Control childControl in Controls)
				{
					if (childControl is LinkButton)
					{
						LinkButton linkButton = childControl as LinkButton;
						linkButton.Enabled = true;
						if (linkButton.CssClass.EndsWith("Selected")) linkButton.CssClass = linkButton.CssClass.Replace("Selected", "");

						if (linkButton.CommandArgument == e.CommandArgument.ToString())
						{
							linkButton.Enabled = false;
							linkButton.CssClass += "Selected";
						}
					}
				}

				ControlCommand(sender, e);
			}
		}

		#region CompositeDataBoundControl override

		protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
		{
			if (dataBinding && dataSource != null)
			{
				Items = new List<KeyValuePair<string, string>>();

				foreach (object o in dataSource)
				{
					KeyValuePair<string, string> elem = new KeyValuePair<string, string>(DataBinder.GetPropertyValue(o, CssClassField, null), DataBinder.GetPropertyValue(o, ValueField, null));
					Items.Add(elem);
				}
			}
			
			bool isFirstItem = true;
			foreach (KeyValuePair<string, string> item in Items)
			{
				LinkButton itemControl = new LinkButton();
				itemControl.CommandArgument = (ValueIsScript? item.Key: item.Value);
				itemControl.Command += new CommandEventHandler(ProcessCommand);
				itemControl.ID = item.Key;
				object toolTip = HttpContext.GetLocalResourceObject(Page.Request.Path, item.Key);
				if (toolTip != null) itemControl.ToolTip = toolTip.ToString();
                if (item.Value.ToLower() == "disabled")
                {
                    itemControl.CssClass = item.Key + " disabled";
                    itemControl.Enabled = false;
                }
                else
                {
                    if (!String.IsNullOrEmpty(ClientScriptFunction) || (ValueIsScript && !String.IsNullOrEmpty(item.Value)))
                    {
                        itemControl.OnClientClick = (ValueIsScript ? item.Value : String.Format("javascript:return {0}('{1}');", ClientScriptFunction, item.Value));
                    }
                    if (item.Value == SelectedValue)
                    {
                        itemControl.CssClass = item.Key + "Selected";
                        itemControl.Enabled = false;
                    }
                    else
                    {
                        itemControl.CssClass = item.Key;
                    }
                }
				if (!isFirstItem)
				{
					Panel spacer = new Panel();
					Image blankImage = new Image();
					blankImage.ImageUrl = "/Images/spacer.gif";
					spacer.Controls.Add(blankImage);
					spacer.Style.Add("width", Spacing);
					spacer.Style.Add("float", "left");
					Controls.Add(spacer);
				}
				else
				{
					isFirstItem = false;
				}

				Controls.Add(itemControl);
			}

			return Items.Count;
		}

		#endregion
	}
}
