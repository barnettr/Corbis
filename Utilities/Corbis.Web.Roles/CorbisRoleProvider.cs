using System;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Security;
using Corbis.Web.Utilities;

namespace Corbis.Web.Roles
{
    public class CorbisRoleProvider : RoleProvider
    {
        #region ProviderBase Overrides

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            if (config == null)
            {
                throw new ConfigurationErrorsException("CorbisRoleProvider: Initialize() - RoleManager configuration doesn't exist.");
            }

            ApplicationName = config["applicationName"];
            if (StringHelper.IsNullOrTrimEmpty(ApplicationName))
            {
                ApplicationName = "Corbis.Web";
            }

            base.Initialize(name, config);

            if (config.Count > 0)
            {
                if (!StringHelper.IsNullOrTrimEmpty(config.GetKey(0)))
                {
                    throw new ProviderException(string.Format("CorbisRoleProvider: Initialize() - Unrecognized attribute '{0}.'", config.GetKey(0)));
                }
            }
        }

        #endregion

        #region RoleProvider Overrides

        private string applicationName;
        public override string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("CorbisRoleProvider: AddUsersToRoles - The method or operation is not implemented.");
        }

        public override void CreateRole(string roleName)
        {
            throw new Exception("CorbisRoleProvider: CreateRole - The method or operation is not implemented.");
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new Exception("CorbisRoleProvider: DeleteRole - The method or operation is not implemented.");
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new Exception("CorbisRoleProvider: FindUsersInRole - The method or operation is not implemented.");
        }

        public override string[] GetAllRoles()
        {
            throw new Exception("CorbisRoleProvider: GetAllRoles - The method or operation is not implemented.");
        }

        public override string[] GetRolesForUser(string username)
        {
            throw new Exception("CorbisRoleProvider: GetRolesForUser - The method or operation is not implemented.");
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new Exception("CorbisRoleProvider: GetUsersInRole - The method or operation is not implemented.");
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new Exception("CorbisRoleProvider: IsUserInRole - The method or operation is not implemented.");
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new Exception("CorbisRoleProvider: RemoveUsersFromRoles - The method or operation is not implemented.");
        }

        public override bool RoleExists(string roleName)
        {
            throw new Exception("CorbisRoleProvider: RoleExists - The method or operation is not implemented.");
        }

        #endregion
    }
}
