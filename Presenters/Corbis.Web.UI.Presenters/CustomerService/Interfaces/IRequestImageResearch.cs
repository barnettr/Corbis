using System;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.CustomerService.ViewInterfaces
{
    public interface IRequestImageResearch : IView
    {
        string ImageDescription { get; }
        string ProjectDeadline { get; }
        string ModelRelease { get; }
        string ProjectClient { get; }
        string JobNumber { get; }
        string NatureOfBusiness { get; }
        string OtherDescription { get; }
    }
}
