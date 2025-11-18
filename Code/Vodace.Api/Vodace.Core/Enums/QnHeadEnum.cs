using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    public static class QnHeadTypeHelper
    {
        public static string GetQnHeadTypeStr(string key)
        {
            switch (key)
            {
                case "Tender Document":
                    return "招标文件";
                case "Sitevisit":
                    return "工地考察";
                case "Quotation":
                    return "报价";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 根据期限类型获取负责人类型
        /// 合同资料、预审问答（预审资格、预审资格问答）、投标问答、招标面试是招标（Tender Document）
        /// 标书资料（提交文件那是负责完成）是报价（Quotation）
        /// 工地考察是工地考察（Sitevisit）
        /// </summary>
        /// <param name="deadlineType"></param>
        /// <returns></returns>
        public static string GetDeadlineTypeByHeadlerType(string deadlineType)
        {
            switch (deadlineType)
            {
                case "Pre-qualification":
                    return "Tender Document";
                case "Sitevisit":
                    return "Sitevisit";
                case "Tender":
                    return "Quotation";
                case "Advertisement": 
                    return "Tender Document";
                case "Pre-qualification Q&A":
                    return "Tender Document";
                case "Preliminary Enquiry（PEI）":
                    return "Tender Document";
                case "Tender Q&A":
                    return "Tender Document";
                case "Tender Interview":
                    return "Tender Document";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 根据事件类型获取负责人类型
        /// 合同资料、预审问答（预审资格、预审资格问答）、投标问答、招标面试是招标（Tender Document）
        /// 标书资料（提交文件那是负责完成）是报价（Quotation）
        /// 工地考察是工地考察（Sitevisit）
        /// </summary>
        /// <param name="upcomingEventsEnum"></param>
        /// <returns></returns>
        public static string GetQnHeadTypeByDeadlineType(UpcomingEventsEnum upcomingEventsEnum)
        {
            switch (upcomingEventsEnum)
            {
                case UpcomingEventsEnum.Quotation:
                    return "";
                case UpcomingEventsEnum.Project:
                    return "";
                case UpcomingEventsEnum.Rolling: 
                    return "";
                case UpcomingEventsEnum.QnPQ:
                    return "Tender Document";
                case UpcomingEventsEnum.QnPE:
                    return "Sitevisit";
                case UpcomingEventsEnum.QnTender:
                    return "Quotation";
                case UpcomingEventsEnum.QnAdvertisement:
                    return "Tender Document";
                case UpcomingEventsEnum.QnPQQA:
                    return "Tender Document"; 
                case UpcomingEventsEnum.QnPEI:
                    return "Tender Document";
                case UpcomingEventsEnum.QnTenderQA:
                    return "Tender Document";
                case UpcomingEventsEnum.QnTenderInterview:
                    return "Tender Document";
                default:
                    return "";
            }
        }

    }
}
