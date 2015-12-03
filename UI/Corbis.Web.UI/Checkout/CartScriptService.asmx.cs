using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Script.Services;
using Corbis.Framework.Logging;
using Corbis.Web.UI.Presenters.Checkout;
using Corbis.Web.UI.Cart.ViewInterfaces;
using Corbis.LightboxCart.Contracts.V1;
using Corbis.Search.Contracts.V1;
using Corbis.Web.Authentication;
using Corbis.Web.Entities;
using System.Threading;
using System.Globalization;
using Corbis.Web.UI.src;

namespace Corbis.Web.UI.Checkout
{
    /// <summary>
    /// Summary description for CartScriptService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
    public class CartScriptService : CorbisWebService, ICartScriptServiceView
    {

        private ILogging loggingContext;
        private CheckoutPresenter presenter;
        private List<WebProductValidToCheckoutStatus> webProductsValidToCheckoutStatus;

        public CartScriptService()
        {
            presenter = new CheckoutPresenter(this);
            webProductsValidToCheckoutStatus = new List<WebProductValidToCheckoutStatus>();
        }

        [WebMethod(true)]
        public List<DragResult> ValidateItems(List<ValidateItemsItem> productUids)
        {
            Dictionary<Guid, string> dictionary = new Dictionary<Guid,string>();

            foreach (ValidateItemsItem item in productUids)
            {
                dictionary.Add(item.Guid, item.CorbisId);
            }

            List<DragResult> validationResult = presenter.ValidateItems(dictionary);
            return validationResult;
        }

        [WebMethod(true)]
        public void UpdateCartItemPrice(Guid productUid)
        {
            List<Guid> productUidList = new List<Guid>();
            productUidList.Add(productUid);

            presenter.UpdateCartItemPrice(productUid);
            presenter.AddItemsToCartContainer(productUidList, CartContainerEnum.Checkout);
        }

        #region ICartScriptServiceView Members

        public List<WebProductValidToCheckoutStatus> WebProductsValidToCheckoutStatus
        {
            get
            {
                return webProductsValidToCheckoutStatus;
            }
            set
            {
                webProductsValidToCheckoutStatus = value;
            }
        }

		[WebMethod(true)]
        public void AddItemsToCartContainer(List<Guid> cartProductUids, CartContainerEnum cartContainer)
        {
            presenter.AddItemsToCartContainer(cartProductUids, cartContainer);
        }

		[WebMethod(true)]
        public List<DeleteCartItemConfirm> DeleteItemsFromCartContainer(List<Guid> cartProductUids, CartContainerEnum cartContainer)
        {
            return presenter.DeleteItemsFromCartContainer(cartProductUids, cartContainer);
        }

        [WebMethod(true)]
        public List<DeleteCartItemConfirm> MoveItemWithInCart(List<Guid> cartProductUids, CartContainerEnum cartContainer)
        {
            return presenter.MoveItemWithInCart(cartProductUids, cartContainer);
        }

		[WebMethod(true)]
		public int AddOfferingToCart(Guid offeringUid)
		{
			return presenter.AddOfferingToCart(offeringUid);
		}

        [WebMethod(true)]
        public int AddProductToCart(String corbisId, Guid productUid)
        {
            return presenter.AddProductToCart(corbisId, productUid);
        }

        [WebMethod(true)]
        public PricingDisplay GetPricedPricingDisplay(Guid productUid)
        {

            return presenter.GetPricedPricingDisplay(productUid, CorbisBasePage.GetEnumDisplayTexts<PriceStatus>(), Resources.Resource.priceNow);
        }

        #endregion

        #region IView Members

        public Corbis.Framework.Logging.ILogging LoggingContext
        {
            get
            {
                return loggingContext;        
            }
            set
            {
                loggingContext = value;
            }
        }

        public System.Collections.Generic.IList<Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WCF.ValidationDetail> ValidationErrors
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

    }
}
