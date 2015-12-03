using System;
using System.Web;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Configuration;
using Corbis.CommonSchema.Contracts.V1.Image;
using Corbis.Web.Entities;
using Corbis.Web.Authentication;
using Corbis.MediaDownload.Contracts.V1;
using Corbis.MediaDownload.ServiceAgents.V1;
using Corbis.Web.Utilities.StateManagement;

namespace Corbis.Web.UI.Handlers
{
    public class ZipHandler : IHttpHandler, IReadOnlySessionState
    {

        #region private variables

        private IMediaDownloadContract _downloadAgent;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnlargementImageHandler"/> class.
        /// </summary>
        public ZipHandler() : this(new MediaDownloadServiceAgent()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipHandler"/> class.
        /// </summary>
        /// <param name="imageAgent">The image agent.</param>
        public ZipHandler(IMediaDownloadContract downloadAgent)
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
            System.IO.Stream stream = null;
            try
            {
                Guid orderUid = new Guid(context.Request.QueryString[ZipHandlerKeys.OrderUid]);
                Guid archiveUid = new Guid(context.Request.QueryString[ZipHandlerKeys.ArchiveUid]);
                // Try and Get the file Size
                int fileSize = 0;
                int.TryParse(context.Request.QueryString[ZipHandlerKeys.FileSize], out fileSize);

                StateItemCollection stateItems = new StateItemCollection(context);
                List<Guid> sessionArchiveUids = stateItems.GetStateItemValue<List<Guid>>(
                    OrderKeys.Name,
                    OrderKeys.ArchiveUids,
                    StateItemStore.AspSession);
                if (sessionArchiveUids == null ||
                    !sessionArchiveUids.Contains(archiveUid))
                {
                    throw new ApplicationException("ArchiveUid not in Users Session!");
                }

                stream = _downloadAgent.GetPackagedImagesStream(
                    Profile.Current.UserName,
                    orderUid,
                    archiveUid,
                    fileSize);

                // Content Type Logic copied from old ExternalUI, DownloadZipFile method on file 
                // $/Sustainment/DEV/Common/Class Libraries/UIBusinessObjects/BusinessObjects/Order.cs 
                string contentType = "application/zip-compressed";
                // Prep the Http response
                // Mac use a slightly different ContentType
                if (context.Request.Browser.Platform.ToLower().IndexOf("mac") != -1)
                {
                    contentType = "application/x-download";
                }
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.BufferOutput = false;
                context.Response.ContentType = contentType;

                int lastSlash = context.Request.Path.LastIndexOf("/");
                string fileName = context.Request.Path.Substring(lastSlash + 1);
                context.Response.AppendHeader("Content-Disposition", "filename=\"" + fileName + "\"");
                if (fileSize > 0)
                {
                    context.Response.AppendHeader("Content-Length", fileSize.ToString());
                }
                // Send Headers now
                context.Response.Flush();

                // 64k chunks
                int chunkSize = 1024 * 64;
                byte[] bytes = new byte[chunkSize];
                int bytesRead = stream.Read(bytes, 0, chunkSize);
                while (bytesRead > 0)
                {
                    context.Response.OutputStream.Write(bytes, 0, bytesRead);
                    bytesRead = stream.Read(bytes, 0, chunkSize);
                }
                context.Response.Flush();
            }
            catch
            {
                context.Response.Redirect(SiteUrls.UnexpectedError);
            }
            finally
            {
                // Always make sure the stream is closed and disposed
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }

        #endregion

    }
}
