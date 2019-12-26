using AutoMapper;
using Database.Interfaces;
using Database.Models;
using Messenger.Interfaces;
using Messenger.Resources;
using Messenger.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Services
{
    public class ConversationService : IConversationService
    {
        private IConversationRepository conversationRepository;
        private IUserRepository userRepository;
        private IMessageRepository messageRepository;
        private IFileService fileService;
        private IMapper mapper;

        public ConversationService(IConversationRepository conversationRepository, IUserRepository userRepository, IFileService fileService, IMapper mapper, IMessageRepository messageRepository)
        {
            this.conversationRepository = conversationRepository;
            this.userRepository = userRepository;
            this.fileService = fileService;
            this.mapper = mapper;
            this.messageRepository = messageRepository;
        }

        public async Task<Response<IEnumerable<ConversationResource>>> ListByUserIdAsync(string userId)
        {
            try
            {
                var list = await conversationRepository.GetAllConversationsByUserId(userId);
                var resourceList = new List<ConversationResource>();
                foreach (var conversation in list)
                {
                    var users = await userRepository.FindByUsersIds(conversation.Users.Select(m => m.UserId));
                    var messages = await messageRepository.GetAllMessagesByConversationId(conversation.Id); 
                    var resource = new ConversationResource
                    {
                        ConversationId = conversation.Id,
                        Users = await Task.WhenAll(users.Select(async m => new UserResource()
                        {
                            FirstName = m.FirstName,
                            SecondName = m.SecondName,
                            Id = m.Id,
                            Username = m.Username,
                            ImageData = await fileService.GetImageData(m.ImagePath),
                        })),
                        Messages = messages,
                    };
                    resourceList.Add(resource);
                }
                return new Response<IEnumerable<ConversationResource>>(resourceList);
            }
            catch (Exception exception)
            {
                return new Response<IEnumerable<ConversationResource>>(exception.Message);
            }
        }

        public async Task<Response<Conversation>> CreateAsync(Conversation conversation)
        {
            try
            {
                var users = await userRepository.FindByUsersIds(conversation.Users.Select(m => m.UserId));
                if (users.Count() != conversation.Users.Count)
                {
                    return new Response<Conversation>("Invalid users ids");
                }
                await conversationRepository.Create(conversation);
                return new Response<Conversation>(conversation);
            }
            catch (Exception exception)
            {
                return new Response<Conversation>(exception.Message);
            }
        }

        public async Task<Response<string>> DeleteAsync(string conversationId)
        {
            try
            {
                var conversation = await conversationRepository.GetById(conversationId);
                if (conversation == null)
                {
                    return new Response<string>("Invalid id");
                }
                await conversationRepository.Delete(conversationId);
                return new Response<string>(data: "Success");
            }
            catch (Exception exception)
            {
                return new Response<string>(message: exception.Message);
            }
        }

        public async Task<Response<string>> UpdateAsync(Conversation model)
        {
            try
            {
                var conversation = await conversationRepository.GetById(model.Id);
                if (conversation == null)
                {
                    return new Response<string>("Invalid id");
                }
                var users = await userRepository.FindByUsersIds(model.Users.Select(m => m.UserId));
                if (users.Count() != model.Users.Count)
                {
                    return new Response<string>(message: "Invalid users ids");
                }
                await conversationRepository.Update(conversation);
                return new Response<string>(data: "Success");
            }
            catch (Exception exception)
            {
                return new Response<string>(message: exception.Message);
            }
        }

        public async Task<Response<ConversationResource>> ById(string id)
        {
            try
            {
                var conversation = await conversationRepository.GetById(id);
                if (conversation == null)
                {
                    return new Response<ConversationResource>("Invalid id");
                }
                var users = await userRepository.FindByUsersIds(conversation.Users.Select(m => m.UserId));
                var resource = new ConversationResource
                {
                    ConversationId = conversation.Id,
                    Messages = await messageRepository.GetAllMessagesByConversationId(conversation.Id),
                    Users = await Task.WhenAll(users.Select(async m => new UserResource()
                    {
                        FirstName = m.FirstName,
                        SecondName = m.SecondName,
                        Id = m.Id,
                        Username = m.Username,
                        ImageData = await fileService.GetImageData(m.ImagePath),
                    })),
                };
                return new Response<ConversationResource>(resource);
            }
            catch (Exception exception)
            {
                return new Response<ConversationResource>(message: exception.Message);
            }
        }

        public async Task<Response<IEnumerable<PossibleChatResource>>> FindPossibleChatsAsync(string username)
        {
            try
            {
                var users = await userRepository.FindBySimilarUsername(username);
                var list = await Task.WhenAll(users.Select(async m => new PossibleChatResource()
                {
                    Title = $"{m.FirstName} {m.SecondName}",
                    Username = m.Username,
                    ImageData = await fileService.GetImageData(m.ImagePath),
                    IsUser = true,
                }));

                return new Response<IEnumerable<PossibleChatResource>>(list);
            }
            catch (Exception exception)
            {
                return new Response<IEnumerable<PossibleChatResource>>(exception.Message);
            }
        }

        public Task<Response<string>> UpdatePreferenceAsync(string conversationId, Preference preference)
        {
            throw new NotImplementedException();
        }
    }
}
