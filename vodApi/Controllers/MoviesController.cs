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
        private ICoreLogic _core;

        public MoviesController(ICoreLogic core)
        {
            _core = core;
        }

        [HttpGet]
        public JsonResult Get(int filmType)
        {
            return new JsonResult(_core.GetResults((MovieTypes)filmType));
        }
    }
}