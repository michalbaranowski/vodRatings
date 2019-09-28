using Microsoft.AspNetCore.Mvc;

namespace vodApi.Controllers
{
    [Route("[controller]")]
    [Route("/")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Home");
        }
    }
}