using Vodace.Core.Dapper;
using Vodace.Core.DBManager;
using Vodace.Core.EFDbContext;
using Vodace.Core.Extensions;
using Vodace.Entity.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.BaseProvider.DictionaryComponent
{
    /// <summary>
    /// 组件视图，参照：https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components?view=aspnetcore-2.1
    /// 与Controller命名一样必须以ViewComponent结尾
    /// </summary>
    public class DictionaryViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string dropDownIds)
        {
            if (string.IsNullOrEmpty(dropDownIds))
                return null;

            string[] dicNos = dropDownIds.Split(',');
            StringBuilder stringBuilder = new StringBuilder();
            VOLContext context = DBServerProvider.GetEFDbContext();
            var dicData =await (from d in context.Set<Sys_Dictionary>()
                           join list in context.Set<Sys_Dictionary_List>()
                           on d.dic_id equals list.dic_id
                           into t
                           from list in t.DefaultIfEmpty()
                           where dicNos.Contains(d.dic_no)
                           select new { list.dic_value, list.dic_name, d.config, d.db_sql, list.order_no, d.dic_no }).ToListAsync();

            foreach (var item in dicData.GroupBy(x => x.dic_no))
            {
                stringBuilder.AppendLine($" var optionConfig{item.Key} = {item.Select(x => x.config).FirstOrDefault()}");

                string dbSql = item.Select(s => s.db_sql).FirstOrDefault();

                stringBuilder.AppendLine($@" var dataSource{item.Key} = {
                    (!string.IsNullOrEmpty(dbSql)
                    ? DBServerProvider.GetSqlDapper().QueryList<object>(dbSql, null).Serialize()
                    : item.OrderByDescending(o => o.order_no).
                            Select(s => new { s.dic_name, s.dic_value }).ToList()
                            .Serialize())
                     }.convertToValueText(optionConfig{item.Key})");
                stringBuilder.AppendLine($" optionConfig{item.Key}.data = dataSource{item.Key};");
            }
            ViewBag.Dic = stringBuilder.ToString();
            return View("~/Views/Shared/Dictionary.cshtml");
        }

    }
}
