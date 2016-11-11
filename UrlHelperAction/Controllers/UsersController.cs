using Consanta.Data;
using Microsoft.AspNetCore.Mvc;

namespace Consanta.Controllers
{
    public class UsersController: Controller
    {
        private IUserStore _userStore;
        public UsersController(IUserStore userStore)
        {
            _userStore = userStore;
        }


        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
