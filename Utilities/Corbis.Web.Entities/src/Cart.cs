using System;
using System.Collections.Generic;
using System.Text;
using Corbis.Cart.Contracts.v1;
using SvcCart = Corbis.Cart.Contracts.v1.Cart;
using Corbis.Products.Contracts.v1;

namespace Corbis.Web.Entities
{
    public class Cart
    {
        #region Fields

        private SvcCart cart;
        // TODO: remove
        private List<Guid> productUids;

        #endregion

        #region Properties

        public Guid CartUid
        {
            get { return cart.CartUid; }
            set { cart.CartUid = value; }
        }

        public DateTime ChangedAtUtc
        {
            get { return cart.ChangedAtUtc; }
            set { cart.ChangedAtUtc = value; }
        }

        public DateTime CreatedAtUtc
        {
            get { return cart.CreatedAtUtc; }
            set { cart.CreatedAtUtc = value; }
        }

        // TODO: remove when service cart entity is updated
        public string OwnerBPNumber
        {
            get { return cart.OwnerBPNumber; }
            set { cart.OwnerBPNumber = value; }
        }

        public int ProductCount
        {
            get { return cart.ProductCount; }
            set { cart.ProductCount = value; }
        }

        // TODO: modify this when can get from service cart entity
        public List<Guid> ProductUids
        {
            get { return productUids; }
            set { productUids = value; }
        }

        #endregion

        #region Constructors

        public Cart()
        {
            cart = new SvcCart();
            productUids = new List<Guid>();
        }

        public Cart(SvcCart cart)
        {
            this.cart = cart;
        }

        #endregion
    }
}
