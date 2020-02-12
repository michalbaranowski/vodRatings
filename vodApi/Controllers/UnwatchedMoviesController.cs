using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vod.Core.Boundary.Interfaces;
using vod.Core.Boundary.Model;

namespace vodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnwatchedMoviesController : ControllerBase
    {
        private ICoreLogic _coreLogic;

        public UnwatchedMoviesController(ICoreLogic coreLogic)
        {
            _coreLogic = coreLogic;
        }

        [HttpPost]
        [Authorize]
        public void Post(WatchedMovie movie)
        {
            movie.Username = User.FindFirst(ClaimTypes.Name)?.Value;
            _coreLogic.RemoveAlreadyWatchedMovie(movie);
        }
    }
}