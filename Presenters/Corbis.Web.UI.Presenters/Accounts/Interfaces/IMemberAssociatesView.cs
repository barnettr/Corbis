using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.Accounts.ViewInterfaces
{
    public interface IMemberAssociatesView : IView
    {
        List<MemberAssociateDisplay> Associates { set; }
        List<String> SelectedAssociateUserNames { get; }
    }
}
