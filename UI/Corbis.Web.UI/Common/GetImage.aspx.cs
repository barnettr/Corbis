using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Corbis.Web.UI.Common
{
    /// <summary>
    /// Summary description for Get256Image.
    /// </summary>
    public class GetImage : System.Web.UI.Page
    {

        private Int32 maxSize = 256;

        ///<bp>***************************************************************************</bp>
        /// <summary>
        /// Page Load
        /// </summary>
        /// 
        /// <revisions>
        ///		<revision author="Travis Ortlieb" date="08/16/2005">Created.</revision>
        /// </revisions>
        ///<ep>***************************************************************************</ep>
        private void Page_Load(object sender, System.EventArgs e)
        {
            ShowResizedImage();
        }

        ///<bp>***************************************************************************</bp>
        /// <summary>
        /// Retrieves the image from the cache, or from the web server and resizes it,
        /// then writes it out to the response.
        /// </summary>
        /// 
        /// <revisions>
        ///		<revision author="Travis Ortlieb" date="08/16/2005">Created.</revision>
        /// </revisions>
        ///<ep>***************************************************************************</ep>
        private void ShowResizedImage()
        {
            try
            {
                // Get image path from querystring
                string imagePath = Request.QueryString["im"];
                imagePath = "http://cachens.corbis.com/" + imagePath;

                System.Drawing.Image smallImage;

                if (!String.IsNullOrEmpty(Request.QueryString["sz"]))
                {
                    maxSize = Convert.ToInt32(Request.QueryString["sz"]);
                }
                else
                {
                    maxSize = 0;
                }

                if (Cache.Get(imagePath + maxSize.ToString()) != null)
                {
                    smallImage = (System.Drawing.Image)Cache.Get(imagePath + maxSize.ToString());
                }
                else
                {
                    // Get image from URL
                    System.Drawing.Image bigImage = GetImageFromURL(imagePath);
                    smallImage = ResizeImage(bigImage);
                    // Cache this for at least 2 hours from last access
                    Cache.Insert(imagePath + maxSize.ToString(), smallImage, null, System.DateTime.MaxValue, System.TimeSpan.FromHours(2));
                }
                // Send the image out
                Response.ContentType = "image/jpeg";
                smallImage.Save(System.Web.HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                // Nothing to do but write out the error message
                Response.Write(ex.Message);
            }

        }

        ///<bp>***************************************************************************</bp>
        /// <summary>
        /// Get the image located at the specified URL
        /// </summary>
        /// 
        /// <revisions>
        ///		<revision author="Travis Ortlieb" date="08/16/2005">Created.</revision>
        /// </revisions>
        /// 
        /// <param name="Url">Url of the image</param>
        /// <returns>Image object</returns>
        ///<ep>***************************************************************************</ep>
        private System.Drawing.Image GetImageFromURL(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            System.Drawing.Image bigImage = System.Drawing.Image.FromStream(resStream);
            return bigImage;
        }

        ///<bp>***************************************************************************</bp>
        /// <summary>
        /// Resizes the passed-in image to maxsize pixels max
        /// </summary>
        /// 
        /// <revisions>
        ///		<revision author="Travis Ortlieb" date="08/16/2005">Created.</revision>
        /// </revisions>
        /// 
        /// <param name="img">Image object, bigger than maxsize</param>
        /// <returns>Image object, maxsize sized</returns>
        ///<ep>***************************************************************************</ep>
        private System.Drawing.Image ResizeImage(System.Drawing.Image img)
        {
            if (maxSize > 0)
            {
                // Set the width and height to resize to
                Int32 width = 0;
                Int32 height = 0;
                GetDimensions(img, ref width, ref height);

                // Create new image to draw into
                System.Drawing.Image imageNew = new Bitmap(width, height, PixelFormat.Format32bppRgb);

                Graphics oGraphic = Graphics.FromImage(imageNew);
                oGraphic.CompositingQuality = CompositingQuality.HighQuality;
                oGraphic.SmoothingMode = SmoothingMode.HighQuality;
                oGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                // Create a new rectangle to push the image into
                Rectangle oRectangle = new Rectangle(0, 0, width, height);
                oGraphic.DrawImage(img, oRectangle);

                return imageNew;
            }
            else
            {
                return img;
            }
        }

        ///<bp>***************************************************************************</bp>
        /// <summary>
        /// Sets the dimensions to resize the image, max maxsize pixels either way
        /// </summary>
        /// 
        /// <revisions>
        ///		<revision author="Travis Ortlieb" date="08/16/2005">Created.</revision>
        /// </revisions>
        /// 
        /// <param name="img">Image object, large version</param>
        /// <param name="width">Ref Int32 for width</param>
        /// <param name="height">Ref Int32 for height</param>
        /// <returns>void</returns>
        ///<ep>***************************************************************************</ep>
        private void GetDimensions(System.Drawing.Image img, ref Int32 width, ref Int32 height)
        {
            if (img.Height > img.Width)
            {
                // Set max height to maxsize
                height = maxSize;
                width = Convert.ToInt32(Math.Floor((Convert.ToDouble(maxSize) / img.Height) * img.Width));
            }
            if (img.Width > img.Height)
            {
                // Set max width to maxSize
                width = maxSize;
                height = Convert.ToInt32(Math.Floor((Convert.ToDouble(maxSize) / img.Width) * img.Height));
            }
            if (img.Width == img.Height)
            {
                // Set both dimensions to maxSize
                width = maxSize;
                height = maxSize;
            }
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion
    }
}
