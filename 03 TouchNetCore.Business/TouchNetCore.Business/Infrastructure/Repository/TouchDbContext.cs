using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TouchNetCore.Business.Entity;
using TouchNetCore.Component.Autofac;

namespace TouchNetCore.Business.Infrastructure.Repository
{
    /// <summary>
    /// 数据库上下文
    /// </summary>
    public class TouchDbContext : DbContext, ITransientDependency
    {
        public TouchDbContext(DbContextOptions<TouchDbContext> options)
            : base(options)
        {
        }
        public DbSet<test> test { get; set; }
    }
}
