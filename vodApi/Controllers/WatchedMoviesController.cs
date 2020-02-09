using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vod.Core.Boundary.Interfaces;
using vod.Core.Boundary.Model;

namespace vodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchedMoviesController : ControllerBase
    {
        private ICoreLogic _coreLogic;

        public WatchedMoviesController(ICoreLogic coreLogic)
        {
            _coreLogic = coreLogic;
        }

        [HttpPost]
        [Authorize]
        public void Post(WatchedMovie movie)
        {
            movie.Username = User.FindFirst(ClaimTypes.Name)?.Value;
            _coreLogic.AddAlreadyWatchedMovie(movie);
        }
    }
}