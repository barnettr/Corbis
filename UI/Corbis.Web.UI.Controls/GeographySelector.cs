using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Corbis.LightboxCart.Contracts.V1;

namespace Corbis.Web.UI.Controls
{
    [ControlValuePropertyAttribute("CommaDelimitedSelectedValueUids")]
    [ValidationPropertyAttribute("CommaDelimitedSelectedValueUids")]     
    public class GeographySelector : CompositeControl
    {

        private string separatorText = "--------------------------";
        public string SeparatorText
        {
            set
            {
                separatorText = value;
            }
        }

        private string unknownGeographyText = "??????????????????????????";
        public string UnknownGeographyText
        {
            set
            {
                unknownGeographyText = value;
            }
        }



        private string addButtonText = "Add ->";
        public string AddButtonText
        {
            set
            {
                addButtonText = value;
            }
        }

        private string removeButtonText = "<- Remove";
        public string RemoveButtonText
        {
            set
            {
                removeButtonText = value;
            }
        }

        private int listRows = 10;
        public int ListRows
        {
            set
            {
                listRows = value;
            }
        }

        public EventHandler SelectedValueChanged;
            
        public List<Guid> SelectedValueUids
        {
            get
            {
                List<Guid> returnList = new List<Guid>();
                if (!String.IsNullOrEmpty(((TextBox)this.FindControl("SelectedGeography")).Text))
                {
                    returnList = new List<string>(((TextBox)this.FindControl("SelectedGeography")).Text.Split(',')).ConvertAll<Guid>(new Converter<string, Guid>(delegate(string guidString) { return new Guid(guidString); }));
                }

                return returnList;
            }
            set
            {
                SetSelectedGeography(value);
                ListBox rightSide = (ListBox)this.FindControl("RightSide");
                string[] itemGuids = new string[rightSide.Items.Count];

                for (int i=0; i<rightSide.Items.Count; i++)
                {
                    itemGuids[i] = rightSide.Items[i].Value;
                }

                ((TextBox)this.FindControl("SelectedGeography")).Text = String.Join(",", itemGuids);
            }
        }

        public string CommaDelimitedSelectedValueUids
        {
            get { return ((TextBox)this.FindControl("SelectedGeography")).Text; }
        }

        public List<string> SelectedItemsText
        {
            get
            {
                List<string> returnList = new List<string>();
                ListBox leftSide = (ListBox)this.FindControl("LeftSide");
                foreach (Guid item in SelectedValueUids)
                {
                    ListItem listItem = leftSide.Items.FindByValue(item.ToString());
                    if (listItem != null)
                    {
                        returnList.Add(listItem.Text);
                    }
                }

                return returnList;
            }
        }

        private bool autoPostBack = false;
        public bool AutoPostBack
        {
            set
            {
                autoPostBack = value;
            }
            get
            {
                return autoPostBack;
            }
        }

        public List<UseTypeAttributeValue> DataSource
        {
            set
            {
                ViewState["DataSource"] = value;
                List<UseTypeAttributeValue>  dataSource = value;
                foreach (UseTypeAttributeValue utav in dataSource)
                {
                    if (utav.ValueUid == Guid.Empty)
                    {
                        utav.DisplayText = separatorText;
                    }
                }

                ListBox ddLeft = (ListBox)FindControl("LeftSide");
                ddLeft.DataSource = dataSource;
                ddLeft.DataTextField = "DisplayText";
                ddLeft.DataValueField = "ValueUid";
                ddLeft.DataBind();
            }
            get
            {
                return (List<UseTypeAttributeValue>)ViewState["DataSource"];
            }
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            #region Main Div
            HtmlGenericControl mainDiv = new HtmlGenericControl("div");
            mainDiv.Attributes.Add("class", "GeographyControl");
            #endregion

            #region Left side dropdown
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "GeographyControlLeftList");

            ListBox ddLeft = new ListBox();
            ddLeft.Rows = listRows;
            ddLeft.ID = "LeftSide";
            ddLeft.Attributes.Add("ondoubleclick", "GeographySelectorAdd();");
            ddLeft.SelectionMode = ListSelectionMode.Multiple;
            

            div.Controls.Add(ddLeft);

            mainDiv.Controls.Add(div);

            #endregion

            #region Buttons
            div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "GeographyControlButtons");

            // Add button
            HtmlGenericControl div2 = new HtmlGenericControl("div");
            div2.Attributes.Add("class", "GeographyControlAddButton");

            Button addButton = new Button();
            addButton.CssClass = "NextButton";
            //addButton.Attributes["class"] = "NextButton";
            //addButton.Text = addButtonText;
            addButton.UseSubmitBehavior = true;
            addButton.CausesValidation = false;
            addButton.OnClientClick = "GeographySelectorAdd();return false;";

            div2.Controls.Add(addButton);
            div.Controls.Add(div2);

            div2 = new HtmlGenericControl("div");
            div2.Attributes.Add("class", "GeographyControlRemoveButton");

            Button removeButton = new Button();
            removeButton.CssClass = "GeoPrevButton";
            removeButton.UseSubmitBehavior = true;
            removeButton.CausesValidation = false;
            removeButton.OnClientClick = "GeographySelectorRemove();return false;";

            div2.Controls.Add(removeButton);
            div.Controls.Add(div2);

            mainDiv.Controls.Add(div);
            #endregion

            #region Right side dropdown
            div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "GeographyControlRightList");

            ListBox ddRight = new ListBox();
            ddRight.Rows = listRows;
            ddRight.ID = "RightSide";
            ddRight.Attributes.Add("ondoubleclick", "GeographySelectorRemove();");
            ddRight.SelectionMode = ListSelectionMode.Multiple;

            div.Controls.Add(ddRight);

            System.Web.UI.WebControls.TextBox SelectedGeography = new TextBox();
            // Need to add the same attributes to the textbox as that's the control that 
            // is validated
            foreach (string key in base.Attributes.Keys)
            {
                SelectedGeography.Attributes.Add(key, base.Attributes[key]);
            }
            SelectedGeography.Style.Add("display", "none");
            if (autoPostBack)
            {
                SelectedGeography.TextChanged += SelectedValueChanged;
                SelectedGeography.AutoPostBack = true;
            }
            SelectedGeography.ID = "SelectedGeography";
            div.Controls.Add(SelectedGeography);

            mainDiv.Controls.Add(div);

            #endregion
            this.Controls.Add(mainDiv);

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetSelectedGeography(SelectedValueUids);
        }

        private void SetSelectedGeography(List<Guid> selectedGeography)
        {
            ListBox rightSide = (ListBox)this.FindControl("RightSide");
            ListBox leftSide = (ListBox)this.FindControl("LeftSide");
            rightSide.Items.Clear();
            foreach (Guid itemGuid in selectedGeography)
            {
                ListItem listItem = leftSide.Items.FindByValue(itemGuid.ToString());
                if (listItem != null)
                {
                    listItem.Selected = false;
                    rightSide.Items.Add(listItem);
                }
                else
                {
                    // still need to add it even if it's not found for Validation reasons
                    listItem = new ListItem(unknownGeographyText, itemGuid.ToString());
                    rightSide.Items.Add(listItem);
                }
            }
        }
    }
}
