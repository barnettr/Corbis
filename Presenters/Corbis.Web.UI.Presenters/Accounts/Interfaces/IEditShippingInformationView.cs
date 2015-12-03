using System;
using System.Collections.Generic;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;

using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.Accounts.ViewInterfaces
{
    public interface IEditShippingInformationView : IView
    {
        #region properties

        /// <summary>
        /// Gets or sets the address uid.
        /// </summary>
        /// <value>The address uid.</value>
        Guid AddressUid { get; set; }
        /// <summary>
        /// Gets the edit mode.
        /// </summary>
        /// <value>The edit mode.</value>
        AccountsEditMode EditMode { get; set; }
        /// <summary>
        /// Gets or sets the shipping address to edit.
        /// </summary>
        /// <value>The shipping address.</value>
        MemberAddress ShippingAddress { get; set; }

        bool ShowUnicodeErrorMessage { set; }

        #endregion

        #region Set View Methods

        /// <summary>
        /// Whether or not to display the delete section on the view
        /// </summary>
        /// <param name="value">
        /// true to display the delete section, false otherwise
        /// </param>
        /// <param name="isDefault">
        /// If true, this is the default shipping address
        /// </param>
        void DisplayDeleteSection(bool value, bool isDefault, Guid addressUid);

        /// <summary>
        /// Whether or not to display the create/edit section on the view
        /// </summary>
        /// <param name="value">true to display the create/edit section, false otherwise</param>
        void DisplayEditSection(bool value, string commandName);

        /// <summary>
        /// Gets the selected country.
        /// </summary>
        /// <value>The country code.</value>
        string CountryCode { get; }

        /// <summary>
        /// Gets or sets the List of Regions to display.
        /// </summary>
        /// <value>The regions.</value>
        List<ContentItem> Regions { get; set; }

        #endregion

    }
}
