using System;
using System.Collections.Generic;
using System.Web.UI;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Lightboxes
{
    public partial class DeleteLightbox : CorbisBaseUserControl
    {
		private EventHandler deleteLightbox;

		public event EventHandler DeleteLightboxHandler
		{
			add { deleteLightbox += value; }
			remove { deleteLightbox -= value; }
		}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            HookupEventHandlers();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void HookupEventHandlers()
        {
            delete.Click += new EventHandler(Delete_Click);
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
			if (deleteLightbox != null)
			{
				deleteLightbox(sender, e);
			}
        }
    }
}