using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize]
        public JsonResult Get(int filmType)
        {
            return new JsonResult(_core.GetResultsUsingStorage((MovieTypes)filmType));
        }
    }
}