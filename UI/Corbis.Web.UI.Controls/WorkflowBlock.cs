using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Corbis.Web.Utilities;

namespace Corbis.Web.UI.Controls
{
	public class WorkflowBlock : Control, INamingContainer
	{
        private string _workflowType;
        public string WorkflowType
        {
            get
            {
                return _workflowType;
            }
            set
            {
                this._workflowType = value;
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (WorkflowHelper.IsWorkflowOfType(WorkflowType))
            {
                base.Render(writer);
            }
        }
        protected override void CreateChildControls()
        {
            if(WorkflowHelper.IsWorkflowOfType(WorkflowType))
                base.CreateChildControls();
        }
	}
}
