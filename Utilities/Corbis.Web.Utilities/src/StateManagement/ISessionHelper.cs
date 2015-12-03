using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Web.Utilities.StateManagement
{
    public interface ISessionHelper
    {
        void ClearSession();
        void DeleteValue(string key);
        T GetValue<T>(string key);
        bool TryGetValue<T>(string key, out T value);
        void UpdateValue<T>(string key, T value);
        void Persist();
    }
}
