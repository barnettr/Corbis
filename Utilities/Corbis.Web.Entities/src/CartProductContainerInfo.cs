using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
	[Serializable]
    public class CartProductContainerInfo
    {
        #region Instance Variables
        private CartContainerEnum cartContainer;
        private int ordinal;

        #endregion

        #region Constructor

        public CartProductContainerInfo(
                                CartContainerEnum container,
                                int ordinal
            )
        {
            this.CartContainer = container;
            this.Ordinal = ordinal;
        }

        #endregion

        #region Public Properties

        public CartContainerEnum CartContainer
        {
            get { return cartContainer; }
            set { cartContainer = value; }
        }

        public int Ordinal
        {
            get { return ordinal; }
            set { ordinal = value; }
        }

        #endregion

    }
}
