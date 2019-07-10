using System;
using System.Collections.Generic;
using System.Text;
using TouchNetCore.Business.Infrastructure.Repository;

namespace TouchNetCore.Business.Repository
{
    public interface ISysUserRepository<T> : IBaseRepository<T> where T : class, new()
    {

    }
}
