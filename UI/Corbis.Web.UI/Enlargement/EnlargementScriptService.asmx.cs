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
using Corbis.Web.UI.Presenters;
using Corbis.Web.UI.Enlargement.ViewInterfaces;
using Corbis.Web.Authentication;
using Corbis.Web.Entities;
using Corbis.Image.Contracts.V1;
using Corbis.Web.UI.Presenters.Enlargement;


namespace Corbis.Web.UI.Enlargement
{
    /// <summary>
    /// Summary description for EnlargementScriptService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    [ToolboxItem(false)]
    public class EnlargementScriptService : Corbis.Web.UI.src.CorbisWebService, IEnlargementServiceView
    {

        private ILogging loggingContext;
        private EnlargementPresenter presenter;

        public EnlargementScriptService()
        {
            presenter = new EnlargementPresenter(this);
        }

        [WebMethod(true)]
        public List<RelatedImagesSetForScriptService> GetRelatedImages(string corbisId)
        {
            List<RelatedImagesSetForScriptService> relatedImagesSet = presenter.GetRelatedImages(corbisId);

              return EncodeRelatedImagesResult(relatedImagesSet);  
           
        }

		[WebMethod(true)]
		public List<string> GetImageList(int imageNumber, string caller, int pageSize, string queryString, string lightboxId)
		{	
			int intLightboxId;
			this.Caller = caller;
			this.ImageListPageSize = pageSize;
			this.ImageListQuery = queryString;
			int.TryParse(lightboxId, out intLightboxId);
			this.LightboxId = intLightboxId;

			presenter.GetImageList(imageNumber);

			this.ImageList.Insert(0, this.ImageListPageNo.ToString());
			this.ImageList.Insert(0, this.TotalImageCount.ToString());

			//get and insert corbisId
			int imageListIndex = (imageNumber - ((this.ImageListPageNo - 1) * pageSize)) - 1;
			this.ImageList.Insert(0, this.ImageList[imageListIndex]);

			return this.ImageList;
		}

        private List<RelatedImagesSetForScriptService> EncodeRelatedImagesResult(List<RelatedImagesSetForScriptService> relatedImagesSet)
        {
            List<RelatedImagesSetForScriptService> encodedRelatedImagesSet = relatedImagesSet;
             if (relatedImagesSet != null)
             {
                 foreach (RelatedImagesSetForScriptService relatedImage in encodedRelatedImagesSet)
                 {
                     if (!string.IsNullOrEmpty(relatedImage.Name))
                     {
                         // TODO: Security - need to call string helper to handle angular brackets.
                         relatedImage.Name = Server.HtmlEncode(relatedImage.Name);
                     }

                     if (!string.IsNullOrEmpty(relatedImage.Description))
                     {
                         // TODO: Security - need to call string helper to handle angular brackets.
                         relatedImage.Name = Server.HtmlEncode(relatedImage.Description);
                     }

                 }
             }

            return encodedRelatedImagesSet;

        }

		#region IEnlargementServiceView Members

		private string corbisId;
		public string CorbisId
		{
			get { return corbisId; }
			set { corbisId = value; }
		}

		public int ImageListPageSize { get; set; }

		public string Caller { get; set; }

		public string ImageListQuery { get; set; }

		public int LightboxId { get; set; }

		public int TotalImageCount { get; set; }

		public List<string> ImageList { get; set; }

		public int ImageListPageNo { get; set; }

		#endregion
 
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
	}
}
