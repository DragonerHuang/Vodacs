/*
*所有关于Sys_QuartzOptions类的业务代码接口应在此处编写
*/
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Utilities;
using Vodace.Entity;
using Vodace.Entity.DomainModels;

namespace Vodace.Sys.IServices
{
    public partial interface ISys_QuartzOptionsService
    {

        /// <summary>
        /// 手动执行一次
        /// </summary>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<object> Run(Sys_QuartzOptions taskOptions);
        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<object> Start(Sys_QuartzOptions taskOptions);
        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="schedulerFactory"></param>
        /// <param name="taskOptions"></param>
        /// <returns></returns>
        Task<object> Pause(Sys_QuartzOptions taskOptions);

        Task<WebResponseContent> GetListByPage(PageInput<TaskQuery> query);
        WebResponseContent Add(TaskAddDto dto);
        WebResponseContent Edit(TaskEditDto dto);
        Task<WebResponseContent> SwitchStatus(Guid id);
        WebResponseContent DelData(Guid id);
        void SendMsg(string userId, string title, string message);
    }
}
