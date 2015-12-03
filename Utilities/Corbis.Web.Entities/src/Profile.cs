using System;
using System.Web;
using System.Web.Profile;
using Globalization = Corbis.Framework.Globalization;
using Corbis.Membership.Contracts.V1;
using Corbis.Web.Profile;

namespace Corbis.Web.Entities
{
    public class ProfileCommon : ProfileBase
    {
        CorbisProfileProvider provider;

        // TODO: remove
        public virtual string BPNumber
        {
            get { return "14186844"; }
        }

        public virtual string FullName
        {
            get
            {
                if (Member != null)
                {
                    if (Member.LanguageCode == Globalization.Language.Japanese.CultureInfo.Name)
                    {
                        return Member.FuriganaFirstName + " " + Member.FuriganaLastName;
                    }
                    return Member.FirstName + " " + Member.LastName;
                }
                return string.Empty;
            }
        }

        public virtual Member Member
        {
            get { return (Member)this.GetPropertyValue("Member"); }
            set { this.SetPropertyValue("Member", value); }
        }

        protected void Profile()
        {
            provider = new CorbisProfileProvider();
        }

        public virtual ProfileCommon GetProfile(string username)
        {
            return (ProfileCommon)ProfileBase.Create(username);
        }

        public override void Save()
        {
            provider.SaveProfile();
        }
    }
}
