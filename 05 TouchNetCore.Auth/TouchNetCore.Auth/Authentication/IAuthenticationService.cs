using System;
using System.Collections.Generic;
using System.Text;

namespace TouchNetCore.Auth.Authentication
{
    public interface IAuthenticationService
    {
        IIdentity Login(string userName, string password);

        void Logout();

        IToken RefreshToken(string refreshToken);

        IIdentity ValidateToken(string accessToken);

    }
}
