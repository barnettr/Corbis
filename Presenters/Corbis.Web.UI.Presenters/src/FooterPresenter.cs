using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Corbis.Framework.Globalization;
using Corbis.Web.UI.ViewInterfaces;
using Corbis.Web.Authentication;

namespace Corbis.Web.UI.Presenters
{
   public class FooterPresenter:BasePresenter
   {
       #region Member variables
       private IFooterView footerView;
       #endregion

       #region Constructors
       /// <summary>
       /// Initializes a new instance of the <see cref="FooterPresenter"/> class.
       /// </summary>


       //public FooterPresenter(IView view)
       //{
       //    if (view == null)
       //    {
       //        throw new ArgumentNullException("FooterPresenter: FooterPresenter() - Footer view cannot be null.");
       //    } 
       //    //this.view = view;
       //    //FooterView = view as IFooterView;
       //   // FooterView = this.view;
       //}
       #endregion

       #region properties

       public IFooterView FooterView
       {
           get
           {
               if (footerView == null)
               {
                   throw new ArgumentNullException("FooterPresenter: FooterPresenter() - Footer view cannot be null.");

               }
               else
               {
                   return footerView;
               }
           }
           set { this.footerView = value; }
       }

       #endregion

       #region Public Methods      

       public void SetFooterVisibility()
       {
           try
           {
               if ((Profile.IsAnonymous == true))
               {
                   if (Profile.CountryCode.Equals("DE") || Language.CurrentLanguage.LanguageCode.Equals("de-DE"))
                   {
                       footerView.ImprintVisibility = true;
                   }
                   else
                   {
                       footerView.ImprintVisibility = false;
                   }
               }
               else if (Profile.IsAuthenticated == true || ((Profile.IsAnonymous == false) && (Profile.IsAuthenticated == false)))//paritially Authenticated
               {
                   if (Profile.CountryCode.Equals("DE") || Language.CurrentLanguage.LanguageCode.Equals("de-DE"))
                   {
                       footerView.ImprintVisibility = true;
                   }
                   else
                   {
                       footerView.ImprintVisibility = false;
                   }
               }
               else
               {
                   footerView.ImprintVisibility = false;
               }
           } catch(Exception)
           {}
       }
       
       #endregion
   }

}
