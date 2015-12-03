using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{  [Serializable]
    public class QuickPicItem
    {
        private string corbisID;
        private string url128;
        private string licenceModel;
        private decimal _aspectRatio;
        private string title;


        public QuickPicItem(string corbisID, string url128, string licenceModel,string aspectRatio,string title)
        {
            this.corbisID=corbisID;
            this.url128=url128;
            this.licenceModel=licenceModel;
            //this.aspectRatio = decimal.Parse(aspectRatio);
            decimal.TryParse(aspectRatio, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out _aspectRatio);
            this.title = title;
        }


        public string CorbisID
        {

            get
            {

                return corbisID;
            }

            set
            {

                corbisID = value;
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

        public string LicenseModel
        {

            get
            {

                return licenceModel;
            }

            set
            {
                licenceModel = value;
            }


        }

        public decimal AspectRatio
        {

            get
            {

                return _aspectRatio;
            }

            set
            {
                _aspectRatio = value;
            }


        }

        public string Title
        {

            get
            {

                return title;
            }

            set
            {
                title = value;
            }


        }







    }
}
