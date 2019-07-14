using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchNetCore.Auth.Authentication;
using TouchNetCore.Auth.Security.Session;
using TouchNetCore.Component.Autofac;

namespace TouchNetCore.Auth
{
    public class SampleAuthenticationService : IAuthenticationService, ITransientDependency
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

        public SampleAuthenticationService(IClaimsSession session)
        {
            this.Session = session;
        }

        public IIdentity Login(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
                throw new ArgumentNullException(nameof(userName));
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            var user = users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase) && u.Password == password);
            if (user == null)
                return null;

            var token = SimpleToken.NewToken(60);
            var identity = new Identity(user.Id, user.UserName, token);
            identities.Add(identity);
            return identity;
        }

        public void Logout()
        {
            var identity = identities.FirstOrDefault(p => p.UserId == Session.UserId && p.AccessToken == Session.AccessToken);
            if (identity != null)
                identity.Invalid = true;
        }

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

        public IIdentity ValidateToken(string accessToken)
        {
            var identity = identities.FirstOrDefault(p => p.AccessToken == accessToken && !p.Invalid);
            return identity;
        }

    }
}
