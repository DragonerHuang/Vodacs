using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vodace.Core.DBManager;
using Vodace.Core.Utilities.Log4Net;
using Vodace.Entity.DomainModels;

namespace Vodace.Core.Common
{
    /// <summary>
    /// 序号生成类
    /// </summary>
    public class DataSequence
    {
        /// <summary>
        /// 获取序号
        /// </summary>
        /// <param name="prefix">前缀</param>
        /// <param name="contract_no">VO/WO时使用</param>
        /// <returns></returns>
        /// <remarks>
        /// 目前仅支持：QN、CO、VO、WO
        /// </remarks>
        public static string GetSequence(string prefix, string contract_no = null)
        {
            string res = string.Empty;
            string miniYear = DateTime.Now.Year.ToString().Substring(2);
            string number = string.Empty;
            prefix = prefix.ToUpper();
            try
            {
                switch (prefix)
                {
                    case "QN":
                        var qn = DBServerProvider.DbContext.Set<Biz_Quotation>()
                            .Select(s => new { s.qn_no, s.create_date })
                            .OrderByDescending(o => o.create_date)
                            .FirstOrDefault();

                        if (qn != null)
                            number = qn.qn_no.Substring(5);
                        else number = "000";

                        number = (int.Parse(number) + 1).ToString();

                        if (int.Parse(number) < 10)
                            number = "00" + int.Parse(number);
                        if (int.Parse(number) >= 10 && int.Parse(number) < 100)
                            number = "0" + int.Parse(number);

                        res = $"{prefix}-{miniYear}{number}";
                        break;
                    case "CO":
                        var m_co = DBServerProvider.DbContext.Set<Biz_Confirmed_Order>()
                            .Select(s => new { s.co_no, s.create_date })
                            .OrderByDescending(o => o.create_date)
                            .FirstOrDefault();

                        if (m_co != null)
                            number = m_co.co_no.Substring(5);
                        else number = "000";

                        number = (int.Parse(number) + 1).ToString();

                        if (int.Parse(number) < 10)
                            number = "00" + int.Parse(number);
                        if (int.Parse(number) >= 10 && int.Parse(number) < 100)
                            number = "0" + int.Parse(number);

                        res = $"{prefix}{miniYear}-{number}";
                        break;
                    case "VO":
                    case "WO":
                        /*                         
                        一个CO对应一个Contract
                        一个CO可以有多个VO/WO
                         */

                        /*
                        1. 通过M_Contract.Co_Id = M_Confirmed_Order.id
                        2. M_Confirmed_Order.Pro_Id = M_Project.ID
                        3. M_Project.Number = pro_no
                        4. 查询M_Contract和M_Various_Work_Order中Number的最后一条数据
                        
                        var contractQuery = from c in DBServerProvider.DbContext.Set<Biz_Contract>()
                                            join p in DBServerProvider.DbContext.Set<Biz_Project>() on c.project_id equals p.id
                                            where p.project_no == pro_no
                                            select new { c.contract_no, c.create_date };

                        var workOrderQuery = from w in DBServerProvider.DbContext.Set<Biz_Various_Work_Order>()
                                             join co in DBServerProvider.DbContext.Set<Biz_Confirmed_Order>() on w.co_id equals co.id
                                             join p in DBServerProvider.DbContext.Set<Biz_Project>() on co.project_id equals p.id
                                             where p.project_no == pro_no
                                             select new { w.vo_wo_no, w.create_date };

                        var lastContract = contractQuery.OrderByDescending(x => x.create_date).FirstOrDefault();
                        var lastWorkOrder = workOrderQuery.OrderByDescending(x => x.create_date).FirstOrDefault();


                        if (lastContract == null && lastWorkOrder == null)
                        {
                            number = "01";
                        }
                        else if (lastContract != null && lastWorkOrder != null)
                        {
                            if (lastContract.create_date >= lastWorkOrder.create_date)
                                number = lastContract.contract_no.Substring(lastContract.contract_no.IndexOf(prefix) + 3);
                            else
                                number = lastWorkOrder.vo_wo_no.Substring(lastWorkOrder.vo_wo_no.IndexOf(prefix) + 3);
                        }
                        else if (lastContract != null)
                        {
                            number = lastContract.contract_no.Substring(lastContract.contract_no.IndexOf(prefix) + 3);
                        }
                        else if (lastWorkOrder != null)
                        {
                            number = lastWorkOrder.vo_wo_no.Substring(lastWorkOrder.vo_wo_no.IndexOf(prefix) + 3);
                        }

                        number = (int.Parse(number) + 1).ToString();
                        if (int.Parse(number) < 10)
                            number = "0" + int.Parse(number);

                        res = $"{pro_no} {prefix}.{number}";

                        */

                        var list = DBServerProvider.DbContext.Set<Biz_Contract>()
                            .Where(a => a.contract_no == contract_no)
                            .Select(s => new { s.contract_no, s.create_date })
                            .OrderByDescending(o => o.create_date).ToList();
                        if(list.Count > 0)
                        {
                            var strContractNo = list[0].contract_no;
                            var c = strContractNo.Replace(strContractNo, "").Trim();
                            if(string.IsNullOrEmpty(c))
                                number = "00";
                            else
                            {
                                number = c.Replace(prefix + ".", "").Trim();
                                //vac123 vo.12
                            }
                        }
                        else
                        {
                            number = "00";
                        }

                        number = (int.Parse(number) + 1).ToString();
                        if (int.Parse(number) < 10)
                            number = "0" + int.Parse(number);

                        res = $"{contract_no} {prefix}.{number}";

                        break;
                    case "PR":
                        var PR = DBServerProvider.DbContext.Set<Biz_Project>()
                           .Where(p => p.project_no.Length == 8)
                           .Select(s => new { s.project_no, s.create_date })
                           .OrderByDescending(o => o.create_date)
                           .FirstOrDefault();


                        if (PR != null)
                            number = PR.project_no.Substring(5);
                        else number = "000";

                        number = (int.Parse(number) + 1).ToString();

                        if (int.Parse(number) < 10)
                            number = "00" + int.Parse(number);
                        if (int.Parse(number) >= 10 && int.Parse(number) < 100)
                            number = "0" + int.Parse(number);

                        res = $"PR-{miniYear}{number}";

                        break;
                }
            }
            catch(Exception e)
            {
                Log4NetHelper.Error("Vodace.Core.Common.DataSequence.GetSequence", e);
            }

            return res;
        }
    }
}
