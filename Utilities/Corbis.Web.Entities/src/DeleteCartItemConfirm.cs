using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    public class DeleteCartItemConfirm
    {
        private Guid productUid;

        public Guid ProductUid
        {
            get { return productUid; }
            set { productUid = value; }
        }
        private bool deleteSucceeded;

        public bool DeleteSucceeded
        {
            get { return deleteSucceeded; }
            set { deleteSucceeded = value; }
        }
    }
}
