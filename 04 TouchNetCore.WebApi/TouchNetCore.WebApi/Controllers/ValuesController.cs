using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TouchNetCore.Business.Entity;
using TouchNetCore.Business.Infrastructure.Repository;
using TouchNetCore.Component.Redis;

namespace TouchNetCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public ValuesController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        IConfiguration Configuration;
        public TouchDbContext touchDbContext { get; set; }
        public Iperson i { get; set; }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            RedisHelper redisHelper = new RedisHelper(0);
            List<test> t = touchDbContext.test.ToList();
            string s = i.ss();
            return new string[] { Configuration["RedisConnectionString"].ToString(), redisHelper.StringGet("1t"), Configuration["envName"].ToString() };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
