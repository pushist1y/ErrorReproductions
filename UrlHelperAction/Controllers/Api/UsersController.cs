using System.Linq;
using Consanta.Data;
using Consanta.Models;
using Consanta.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Consanta.Controllers.Api
{
    [Route("api/[controller]")]
    public class UsersController
    {
        private readonly IUserStore _userStore;
        public UsersController(IUserStore userStore)
        {
            _userStore = userStore;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var usersDTO = (from ConsantaUser user in _userStore.Users.ToList() select new UserDTO(user));
            return new JsonResult(usersDTO,
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userStore.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return new NotFoundResult();
            }
            var userDTO = new UserDTO(user);
            return new JsonResult(userDTO,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }

    
}
