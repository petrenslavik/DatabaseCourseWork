using System;
using AutoMapper;
using Database.Models;
using Messenger.Extensions;
using Messenger.Interfaces;
using Messenger.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Messenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IConversationService conversationService;
        private readonly IMessageService messageService;
        private readonly IFileService fileService;
        private readonly IHubContext<MessengerHub> hubContext;
        private readonly IMapper mapper;

        public MessagesController(IMessageService conversationService, IHubContext<MessengerHub> hubContext, IMapper mapper, IConversationService conversationService1, IFileService fileService)
        {
            this.messageService = conversationService;
            this.hubContext = hubContext;
            this.mapper = mapper;
            this.conversationService = conversationService1;
            this.fileService = fileService;
        }

        private string UserId => ControllerContext.HttpContext.User.FindFirstValue("ID");
        // GET: api/Messages
        [HttpGet("all/{id}")]
        public async Task<IActionResult> GetAll(string id)
        {
            var result = await messageService.ListAllByConversationId(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await messageService.GetAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] SaveMessageResource model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            string conversationId = model.ConversationId;
            if (conversationId == null)
            {
                var conversation = new Conversation()
                {
                    Users = new List<Participant>()
                };
                conversation.Users.Add(new Participant { IsCreator = false, UserId = UserId });
                conversation.Users.Add(new Participant { IsCreator = false, UserId = model.UserId });
                var convResult = await conversationService.CreateAsync(conversation);
                if (!convResult.Success)
                {
                    return BadRequest(convResult.Message);
                }
                conversationId = convResult.Data.Id;
                var userList = new List<string>();
                foreach (var user in convResult.Data.Users)
                {
                        userList.Add(user.UserId);
                }
                await hubContext.Clients.Users(userList).SendAsync("ReceivedNewConversation", conversationId);
            }

            var conv = await conversationService.ById(conversationId);
            var list = new List<string>();
            foreach (var user in conv.Data.Users)
            {
                if (user.Id != UserId)
                {
                    list.Add(user.Id);
                }
            }
            if (model.File != null)
            {
                var fileMessage = new FileMessage()
                {
                    AuthorId = UserId,
                    ConversationId = conversationId,
                };
                var result = await messageService.CreateFileMessageAsync(fileMessage,model.File);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                await hubContext.Clients.Users(list).SendAsync("ReceivedNewMessage", fileMessage.Id);
                return Ok(result.Data);
            }

            var message = new TextMessage()
            {
                AuthorId = UserId,
                ConversationId = conversationId,
                Text = model.Text,
            };
            var res = await messageService.CreateTextMessageAsync(message);
            if (!res.Success)
            {
                return BadRequest(res.Message);
            }

            await hubContext.Clients.Users(list).SendAsync("ReceivedNewMessage",message.Id);
            return Ok(res.Data);
        }

        // PUT: api/Messages/5
        [HttpPut]
        public async Task<IActionResult> Put(UpdateTextMessageResource model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var message = mapper.Map<UpdateTextMessageResource, TextMessage>(model);
            message.AuthorId = UserId;
            var result = await messageService.UpdateAsync(message);

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
            var result = await messageService.DeleteAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok();
        }

        [HttpGet("file/{id}")]
        public async Task<IActionResult> GetFile(string id)
        {
            var result = await messageService.GetAsync(id);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            if (!(result.Data is FileMessage fileMessage))
            {
                return BadRequest("Invalid id");
            }

            var stream = await fileService.GetFile(fileMessage.FileUri);
            return File(stream, "application/octet-stream", fileMessage.FileName);
        }
    }
}
