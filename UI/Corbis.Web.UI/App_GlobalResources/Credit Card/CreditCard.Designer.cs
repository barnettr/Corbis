//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "9.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CreditCard {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CreditCard() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.CreditCard", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cardholder name.
        /// </summary>
        internal static string CardholderCaption {
            get {
                return ResourceManager.GetString("CardholderCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The cardholder name is invalid.
        /// </summary>
        internal static string CardholderErrorMessage {
            get {
                return ResourceManager.GetString("CardholderErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number.
        /// </summary>
        internal static string CardNumberCaption {
            get {
                return ResourceManager.GetString("CardNumberCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The card number entered is invalid.
        /// </summary>
        internal static string CardNumberErrorMessage {
            get {
                return ResourceManager.GetString("CardNumberErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Card type.
        /// </summary>
        internal static string CardTypeCaption {
            get {
                return ResourceManager.GetString("CardTypeCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The card type entered is invalid.
        /// </summary>
        internal static string CardTypeErrorMessage {
            get {
                return ResourceManager.GetString("CardTypeErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Expiration date.
        /// </summary>
        internal static string ExpirationDateCaption {
            get {
                return ResourceManager.GetString("ExpirationDateCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expiration date entered is invalid.
        /// </summary>
        internal static string ExpirationDateErrorMessage {
            get {
                return ResourceManager.GetString("ExpirationDateErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to mm.
        /// </summary>
        internal static string MonthFormat {
            get {
                return ResourceManager.GetString("MonthFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to yyyy.
        /// </summary>
        internal static string YearFormat {
            get {
                return ResourceManager.GetString("YearFormat", resourceCulture);
            }
        }
    }
}
