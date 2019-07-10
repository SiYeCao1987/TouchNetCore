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
        [Key]
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
