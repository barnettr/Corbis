using System;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.Web.Authentication;
using Corbis.MediaDownload.Contracts.V1;
using Corbis.MediaDownload.ServiceAgents.V1;


namespace Corbis.Web.UI.Handlers
{
    public class ImageHandler : IHttpHandler, IReadOnlySessionState
    {

        #region private variables

        private IMediaDownloadContract _downloadAgent;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnlargementImageHandler"/> class.
        /// </summary>
        public ImageHandler() : this(new MediaDownloadServiceAgent()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnlargementImageHandler"/> class.
        /// </summary>
        /// <param name="imageAgent">The image agent.</param>
        public ImageHandler(IMediaDownloadContract downloadAgent)
        {
            _downloadAgent = downloadAgent;
        }

        #endregion

        #region IHttpHandler Members

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {

            string languageName;
            if (HttpContext.Current.Request.Cookies["Profile"] == null || HttpContext.Current.Request.Cookies["Profile"]["Culture"] == null)
                languageName = "en-US";
            else
            {
                languageName = HttpContext.Current.Request.Cookies["Profile"]["Culture"];
                languageName = languageName.Replace("%2D", "-");
                
            }
            

            string requestedFileName = System.IO.Path.GetFileName(context.Request.FilePath);
            context.Response.AppendHeader("Content-Disposition", "filename=\"" + requestedFileName + "\"");
            context.Response.ContentType = "image/jpeg";

            Guid offeringUid = Guid.Empty;
            FileSize fileSize = FileSize.Unknown;

            string uid = context.Request.QueryString["uid"];
            if (String.IsNullOrEmpty(uid))
            {
                if (System.IO.File.Exists(context.Request.PhysicalPath))
                {
                    GetImageFromFile(context);
                    return;
                }
                else
                {
                    DisplayImageNotAvailable(context,languageName);
                    return;
                }
            }
            
            // uid is in the QueryString, make sure it's a valid Guid ...
            try
            {

                offeringUid = new Guid(uid);
            }
            catch
            {
                DisplayImageNotAvailable(context, languageName);
                return;
            }

            // Try and get the FileSize code ...
            try
            {
                fileSize = (FileSize)Enum.Parse(typeof(FileSize), context.Request.QueryString["size"]);
            }
            catch
            {
                DisplayImageNotAvailable(context, languageName);
                return;
            }

            // IR 25786:  Allowing client side caching leaves watermark on enlargement after login
            // Cannot allow browser caching -- Travis O. 5/2/2005
            //context.Response.Cache.SetExpires(DateTime.Now.AddDays(-365));
            //context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            
            // Get the countrycode and username if the user is not anonymous
            Profile profile = Profile.Current;
            string countryCode = profile.CountryCode;
            string userName = String.Empty;
            if (!profile.IsAnonymous)
            {
                userName = profile.UserName;
            }

            byte[] imageBytes = null;

            try
            {
                imageBytes = _downloadAgent.GetImageBytes(
                userName,
                countryCode,
                fileSize,
                offeringUid);

            }
            catch { }

            if (imageBytes == null || imageBytes.Length == 0)
            {
                DisplayImageNotAvailable(context,languageName);
                return;
            }

            // Check the HTTP Request.  If it is HEAD, we don't need to return actual bytes of the image,
            // but we do need to know the size of the image.
            if (context.Request.HttpMethod == "HEAD")
            {
                context.Response.AppendHeader("Content-Length", imageBytes.Length.ToString());
            }
            else
            {
                context.Response.BinaryWrite(imageBytes);
                context.Response.Flush();
            }

        }

        #endregion

        #region private methods

        private void GetImageFromFile(HttpContext context)
        {
            context.Response.WriteFile(context.Request.PhysicalPath);
        }

        private void DisplayImageNotAvailable(HttpContext context,string language)
        {
            context.Response.Redirect(string.Format(SiteUrls.ImageNotAvailable256, language), true);
        }

        #endregion
    }
}
