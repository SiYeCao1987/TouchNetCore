using System;
using System.Collections.Generic;
using System.Text;

namespace TouchNetCore.Auth.Security
{
    /// <summary>
    /// 定义已知的常用声明类型。此类允许继承以方便追加自定义类型。
    /// </summary>
    public class ClaimTypes //不要添加static或sealed
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public const string AccessToken = "";

        /// <summary>
        /// 用户ID
        /// </summary>
        public const string UserId = System.Security.Claims.ClaimTypes.NameIdentifier;

        /// <summary>
        /// 用户名(账号)
        /// </summary>
        public const string UserName = System.Security.Claims.ClaimTypes.Name;

        /// <summary>
        /// 真实姓名
        /// </summary>
        public const string RealName = "";

        /// <summary>
        /// 手机号码
        /// </summary>
        public const string MobilePhone = System.Security.Claims.ClaimTypes.MobilePhone;

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public const string Email = System.Security.Claims.ClaimTypes.Email;
    }
}
