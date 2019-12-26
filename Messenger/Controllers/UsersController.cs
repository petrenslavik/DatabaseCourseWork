using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Database.Models;
using Messenger.Interfaces;
using Messenger.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public UsersController(IUserService userService, IMapper mapper, IFileService fileService)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        // GET: api/Users
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Users
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("find/{username}")]
        public async Task<IActionResult> FindUser(string username)
        {
            var result = await userService.ByUsername(username);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var userResource = mapper.Map<User, UserResource>(result.Data);
            userResource.ImageData = await fileService.GetImageData(result.Data.ImagePath);

            return Ok(userResource);
        }
    }
}
