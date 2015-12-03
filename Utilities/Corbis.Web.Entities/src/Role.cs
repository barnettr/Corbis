using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    public static class Role
    {
        public enum Permissions
        {
            Anonymous, 
            Authenticated, 
            KnownNotAuthenticated,
            NotKnown
        };

        public enum Roles
        {
            TODO
        };
    }
}
