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
    }
}