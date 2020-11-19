using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using TestService.Data;
using TestService.Services.Interfaces;

namespace TestService.Controllers
{
    [ApiController]
    [Route("Api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public TestController(ICacheService cacheService, ApplicationDbContext context)
        {
            _cacheService = cacheService;
        }

        public ActionResult<String> TestCache(string userName)
        {
            var sessionId = Guid.NewGuid().ToString();
            _cacheService.Set<string>(sessionId, userName);

            return sessionId;
        }
    }
}