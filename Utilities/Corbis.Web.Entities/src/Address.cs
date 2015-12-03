using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
   public class Address
    {

       private string address1;
       private string address2;
       private string address3;
       private string addressType;
       private Guid addressUid;
       private string city;
       private string companyName;
       private string countryCode;
       private string name;
       private bool isdefaultForType;
       private string postalCode;
       private string regionCode;



       public string Address1
       {
           get
           {
               return address1;
           }
           set
           {
               address1 = value;
           }
       }

       public string Address2
       {
           get
           {
               return address2;
           }
           set
           {
               address2 = value;
           }
       }
       public string Address3
       {
           get
           {
               return address3;
           }
           set
           {
               address3 = value;
           }
       }
       public Guid AddressUid
       {
           get
           {
               return addressUid;
           }
           set
           {
               addressUid = value;
           }
       }
       public string AddressType
       {
           get
           {
               return addressType;
           }
           set
           {
               addressType = value;
           }
       }
       public string City
       {
           get
           {
               return city;
           }
           set
           {
               city = value;
           }
       }
       public string CompanyName
       {
           get
           {
               return companyName;
           }
           set
           {
               companyName = value;
           }
       }
       public string CountryCode
       {
           get
           {
               return countryCode;
           }
           set
           {
               countryCode = value;
           }
       }
       public string Name
       {
           get
           {
               return name;
           }
           set
           {
               name = value;
           }
       }
       public bool IsDefaultForType
       {
           get
           {
               return isdefaultForType;
           }
           set
           {
               isdefaultForType = value;
           }
       }
       public string PostalCode
       {
           get
           {
               return postalCode;
           }
           set
           {
               postalCode = value;
           }
       }
       public string RegionCode
       {
           get
           {
               return regionCode;
           }
           set
           {
               regionCode = value;
           }
       }      

    }
}
