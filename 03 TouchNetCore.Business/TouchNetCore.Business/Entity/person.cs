using System;
using System.Collections.Generic;
using System.Text;
using TouchNetCore.Business.Infrastructure.Repository;
using TouchNetCore.Component.Autofac;

namespace TouchNetCore.Business.Entity
{
    public class person : Iperson, ITransientDependency
    {
        public string ss()
        {
            return "1";
        }
    }
}
