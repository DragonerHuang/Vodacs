using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Entity.DomainModels.Biz.partial
{
    public class FileInfoEx
    {
        public Guid FileId { get; set; } = Guid.Empty;
        public string OriginalName { get; set; } = string.Empty;
        public string StoragePath { get; set; } = string.Empty; 
        public string MimeType { get; set; } = string.Empty;
        public long Size { get; set; }
        public string UploaderId { get; set; } = string.Empty;
    }

    public class TemporaryTokenData
    {
        public Guid fileId { get; set; } = Guid.Empty;
        public string userId { get; set; } = string.Empty;
        public long exp { get; set; }
        public string salt { get; set; } = string.Empty;
    }
}
