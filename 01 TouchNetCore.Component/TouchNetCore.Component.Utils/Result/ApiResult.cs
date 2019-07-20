using System;
using System.Collections.Generic;
using System.Text;

namespace TouchNetCore.Component.Utils.Result
{
    /// <summary>
    /// api接口响应结果
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 实体数据
        /// </summary>
        public object Data { get; set; }
    }
}
