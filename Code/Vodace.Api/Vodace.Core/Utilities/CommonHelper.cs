using Dm.util;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vodace.Core.Configuration;
using Vodace.Core.DBManager;
using Vodace.Core.Enums;
using Vodace.Core.ManageUser;
using Vodace.Core.Utilities.PDFHelper;
using Vodace.Entity;
using Vodace.Entity.DomainModels;


namespace Vodace.Core.Utilities
{
    public static class CommonHelper
    {
        /// <summary>
        /// 判断是否中文字符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsChineseChar(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fff\u3400-\u4dbf]");
        }

        public static string GetCompanyCode(string engStr)
        {
            if (engStr.Length > 2)
            {
                var proEx = engStr.Substring(0, 2).ToUpper();
                var data = DBServerProvider.DbContext.Set<Sys_Company>().OrderByDescending(d => d.create_date).Select(d => d.company_no).FirstOrDefault();
                //var Code = data.length() >= 8 ? data.substring(2):"";
                var Code = data != null && data.length() >= 8 ? data.substring(2) : "0";
                int maxCode = Convert.ToInt32(Code) + 1;
                var comCode = proEx + FullString(maxCode);
                return comCode;
            }
            return "";
        }

        private static string FullString(int val)
        {
            int intLenght = val.toString().length();
            switch (intLenght)
            {
                case 1:
                    return "00000" + val;
                case 2:
                    return "0000" + val;
                case 3:
                    return "000" + val;
                case 4:
                    return "00" + val;
                case 5:
                    return "0" + val;
                case 6:
                    return val.toString();
                default:
                    return val.toString();
            }
        }

        public static string GetAuditStatusStr(int status)
        {
            switch (status)
            {
                case (int)AuditEnum.WaitAudit:
                    return "待审核";
                case (int)AuditEnum.UnderReview:
                    return "审核中";
                case (int)AuditEnum.Passed:
                    return "审核通过";
                case (int)AuditEnum.Reject:
                    return "驳回";
                default:
                    return "待审核";
            }

        }

        public static string GetMessageTypeStr(int type)
        {
            switch (type)
            {
                case (int)MessageTypeEnum.Notice:
                    return "通知";
                case (int)MessageTypeEnum.Message:
                    return "消息";
                case (int)MessageTypeEnum.Todo:
                    return "代办";
                default:
                    return "通知";
            }
        }
        public static string GetMessageStatusStr(int status)
        {
            switch (status)
            {
                case (int)MessageStatus.Unread:
                    return "未读";
                case (int)MessageStatus.Read:
                    return "已读";
                case (int)MessageStatus.Processed:
                    return "已处理";
                case (int)MessageStatus.Expected:
                    return "已逾期";
                default:
                    return "通知";
            }
        }

        public static string GetUpcomingEventsStr(int status)
        {
            switch (status)
            {
                case (int)UpcomingEventsEnum.Quotation:
                    return "报价";
                case (int)UpcomingEventsEnum.Project:
                    return "项目";
                case (int)UpcomingEventsEnum.Rolling:
                    return "滚动计划";
                case (int)UpcomingEventsEnum.QnPQ:
                    return "预审";
                case (int)UpcomingEventsEnum.QnPE:
                    return "现场考察";
                case (int)UpcomingEventsEnum.QnTender:
                    return "招标";
                case (int)UpcomingEventsEnum.QnAdvertisement:
                    return "公开招标";
                case (int)UpcomingEventsEnum.QnPQQA:
                    return "预审问答";
                case (int)UpcomingEventsEnum.QnPEI:
                    return "邀请招标";
                case (int)UpcomingEventsEnum.QnTenderQA:
                    return "招标问答";
                case (int)UpcomingEventsEnum.QnTenderInterview:
                    return "面试";
                default:
                    return "";
            }
        }

        public static string GetUpcomingEventsStrEng(int status)
        {
            switch (status)
            {
                case (int)UpcomingEventsEnum.Quotation:
                    return "Quotation";
                case (int)UpcomingEventsEnum.Project:
                    return "Project";
                case (int)UpcomingEventsEnum.Rolling:
                    return "Rolling";
                case (int)UpcomingEventsEnum.QnPQ:
                    return "Pre-qualification";
                case (int)UpcomingEventsEnum.QnPE:
                    return "Sitevisit";
                case (int)UpcomingEventsEnum.QnTender:
                    return "Tender";
                case (int)UpcomingEventsEnum.QnAdvertisement:
                    return "Advertisement";
                case (int)UpcomingEventsEnum.QnPQQA:
                    return "Pre-qualification Q&A";
                case (int)UpcomingEventsEnum.QnPEI:
                    return "Preliminary Enquiry（PEI）";
                case (int)UpcomingEventsEnum.QnTenderQA:
                    return "Tender Q&A";
                case (int)UpcomingEventsEnum.QnTenderInterview:
                    return "Tender Interview";
                default:
                    return "";
            }
        }

        public static UpcomingEventsEnum GetUpcomingEventsEnum(string key)
        {
            switch (key)
            {
                case "Quotation":
                    return UpcomingEventsEnum.Quotation;
                case "Project":
                    return UpcomingEventsEnum.Project;
                case "Rolling":
                    return UpcomingEventsEnum.Rolling;
                case "Pre-qualification":
                    return UpcomingEventsEnum.QnPQ;
                case "Sitevisit":
                    return UpcomingEventsEnum.QnPE;
                case "Tender":
                    return UpcomingEventsEnum.QnTender;
                case "Advertisement":
                    return UpcomingEventsEnum.QnAdvertisement;
                case "Pre-qualification Q&A":
                    return UpcomingEventsEnum.QnPQQA;
                case "Preliminary Enquiry（PEI）":
                    return UpcomingEventsEnum.QnPEI;
                case "Tender Q&A":
                    return UpcomingEventsEnum.QnTenderQA;
                case "Tender Interview":
                    return UpcomingEventsEnum.QnTenderInterview;
                default:
                    return UpcomingEventsEnum.Quotation;
            }
        }


        /// <summary>
        /// 判断文件扩展名是否合法
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="allowExt">扩展名</param>
        /// <returns></returns>
        public static bool ChekcFileExt(string fileName, string[] allowExt)
        {
            var fileExt = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
            if (allowExt.Contains(fileExt))
            {
                return true;
            }
            return false;
        }

        #region 拓展BuildExtendSelectExpre方法

        /// <summary>
        /// 组合继承属性选择表达式树,无拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, TResult>> BuildExtendSelectExpre<TBase, TResult>(this Expression<Func<TBase, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,1个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, TResult>> BuildExtendSelectExpre<TBase, T1, TResult>(this Expression<Func<TBase, T1, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,2个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, TResult>> BuildExtendSelectExpre<TBase, T1, T2, TResult>(this Expression<Func<TBase, T1, T2, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,3个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, TResult>(this Expression<Func<TBase, T1, T2, T3, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,4个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,5个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,6个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="T6">拓展类型6</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, T6, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, T6, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, T6, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, T6, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,7个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="T6">拓展类型6</typeparam>
        /// <typeparam name="T7">拓展类型7</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, T6, T7, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, T6, T7, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,8个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="T6">拓展类型6</typeparam>
        /// <typeparam name="T7">拓展类型7</typeparam>
        /// <typeparam name="T8">拓展类型8</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, TResult>>(expression);
        }

        /// <summary>
        /// 组合继承属性选择表达式树,9个拓展参数
        /// TResult将继承TBase的所有属性
        /// </summary>
        /// <typeparam name="TBase">原数据类型</typeparam>
        /// <typeparam name="T1">拓展类型1</typeparam>
        /// <typeparam name="T2">拓展类型2</typeparam>
        /// <typeparam name="T3">拓展类型3</typeparam>
        /// <typeparam name="T4">拓展类型4</typeparam>
        /// <typeparam name="T5">拓展类型5</typeparam>
        /// <typeparam name="T6">拓展类型6</typeparam>
        /// <typeparam name="T7">拓展类型7</typeparam>
        /// <typeparam name="T8">拓展类型8</typeparam>
        /// <typeparam name="T9">拓展类型9</typeparam>
        /// <typeparam name="TResult">返回类型</typeparam>
        /// <param name="expression">拓展表达式</param>
        /// <returns></returns>
        public static Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> BuildExtendSelectExpre<TBase, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(this Expression<Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> expression)
        {
            return GetExtendSelectExpre<TBase, TResult, Func<TBase, T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>>(expression);
        }

        private static Expression<TDelegate> GetExtendSelectExpre<TBase, TResult, TDelegate>(Expression<TDelegate> expression)
        {
            NewExpression newBody = Expression.New(typeof(TResult));
            MemberInitExpression oldExpression = (MemberInitExpression)expression.Body;

            ParameterExpression[] oldParamters = expression.Parameters.ToArray();
            List<string> existsProperties = new List<string>();
            oldExpression.Bindings.ToList().ForEach(aBinding =>
            {
                existsProperties.Add(aBinding.Member.Name);
            });

            List<MemberBinding> newBindings = new List<MemberBinding>();
            typeof(TBase).GetProperties().Where(x => !existsProperties.Contains(x.Name)).ToList().ForEach(aProperty =>
            {
                if (typeof(TResult).GetMembers().Any(x => x.Name == aProperty.Name))
                {
                    MemberBinding newMemberBinding = null;
                    var valueExpre = Expression.Property(oldParamters[0], aProperty.Name);
                    if (typeof(TBase).IsAssignableFrom(typeof(TResult)))
                    {
                        newMemberBinding = Expression.Bind(aProperty, valueExpre);
                    }
                    else
                    {
                        newMemberBinding = Expression.Bind(typeof(TResult).GetProperty(aProperty.Name), valueExpre);
                    }
                    newBindings.Add(newMemberBinding);
                }
            });

            newBindings.AddRange(oldExpression.Bindings);

            var body = Expression.MemberInit(newBody, newBindings.ToArray());
            var resExpression = Expression.Lambda<TDelegate>(body, oldParamters);

            return resExpression;
        }

        #endregion

        #region 分页及IQueryable

        /// <summary>
        /// 获取分页数据(包括总数量)
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="pageInput">分页参数</param>
        /// <returns></returns>
        public static async Task<PageData<T>> GetPageResultAsync<T>(this IQueryable<T> source, PageInput pageInput)
        {
            try
            {
                int count = await source.CountAsync();
                //int count = await EntityFrameworkQueryableExtensions.CountAsync(source);

                // 应用排序
                if (!string.IsNullOrEmpty(pageInput.sort_field))
                {
                    source = source.OrderByDynamic(pageInput.sort_field, pageInput.sort_type);
                }
                var list = await source
                    .Skip((pageInput.page_index - 1) * pageInput.page_rows)
                    .Take(pageInput.page_rows)
                    .ToListAsync();

                return new PageData<T> { data = list, total = count };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static PageData<T> GetPageResult<T>(this IEnumerable<T> source, PageInput pageInput)
        {
            try
            {
                int count = source.Count();
                var list = source
                    .Skip((pageInput.page_index - 1) * pageInput.page_rows)
                    .Take(pageInput.page_rows)
                    .ToList();

                return new PageData<T> { data = list, total = count };
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// 根据字符串字段名和排序类型动态排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortField">排序字段</param>
        /// <param name="sortType">排序类型：ASC或DESC</param>
        /// <returns></returns>
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string sortField, string sortType = "asc")
        {
            if (string.IsNullOrEmpty(sortField))
                return source;

            // 获取实体类型
            Type type = typeof(T);
            // 获取排序字段的属性信息
            var property = type.GetProperty(sortField, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (property == null)
                return source;

            // 创建参数表达式
            var parameter = Expression.Parameter(type, "x");
            // 创建属性访问表达式
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            // 创建Lambda表达式：x => x.Property
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // 根据排序类型选择OrderBy或OrderByDescending方法
            string methodName = sortType.ToLower().Equals("desc", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";

            // 获取Queryable的OrderBy或OrderByDescending方法
            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new Type[] { type, property.PropertyType },
                source.Expression,
                Expression.Quote(orderByExpression));

            // 返回排序后的IQueryable
            return source.Provider.CreateQuery<T>(resultExpression);
        }

        #endregion

        public static int DiffDays(DateTime date, DateTime? date2 = null)
        {
            try
            {
                //DateTime date2 = DateTime.Now;
                if (date2 == null)
                    date2 = DateTime.Now;

                int days = (date - (DateTime)date2).Days;
                return days;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// 生成包含英文字母和数字的随机数
        /// </summary>
        /// <param name="length">默认获取4位随机数</param>
        /// <returns>包含英文和数字的随机数字符串</returns>
        public static string GenerateRandomDigitString(int length = 4)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string allChars = letters + digits;

            Random random = new Random();
            string result;

            do
            {
                // 创建一个char数组来存储结果
                char[] chars = new char[length];

                // 随机生成每个字符
                for (int i = 0; i < length; i++)
                {
                    chars[i] = allChars[random.Next(allChars.Length)];
                }

                result = new string(chars);

                // 检查结果是否同时包含字母和数字
            } while (!result.Any(c => letters.Contains(c)) || !result.Any(c => digits.Contains(c)));

            return result;
        }

        public static string GetSubmissionVersionStr(int version)
        {
            switch (version)
            {
                case 0:
                    return "--";
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";
                case 6:
                    return "F";
                case 7:
                    return "G";
                case 8:
                    return "H";
                case 9:
                    return "I";
                case 10:
                    return "J";
                case 11:
                    return "K";
                case 12:
                    return "L";
                case 13:
                    return "M";
                case 14:
                    return "N";
                case 15:
                    return "O";
                case 16:
                    return "P";
                case 17:
                    return "Q";
                case 18:
                    return "R";
                case 19:
                    return "S";
                case 20:
                    return "T";
                case 21:
                    return "U";
                case 22:
                    return "V";
                case 23:
                    return "W";
                case 24:
                    return "X";
                case 25:
                    return "Y";
                case 26:
                    return "Z";
                default:
                    return "--";
            }
        }

        public static string GetSubmissionVersionIntStr(int version) 
        {
            if (version < 10) return "0" + version;
            return version.ToString();
        }

        /// <summary>
        /// 获取文件的MIME类型
        /// </summary>
        public static string GetContentType(string filePath)
        {
            var contentType = "application/octet-stream";
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            // 根据文件扩展名设置MIME类型
            var mimeTypes = new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/msword"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".zip", "application/zip"}
            };

            if (mimeTypes.ContainsKey(extension))
            {
                contentType = mimeTypes[extension];
            }

            return contentType;
        }

        /// <summary>
        /// 转换pdf
        /// </summary>
        /// <param name="strInputPath"></param>
        /// <returns></returns>
        public static string ChangePdf(string strInputPath)
        {

            var strPdfFolder = Path.Combine(AppSetting.FileSaveSettings.TemporaryFolder, $"{UserContext.Current.UserId}\\{DateTime.Now.ToString("yyyMMddHHmmss")}\\");
            if (!Directory.Exists(strPdfFolder))
            {
                Directory.CreateDirectory(strPdfFolder);
            }
            var strPdfPath = Path.Combine(strPdfFolder, $"{DateTime.Now.ToString("yyyMMddHHmmss")}.pdf");
            var strInputCopyPath = Path.Combine(strPdfFolder, Path.GetFileName(strInputPath));
            try
            {
                File.Copy(strInputPath, strInputCopyPath, true);


                //var bolOk = await _IOfficeConversionService.ConvertToPdfAsync(strInputCopyPath, strPdfPath);

                var bolOk = Vodace.Core.Utilities.PDFHelper.PDFHelper.ConvertToPdfUsingLibreOffice(strInputCopyPath, strPdfPath);
                return bolOk ? strPdfPath : string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (File.Exists(strInputCopyPath) && strInputCopyPath.startsWith(AppSetting.FileSaveSettings.TemporaryFolder))
                {
                    File.Delete(strInputCopyPath);
                }
            }
        }

        // 根据文件扩展名获取正确的 MIME 类型
        public static string GetMimeType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return "application/octet-stream";

            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();

            return extension switch
            {
                // 图片类型
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                ".ico" => "image/x-icon",

                // 文档类型
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".ppt" => "application/vnd.ms-powerpoint",
                ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",

                // 文本类型
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                ".html" => "text/html",
                ".css" => "text/css",
                ".js" => "application/javascript",
                ".json" => "application/json",

                // 压缩文件
                ".zip" => "application/zip",
                ".rar" => "application/x-rar-compressed",

                _ => "application/octet-stream"
            };
        }
    }
}
