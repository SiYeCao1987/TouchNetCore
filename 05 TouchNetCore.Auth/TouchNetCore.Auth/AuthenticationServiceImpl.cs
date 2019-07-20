using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchNetCore.Auth.Authentication;
using TouchNetCore.Auth.Security.Session;
using TouchNetCore.Business.Entity;
using TouchNetCore.Business.Repository;
using TouchNetCore.Component.Autofac;

namespace TouchNetCore.Auth
{
    /// <summary>
    /// 认证服务实现
    /// </summary>
    public class AuthenticationServiceImpl : IAuthenticationService, ITransientDependency
    {
        class User
        {
            internal int Id;
            internal string UserName;
            internal string Password;
            internal User(int id, string name, string pw) { Id = id; UserName = name; Password = pw; }
        }

        private readonly static List<Identity> identities = new List<Identity>();
        private readonly IClaimsSession Session;
        private readonly List<User> users = new List<User>
        {
            new User(1, "admin", "123456"),
            new User(2, "user", "123" ),
            new User(3, "tester", "123")
        };

        public ISysUserRepository<SysUser> sysUserRepository { get; set; }

        public AuthenticationServiceImpl(IClaimsSession session)
        {
            this.Session = session;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IIdentity Login(string userName, string passWord)
        {
            decimal i = 0;
            decimal a = 1 / i;
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(passWord))
                throw new ArgumentNullException(nameof(passWord));
            var sysUser = sysUserRepository.FindEntity(u => u.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase) && u.PassWord == passWord);
            if (sysUser == null)
            {
                return null;
            }
            var token = SimpleToken.NewToken(60);
            var identity = new Identity(sysUser.UserId, sysUser.UserName, token);
            identities.Add(identity);
            return identity;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void Logout()
        {
            var identity = identities.FirstOrDefault(p => p.UserId == Session.UserId && p.AccessToken == Session.AccessToken);
            if (identity != null)
            {
                identity.Invalid = true;
            }
        }

        /// <summary>
        /// 刷新token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public IToken RefreshToken(string refreshToken)
        {
            var identity = identities.FirstOrDefault(p => p.AccessToken == Session.AccessToken && p.RefreshToken == refreshToken && !p.Invalid);
            if (identity == null)
            {
                throw new Exception("令牌刷新失败！请确认提供的令牌是否有效。");
            }
            var token = SimpleToken.NewToken(identity.ExpiresIn);
            identity.Merge(token);
            return token;
        }

        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public IIdentity ValidateToken(string accessToken)
        {
            var identity = identities.FirstOrDefault(p => p.AccessToken == accessToken && !p.Invalid);
            return identity;
        }

    }
}
