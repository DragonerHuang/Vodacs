using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Vodace.Entity.DomainModels
{
    public class LoginInfo
    {

        public string Com_No { get; set; }
        [Display(Name = "用户名")]
        [MaxLength(50)]
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }
        [MaxLength(50)]
        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        [MaxLength(6)]
        [Display(Name = "验证码")]
        [Required(ErrorMessage = "验证码不能为空")]
        public string VerificationCode { get; set; }
        [Required(ErrorMessage = "参数不完整")]
        /// <summary>
        /// 2020.06.12增加验证码
        /// </summary>
        public string UUID { get; set; }
        [Display(Name = "来源（0：平台；1：APP）")]
        [Required(ErrorMessage = "登录来源不能为空")]
        public int source { get; set; }
    }

    public class UserPwdInfo
    {
        public int user_id { get; set; }
        public string password { get; set; }
    }

    public class PwdInfo
    {
        public string old_pwd { get; set; }
        public string new_pwd { get; set; }
    }

    public class SwitchLangDto 
    {
        public int user_id { get; set; }
        public int lang { get; set; }
    }

    public class LoginInfoNew
    {
        [Display(Name = "用户名")]
        [MaxLength(50)]
        [Required(ErrorMessage = "用户名不能为空")]
        public string Account { get; set; }
        [MaxLength(50)]
        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        [MaxLength(6)]
        [Display(Name = "验证码")]
        [Required(ErrorMessage = "验证码不能为空")]
        public string VerificationCode { get; set; }
        [Required(ErrorMessage = "参数不完整")]
        /// <summary>
        /// 验证码
        /// </summary>
        public string UUID { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public Guid Com_Id { get; set; }
    }
}
