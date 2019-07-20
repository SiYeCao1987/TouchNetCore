using log4net.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TouchNetCore.WebApi.MiddleWare.Log
{
    /// <summary>
    /// 指定log4net需要的Repository
    /// </summary>
    public class InitRepository
    {
        public static ILoggerRepository LoggerRepository { get; set; }
    }
}
