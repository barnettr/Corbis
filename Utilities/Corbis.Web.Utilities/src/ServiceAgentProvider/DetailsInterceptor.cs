using System;
using System.Text;
using Castle.Core.Interceptor;
using Corbis.Framework.Logging;

namespace Corbis.Web.Utilities.Providers
{
    public class DetailsInterceptor : StandardInterceptor
    {

        ILogging log;

        public DetailsInterceptor(ILogging logger)
        {
            log = logger;
        }

        protected override void PreProceed(IInvocation invocation)
        {



            base.PreProceed(invocation);


        }

        protected override void PostProceed(IInvocation invocation)
        {
            if (log != null)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Method Signature:  " + invocation.Method.Name + " (");
                if (invocation.Arguments.Length > 0)
                {
                    for (int i = 0; i < invocation.Arguments.Length; i++)
                    {

                        builder.Append(invocation.Arguments[i].ToString() + ", ");
                    }
                    builder.Remove(builder.Length - 2, 2);
                }
                if (invocation.ReturnValue != null)
                {
                    builder.AppendLine(") ");
                    builder.AppendLine("Return Value = " + invocation.ReturnValue.ToString());
                }
                else
                {
                    builder.AppendLine(")");
                }



                log.LogInformationMessage("Service Agent Operation Completed", builder.ToString());

            }
            base.PostProceed(invocation);
        }

    }
}
