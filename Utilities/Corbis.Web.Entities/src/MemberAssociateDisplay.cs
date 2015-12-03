using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.Entities
{
    public class MemberAssociateDisplay
    {
        public string Username { get; set; }
        public string AssociateDisplay { get; set; }
        public MemberAssociateStatus Status { get; set; }
        public bool AddErrorOcurred { get; set; }
        public string ErrorMessage { get; set; }
    }
}
