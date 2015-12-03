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
using Corbis.Web.UI.Presenters;
using AjaxControlToolkit;
using Corbis.Web.Entities;
using System.Web;

namespace Corbis.Web.UI.Controls
{
  public class LanguageSelectorMenu:CompositeControl
  {

      #region Controls
          private HtmlGenericControl mainDiv;
         
          
          
      #endregion

      #region Property

         private string selectedCultureValue;
         private List<ContentItem> languageList;

       #endregion


      #region Events
          // ******************************************************************************
          // Public events
          public event EventHandler<LanguageMenuChangedEventArgs> ItemCommand;
          #endregion

     #region Property methods

          [Description("Gets the collection of the menu items")]
          public List<ContentItem> LanguageList
          {
              get
              {
                  if (languageList == null)
                      languageList = new List<ContentItem>();
                  return languageList;
              }

              set
              {

                  this.languageList = value;
              }
          }


          [Description("Selected Language  Culture Text")]
          public string SelectedCultureName
          {

              get
              {
                  if (SelectedCultureValue != null)
                  {

                      foreach (ContentItem item in languageList)
                      {
                          if (SelectedCultureValue == item.Key)
                          {

                              return item.ContentValue;
                          }

                      }
                  }
                      return "English (US)";
               
              }
              set
              {
                  ViewState["SelectedCultureName"] = value;
              }

          }

          [Description("Selected Langualge Culture Value")]
          public string SelectedCultureValue
          {

              get
              {
                  if (string.IsNullOrEmpty(selectedCultureValue))
                      return "en-US";
                  return selectedCultureValue;
              }
              set
              {
                  selectedCultureValue = value;
              }

          }



          #endregion



       protected override void CreateChildControls()
       {
           Controls.Clear();

          #region Main Div
          mainDiv = new HtmlGenericControl("div");
          mainDiv.Attributes.Add("class", "LanguageMenuBottom");
          #endregion

      

          #region Main Selector Div

         
          foreach (ContentItem item in LanguageList)
          {

              HtmlGenericControl menuDiv = new HtmlGenericControl("div");

              // Add the link for post back
              LinkButton button = new LinkButton();
              button.CausesValidation = false;
              button.ID = "button" + item.Key;
              button.Click += new EventHandler(this.ButtonClicked);
              button.CommandName = item.ContentValue;
              button.CommandArgument = item.Key;

              Label label = new Label();
              label.ID = "label" + item.Key;
              label.Text = item.ContentValue;

              HtmlGenericControl menuItemDiv = new HtmlGenericControl("div");
              if (item.Key != SelectedCultureValue)
              {
                  menuItemDiv.Attributes.Add("class", "MenuItem");
              }
              else
              {
                  menuItemDiv.Attributes.Add("class", "MenuItem Selected");

              }
              menuItemDiv.ID = "menuItem" + item.Key;
              
              menuDiv.Attributes.Add("class", "MenuDiv");

              menuItemDiv.Controls.Add(label);
              button.Controls.Add(menuItemDiv);
              menuDiv.Controls.Add(button);
              mainDiv.Controls.Add(menuDiv);

          }
           // The number 24 is the height of each menu item.
           // TODO: is there a way to get this size without entering a number?
           mainDiv.Attributes["height"] = (languageList.Count*24).ToString();

          #endregion

        

          this.Controls.Add(mainDiv);
         
      }


      #region Render

      // **************************************************************************************************
      // METHOD OVERRIDE: Render
      // Renders the UI of the control 
      protected override void Render(HtmlTextWriter writer)
      {
                 
        RenderContents(writer);
          // base.Render(writer);
      }

      #endregion


      #region Event-related Members
      // *******************************************************************************************
          // METHOD: ButtonClicked
          // Fires the ItemCommand event to Redirect to the page
          protected void ButtonClicked(object sender, EventArgs e)
          {
              LinkButton button = sender as LinkButton;
              if (button != null)
              {
                  LanguageMenuChangedEventArgs args = new LanguageMenuChangedEventArgs(button.CommandArgument,button.CommandName);
                  OnItemCommand(args);
              }
          }




          // *******************************************************************************************
          // METHOD: OnItemCommand 
          // Fires the ItemCommand event to the host page
         protected virtual void OnItemCommand(LanguageMenuChangedEventArgs e)
          {
              if (ItemCommand != null)
                  ItemCommand(this, e);
          }



          #endregion


      }


    public class LanguageMenuChangedEventArgs : System.EventArgs
    {
        #region Member Variables
      
        private string languageCultureValue;
        private string languageCultureText;


        #endregion

        #region Constructors

        public LanguageMenuChangedEventArgs()
            : base()
        {

        }

        public LanguageMenuChangedEventArgs(string languageCultureValue,string languageCultureText)
        {
            this.languageCultureValue = languageCultureValue;
            this.languageCultureText = languageCultureText;
        }

        #endregion

        #region Public Properties

        public string LanguageCultureValue
        {
            get
            {
                return languageCultureValue;
            }

            set
            {

                languageCultureValue = value;
            }

        }

        public string LanguageCultureText 
        {
            get
            {
                return languageCultureText;
            }

            set
            {

                languageCultureText = value;
            }

        }
        



        #endregion
    }
}
