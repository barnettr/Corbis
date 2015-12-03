using System;
using System.Collections.Generic;
using System.Reflection;

namespace Corbis.Web.Utilities
{
    public static class CollectionHelper
    {
        public static List<T> SearchCollectionExactMatch<T>(List<T> source, string propertyName, string propertyValue)
        {
            Assembly assemply = Assembly.GetAssembly(typeof (T));
            Type type = assemply.GetType(typeof (T).ToString(), false, true);
            PropertyInfo properties = type.GetProperty(propertyName);
            List<T> filteredList = new List<T>();

            foreach (T t in source)
            {
                string propertyVal = properties.GetValue(t, null).ToString();
                if (propertyVal.ToLower().Equals(propertyValue.ToLower()))
                {
                    filteredList.Add(t);
                }
            }
            return filteredList;
        }

        public static List<T> SearchCollectionSimilarMatch<T>(List<T> source, string propertyName, string propertyValue)
        {
            Assembly assemply = Assembly.GetAssembly(typeof(T));
            Type type = assemply.GetType(typeof(T).ToString(), false, true);
            PropertyInfo properties = type.GetProperty(propertyName);
            List<T> filteredList = new List<T>();

            foreach (T t in source)
            {
                string propertyVal = properties.GetValue(t, null).ToString();
                if (propertyVal.ToLower().Contains(propertyValue.ToLower()))
                {
                    filteredList.Add(t);
                }
            }
            return filteredList;
        }

        public static List<T> SearchNodeSimilarMatch<T>(T source, string propertyName, string propertyValue)
        {
            Assembly assemply = Assembly.GetAssembly(typeof(T));
            Type type = assemply.GetType(typeof(T).ToString(), false, true);
            PropertyInfo properties = type.GetProperty(propertyName);
            List<T> filteredList = new List<T>();

            string propertyVal = properties.GetValue(source, null).ToString();
            if (propertyVal.ToLower().Contains(propertyValue.ToLower()))
            {
                filteredList.Add(source);
            }
            return filteredList;
        }


    }
}