using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using vod.Core.Boundary;
using vod.Core.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ICoreLogic _core;

        public MoviesController(ICoreLogic core)
        {
            _core = core;
        }

        [HttpGet]
        public JsonResult Get(int filmType)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var results = _core.GetResultsUsingStorage((MovieTypes)filmType, username);
            return new JsonResult(results);
        }
    }
}