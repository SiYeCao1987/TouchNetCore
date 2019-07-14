using System;
using System.Collections.Generic;
using System.Text;

namespace TouchNetCore.Auth.Authentication
{
    /// <summary>
    /// 登录令牌
    /// </summary>
    public class SimpleToken : IToken
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// 令牌类型(默认为Bearer)
        /// </summary>
        public virtual string TokenType { get; set; } = "bearer";

        /// <summary>
        /// 令牌有效时长(秒)
        /// </summary>
        public virtual int ExpiresIn { get; set; }

        /// <summary>
        /// 令牌颁发时间
        /// </summary>
        public virtual DateTime IssueTime { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public virtual string RefreshToken { get; set; }

        /// <summary>
        /// 令牌失效时间
        /// </summary>
        public virtual DateTime ExpireTime => IssueTime.AddSeconds(ExpiresIn);

        /// <summary>
        /// 无参构造函数(用于json序列化/反序列化)
        /// </summary>
        public SimpleToken()
        {
            this.IssueTime = DateTime.Now;
        }

        /// <summary>
        /// 创建一个新的令牌
        /// </summary>
        /// <param name="access_token">访问令牌</param>
        /// <param name="refresh_token">刷新令牌</param>
        /// <param name="expires_in">令牌有效时间(秒)</param>
        public SimpleToken(string access_token, string refresh_token, int expires_in) : this()
        {
            this.AccessToken = access_token;
            this.RefreshToken = refresh_token;
            this.ExpiresIn = expires_in;
            this.IssueTime = DateTime.Now;
        }

        public static SimpleToken NewToken(int expires_in)
        {
            var accessToken = Guid.NewGuid().ToString("N");
            var refreshToken = Guid.NewGuid().ToString("N");
            var token = new SimpleToken(accessToken, refreshToken, expires_in);
            return token;
        }
    }
}
