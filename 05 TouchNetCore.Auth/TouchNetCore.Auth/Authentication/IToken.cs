using System;
using System.Collections.Generic;
using System.Text;

namespace TouchNetCore.Auth.Authentication
{
    /// <summary>
    /// 登录令牌接口
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        string AccessToken { get; }

        /// <summary>
        /// 令牌类型(默认为Bearer)
        /// </summary>
        string TokenType { get; }

        /// <summary>
        /// 令牌有效时长(秒)
        /// </summary>
        int ExpiresIn { get; }

        /// <summary>
        /// 令牌颁发时间
        /// </summary>
        DateTime IssueTime { get; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        string RefreshToken { get; }

        /// <summary>
        /// 令牌失效时间
        /// </summary>
        DateTime ExpireTime { get; }
    }
}
