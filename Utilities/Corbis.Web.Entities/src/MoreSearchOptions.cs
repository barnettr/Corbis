using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Corbis.Web.Entities
{
    [Serializable]
    public class MoreSearchOptions
    {
        public string DateCreated { get; set; }
        public string Days { get; set; }
        public bool DaysChecked { get; set; }
        public bool DateRangeChecked { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string Location { get; set; }
        public string Photographer { get; set; }
        public string Provider { get; set; }
        public bool HorizontalCheckbox { get; set; }
        public bool PanoramaCheckbox { get; set; }
        public bool VerticalCheckbox { get; set; }
        public string PointOfView { get; set; }
        public string NumberOfPeople { get; set; }
        public string ImmediateAvailability { get; set; }
        public string ImageNumbers { get; set; }
        public List<string> SelectedMarketingCollection { get; set; }
    }
}
