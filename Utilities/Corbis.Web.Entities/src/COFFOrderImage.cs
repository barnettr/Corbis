using Corbis.CommonSchema.Contracts.V1.Image;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Corbis.Web.Entities
{
    [Serializable]
    public class COFFOrderImage
    {
        public string CorbisId { get; set; }
        public FileSize FileSize { get; set; }
        public Guid ImageUid { get; set; }
        public Guid ProductUid { get; set; }
    }
}