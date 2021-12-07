using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GlobalBlue.CustomerManager.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(new Customer { Id = id, Name = "Frank", EmailAddress = "frank@gmail.com", Password = "some_hash" });
        }

        public class Customer
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string EmailAddress { get; set; }
            public string Password { get; set; }
        }

    }
}
