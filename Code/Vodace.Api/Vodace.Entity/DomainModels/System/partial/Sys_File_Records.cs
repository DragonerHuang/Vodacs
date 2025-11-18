
using System;

namespace Vodace.Entity.DomainModels
{

    public partial class Sys_File_Records
    {
        //此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
    }

    #region Request

    /// <summary>
    /// 文件下载请求
    /// </summary>
    public class DownLoadDto
    {
        public Guid id { get; set; }
    }

    #endregion


    #region Response

    /// <summary>
    /// 上传文件返回文件信息
    /// </summary>
    public class FileInfoDto
    {
        /// <summary>
        /// id
        /// </summary>
        public Guid? id { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string file_name { get; set; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string file_ext { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int file_size { get; set; }

        /// <summary>
        /// 文件备注
        /// </summary>
        public string file_remark { get; set; }

        /// <summary>
        /// 文件保存相对位置
        /// </summary>
        public string file_relative_path { get; set; }  

    }

    /// <summary>
    /// 文件下载信息
    /// </summary>
    public class FileDownLoadDto
    {
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] file_bytes { get; set; }

        /// <summary>
        /// 获取文件的MIME类型
        /// </summary>
        public string cntent_type { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string file_name { get; set; }
    }

    #endregion

}