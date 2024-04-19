using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab_07.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        // GET: api/<RandomController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<RandomController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<RandomController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<RandomController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RandomController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
