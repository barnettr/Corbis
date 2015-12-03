using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Corbis.Common.FaultContracts;
using Corbis.Common.ServiceFactory;
using Castle.Core.Interceptor;

namespace Corbis.Web.Utilities
{
    public static class ServiceHelper
    {
        public static CorbisFault HandleServiceException(Exception exception)
        {

            FaultException<CorbisFault> faultException = exception as FaultException<CorbisFault>;

            if (faultException != null)
            {
                return faultException.Detail;
            }

            return null;
        }

        public static T GetServiceAgentProxy<T>() where T : new()
        {
            Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
            IInterceptor[] interceptors = new IInterceptor[]{new ServiceAgentInterceptor()};
            T serviceProxy = generator.CreateClassProxy<T>(interceptors);
            return serviceProxy;
        }

        public static T GetServiceAgentProxy1<T>() where T : new()
        {
            T result;
            Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
            T actualObject = new T();
            result = generator.CreateInterfaceProxyWithTarget<T>(actualObject,
                                    new IInterceptor[] { new ServiceAgentInterceptor() });
            
            return result;
        }
    }    

    public class ServiceAgentInterceptor : StandardInterceptor
    {
        protected override void PreProceed(Castle.Core.Interceptor.IInvocation invocation)
        {
            
            //log the service invocation data
            System.Diagnostics.Trace.WriteLine(Environment.NewLine + Environment.NewLine + "Calling : " + invocation.Method.Name); //etc
            //System.Diagnostics.Trace.TraceInformation(String.Join("|", invocation.Arguments)); //etc

            base.PreProceed(invocation);


        }

        protected override void PostProceed(Castle.Core.Interceptor.IInvocation invocation)
        {
            //log the service invocation data
            System.Diagnostics.Trace.WriteLine(Environment.NewLine + Environment.NewLine + "Returning : " + invocation.Method.Name + ", Return Value : "); //etc
            System.Diagnostics.Trace.Write((invocation.ReturnValue == null) ? "NO RETURN VALUE" : invocation.ReturnValue.ToString()); //etc

            base.PostProceed(invocation);
        }
       
    }
}
