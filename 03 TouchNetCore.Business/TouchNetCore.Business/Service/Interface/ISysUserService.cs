using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TouchNetCore.Business.Entity;
using TouchNetCore.Component.Utils.Helper;

namespace TouchNetCore.Business.Service.Interface
{
    public interface ISysUserService
    {
        int AddUser(SysUser sysUser);
        List<SysUser> getSysUserPagination(Pagination pagination);
        List<SysUser> getSysUserPaginationExpression(string userId,Pagination pagination);
    }
}
