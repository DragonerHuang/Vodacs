using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Entity
{
    #region 分页实体类

    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageData<T>
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public List<T> data { get; set; }
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageInput<T> : PageInput where T : new()
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        public T search { get; set; } = new T();
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    public class PageInput
    {
        private string _sortType { get; set; } = "asc";

        /// <summary>
        /// 当前页码
        /// </summary>
        public int page_index { get; set; } = 1;

        /// <summary>
        /// 每页行数
        /// </summary>
        public int page_rows { get; set; } = int.MaxValue;

        /// <summary>
        /// 排序列
        /// </summary>
        public string sort_field { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public string sort_type { get => _sortType; set => _sortType = (value ?? string.Empty).ToLower().Contains("desc") ? "desc" : "asc"; }
    }

    #endregion
}
