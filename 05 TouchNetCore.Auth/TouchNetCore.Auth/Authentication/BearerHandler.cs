using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;

namespace TouchNetCore.Auth.Authentication
{
    public class BearerHandler : AuthenticationHandler<BearerOptions>
    {
        private const string KEY_AUTHORIZATION = "authorization";
        private readonly IAuthenticationService authService;

        public BearerHandler(
            IAuthenticationService authService,
            IOptionsMonitor<BearerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock
            )
            : base(options, logger, encoder, clock)
        {
            var type = typeof(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerHandler);
            this.authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Thread.CurrentPrincipal != null)
            {
                //这个地方特别留意：如果不将现有的Thread.CurrentPrincipal设为null，当令牌为空或过期时，将会取到错误的Session。
                Thread.CurrentPrincipal = null;
            }
            string accessToken = Request.Headers[KEY_AUTHORIZATION];
            if (string.IsNullOrEmpty(accessToken))
            {
                accessToken = Request.Query[KEY_AUTHORIZATION];
            }
            if (string.IsNullOrEmpty(accessToken))
            {
                if (Request.Method != "OPTIONS")
                    Logger.LogInformation("请求头authorization为空，目标路径：" + Request.Path);
                return AuthenticateResult.NoResult();
            }
            this.Logger.LogDebug("accessToken:" + accessToken);
            IIdentity identity;
            try
            {
                identity = authService.ValidateToken(accessToken);
                System.Diagnostics.Debug.Assert(identity?.AccessToken == accessToken);
            }
            catch (Exception ex)
            {
                this.Logger.LogDebug(accessToken + " validate failed: " + ex.Message);
                return AuthenticateResult.Fail(ex.Message);
            }
            if (identity == null) //如果验证失败(例如令牌无效或是过期)，不要返回错误，返回无结果就行了。
            {
                //因为有可能发生这样的场景：前端在访问登录接口时附上了过期的令牌，但应该放行，只要不给Thread.CurrentPrincipal赋值就行了，因为后面还会有权限校验。
                return AuthenticateResult.NoResult();
            }
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, identity.UserName),
                new Claim(ClaimTypes.NameIdentifier, identity.UserIdentity.ToString()),
                new Claim(Security.ClaimTypes.AccessToken, identity.AccessToken)
            };
            if (identity.Role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, identity.Role));
            }
            if (identity.AdditionalClaims != null)
            {
                foreach (var item in identity.AdditionalClaims)
                {
                    if (item.Value != null)
                        claims.Add(new Claim(item.Key, item.Value));
                }
            }
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));
            var validatedContext = new BearerValidatedContext(Context, Scheme, Options)
            {
                Principal = principal,
                //SecurityToken = validatedToken
            };
            validatedContext.Success();
            Thread.CurrentPrincipal = principal;
            var result = validatedContext.Result;
            this.Logger.LogDebug(accessToken + " is valid! user name: " + identity.UserName);
            return result;
            //foreach (var validator in Options.SecurityTokenValidators)
        }
    }
}
