using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Web;

namespace Corbis.Web.Utilities.StateManagement
{
    //public class UserState
    //{
    //    private const string userCultureName = "UserCultureName";
    //    private const string userCanCheckout = "UserCanCheckout";
    //    private const string userTheme = "UserTheme";

    //    public UserState(HttpContext httpContext) : base(httpContext, false)
    //    {
    //    }

    //    /// <summary>
    //    /// Gets or sets whether the user can checkout
    //    /// </summary>
    //    /// <value>true if the user can checkout, false otherwise</value>
    //    /// <remarks>
    //    /// This property is persisted in the Cookie
    //    /// </remarks>
    //    //[StateItemDesc(ItemName = userCanCheckout, ItemStore = StateItemStore.Cookie)]
    //    [StateItemDesc(userCanCheckout, StateItemStore.Cookie, StatePersistenceFormat.String, StatePersistenceDuration.Session)]
    //    public string CanCheckout
    //    {
    //        get{return GetStateItem(userCanCheckout, StateItemStore.Cookie).Value;}
    //        set{SetStateItem(userCanCheckout, value, StateItemStore.Cookie);}
    //    }

    //    /// <summary>
    //    /// Gets or sets the culture name.
    //    /// </summary>
    //    /// <value>The name of the culture.</value>
    //    /// <remarks>
    //    /// This property is persisted in the session
    //    /// </remarks>
    //    [StateItemDesc(userCultureName, StateItemStore.Session, StatePersistenceFormat.String, StatePersistenceDuration.Session)]
    //    public string CultureName
    //    {
    //        get{return GetStateItem(userCultureName, StateItemStore.Session).Value;}
    //        set { SetStateItem(userCultureName, value, StateItemStore.Session); }
    //    }

    //    /// <summary>
    //    /// Gets or sets the theme.
    //    /// </summary>
    //    /// <value>The theme.</value>
    //    /// <remarks>
    //    /// This property is persisted in both the cookie and session. Gets will look in both places
    //    /// to retreive the value if it can't find it in the first place
    //    /// </remarks>
    //    [StateItemDesc(userTheme, StateItemStore.Session | StateItemStore.Cookie, StatePersistenceFormat.String, StatePersistenceDuration.Session)]
    //    public string Theme
    //    {
    //        get { return GetStateItem(userTheme, StateItemStore.Session | StateItemStore.Cookie).Value; }
    //        set { SetStateItem(userTheme, value, StateItemStore.Session | StateItemStore.Cookie); }
    //    }
    //}
}
