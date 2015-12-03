using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace Corbis.Web.Utilities
{
    public static class WorkflowHelper
    {
        public static string COFF_FLOW = "COFF";
        public static string CART_FLOW = "CART";
        public static bool IsCOFFWorkflow(HttpRequest request)
        {
            return (request.Params["OrderType"] != null && request.Params["OrderType"].Equals(COFF_FLOW));
        }
        public static bool IsCartWorkflow(HttpRequest request)
        {
            return (request.Params["OrderType"] == null || request.Params["OrderType"].Equals(CART_FLOW));
        }
        public static bool IsWorkflowOfType(string workflowType)
        {
            HttpRequest httpRequest = HttpContext.Current.Request;

            if (workflowType != null && workflowType.Equals(CART_FLOW) && httpRequest.Params["OrderType"] == null)
            {
                return true;
            }
            return (httpRequest.Params["OrderType"] != null && httpRequest.Params["OrderType"].Equals(workflowType));
        }

    }
}