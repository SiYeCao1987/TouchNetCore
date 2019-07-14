using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace TouchNetCore.Auth.Authentication
{
    public class BearerValidatedContext : ResultContext<BearerOptions>
    {
        public BearerValidatedContext(HttpContext context, AuthenticationScheme scheme, BearerOptions options)
            : base(context, scheme, options)
        {
            
        }
    }
}
