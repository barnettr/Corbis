using System;
using System.Text;

namespace Corbis.Web.Utilities
{
    public static class GuidHelper
    {
        public static bool TryParse(string s, out Guid result)
        {
            try
            {
                result = new Guid(s);
                return true;
            }
            catch
            {
                result = Guid.Empty;
                return false;
            }
        }
    }
}
