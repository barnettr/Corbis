using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Corbis.Web.UI.Controls
{
    public class TextArea : TextBox
    {

        protected override void OnPreRender(EventArgs e)
        {
            if (MaxLength > 0 && TextMode == TextBoxMode.MultiLine)
            {
                // Add javascript handlers for paste and keypress
                Attributes.Add("onkeypress","doKeypress(this,event);");
                Attributes.Add("onbeforepaste","doBeforePaste(this,event);");
                Attributes.Add("onpaste","doPaste(this,event);");       // Add attribute for access of maxlength property on client-side
                Attributes.Add("maxLength", this.MaxLength.ToString());           // Register client side include - only once per page
                if(!Page.ClientScript.IsClientScriptIncludeRegistered("TextArea"))
                {
                    Page.ClientScript.RegisterClientScriptInclude("TextArea", ResolveClientUrl("~/Scripts/textArea.js"));
                }
            }
            base.OnPreRender(e);
        }
    }

}

