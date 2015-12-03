using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Corbis.Web.UI.Controls
{
	public class ModalPopup : Control, INamingContainer
	{
		private List<Control> popupControls = new List<Control>();
        HtmlGenericControl _messageControl = new HtmlGenericControl("div");

        private bool _hideClose = false;
        public bool HideClose
        {
            get { return _hideClose; }
            set { _hideClose = value; }
        }
		private string _containerID;
		public string ContainerID
		{
			get { return _containerID; }
			set { _containerID = value; }
		}

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        private bool _useDefaultPadding = true;
        public bool UseDefaultPadding
        {
            get { return _useDefaultPadding; }
            set { _useDefaultPadding = value; }
        }

        private string _message;
		public string Message
		{
            get { return _message; }
            set
            {
                _message = value;
            }
		}

		private int _width;
		public int Width
		{
			get { return _width; }
			set { _width = value; }
		}

        private string closeScript = "javascript:HideModal('{0}');return false;";
        public string CloseScript
        {
            get
            {
                return closeScript;
            }
            set
            {
                if (!value.StartsWith("javascript:"))
                {
                    value = "javascript:" + value;
                }
                closeScript = value;
            }
        }

		protected override void AddParsedSubObject(Object obj)
		{
			popupControls.Add((Control)obj);
		}
        protected Control CreateTitleTextControl()
        {
            HtmlGenericControl title = new HtmlGenericControl("span");
            title.Attributes.Add("class", "Title");
            title.Controls.Add(CreateTitleBarCloseControl());
            CustomizeTitleBarControl(title);
            Localize titleText = new Corbis.Web.UI.Controls.Localize();
            titleText.Text = Title;
            title.Controls.Add(titleText);
            return title;
        }
        /**
         * Add Layout manager to add layout based control additions
         **/
        protected Control CreateTitleBarControl()
        {
            HtmlGenericControl titleBar = new HtmlGenericControl("div");
            titleBar.Attributes.Add("class", "ModalTitleBar");
            //Add the title section
            titleBar.Controls.Add(CreateTitleTextControl());
            return titleBar;
        }
        protected Control CreateTitleBarCloseControl()
        {
            //Add the close button icon
            HtmlInputImage closeButton = new HtmlInputImage();
            closeButton.Attributes.Add("class", "Close");
            if (!HideClose)
            {
                closeButton.Attributes.Add("style", "border-width: 0px;");
            }
            else
            {
                closeButton.Attributes.Add("style", "visibility:hidden;" + Width);
            }
            closeButton.Attributes.Add("onclick", String.Format(this.CloseScript, ContainerID));
            closeButton.Src = "~/Images/iconClose.gif";
            return closeButton;
        }
        /**
         * Allow subclasses to customize the title bar control
         **/
        public virtual void CustomizeTitleBarControl(HtmlGenericControl titleBar)
        {

        }
		protected override void CreateChildControls()
		{
			string modalWidth = String.Format("width: {0}px;", (Width > 0 ? Width.ToString() : "300"));

			base.CreateChildControls();
			HtmlGenericControl modal = new HtmlGenericControl("div");
			modal.Attributes.Add("id", ContainerID);
			modal.Attributes.Add("style", "display: none;" + modalWidth);

			HtmlGenericControl modalDialog = new HtmlGenericControl("div");
			modalDialog.Attributes.Add("class", "ModalPopupPanelDialog");
			modalDialog.Attributes.Add("style", modalWidth);

            //Add titlebar control
    		modalDialog.Controls.Add(CreateTitleBarControl());

			HtmlGenericControl modalContent = new HtmlGenericControl("div");
			modalContent.Attributes.Add("class", 
                string.Format("ModalPopupContent{0}", UseDefaultPadding ? String.Empty : " reset_MP")
                );

            modalContent.Controls.Add(_messageControl);

			HtmlGenericControl modalControls = new HtmlGenericControl("div");
			modalControls.Attributes.Add("class", "FormButtons");

			foreach (Control control in popupControls)
			{
				modalControls.Controls.Add((Control)control);
			}

			modalContent.Controls.Add(modalControls);

			modalDialog.Controls.Add(modalContent);


			modal.Controls.Add(modalDialog);

			this.Controls.Add(modal);
		}

        protected override void OnPreRender(EventArgs e)
        {
            _messageControl.InnerHtml = String.IsNullOrEmpty(_message)
                ? String.Empty
                : _message;
            base.OnPreRender(e);
        }
	}
}
