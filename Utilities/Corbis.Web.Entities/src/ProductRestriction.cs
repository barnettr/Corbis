using System;
using System.Collections.Generic;
using Corbis.Image.Contracts.V1;
using System.Globalization;

using System.Text;

namespace Corbis.Web.Entities
{
    public class ProductRestriction
    {

        #region Private Instance variables

        private string modelReleaseStatus;
        private bool propertyReleaseStatus;
        private string propertyReleaseStatusText;
        private List<Restriction> restrications;
        private string domesticEmbargoDate;
        private string internationalEmbargoDate;
        private decimal aspectRatio;
        private string url128;

       

        #endregion



        public string ModelReleaseStatus
        {
            get
            {
                return modelReleaseStatus;
            }
            set
            {
                modelReleaseStatus = value;
            }
        }

        public bool PropertyReleaseStatus
        {
            get
            {
                
                return propertyReleaseStatus;

            }
            set
            {               
                
                
                propertyReleaseStatus = value;
            }
        }

        public string PropertyReleaseStatusText
        {
            get
            {
                return propertyReleaseStatusText;

            }
            set
            {
                propertyReleaseStatusText = value;
            }
        }      

        public List<Restriction> Restrications
        {
            get
            {
                return restrications;
            }
            set
            {
                restrications = value;
            }
        }
        public string DomesticEmbargoDate
        {
            get
            {
                return domesticEmbargoDate;
            }
            set
            {
                domesticEmbargoDate = value;
            }
        }
        public string InternationalEmbargoDate
        {
            get
            {
                return internationalEmbargoDate;
            }
            set
            {
                internationalEmbargoDate = value;
            }
        }
        public decimal AspectRatio
        {
            get
            {
                return aspectRatio;
            }
            set
            {
                aspectRatio = value;
            }

        }
        public string Url128
        {
            get
            {
                return url128;
            }
            set
            {
                url128 = value;
            }
        }



    }
}
