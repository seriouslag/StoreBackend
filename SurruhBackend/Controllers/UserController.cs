using Microsoft.AspNetCore.Mvc;

namespace SurruhBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
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