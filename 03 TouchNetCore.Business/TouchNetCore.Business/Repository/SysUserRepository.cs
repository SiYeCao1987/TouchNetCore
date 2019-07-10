using System;
using System.Collections.Generic;
using System.Text;
using TouchNetCore.Business.Entity;
using TouchNetCore.Business.Infrastructure.Repository;
using TouchNetCore.Component.Autofac;

namespace TouchNetCore.Business.Repository
{
    public class SysUserRepository : BaseRepository<SysUser>, ISysUserRepository<SysUser>, ITransientDependency
    {

    }
}
