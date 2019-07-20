using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using TouchNetCore.Component.Autofac;

namespace TouchNetCore.Auth.Security.Session
{
    public class ClaimsSession : IClaimsSession, ISingletonDependency
    {
        protected virtual string ReadClaim(string claimType)
        {
            if (!(Thread.CurrentPrincipal is ClaimsPrincipal claimsPrincipal))
            {
                return null;
            }

            if (!(claimsPrincipal.Identity is ClaimsIdentity claimsIdentity))
            {
                return null;
            }

            var claim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == claimType);
            if (claim == null || string.IsNullOrEmpty(claim.Value))
            {
                return null;
            }
            return claim.Value;
        }

        protected virtual Nullable<T> ReadClaim<T>(string claimType) where T : struct
        {
            var value = ReadClaim(claimType);
            if (value == null)
                return null;
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken => ReadClaim(ClaimTypes.AccessToken);

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId => ReadClaim(ClaimTypes.UserId);

        /// <summary>
        /// 用户名(账号)
        /// </summary>
        public string UserName => ReadClaim(ClaimTypes.UserName);

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName => ReadClaim(ClaimTypes.RealName);

        /// <summary>
        /// 手机号码
        /// </summary>
        public string MobilePhone => ReadClaim(ClaimTypes.MobilePhone);

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string Email => ReadClaim(ClaimTypes.Email);

    }
}
