using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TouchNetCore.Auth;
using TouchNetCore.Auth.Authentication;
using TouchNetCore.Auth.Security.Session;
using TouchNetCore.Business.Dto;
using TouchNetCore.Component.Utils.Helper;
using TouchNetCore.Component.Utils.Result;

namespace TouchNetCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IAuthenticationService authService { get; set; }
        public IClaimsSession Session { get; set; }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="input">登录实体</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public ActionResult Login([FromBody]LoginInputDto input)
        {
            ApiResult apiResult = new ApiResult();
            IIdentity identity;
            try
            {
                identity = authService.Login(input.UserName, input.PassWord);
            }
            catch (Exception ex)
            {
                apiResult.Code = ApiResultType.Fail;
                apiResult.Message = ex.Message;
                return Content(apiResult.ToJson());
            }
            if (identity == null)
            {
                apiResult.Code = ApiResultType.Fail;
                apiResult.Message = "用户名称或者密码错误";
               return Content(apiResult.ToJson());
            }
            if (!string.IsNullOrEmpty(Session.UserId))
            {
                authService.Logout();
            }
            apiResult.Code = ApiResultType.Success;
            apiResult.Message = "登录成功";
            apiResult.Data = identity;
            return Content(apiResult.ToJson());
        }
    }
}