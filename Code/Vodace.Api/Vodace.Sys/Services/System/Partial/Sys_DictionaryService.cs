using AutoMapper;
using Castle.Core.Internal;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vodace.Core.BaseProvider;
using Vodace.Core.Const;
using Vodace.Core.Enums;
using Vodace.Core.Extensions;
using Vodace.Core.Infrastructure;
using Vodace.Core.Localization;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity;
using Vodace.Entity.DomainModels;
using Vodace.Sys.IRepositories;

namespace Vodace.Sys.Services
{
    public partial class Sys_DictionaryService
    {
        private readonly ILocalizationService _localizationService;
        private readonly IMapper _mapper;
        private readonly ISys_DictionaryListRepository _DictionaryListRepository;
        private readonly ISys_DictionaryRepository _DictionaryRepository;

        private readonly ISys_DictionaryRepository _repository;//访问数据库

        public Sys_DictionaryService(
            ILocalizationService localizationService,
            IMapper mapper,
            ISys_DictionaryListRepository dictionaryListRepository,
            ISys_DictionaryRepository dictionaryRepository,
            ISys_DictionaryRepository dbRepository)
            : base(dbRepository)
        {
            _localizationService = localizationService;
            _mapper = mapper;
            _DictionaryListRepository = dictionaryListRepository;
            _DictionaryRepository = dictionaryRepository;

            _repository = dbRepository;

        }

        protected override void Init(IRepository<Sys_Dictionary> repository)
        {

        }
        /// <summary>
        /// 代码生成器获取所有字典项编号(超级管理权限)
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetBuilderDictionary()
        {
            return await repository.FindAsync(x => 1 == 1, s => s.dic_no);
        }

        public List<Sys_Dictionary> Dictionaries
        {
            get { return DictionaryManager.Dictionaries; }
        }

        public WebResponseContent GetVueDictionary(string[] dicNos)
        {
            if (dicNos == null || dicNos.Count() == 0) return WebResponseContent.Instance.Error(_localizationService.GetString("connot_be_empty"));
            var dicConfig = DictionaryManager.GetDictionaries(dicNos, false).Select(s => new
            {
                dicNo = s.dic_no,
                //config = s.config,
                dbSql = s.db_sql,
                list = s.Sys_DictionaryList.OrderByDescending(o => o.order_no)
                          .Select(list => new { key = list.dic_name, value = list.dic_value })
            }).ToList();

            object GetSourceData(string dicNo, string dbSql, object data)
            {
                dbSql = DictionaryHandler.GetCustomDBSql(dicNo, dbSql);
                if (string.IsNullOrEmpty(dbSql))
                {
                    return data as object;
                }
                return repository.DapperContext.QueryList<object>(dbSql, null);
            }
            var data = dicConfig.Select(item => new
            {
                item.dicNo,
                //item.config,
                data = GetSourceData(item.dicNo, item.dbSql, item.list)
            }).ToList();

            return WebResponseContent.Instance.OK("Ok", data);
        }


        /// <summary>
        /// 通过远程搜索
        /// </summary>
        /// <param name="dicNo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public object GetSearchDictionary(string dicNo, string value)
        {
            if (string.IsNullOrEmpty(dicNo) || string.IsNullOrEmpty(value))
            {
                return null;
            }
            //  2020.05.01增加根据用户信息加载字典数据源sql
            string sql = Dictionaries.Where(x => x.dic_no == dicNo).FirstOrDefault()?.db_sql;
            sql = DictionaryHandler.GetCustomDBSql(dicNo, sql);
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }
            sql = $"SELECT * FROM ({sql}) AS t WHERE value LIKE @value";
            return repository.DapperContext.QueryList<object>(sql, new { value = "%" + value + "%" });
        }

        /// <summary>
        /// 表单设置为远程查询，重置或第一次添加表单时，获取字典的key、value
        /// </summary>
        /// <param name="dicNo"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<object> GetRemoteDefaultKeyValue(string dicNo, string key)
        {
            return await Task.FromResult(1);
            //if (string.IsNullOrEmpty(dicNo) || string.IsNullOrEmpty(key))
            //{
            //    return null;
            //}
            //string sql = Dictionaries.Where(x => x.DicNo == dicNo).FirstOrDefault()?.DbSql;
            //if (string.IsNullOrEmpty(sql))
            //{
            //    return null;
            //}
            //sql = $"SELECT * FROM ({sql}) AS t WHERE t.key = @key";
            //return await Task.FromResult(repository.DapperContext.QueryFirst<object>(sql, new { key }));
        }


        /// <summary>
        ///  table加载数据后刷新当前table数据的字典项(适用字典数据量比较大的情况)
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        public object GetTableDictionary(Dictionary<string, object[]> keyData)
        {
            // 2020.08.06增加pgsql获取数据源
            if (DBType.Name == DbCurrentType.PgSql.ToString())
            {
                return GetPgSqlTableDictionary(keyData);
            }
            var dicInfo = Dictionaries.Where(x => keyData.ContainsKey(x.dic_no) && !string.IsNullOrEmpty(x.db_sql))
                .Select(x => new { x.dic_no, x.db_sql })
                .ToList();
            List<object> list = new List<object>();
            string keySql = DBType.Name == DbCurrentType.MySql.ToString() ? "t.key" : "t.[key]";
            dicInfo.ForEach(x =>
            {
                if (keyData.TryGetValue(x.dic_no, out object[] data))
                {
                    //  2020.05.01增加根据用户信息加载字典数据源sql
                    string sql = DictionaryHandler.GetCustomDBSql(x.dic_no, x.db_sql);
                    sql = $"SELECT * FROM ({sql}) AS t WHERE " +
                   $"{keySql}" +
                   $" in @data";
                    list.Add(new { key = x.dic_no, data = repository.DapperContext.QueryList<object>(sql, new { data }) });
                }
            });
            return list;
        }

        /// <summary>
        ///  2020.08.06增加pgsql获取数据源
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        public object GetPgSqlTableDictionary(Dictionary<string, object[]> keyData)
        {
            var dicInfo = Dictionaries.Where(x => keyData.ContainsKey(x.dic_no) && !string.IsNullOrEmpty(x.db_sql))
                .Select(x => new { x.dic_no, x.db_sql })
                .ToList();
            List<object> list = new List<object>();

            dicInfo.ForEach(x =>
            {
                if (keyData.TryGetValue(x.dic_no, out object[] data))
                {
                    string sql = DictionaryHandler.GetCustomDBSql(x.dic_no, x.db_sql);
                    sql = $"SELECT * FROM ({sql}) AS t WHERE t.key=any(@data)";
                    list.Add(new { key = x.dic_no, data = repository.DapperContext.QueryList<object>(sql, new { data = data.Select(s => s.ToString()).ToList() }) });
                }
            });
            return list;
        }


        public override PageGridData<Sys_Dictionary> GetPageData(PageDataOptions pageData)
        {
            //增加查询条件
            base.QueryRelativeExpression = (IQueryable<Sys_Dictionary> fun) =>
            {
                return fun.Where(x => 1 == 1);
            };
            return base.GetPageData(pageData);
        }
        public override WebResponseContent Update(SaveModel saveDataModel)
        {
            if (saveDataModel.MainData.DicKeyIsNullOrEmpty("DicNo")
                || saveDataModel.MainData.DicKeyIsNullOrEmpty("Dic_ID"))
                return base.Add(saveDataModel);
            //判断修改的字典编号是否在其他ID存在
            string dicNo = saveDataModel.MainData["DicNo"].ToString().Trim();
            if (base.repository.Exists(x => x.dic_no == dicNo && x.dic_id != saveDataModel.MainData["Dic_ID"].GetInt()))
                return new WebResponseContent().Error($"字典编号:{dicNo}已存在。!");

            base.UpdateOnExecuting = (Sys_Dictionary dictionary, object addList, object editList, List<object> obj) =>
            {
                List<Sys_Dictionary_List> listObj = new List<Sys_Dictionary_List>();
                listObj.AddRange(addList as List<Sys_Dictionary_List>);
                listObj.AddRange(editList as List<Sys_Dictionary_List>);

                WebResponseContent _responseData = CheckKeyValue(listObj);
                if (!_responseData.status) return _responseData;

                dictionary.db_sql = SqlFilters(dictionary.db_sql);
                return new WebResponseContent(true);
            };
            return RemoveCache(base.Update(saveDataModel));

        }


        private WebResponseContent CheckKeyValue(List<Sys_Dictionary_List> dictionaryLists)
        {
            WebResponseContent webResponse = new WebResponseContent();
            if (dictionaryLists == null || dictionaryLists.Count == 0) return webResponse.OK();

            if (dictionaryLists.GroupBy(g => g.dic_name).Any(x => x.Count() > 1))
                return webResponse.Error("【字典项名称】不能有重复的值");

            if (dictionaryLists.GroupBy(g => g.dic_value).Any(x => x.Count() > 1))
                return webResponse.Error("【字典项Key】不能有重复的值");

            return webResponse.OK();
        }

        private static string SqlFilters(string source)
        {
            if (string.IsNullOrEmpty(source)) return source;

            //   source = source.Replace("'", "''");
            source = Regex.Replace(source, "-", "", RegexOptions.IgnoreCase);
            //去除执行SQL语句的命令关键字
            source = Regex.Replace(source, "insert ", "", RegexOptions.IgnoreCase);
            // source = Regex.Replace(source, "sys.", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "update ", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "delete ", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "drop ", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "truncate ", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "declare ", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "xp_cmdshell ", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "/add ", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, " net user ", "", RegexOptions.IgnoreCase);
            //去除执行存储过程的命令关键字 
            source = Regex.Replace(source, " exec ", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, " execute ", "", RegexOptions.IgnoreCase);
            //防止16进制注入
            source = Regex.Replace(source, "0x", "0 x", RegexOptions.IgnoreCase);

            return source;
        }
        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            //saveDataModel.MainData["parent_id"] = 0;
            if (saveDataModel.MainData.DicKeyIsNullOrEmpty("dic_no")) return base.Add(saveDataModel);

            string dicNo = saveDataModel.MainData["dic_no"].ToString();
            if (base.repository.Exists(x => x.dic_no == dicNo))
                return new WebResponseContent().Error("字典编号:" + dicNo + "已存在");

            base.AddOnExecuting = (Sys_Dictionary dic, object obj) =>
            {
                WebResponseContent _responseData = CheckKeyValue(obj as List<Sys_Dictionary_List>);
                if (!_responseData.status) return _responseData;

                dic.db_sql = SqlFilters(dic.db_sql);
                return new WebResponseContent(true);
            };
            return RemoveCache(base.Add(saveDataModel));
        }

        public override WebResponseContent Del(object[] keys, bool delList = false)
        {
            //delKeys删除的key
            base.DelOnExecuting = (object[] delKeys) =>
            {
                return new WebResponseContent(true);
            };
            //true将子表数据同时删除
            return RemoveCache(base.Del(keys, true));
        }

        private WebResponseContent RemoveCache(WebResponseContent webResponse)
        {
            if (webResponse.status)
            {
                CacheContext.Remove(DictionaryManager.Key);
            }
            return webResponse;
        }

        public async Task<WebResponseContent> AddData(DictionaryDto dictionaryDto)
        {
            try
            {
                if (dictionaryDto == null) return WebResponseContent.Instance.Error(_localizationService.GetString("content_connot_be_empty"));
                var entData = _mapper.Map<Sys_Dictionary>(dictionaryDto);
                var isexistDic_no = _DictionaryRepository.Exists(d => d.dic_no == entData.dic_no && d.delete_status == 0);
                if (isexistDic_no) return WebResponseContent.Instance.Error(_localizationService.GetString("dic_no_exist"));
                var isexistDic_name = _DictionaryRepository.Exists(d => d.dic_name == entData.dic_name && d.delete_status == 0);
                if (isexistDic_name) return WebResponseContent.Instance.Error(_localizationService.GetString("dic_name_exist"));
                entData.create_id = UserContext.Current.UserId;
                entData.create_date = DateTime.Now;
                entData.delete_status = (int)SystemDataStatus.Valid;
                entData.create_name = UserContext.Current.UserName;

                _DictionaryRepository.Add(entData,true);
                _DictionaryRepository.SaveChanges();
                if (dictionaryDto.dictionaryList.Count > 0)
                {
                    List<Sys_Dictionary_List> list = new List<Sys_Dictionary_List>();
                    foreach (var item in dictionaryDto.dictionaryList)
                    {
                        list.Add(new Sys_Dictionary_List
                        {
                            dic_id = entData.dic_id,
                            dic_name = item.dic_name,
                            dic_name_eng = item.dic_name_eng,
                            dic_value = item.dic_value,
                            create_id = UserContext.Current.UserId,
                            delete_status = (int)SystemDataStatus.Valid,
                            create_date = DateTime.Now,
                            create_name = UserContext.Current.UserName,
                        });
                    }
                    await _DictionaryListRepository.AddRangeAsync(list);
                }
                await _DictionaryListRepository.SaveChangesAsync();
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> EditData(DictionaryEditDto dictionaryDto)
        {
            try
            {
                if (dictionaryDto.dic_id == 0) return WebResponseContent.Instance.Error(_localizationService.GetString("update_id_empty"));
                var isexistDic_no = _DictionaryRepository.Exists(d => d.dic_no == dictionaryDto.dic_no && d.dic_id != dictionaryDto.dic_id);
                if (isexistDic_no) return WebResponseContent.Instance.Error(_localizationService.GetString("dic_no_exist"));
                var isexistDic_name = _DictionaryRepository.Exists(d => d.dic_name == dictionaryDto.dic_name && d.dic_id != dictionaryDto.dic_id);
                if (isexistDic_name) return WebResponseContent.Instance.Error(_localizationService.GetString("dic_name_exist"));
                var entData = _DictionaryRepository.Find(d => d.dic_id == dictionaryDto.dic_id).FirstOrDefault();
                if (entData != null)
                {
                    entData.dic_name = dictionaryDto.dic_name;
                    entData.dic_no = dictionaryDto.dic_no;
                    entData.db_sql = dictionaryDto.db_sql;
                    entData.remark = dictionaryDto.remark;
                    entData.enable = dictionaryDto.enable;
                    //entData.config = dictionaryDto.config;
                    entData.parent_id = dictionaryDto.parent_id;
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    var res = _DictionaryRepository.Update(entData,true);
                    if (dictionaryDto.dictionaryList.Count > 0)
                    {
                        var lstChild = _DictionaryListRepository.Find(d => d.dic_id == dictionaryDto.dic_id).ToList();
                        List<Sys_Dictionary_List> lstUpd = new List<Sys_Dictionary_List>();
                        List<Sys_Dictionary_List> lstAdd = new List<Sys_Dictionary_List>();
                        foreach (var item in dictionaryDto.dictionaryList)
                        {
                            var updData = lstChild.Where(d => d.dic_list_id == item.dic_list_id).FirstOrDefault();
                            if (updData != null)
                            {
                                updData.dic_name_eng = item.dic_name_eng;
                                updData.dic_name = item.dic_name;
                                updData.dic_value = item.dic_value;
                                updData.enable = item.enable;
                                updData.modify_date = DateTime.Now;
                                updData.modify_id = UserContext.Current.UserId;
                                updData.modify_name = UserContext.Current.UserName;
                                updData.dic_value = item.dic_value;
                                updData.dic_name = item.dic_name;

                                lstUpd.AddRange(lstUpd);
                            }
                            else
                            {
                                lstAdd.Add(new Sys_Dictionary_List
                                {
                                    dic_id = entData.dic_id,
                                    dic_name = item.dic_name,
                                    dic_name_eng = item.dic_name_eng,
                                    dic_value = item.dic_value,
                                    create_id = UserContext.Current.UserId,
                                    delete_status = (int)SystemDataStatus.Valid,
                                    create_date = DateTime.Now,
                                    create_name = UserContext.Current.UserName,
                                });
                            }
                        }
                        _DictionaryListRepository.AddRange(lstAdd);
                        _DictionaryListRepository.UpdateRange(lstUpd);
                    }
                    await _DictionaryListRepository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                else
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> DelData(int id)
        {
            try
            {
                if (id == 0) return WebResponseContent.Instance.Error(_localizationService.GetString("delete_id_empty"));
                var entData = _DictionaryRepository.Find(d => d.dic_id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.delete_status = (int)SystemDataStatus.Invalid;
                    //entData.enable = 0;
                    entData.modify_date = DateTime.Now;
                    entData.modify_id = UserContext.Current.UserId;
                    entData.modify_name = UserContext.Current.UserName;
                    var res = _DictionaryRepository.Update(entData);
                    if (res > 0)
                    {
                        var lstData = _DictionaryListRepository.Find(d => d.dic_id == id).ToList();
                        List<Sys_Dictionary_List> lstUpd = new List<Sys_Dictionary_List>();
                        foreach (var item in lstData)
                        {
                            item.delete_status = (int)SystemDataStatus.Invalid;
                            //item.enable = 0;
                            item.modify_date = DateTime.Now;
                            item.modify_id = UserContext.Current.UserId;
                            item.modify_name = UserContext.Current.UserName;
                            lstUpd.Add(item);
                        }
                        _DictionaryListRepository.UpdateRange(lstUpd);
                    }
                    await _DictionaryRepository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                else
                {
                    return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }

        public async Task<WebResponseContent> SwitchEnable(int id, int enable)
        {
            try
            {
                if (id == 0) return WebResponseContent.Instance.Error("id_null");
                var entData = _DictionaryRepository.Find(d => d.dic_id == id).FirstOrDefault();
                if (entData != null)
                {
                    entData.enable = (byte)enable;
                    entData.modify_date = DateTime.Now;
                    entData.modify_name = UserContext.Current.UserName;

                    _DictionaryRepository.Update(entData,true);
                    await _DictionaryRepository.SaveChangesAsync();
                    return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"));
                }
                return WebResponseContent.Instance.Error($"{_localizationService.GetString("non_existent")}");
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return new WebResponseContent().Error(_localizationService.GetString("operation_failed")); ;
            }
        }
        public WebResponseContent GetPageDataAsync(PageInput<DictionarySearchDto> search)
        {
            try
            {
                var parm = search.search;

                var pageDataOptions = new PageDataOptions
                {
                    Page = search.page_index,
                    Rows = search.page_rows,
                    Sort = "dic_id",
                };
                var searchParameters = new List<SearchParameters>();

                if (!string.IsNullOrEmpty(parm.dic_name))
                {
                    searchParameters.Add(new SearchParameters
                    {
                        Name = "dic_name",
                        Value = parm.dic_name
                    });
                }
                if (!string.IsNullOrEmpty(parm.dic_no))
                {
                    searchParameters.Add(new SearchParameters
                    {
                        Name = "dic_no",
                        Value = parm.dic_no
                    });
                }
                if (parm.parent_id.HasValue)
                {
                    searchParameters.Add(new SearchParameters
                    {
                        Name = "parent_id",
                        Value = parm.parent_id.ToString()
                    });
                }
                if (parm.enable.HasValue)
                {
                    searchParameters.Add(new SearchParameters
                    {
                        Name = "enable",
                        Value = parm.enable == 0 ? "0" : "1"
                    });
                }
                //if (parm.create_date.HasValue)
                //{
                //    searchParameters.Add(new SearchParameters
                //    {
                //        Name = "create_date",
                //        Value = parm.create_date.ToString()
                //    });
                //}
                //if (parm.modify_date.HasValue)
                //{
                //    searchParameters.Add(new SearchParameters
                //    {
                //        Name = "modify_date",
                //        Value = parm.modify_date.ToString(),
                //        DisplayType = "datetime"
                //    });
                //}
                if (searchParameters.Count > 0)
                {
                    pageDataOptions.Wheres = JsonConvert.SerializeObject(searchParameters);
                }

                //增加查询条件
                base.QueryRelativeExpression = (IQueryable<Sys_Dictionary> fun) =>
                {
                    return fun.Where(x => 1 == 1 && x.delete_status == (int)SystemDataStatus.Valid);
                };
                var dic_listData = _DictionaryListRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid);
                var data = base.GetPageData(pageDataOptions);

                var result = new PageData<DictionaryEditDto>
                {
                    total = data.total,
                    data = data.data.Select(d => new DictionaryEditDto
                    {
                        dic_id = d.dic_id,
                        dic_name = d.dic_name,
                        db_sql = d.db_sql,
                        dic_no = d.dic_no,
                        parent_id = d.parent_id,
                        enable = d.enable,
                        remark = d.remark,
                        order_no = d.order_no,
                        dictionaryList = dic_listData.Where(s=>s.dic_id == d.dic_id).Select(s => new DictionaryListDto
                        {
                            dic_list_id = s.dic_list_id,
                            dic_name = s.dic_name,
                            dic_value = s.dic_value,
                            order_no = s.order_no,
                            dic_name_eng = s.dic_name_eng,
                            enable = s.enable,
                            remark = s.remark,
                        }).ToList(),
                    }).ToList()
                };
                return WebResponseContent.Instance.OK(_localizationService.GetString("operation_success"), result);
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(this, ex);
                return WebResponseContent.Instance.Error(_localizationService.GetString("operation_failed"));
            }
        }
        public WebResponseContent GetDataById(int id)
        {
            var dic_listData = _DictionaryListRepository.Find(d => d.delete_status == (int)SystemDataStatus.Valid);
            var data = _repository.Find(d => d.delete_status == (int)SystemDataStatus.Valid && d.dic_id == id).Select(d => new DictionaryEditDto
            {
                dic_id = d.dic_id,
                dic_no = d.dic_no,
                dic_name = d.dic_name,
                db_sql = d.db_sql,
                enable = d.enable,
                remark = d.remark,
                parent_id = d.parent_id,
                dictionaryList = dic_listData.Where(x => d.delete_status == (int)SystemDataStatus.Valid && x.dic_id == d.dic_id).Select(d => new DictionaryListDto
                {
                    dic_list_id = d.dic_list_id,
                    dic_name = d.dic_name,
                    dic_name_eng = d.dic_name_eng,
                    dic_value = d.dic_value,
                    order_no = d.order_no,
                    remark = d.remark
                }).ToList()
            }).FirstOrDefault();
            if (data == null) return WebResponseContent.Instance.Error(_localizationService.GetString("record_null"));
            else return WebResponseContent.Instance.OK(_localizationService.GetString("success"), data);
        }

        public async Task<WebResponseContent> GetListByParent()
        {
            var data = (await _repository.FindAsync(d => d.parent_id == 0 &&  d.delete_status == 0, a => new { a.dic_id, a.dic_name })).ToList();
            return WebResponseContent.Instance.OK("Ok", data);
        }
        
    }
}

