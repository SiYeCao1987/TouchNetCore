using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace TouchNetCore.Auth.Authentication
{
    public static class BearerExtensions
    {
        public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder)
            => builder.AddBearer(BearerDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder, Action<BearerOptions> configureOptions)
            => builder.AddBearer(BearerDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder, string authenticationScheme, Action<BearerOptions> configureOptions)
            => builder.AddBearer(authenticationScheme, displayName: null, configureOptions: configureOptions);

        public static AuthenticationBuilder AddBearer(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<BearerOptions> configureOptions)
        {
            //builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<BearerOptions>, BearerPostConfigureOptions>());
            return builder.AddScheme<BearerOptions, BearerHandler>(authenticationScheme, displayName, configureOptions);
        }
    }
}
