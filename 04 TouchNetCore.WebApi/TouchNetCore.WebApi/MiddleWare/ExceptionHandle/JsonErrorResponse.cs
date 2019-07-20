using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TouchNetCore.WebApi.MiddleWare.ExceptionHandle
{
    public class JsonErrorResponse
    {
        /// <summary>
        /// 生产环境的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 开发环境的消息
        /// </summary>
        public string DevelopmentMessage { get; set; }
    }
}
