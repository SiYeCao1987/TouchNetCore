using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TouchNetCore.Component.Utils.Result
{
    /// <summary>
    /// Api响应枚举
    /// </summary>
    public class ApiResultType
    {
        /// <summary>
        /// 请求成功
        /// </summary>
        public const int Success = 1000;

        /// <summary>
        /// 请求失败
        /// </summary>
        public const int Fail = 1001;

        /// <summary>
        /// 请求参数异常
        /// </summary>
        public const int ParamError = 1002;
    }
}
