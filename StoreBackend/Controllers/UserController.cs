using Microsoft.AspNetCore.Mvc;

namespace StoreBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/user")]
    public class UserController : Controller
    {

        [HttpGet]
        public Value Get()
        {
            if (User.IsInRole("Admin"))
            {
                return new Value("You are an admin!");
            }

            if (User.IsInRole("Auth"))
            {
                return new Value("You are authorized!");
            }

            return new Value("You are not authorized.");
        }

        [HttpGet("admin")]
        public bool GetAdminCheck()
        {
            if (User.IsInRole("Admin"))
            {
                return true;
            }

            return false;
        }
        [HttpGet("auth")]
        public bool GetAuthCheck()
        {
            if (User.IsInRole("Auth"))
            {
                return true;
            }

            return false;
        }
    }

    public class Value
    {
        public string Name { get; set; }

        public Value(string name)
        {
            Name = name;
        }
    }
}