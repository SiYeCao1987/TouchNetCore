using System;
using System.Collections.Generic;
using System.Text;
using TouchNetCore.Auth.Authentication;

namespace TouchNetCore.Auth
{
    /// <summary>
    /// 身份证明
    /// </summary>
    public class Identity : IIdentity//<TPrimaryKey> where TPrimaryKey: struct
    {
        //Identity类中不设主键字段，因为那是数据库层面才考虑的事
        //public virtual TPrimaryKey Id { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public virtual string UserIdentity => UserId.ToString();

        public virtual string UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public virtual string Role { get; set; }

        /// <summary>
        /// 访问令牌
        /// </summary>
        public virtual string AccessToken { get; set; }

        /// <summary>
        /// 颁发时间
        /// </summary>
        public virtual DateTime IssueTime { get; internal set; }

        /// <summary>
        /// 令牌有效时长(秒)
        /// </summary>
        public virtual int ExpiresIn { get; set; }

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public virtual string RefreshToken { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        /// <summary>
        /// 是否已失效
        /// </summary>
        public bool Invalid { get; set; }

        /// <summary>
        /// 其他Claims 供扩展使用
        /// </summary>
        public Dictionary<string, string> AdditionalClaims { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 合并Token令牌到当前身份证
        /// </summary>
        /// <param name="token"></param>
        public void Merge(IToken token)
        {
            AccessToken = token.AccessToken;
            RefreshToken = token.RefreshToken;
            ExpiresIn = token.ExpiresIn;
            IssueTime = token.IssueTime;
        }

        public Identity()
        {
        }

        public Identity(string userId, string userName, IToken token)
        {
            UserId = userId;
            UserName = userName;
            Merge(token);
        }

    }
}
