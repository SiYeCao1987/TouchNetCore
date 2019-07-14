using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace TouchNetCore.Auth.Authentication
{
    public class BearerOptions : AuthenticationSchemeOptions
    {
        public BearerOptions()
        {
            ClaimsIssuer = "My Self!!!";
        }

        public bool RequireHttpsMetadata { get; internal set; }
    }
}
