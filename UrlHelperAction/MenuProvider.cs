using System.Collections.Generic;
using System.Linq;
using Consanta.Data;
using Consanta.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Consanta.Common.Services
{
    public class MenuProvider: IMenuProvider
    {
        private readonly IUserStore _store;
        private readonly IUrlHelper _urlHelper;
        private readonly ConsantaSignInManager<AspNetUser> _signInManager;
        private readonly ConsantaUserManager<AspNetUser> _userManager;
        private readonly ActionContext _actionContext;

        public MenuProvider(IUserStore store, IUrlHelperFactory urlHelperFactory, IActionContextAccessor accessor,
            ConsantaSignInManager<AspNetUser> signInManager, ConsantaUserManager<AspNetUser> userManager)
        {
            _store = store;
            _urlHelper = urlHelperFactory.GetUrlHelper(accessor.ActionContext);
            _actionContext = accessor.ActionContext;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IEnumerable<WebMenuItem> GetUserMenu()
        {
            var userId = 0;
            if (_signInManager.IsSignedIn(_actionContext.HttpContext.User))
            {
                int.TryParse(_userManager.GetUserId(_actionContext.HttpContext.User), out userId);
            }
            if (userId == 0)
            {
                return new List<WebMenuItem>
                {
                    new WebMenuItem {Id = 1, Name = "Вход", Url = _urlHelper.Action("Login", "Auth")}
                };
            }

            var consantUser = _store.Users.FirstOrDefault(u => u.AspNetUserId == userId);
            return new List<WebMenuItem>
            {
                new WebMenuItem {Id = 1, Name = consantUser.AspNetUser.UserName},
                new WebMenuItem {Id = 2, Name = "Выход", ParentId = 1, Url = _urlHelper.Action("Logout", "Auth")}
            };
        }

        public IEnumerable<WebMenuItem> GetMenu()
        {

            var noLoginMenu = new List<WebMenuItem>
            {
                new WebMenuItem {Id = 1, Name = "Главная", Url = _urlHelper.Action("Index", "Home")}
            };

            var userId = 0;
            if (_signInManager.IsSignedIn(_actionContext.HttpContext.User))
            {
                int.TryParse(_userManager.GetUserId(_actionContext.HttpContext.User), out userId);
            }
            if (userId == 0) return noLoginMenu;

            var consantaUser = _store.Users.FirstOrDefault(u => u.AspNetUserId == userId);
            //TODO: check permissions, build menu


//***************** example section ******************************************************************
            var url1 = _urlHelper.Action("Index", "Users");
            // url1 = "/api/Users" - wrong

            var url2 = _urlHelper.RouteUrl("default",
                new Dictionary<string, object> {{"controller", "Users"}, {"action", "Index"}});
            //url2 = "/Users" - right, but usage is counter-intuitive

            //var url3 = _urlHelper.Action("Index", "Users", null, null, null, null, "default");
            //url3 = "/Users" - right, but currently this method does not exist

//***************** end of example section ***********************************************************

            var mainMenu = new List<WebMenuItem>
            {
                new WebMenuItem {Id = 1, Name = "Главная", Url = _urlHelper.Action("Index", "Home")},
                new WebMenuItem {Id = 2, Name = "Администрирование"},
                new WebMenuItem {Id = 3, Name = "Пользователи", ParentId = 2, Url = _urlHelper.Action("Index", "Users")}
            };

            return mainMenu;
        }
    }


}
