using System;
using System.Collections.Generic;
using System.Text;

namespace Corbis.Web.Entities
{
    [Flags]
    public enum ImageResolutionEnum
    {
        Ultra = 0,
        High = 1,
        Medium =  2,
        Low = 4
    }
}
