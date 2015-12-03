using System;
using System.Collections.Generic;
using System.Resources;
using System.Web;
using Resources;
using Corbis.Framework.Globalization;

namespace Corbis.Web.UI
{
    public partial class CorbisBasePage
    {

        /// <summary>
        /// Returns the localized text for the specified enum value using the
        /// Enumerations resource manager.
        /// </summary>
        /// <typeparam name="T">Enum type to localize</typeparam>
        /// <param name="value">value to localize</param>
        /// <returns>Localized text</returns>
        public static string GetEnumDisplayText<T>(T value)
        {
            return GetEnumDisplayText(value, false);
        }

        /// <summary>
        /// Returns the localized text for the specified enum value
        /// </summary>
        /// <typeparam name="T">Enum type to localize</typeparam>
        /// <param name="value">value to localize</param>
        /// <param name="useLocalResource">
        /// if set to <c>true</c> uses the local resource file for the page or control
        /// otherwise uses the Enumerations resource manager.
        /// </param>
        /// <returns>Localized text</returns>
        public static string GetEnumDisplayText<T>(T value, bool useLocalResource)
        {
            string displayText = String.Empty;
            if (useLocalResource)
            {
                string resourceKey = value.GetType().Name + "_" + value.ToString();
                displayText = HttpContext.GetLocalResourceObject(
                    HttpContext.Current.Request.Path, 
                    resourceKey,
                    Language.CurrentCulture) as string;
            }
            else
            {
                displayText = GetEnumDisplayText<T>(value, Enumerations.ResourceManager);
            }
            return displayText;
        }

        /// <summary>
        /// Returns the localized text for the specified enum value using the specified ResourceManager
        /// </summary>
        /// <typeparam name="T">Enum type to localize</typeparam>
        /// <param name="value">value to localize</param>
        /// <param name="resourceManager">
        /// The specific <see cref="ResourceManager"/> to use
        /// </param>
        /// <returns>Localized text</returns>
        public static string GetEnumDisplayText<T>(T value, ResourceManager resourceManager)
        {
            string displayText = String.Empty;
            string resourceKey = value.GetType().Name + "_" + value.ToString();
            displayText = resourceManager.GetString(resourceKey, Language.CurrentCulture);
            return displayText;
        }


        /// <summary>
        /// Returns the Keyed localized text for the specified enum value and key from the 
        /// Enumerations resource manager
        /// </summary>
        /// <typeparam name="T">Enum type to localize</typeparam>
        /// <param name="value">value to localize</param>
        /// <param name="key">Key name to look up</param>
        /// <returns>Localized text</returns>
        public static string GetKeyedEnumDisplayText<T>(T value, string key)
        {
            return GetKeyedEnumDisplayText(value, key, false);
        }

        /// <summary>
        /// Returns the Keyed localized text for the specified enum value and key from the
        /// provided resource manager
        /// </summary>
        /// <typeparam name="T">Enum type to localize</typeparam>
        /// <param name="value">value to localize</param>
        /// <param name="key">Key name to look up</param>
        /// <param name="useLocalResource">
        /// if set to <c>true</c> uses the local resource file for the page or control
        /// otherwise uses the Enumerations resource manager.
        /// </param>
        /// <returns>Localized text</returns>
        public static string GetKeyedEnumDisplayText<T>(T value, string key, bool useLocalResource)
        {
            string displayText = String.Empty;
            if (useLocalResource)
            {
                string resourceKey = value.GetType().Name + "_" + value.ToString() + "_" + key;
                displayText = HttpContext.GetLocalResourceObject(
                    HttpContext.Current.Request.Path,
                    resourceKey,
                    Language.CurrentCulture) as string;
            }
            else
            {
                displayText = GetKeyedEnumDisplayText<T>(value, key, Enumerations.ResourceManager);
            }
            return displayText;
        }

        /// <summary>
        /// Returns the Keyed localized text for the specified enum value and key from the
        /// provided resource manager
        /// </summary>
        /// <typeparam name="T">Enum type to localize</typeparam>
        /// <param name="value">value to localize</param>
        /// <param name="key">Key name to look up</param>
        /// <param name="resourceManager">
        /// The specific <see cref="ResourceManager"/> to use
        /// </param>
        /// <returns>Localized text</returns>
        public static string GetKeyedEnumDisplayText<T>(T value, string key, ResourceManager resourceManager)
        {
            string displayText = String.Empty;
            string resourceKey = value.GetType().Name + "_" + value.ToString() + "_" + key;
            displayText = resourceManager.GetString(resourceKey, Language.CurrentCulture);
            return displayText;
        }


        /// <summary>
        /// Returns the ordinal (sort order) for the specified enum value. Uses the
        /// Enumerations resource manager.
        /// </summary>
        /// <typeparam name="T">Enum type to localize</typeparam>
        /// <param name="value">value to localize</param>
        /// <param name="useLocalResource">
        /// if set to <c>true</c> uses the local resource file for the page or control
        /// otherwise uses the Enumerations resource manager.
        /// </param>
        /// <returns></returns>
        private static int? GetEnumDisplayOrder<T>(T value, bool useLocalResource)
        {
            int? ordinal = 0;
            string ordinalString = GetKeyedEnumDisplayText<T>(value, "Ordinal", useLocalResource);
            
            // If there isn't an ordinal, leave it as 0,
            // If there's a non numeric ordinal, set it to null
            // These values shouldn't display
            if (!string.IsNullOrEmpty(ordinalString))
            {
                int tmpOrdinal;
                if (int.TryParse(ordinalString, out tmpOrdinal))
                {
                    ordinal = tmpOrdinal;
                }
                else
                {
                    ordinal = null;
                }
            }
            return ordinal;
        }

        /// <summary>
        /// Returns the ordinal (sort order) for the specified enum value. Uses the
        /// Enumerations resource manager.
        /// </summary>
        /// <typeparam name="T">Enum type to localize</typeparam>
        /// <param name="value">value to localize</param>
        /// <param name="resourceManager">
        /// The specific <see cref="ResourceManager"/> to use
        /// </param>
        /// <returns></returns>
        private static int? GetEnumDisplayOrder<T>(T value, ResourceManager resourceManager)
        {
            int? ordinal = 0;
            string ordinalString = GetKeyedEnumDisplayText<T>(value, "Ordinal", resourceManager);

            // If there isn't an ordinal, leave it as 0,
            // If there's a non numeric ordinal, set it to null
            // These values shouldn't display
            if (!string.IsNullOrEmpty(ordinalString))
            {
                int tmpOrdinal;
                if (int.TryParse(ordinalString, out tmpOrdinal))
                {
                    ordinal = tmpOrdinal;
                }
                else
                {
                    ordinal = null;
                }
            }
            return ordinal;
        }

        public static Dictionary<T, string> GetEnumDisplayTexts<T>()
        {
            Dictionary<T, string> enumValues = new Dictionary<T, string>();

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                enumValues.Add(value, GetEnumDisplayText<T>(value));
            }

            return enumValues;
        }

        public static Dictionary<T, string> GetKeyedEnumDisplayTexts<T>(string key)
        {
            Dictionary<T, string> enumValues = new Dictionary<T, string>();

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                enumValues.Add(value, GetKeyedEnumDisplayText<T>(value, key));
            }

            return enumValues;
        }

        public static DisplayValue<T> GetEnumDisplayValue<T>(T value)
        {
            return GetEnumDisplayValue<T>(value, false);
        }

        public static DisplayValue<T> GetEnumDisplayValue<T>(T value, bool useLocalResource)
        {
            // Read the localized value from resources
            DisplayValue<T> locValue = new DisplayValue<T>(value, GetEnumDisplayText<T>(value, useLocalResource), value.GetHashCode(), GetEnumDisplayOrder<T>(value, useLocalResource));
            return locValue;
        }

        public static DisplayValue<T> GetEnumDisplayValue<T>(T value, ResourceManager resourceManager)
        {
            // Read the localized value from resources
            DisplayValue<T> locValue = new DisplayValue<T>(
                value, 
                GetEnumDisplayText<T>(value, resourceManager), 
                value.GetHashCode(), 
                GetEnumDisplayOrder<T>(value, resourceManager));
            return locValue;
        }


        /// <summary>
        /// Retrieves localized text values for the enum specified by T from the global
        /// Enumerations resource file.
        /// </summary>
        /// <typeparam name="T">Enum Type to localize</typeparam>
        /// <returns>List of DisplayValues for the enumeration in the current culture</returns>
        public static List<DisplayValue<T>> GetEnumDisplayValues<T>()
        {
            return GetEnumDisplayValues<T>(true, false);
        }

        /// <summary>
        /// Retrieves localized text values for the enum specified by T from the global
        /// Enumerations resource file.
        /// </summary>
        /// <typeparam name="T">Enum Type to localize</typeparam>
        /// <param name="keepEmptyValues">True to keep empty (no resource entry) values; False to remove them</param>
        /// <returns>List of DisplayValues for the enumeration in the current culture</returns>
        public static List<DisplayValue<T>> GetEnumDisplayValues<T>(bool keepEmptyValues)
        {
            return GetEnumDisplayValues<T>(keepEmptyValues, false);
        }

        /// <summary>
        /// Retrieves localized text values for the enum specified by T from the provided
        /// resource mangaer.
        /// </summary>
        /// <typeparam name="T">Enum Type to localize</typeparam>
        /// <param name="keepEmptyValues">True to keep empty (no resource entry) values; False to remove them</param>
        /// <param name="useLocalResource">
        /// if set to <c>true</c> uses the local resource file for the page or control
        /// otherwise uses the Enumerations resource manager.
        /// </param>
        /// <returns>
        /// List of DisplayValues for the enumeration in the current culture
        /// </returns>
        public static List<DisplayValue<T>> GetEnumDisplayValues<T>(bool keepEmptyValues, bool useLocalResource)
        {
            List<DisplayValue<T>> list = new List<DisplayValue<T>>();

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                // Read the localized value from resources
                DisplayValue<T> locValue = GetEnumDisplayValue<T>(value, useLocalResource);

                // Only add empty values to list if keepEmptyValues is true
                if (keepEmptyValues || !string.IsNullOrEmpty(locValue.Text))
                {
                    list.Add(locValue);
                }
            }

            return SortAndRemoveNullOrdinals<T>(list);
        }

        /// <summary>
        /// Retrieves localized text values for the enum specified by T from the provided
        /// resource mangaer.
        /// </summary>
        /// <typeparam name="T">Enum Type to localize</typeparam>
        /// <param name="keepEmptyValues">True to keep empty (no resource entry) values; False to remove them</param>
        /// <param name="resourceManager">
        /// The specific <see cref="ResourceManager"/> to use
        /// </param>
        /// <returns>
        /// List of DisplayValues for the enumeration in the current culture
        /// </returns>
        public static List<DisplayValue<T>> GetEnumDisplayValues<T>(bool keepEmptyValues, ResourceManager resourceManager)
        {
            List<DisplayValue<T>> list = new List<DisplayValue<T>>();

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                // Read the localized value from resources
                DisplayValue<T> locValue = GetEnumDisplayValue<T>(value, resourceManager);

                // Only add empty values to list if keepEmptyValues is true
                if (keepEmptyValues || !string.IsNullOrEmpty(locValue.Text))
                {
                    list.Add(locValue);
                }
            }
            return SortAndRemoveNullOrdinals<T>(list);
        }

        public static List<DisplayValue<T>> SortAndRemoveNullOrdinals<T>(List<DisplayValue<T>> list)
        {
            // Remove any DisplayValues with a null ordinal, these aren't supposed to be displayed, then sort 'em
            list.RemoveAll(delegate(DisplayValue<T> value)
            {
                return !value.Ordinal.HasValue;
            });
            list.Sort(DisplayValue<T>.Compare);
            return list;
        }


    }
}
