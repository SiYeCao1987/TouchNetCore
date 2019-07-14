using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TouchNetCore.Business.Entity;
using TouchNetCore.Business.Repository;
using TouchNetCore.Business.Service.Interface;
using TouchNetCore.Component.Autofac;
using TouchNetCore.Component.Utils.Helper;

namespace TouchNetCore.Business.Service.Impl
{
    public class SysUserServiceImpl : ISysUserService, ITransientDependency
    {
        public ISysUserRepository<SysUser> sysUserRepository { get; set; }
        public int AddUser(SysUser sysUser)
        {
            return sysUserRepository.Insert(sysUser);
        }

        public List<SysUser> getSysUserPagination(Pagination pagination)
        {
            return sysUserRepository.FindList(pagination);
        }

        public List<SysUser> getSysUserPaginationExpression(string userId, Pagination pagination)
        {
            var express = ExtLinq.True<SysUser>();
            express = express.And(t => t.UserId == userId);
            return sysUserRepository.FindList(express, pagination);
        }
    }
}
