using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Models;

namespace vodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnwatchedMoviesController : ControllerBase
    {
        private readonly IAlreadyWatchedFilmService _alreadyWatchedFilmService;

        public UnwatchedMoviesController(IAlreadyWatchedFilmService alreadyWatchedFilmService)
        {
            _alreadyWatchedFilmService = alreadyWatchedFilmService;
        }

        [HttpPost]
        [Authorize]
        public void Post(AlreadyWatchedMovie movie)
        {
            movie.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _alreadyWatchedFilmService.RemoveAt(movie.MovieId, movie.UserId);
        }
    }
}