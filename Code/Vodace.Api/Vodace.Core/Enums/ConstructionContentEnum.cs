using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Enums
{
    public enum ConstructionContentEnum
    {

    }


    /// <summary>
    /// 工程内容枚举
    /// </summary>
    /// <remarks>与Sys_Construction_Content_Init表work_type对应</remarks>
    public class ConstructionContentWorkTypeEnum
    {
        public static string PreWork = "Pre Work";

        public static string SiteWork = "Site Work";

        public static string SiteSurvey = "Site Survey";

        public static string SubCWork = "Sub-C.Work";

        public static string T_C = "T&C";

        public static string O_M = "O&M";
    }

    /// <summary>
    /// 工程内容枚举
    /// </summary>
    /// <remarks>与Sys_Construction_Content_Init表point_type对应</remarks>
    public class ConstructionContentPointTypeEnum
    {
        public static string CheckPoint = "Check Point";

        public static string WholePoint = "Whole Point";

        public static string DailyPoint = "Daily Point";
    }

}
