using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Models;

namespace vodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchedMoviesController : ControllerBase
    {
        private readonly IAlreadyWatchedFilmService _alreadyWatchedFilmService;

        public WatchedMoviesController(IAlreadyWatchedFilmService alreadyWatchedFilmService)
        {
            _alreadyWatchedFilmService = alreadyWatchedFilmService;
        }

        [HttpPost]
        [Authorize]
        public void Post(AlreadyWatchedMovie movie)
        {
            movie.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _alreadyWatchedFilmService.Add(movie);
        }

        [HttpDelete]
        [Authorize]
        public void Delete(int idToDelete)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _alreadyWatchedFilmService.RemoveAt(idToDelete, userId);
        }
    }
}