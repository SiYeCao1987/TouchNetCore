using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TouchNetCore.Business.Dto
{
    public class LoginInputDto
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Required]
        [Description("用户名称")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string PassWord { get; set; }

    }
}
