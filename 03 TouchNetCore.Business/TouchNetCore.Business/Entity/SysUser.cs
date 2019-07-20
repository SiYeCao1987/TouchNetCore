using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TouchNetCore.Business.Entity
{
    [Table("Sys_User")]
    public class SysUser
    {
        /// <summary>
        /// 系统用户id
        /// </summary>
        [Key]
        public string UserId { get; set; }

        /// <summary>
        /// 系统用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 系统用户密码
        /// </summary>
        public string PassWord { get; set; }
    }
}
