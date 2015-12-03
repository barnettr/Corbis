using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Corbis.Web.UI.Presenters;
using Languages = Corbis.Framework.Globalization.Language;

namespace Corbis.Web.UI.Navigation
{
    [DefaultEvent("ItemCommand")]
    [DefaultProperty("dropDownItems")]
   public class DropDownMenu: CompositeControl

   {
       private List<DropDownMenuData> dropDownItems;
       //private ArrayList _boundControls;
      private HtmlGenericControl div;
       //private string imageControlID;


       #region Events
       // ******************************************************************************
       // Public events
       public event EventHandler<DropDownMenuChangedEventArgs> ItemCommand;
       #endregion


       #region Properties: DropDownMenuItems  
   
       [Description("Gets the collection of the menu items")]
       public List<DropDownMenuData> DropDownItems
       {
           get
           {
               if (dropDownItems == null)
                   dropDownItems = new List<DropDownMenuData>();
               return dropDownItems;
           }

           set
           {

               this.dropDownItems = value;
           }
       }

       #endregion


       #region Rendering       



       // **************************************************************************************************
       // METHOD OVERRIDE: CreateChildControls
       // Builds the UI of the control
       protected override void CreateChildControls()
       {
           // A DropDown menu is an invisible DIV that is moved around via scripting when the user
           // hover on a bound HTML tag

           Controls.Clear();
           div = new HtmlGenericControl("div");
           div.Attributes.Add("class", "DivMain");
           div.ID = "Top";

           this.Controls.Add(div);
           //if (AutoHide)
           //{

                //div.Attributes["onmousemove"] = "SetImage('" + imageControlID + "','" + VirtualPathUtility.ToAbsolute("~/App_Themes/BlackBackground/Images/ArrowDown.jpg") + "');";
                //div.Attributes["onmouseout"] = "SetImage('" + imageControlID + "','" + VirtualPathUtility.ToAbsolute("~/App_Themes/BlackBackground/Images/Arrow.jpg") + "');";

           //}

           // Loop on  DropDown Items and add rows to the table
           foreach (DropDownMenuData item in dropDownItems)
           {
               #region parentMenu
               // Configure the menu item
               HtmlGenericControl parentItemContainer = new HtmlGenericControl("div");
               parentItemContainer.ID = "parentItemContainer" + item.ParentText;
               //parentItemContainer.ID = "parentItemContainer" + (String)HttpContext.GetLocalResourceObject("~/src/Navigation/GlobalNav.ascx", item.ParentText);
               parentItemContainer.Attributes.Add("class", "ParentItemContainer");

               div.Controls.Add(parentItemContainer);

               // Add the link for post back
               LinkButton button = new LinkButton();
               button.CausesValidation = false;
               button.ID = "button" + item.ParentText;
               button.Click += new EventHandler(this.ButtonClicked);
               button.ToolTip = (String)HttpContext.GetLocalResourceObject("~/src/Navigation/GlobalNav.ascx", item.ParentTooltip);
               button.CommandArgument = item.ParentPageUrl;

               HtmlGenericControl menuItemDiv = new HtmlGenericControl("div");
               menuItemDiv.ID = "menuItem" + item.ParentText;
               menuItemDiv.Attributes.Add("class", "MenuItem");

               Label label = new Label();
               label.Attributes.Add("class", "MenuItemText");
               label.ID = "label" + item.ParentText;
               label.Text = (String)HttpContext.GetLocalResourceObject("~/src/Navigation/GlobalNav.ascx", item.ParentText);

               menuItemDiv.Controls.Add(label);
               button.Controls.Add(menuItemDiv);
               parentItemContainer.Controls.Add(button);

               if (Context.Request.Url.AbsolutePath.Equals(ResolveUrl(item.ParentPageUrl), StringComparison.InvariantCultureIgnoreCase))
               {
                   button.Enabled = false;
                   parentItemContainer.Attributes.Add("class", "ParentItemContainer Chevron");
               }
               #endregion

               #region for child Menu
               /// for child Directories
               /// 

               if (item.childMenus != null && item.childMenus.Count > 0)
               {
                   foreach (DropDownMenuItemData itemChild in item.childMenus)
                   {

                       HtmlGenericControl childItemContainer = new HtmlGenericControl("div");
                       
                       parentItemContainer.ID = "childItemContainer" + (String)HttpContext.GetLocalResourceObject("~/src/Navigation/GlobalNav.ascx", itemChild.Text);
                       childItemContainer.Attributes.Add("class", "ChildItemContainer");

                       // Add the link for post back to a selected page
                       LinkButton buttonChild = new LinkButton();
                       
                       buttonChild.ID = "buttonChild" + (String)HttpContext.GetLocalResourceObject("~/src/Navigation/GlobalNav.ascx", itemChild.Text);
                       buttonChild.CausesValidation = false;
                       
                       buttonChild.Click += new EventHandler(this.ButtonClicked);
                       buttonChild.ToolTip = (String)HttpContext.GetLocalResourceObject("~/src/Navigation/GlobalNav.ascx", itemChild.Tooltip);
                       buttonChild.CommandArgument = itemChild.PageUrl;

                       menuItemDiv = new HtmlGenericControl("div");
                       
                       menuItemDiv.ID = "menuItem" + (String)HttpContext.GetLocalResourceObject("~/src/Navigation/GlobalNav.ascx", itemChild.Text);
                       menuItemDiv.Attributes.Add("class", "MenuItem");

                       Label labelChild = new Label();
                       labelChild.Attributes.Add("class", "MenuItemText");
                       labelChild.ID = "labelChild" + (String)HttpContext.GetLocalResourceObject("~/src/Navigation/GlobalNav.ascx", itemChild.Text);
                       labelChild.Text = (String)HttpContext.GetLocalResourceObject("~/src/Navigation/GlobalNav.ascx", itemChild.Text);

                       menuItemDiv.Controls.Add(labelChild);
                       buttonChild.Controls.Add(menuItemDiv);
                       childItemContainer.Controls.Add(buttonChild);

                       parentItemContainer.Controls.Add(childItemContainer);

                       if (Context.Request.Url.AbsolutePath.Equals(ResolveUrl(itemChild.PageUrl), StringComparison.InvariantCultureIgnoreCase))
                       {
                           buttonChild.Enabled = false;
                           childItemContainer.Attributes.Add("class", "ChildItemContainer Chevron");
                       }
                   }
               }
               #endregion
           }
       }



       // **************************************************************************************************
       // METHOD OVERRIDE: Render
       // Renders the UI of the control 
       protected override void Render(HtmlTextWriter writer)
       {
           // Ensures the control behaves well at design-time 
           // (You don't need this, if the control supports data-binding because this gets 
           // implicitly called in DataBind()) 
        //    EnsureChildControls();

           // Style controls before rendering
        //   PrepareControlForRendering();           
           RenderContents(writer);
        //  base.Render(writer);
       }
            
    

       #endregion


       #region Event-related Members
       // *******************************************************************************************
       // METHOD: ButtonClicked
       // Fires the ItemCommand event to Redirect to the page
       protected void ButtonClicked(object sender,EventArgs e)
       {
           LinkButton button = sender as LinkButton;
           if (button != null)
           {
               DropDownMenuChangedEventArgs args = new DropDownMenuChangedEventArgs(button.CommandArgument);
               OnItemCommand(args);
           }
       }




       // *******************************************************************************************
       // METHOD: OnItemCommand 
       // Fires the ItemCommand event to the host page
       protected virtual void OnItemCommand(DropDownMenuChangedEventArgs e)
       {
           if (ItemCommand != null)
               ItemCommand(this, e);
       }



       #endregion


       

    }


    public class DropDownMenuChangedEventArgs : System.EventArgs
    {
        #region Member Variables

        private string navigateUrl;
        

        #endregion

        #region Constructors

        public DropDownMenuChangedEventArgs()
            : base()
        {

        }

        public DropDownMenuChangedEventArgs(string navigateUrl)
        {
            this.navigateUrl = navigateUrl;
            this.navigateUrl = navigateUrl;
        }

        #endregion

        #region Public Properties

        public string NavigateUrl
        {
            get
            {
                return navigateUrl;
            }

            set
            {

                navigateUrl = value;
            }

        }

        

  

        #endregion
    }
}
