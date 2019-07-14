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
        /// {"userName":"user","password":"123"}
        /// </remarks>
        /// <param name="input">登录实体</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(Identity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult Login([FromBody]LoginInputDto input)
        {
            IIdentity identity;
            try
            {
                identity = authService.Login(input.UserName, input.Password);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            if (identity == null)
                return Unauthorized("登录失败！请检查用户名及密码是否正确。");
            if (Session.UserId.HasValue)
                authService.Logout();
            
            return Ok(identity);
        }
    }
}