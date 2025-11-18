using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.Utilities;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.IServices.System.Partial
{
    public partial interface ISys_FileService
    {
        Task<WebResponseContent> GetDirectoryContents(GetFileInfoDto dto);
    }
}
