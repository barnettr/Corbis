using System;
using System.Collections.Generic;
using Corbis.Web.UI.Presenters.Accounts;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;

using Corbis.Membership.Contracts.V1;

namespace Corbis.Web.UI.Accounts.ViewInterfaces
{
    public interface IEditMailingInformationView : IView
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
        /// Gets or sets the mailing address to edit.
        /// </summary>
        /// <value>The mailing address.</value>
        MemberAddress MailingAddress { get; set; }

        bool ShowUnicodeErrorMessage { set; }

        #endregion

        #region Set View Methods

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
