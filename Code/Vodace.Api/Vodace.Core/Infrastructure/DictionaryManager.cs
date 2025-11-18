using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Vodace.Core.CacheManager;
using Vodace.Core.DBManager;
using Vodace.Core.Extensions.AutofacManager;
using Vodace.Core.Services;
using Vodace.Entity.DomainModels;

namespace Vodace.Core.Infrastructure
{
    public static class DictionaryManager
    {
        private static List<Sys_Dictionary> _dictionaries { get; set; }

        private static object _dicObj = new object();
        private static string _dicVersionn = "";
        public const string Key = "inernalDic";

        public static List<Sys_Dictionary> Dictionaries
        {
            get
            {
                return GetAllDictionary();
            }
        }

        public static Sys_Dictionary GetDictionary(string dicNo)
        {
            return GetDictionaries(new string[] { dicNo }).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicNos"></param>
        /// <param name="executeSql">是否执行自定义sql</param>
        /// <returns></returns>
        public static IEnumerable<Sys_Dictionary> GetDictionaries(IEnumerable<string> dicNos, bool executeSql = true)
        {
            static List<Sys_Dictionary_List> query(string sql)
            {

                return DBServerProvider.SqlDapper.QueryList<SourceKeyVaule>(sql, null).Select(s => new Sys_Dictionary_List()
                {
                    dic_name = s.Value,
                    dic_value = s.Key.ToString()
                }).ToList();
            }
            foreach (var item in Dictionaries.Where(x => dicNos.Contains(x.dic_no)))
            {
                if (executeSql)
                {
                    //  2020.05.01增加根据用户信息加载字典数据源sql
                    string sql = DictionaryHandler.GetCustomDBSql(item.dic_no, item.db_sql);
                    if (!string.IsNullOrEmpty(item.db_sql))
                    {
                        item.Sys_DictionaryList = query(sql);
                    }
                }
                yield return item;
            }
        }
        /// <summary>
        /// 每次变更字典配置的时候会重新拉取所有配置进行缓存(自行根据实际处理)
        /// </summary>
        /// <returns></returns>
        private static List<Sys_Dictionary> GetAllDictionary()
        {
            ICacheService cacheService = AutofacContainerModule.GetService<ICacheService>();
            //每次比较缓存是否更新过，如果更新则重新获取数据
            if (_dictionaries != null && _dicVersionn == cacheService.Get(Key))
            {
                return _dictionaries;
            }

            lock (_dicObj)
            {
                if (_dicVersionn != "" && _dictionaries != null && _dicVersionn == cacheService.Get(Key)) return _dictionaries;
                _dictionaries = DBServerProvider.DbContext
                    .Set<Sys_Dictionary>()
                    .Where(x => x.enable == 1)
                    .Include(c => c.Sys_DictionaryList).ToList();

                string cacheVersion = cacheService.Get(Key);
                if (string.IsNullOrEmpty(cacheVersion))
                {
                    cacheVersion = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                    cacheService.Add(Key, cacheVersion);
                }
                else
                {
                    _dicVersionn = cacheVersion;
                }
            }
            return _dictionaries;
        }
    }

    public class SourceKeyVaule
    {
        public object Key { get; set; }
        public string Value { get; set; }
    }
}
