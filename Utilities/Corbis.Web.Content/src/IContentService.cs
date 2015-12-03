using System;
using System.Collections.Generic;
using Corbis.Web.Entities;

namespace Corbis.Web.Content
{
    public interface IContentService
    {
        List<Corbis.Web.Entities.ContentItem> GetContent();
    }
}
