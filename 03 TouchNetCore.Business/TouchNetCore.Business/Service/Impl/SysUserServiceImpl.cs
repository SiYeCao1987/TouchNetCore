using System;
using System.Collections.Generic;
using System.Text;
using TouchNetCore.Business.Entity;
using TouchNetCore.Business.Repository;
using TouchNetCore.Business.Service.Interface;
using TouchNetCore.Component.Autofac;

namespace TouchNetCore.Business.Service.Impl
{
    public class SysUserServiceImpl : ISysUserService, ITransientDependency
    {
        public ISysUserRepository<SysUser> sysUserRepository { get; set; }
        public int AddUser(SysUser sysUser)
        {
            return sysUserRepository.Insert(sysUser);
        }
    }
}
