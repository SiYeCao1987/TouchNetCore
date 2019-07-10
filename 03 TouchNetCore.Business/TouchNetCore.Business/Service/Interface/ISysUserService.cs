using System;
using System.Collections.Generic;
using System.Text;
using TouchNetCore.Business.Entity;

namespace TouchNetCore.Business.Service.Interface
{
    public interface ISysUserService
    {
        int AddUser(SysUser sysUser);
    }
}
