using System;
using Corbis.Web.UI.ViewInterfaces;

namespace Corbis.Web.UI.Lightboxes.ViewInterfaces
{
	public interface ISendEmailLightboxView: IView
	{
		int LightboxId { get; }
		string LightboxLink { get; }
		string FromEmail { get; }
		string ToEmails { get; }
		string Subject { get; }
		string Message { get; }
	}
}
