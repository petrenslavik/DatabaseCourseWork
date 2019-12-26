using AutoMapper;
using Database.Models;
using Messenger.Extensions;
using Messenger.Interfaces;
using Messenger.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationsController : ControllerBase
    {
        private readonly IConversationService conversationService;
        private readonly IHubContext<MessengerHub> hubContext;
        private readonly IMapper mapper;

        private string UserId => ControllerContext.HttpContext.User.FindFirstValue("ID");
        public ConversationsController(IConversationService conversationService, IHubContext<MessengerHub> hubContext, IMapper mapper)
        {
            this.conversationService = conversationService;
            this.hubContext = hubContext;
            this.mapper = mapper;
        }
        // GET: api/Conversations
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await conversationService.ListByUserIdAsync(UserId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        // GET: api/Conversations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await conversationService.ById(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        // POST: api/Conversations Create
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SaveConversationResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var conversation = mapper.Map<SaveConversationResource, Conversation>(resource);
            conversation.Users.Add(new Participant() { UserId = UserId, IsCreator = true });
            var result = await conversationService.CreateAsync(conversation);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        // PUT: api/Conversations/5 //Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] SaveConversationResource resource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var conversation = mapper.Map<SaveConversationResource, Conversation>(resource);
            conversation.Id = id;
            if (conversation.Users.Find(m => m.UserId == UserId) == null)
            {
                return Forbid();
            }

            var result = await conversationService.UpdateAsync(conversation);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await conversationService.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }

        [HttpGet("find/{username}", Name = "FindChats")]
        public async Task<IActionResult> FindChats(string username)
        {
            var result = await conversationService.FindPossibleChatsAsync(username);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }
    }
}
