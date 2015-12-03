using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

namespace Corbis.Web.UI.Controls
{
    public class ThemeSelector : CompositeControl
    {
        #region Data Properties
        private string text;
        private List<ThemeItem> themeItems;
        private int themeSelectedIndex;
        private string themeSelectedName;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }

        }

        public List<ThemeItem> ThemeItems
        {
            get
            {
                return themeItems;
            }

            set
            {
                themeItems = value;
            }

        }

        public int ThemeSelectedIndex
        {
            get
            {
                return themeSelectedIndex;
            }

            set
            {
                themeSelectedIndex = value;

            }

        }

        public string ThemeSelectedName
        {
            get
            {
                return themeSelectedName;
            }

            set
            {
                themeSelectedName = value;

            }

        }


        #endregion

        #region Control Property

        private Panel themesMainContainer;


        #endregion


        #region Events

        /// <summary>
        /// Event that occurs when the page index has changed.
        /// </summary>

        public event EventHandler<ThemeChangedEventArgs> ThemeIndexChanged;

        #endregion

        protected override void CreateChildControls()
        {
            HtmlGenericControl topOptionCommand = new HtmlGenericControl();
            topOptionCommand.ID = "topOptionCommand";
            themesMainContainer = new Panel();
            themesMainContainer.ID = "themeMain";

            // Add Label
            HtmlGenericControl divLabel = new HtmlGenericControl("div");
            divLabel.Attributes.Add("class", "Label");
            Label label = new Label();
            label.ID = "colorLabel";
            label.Text = Text;
            divLabel.Controls.Add(label);
            themesMainContainer.Controls.Add(divLabel);
            ThemeItems[ThemeSelectedIndex].ThemeSelected = true;
            foreach (ThemeItem themeItem in ThemeItems)
            {
                HtmlGenericControl divTheme = new HtmlGenericControl("div");
                divTheme.ID = themeItem.ThemeText + "div";
                divTheme.Attributes.Add("class", "Theme");
                ImageButton button = new ImageButton();
                button.ID = themeItem.ThemeText + "button";
                button.CssClass = themeItem.ThemeClass;
                button.ImageUrl = themeItem.ThemeImage;
                // bad, bad.  but if no borderwidth, it is set to zero!
                button.BorderWidth = Unit.Pixel(1);

                button.CausesValidation = false;
                button.ToolTip = themeItem.ThemeText;
                button.CommandName = themeItem.ThemeValue;
                button.CommandArgument = themeItem.ThemeSelectedIndex.ToString();
                button.Click += new ImageClickEventHandler(this.ThemeButtonClicked);

                divTheme.Controls.Add(button);

                if (themeItem.ThemeSelected == true)
                {
                    HtmlGenericControl divThemeArrow = new HtmlGenericControl("div");
                    divThemeArrow.Attributes.Add("class", "Arrow");
                    HtmlImage arrowImg = new HtmlImage();
                    arrowImg.Src = "../../Images/spacer.gif";
                    divThemeArrow.Controls.Add(arrowImg);
                    divTheme.Controls.Add(divThemeArrow);
                }

                themesMainContainer.Controls.Add(divTheme);

            }

            this.Controls.Add(topOptionCommand);
            this.Controls.Add(themesMainContainer);
        }

        /// <summary>
        /// Method which handles the decision of firing the OnThemeIndexChanged event
        /// </summary>
        /// <param name="args"><see cref="PagerIndexChangedEventArgs"/></param>
        protected void OnThemeIndexChanged(ThemeChangedEventArgs args)
        {
            //check if any one has subscribed for this event
            if (ThemeIndexChanged != null)
            {
                ThemeIndexChanged(this, args);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ThemeButtonClicked(object sender, ImageClickEventArgs e)
        {
            ImageButton buttonClicked = (ImageButton)sender;
            this.themeSelectedName = buttonClicked.CommandArgument;
            OnThemeIndexChanged(new ThemeChangedEventArgs(buttonClicked.CommandArgument, buttonClicked.CommandName));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
        }

    }


    public class ThemeChangedEventArgs : System.EventArgs
    {
        #region Member Variables

        private string currentThemeIndex;
        private string currentThemeValue;

        #endregion

        #region Constructors

        public ThemeChangedEventArgs()
            : base()
        {

        }

        public ThemeChangedEventArgs(string currentThemeIndex, string currentThemeValue)
        {
            this.currentThemeIndex = currentThemeIndex;
            this.currentThemeValue = currentThemeValue;
        }

        #endregion

        #region Public Properties

        public string CurrentThemeIndex
        {
            get
            {
                return currentThemeIndex;
            }

            set
            {

                currentThemeIndex = value;
            }

        }

        public string CurrentThemeValue
        {

            get
            {
                return currentThemeValue;
            }
            set
            {
                currentThemeValue = value;
            }



        }

        #endregion
    }


    public class ThemeItem
    {
        #region ThemeItem Instance Variables

        private string themeText;
        private string themeValue;
        private string themeClass;
        private string themeImage;
        private bool themeSelected = false;
        private int themeSelectedIndex;


        #endregion

        public ThemeItem(string themeText, string themeValue, string themeClass, string themeImage, int themeSelectedIndex)
        {

            this.themeText = themeText;
            this.themeValue = themeValue;
            this.themeClass = themeClass;
            this.themeSelectedIndex = themeSelectedIndex;
            this.themeImage = themeImage;

        }


        #region ThemeItem Properties

        public string ThemeText
        {
            get
            {
                return themeText;

            }

            set
            {

                themeText = value;
            }
        }

        public string ThemeValue
        {
            get
            {
                return themeValue;

            }

            set
            {

                themeValue = value;
            }
        }


        public string ThemeClass
        {
            get
            {
                return themeClass;

            }

            set
            {

                themeClass = value;
            }
        }
        public string ThemeImage
        {
            get
            {
                return themeImage;

            }

            set
            {

                themeImage = value;
            }
        }
        public bool ThemeSelected
        {
            get
            {
                return themeSelected;

            }

            set
            {

                themeSelected = value;
            }
        }
        public int ThemeSelectedIndex
        {

            get
            {
                return themeSelectedIndex;
            }

            set
            {
                themeSelectedIndex = value;
            }
        }


        #endregion


    }
    public enum ThemeSelector_Session
    {
        selectedIndex,


    }
}
