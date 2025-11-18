
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_User_RelationService
    {
        WebResponseContent Add(SysUserRelationDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(SysUserRelationDto dto);
        Task<WebResponseContent> GetSysUserRelationList(PageInput<SysUserRelationSearchDto> dto);
    }
 }
