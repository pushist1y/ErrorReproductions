using Microsoft.AspNetCore.Mvc;

namespace AreaRouting.Areas.Api.Controllers
{
    [Area("Api")]
    public class UsersController: Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return new JsonResult(new []
            {
                new {Id = 1, Name = "name1"},
                new {Id = 2, Name = "name2"},
            });
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            return new JsonResult( new { Id = 1, Name = "name1" } );
        }
    }
}
