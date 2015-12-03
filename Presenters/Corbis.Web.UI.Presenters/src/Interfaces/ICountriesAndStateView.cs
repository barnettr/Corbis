using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Entities;

namespace Corbis.Web.UI.Common.ViewInterfaces
{
    public interface ICountriesAndStatesView : IView
    {
        string CountryCode { get; set; }
        List<ContentItem> CountryList { get; set; }
        List<ContentItem> StateList { get; set; }
        string Dashes { get; }
        string SelectOne { get; }
    }
}
