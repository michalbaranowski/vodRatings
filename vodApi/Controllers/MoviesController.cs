﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vod.Core.Boundary;

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
        public JsonResult Get()
        {
            return new JsonResult(_core.GetResults());
        }
    }
}