using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouchNetCore.Component.Utils.Helper;
using TouchNetCore.Component.Utils.Result;

namespace TouchNetCore.WebApi.MiddleWare.ExceptionHandle
{
    public static class ExceptionHandlingExtensions
    {
        public static void UseMyExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(builder => {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = ApiResultType.Fail;
                    context.Response.ContentType = "application/json";
                    var ex = context.Features.Get<IExceptionHandlerFeature>();
                    ApiResult apiResult = new ApiResult();
                    apiResult.Code = ApiResultType.Fail;
                    apiResult.Message = ex?.Error?.Message;
                    if (ex != null)
                    {
                        //记录日志
                        var logger = loggerFactory.CreateLogger("TouchNetCore.WebApi.MiddleWare.ExceptionHandle.ExceptionHandlingExtensions");
                        logger.LogDebug(500, ex.Error, ex.Error.Message);
                    }
                    await context.Response.WriteAsync(apiResult.ToJson());
                });
            });
        }
    }
}
