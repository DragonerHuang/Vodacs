/*
*所有关于Sys_Department类的业务代码接口应在此处编写
*/
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_DepartmentService
    {
        WebResponseContent Add(DepartmentDto dto);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(DepartmentEditDto dto);
        WebResponseContent Enable(DepartmentEnableDto dto);
        Task<WebResponseContent> GetDepartmentList(PageInput<DepartmentEditDto> dto);


        #region  -- 旧版（弃用） --

        WebResponseContent AddData(Department_Old_Dto dept);
        WebResponseContent EditData(Department_Old_Dto deptDto);
        WebResponseContent DelData(Guid id);

        #endregion
    }
}
