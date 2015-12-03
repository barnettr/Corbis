using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.Controls
{
	public class ImageList : CompositeDataBoundControl
	{
		#region Fields

		public delegate void ControlCommandDelegate(object sender, CommandEventArgs e);
		public event ControlCommandDelegate ImageCommand;

		#endregion

		#region Properties

		public List<KeyValuePair<string, string>> Items
		{
			get
			{
				object items = ViewState[this.ID + "_Items"];
				if (items == null)
				{
					items = new List<KeyValuePair<string, string>>();
					ViewState[this.ID + "_Items"] = items;
				}
				return items as List<KeyValuePair<string, string>>;
			}
			set { ViewState[this.ID + "_Items"] = value; }
		}

		private string itemImageUrlField;
		public string ItemImageUrlField
		{
			get { return itemImageUrlField; }
			set { itemImageUrlField = value; }
		}

		private string valueField;
		public string ValueField
		{
			get { return valueField; }
			set { valueField = value; }
		}

		private string spacing;
		public string Spacing
		{
			get { return spacing; }
			set { spacing = value; }
		}

		#endregion

		private void ProcessCommand(object sender, CommandEventArgs e)
		{
			if (ImageCommand != null)
			{
				ImageCommand(sender, e);
			}
		}

		#region CompositeDataBoundControl override

		protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
		{
			if (dataBinding && dataSource != null)
			{
				foreach (object o in dataSource)
				{
					KeyValuePair<string, string> elem = new KeyValuePair<string, string>(DataBinder.GetPropertyValue(o, ItemImageUrlField, null), DataBinder.GetPropertyValue(o, ValueField, null));
					Items.Add(elem);
				}
			}

			bool isFirstItem = true;
			foreach (KeyValuePair<string, string> item in Items)
			{
				ImageButton itemControl = new ImageButton();
				itemControl.CommandArgument = item.Value;
				itemControl.ImageUrl = item.Key;
				itemControl.Command += new CommandEventHandler(ProcessCommand);
				if (!isFirstItem)
				{
					itemControl.Style["padding-left"] = Spacing;
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
