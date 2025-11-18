
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
namespace Vodace.Sys.IServices
{
    public partial interface ISys_File_ConfigService
    {
        WebResponseContent Add(AddFileConfigDto dtoFileConfig);
        WebResponseContent Del(Guid guid);
        WebResponseContent Edit(EditFileConfigDto dtoFileConfig);

        Task<WebResponseContent> GetFileConfigById(Guid guid);

        Task<WebResponseContent> GetFileConfigList(PageInput<AddFileConfigDto> dtoSearchInput);

        Task<WebResponseContent> GetFileConfigAllName();

        /// <summary>
        /// 根据年份 获取项目年度文件夹
        /// </summary>
        /// <returns></returns>
        WebResponseContent GetMainProFolderName(DateTime dateTime);

        /// <summary>
        /// 根据年份 获取项目年度文件夹(异步)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        Task<WebResponseContent> GetMainProFolderNameAsync(DateTime dateTime);
        /// <summary>
        /// 添加项目文件配置
        /// </summary>
        /// <param name="file_code"></param>
        /// <param name="folder_path"></param>
        /// <returns></returns>
        bool AddCofig(string file_code, string folder_path);
    }
 }
