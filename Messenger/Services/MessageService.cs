using Database.Interfaces;
using Database.Models;
using Messenger.Interfaces;
using Messenger.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Messenger.Services
{
    public class MessageService : IMessageService
    {
        private IMessageRepository messageRepository;
        private IConversationRepository conversationRepository;
        private IFileService fileService;

        public MessageService(IFileService fileService, IConversationRepository conversationRepository, IMessageRepository messageRepository)
        {
            this.fileService = fileService;
            this.conversationRepository = conversationRepository;
            this.messageRepository = messageRepository;
        }

        public async Task<Response<IEnumerable<Message>>> ListAllByConversationId(string id)
        {
            try
            {
                var list = await messageRepository.GetAllMessagesByConversationId(id);
                return new Response<IEnumerable<Message>>(list);
            }
            catch (Exception exception)
            {
                return new Response<IEnumerable<Message>>(exception.Message);
            }
        }

        public async Task<Response<Message>> CreateTextMessageAsync(Message message)
        {
            try
            {
                var conversation = await conversationRepository.GetById(message.ConversationId);
                if (conversation == null)
                {
                    return new Response<Message>("Invalid conversationId");
                }
                message.SendDateTime = DateTime.Now;
                message = await messageRepository.Create(message);
                return new Response<Message>(message);
            }
            catch (Exception exception)
            {
                return new Response<Message>(exception.Message);
            }
        }

        public async Task<Response<FileMessage>> CreateFileMessageAsync(FileMessage message, IFormFile file)
        {
            try
            {
                var conversation = await conversationRepository.GetById(message.ConversationId);
                if (conversation == null)
                {
                    return new Response<FileMessage>("Invalid conversationId");
                }

                var fileInfo = await fileService.SaveFile(file);
                message.SendDateTime = DateTime.Now;
                message.FileName = file.FileName;
                message.Size = fileInfo.Length.ToString();
                message.FileUri = fileInfo.FullName;

                message = await messageRepository.Create(message) as FileMessage;
                return new Response<FileMessage>(message);
            }
            catch (Exception exception)
            {
                return new Response<FileMessage>(exception.Message);
            }
        }

        public async Task<Response<string>> DeleteAsync(string messageId)
        {
            try
            {
                var message = await messageRepository.GetById(messageId);
                if (message == null)
                {
                    return new Response<string>("Invalid conversationId");
                }

                await messageRepository.Delete(messageId);
                return new Response<string>(data: "Success");
            }
            catch (Exception exception)
            {
                return new Response<string>(exception.Message);
            }
        }

        public async Task<Response<string>> UpdateAsync(Message message)
        {
            try
            {
                var entity = await messageRepository.GetById(message.Id);
                if (entity == null)
                {
                    return new Response<string>("Invalid id");
                }

                if (entity.AuthorId != message.AuthorId || entity.ConversationId != message.ConversationId)
                {
                    return new Response<string>("Cannot change author id or conversation id");
                }
                await messageRepository.Update(message);
                return new Response<string>(data: "Success");
            }
            catch (Exception exception)
            {
                return new Response<string>(message: exception.Message);
            }
        }

        public async Task<Response<Message>> GetAsync(string id)
        {
            try
            {
                var message = await messageRepository.GetById(id);
                return new Response<Message>(message);
            }
            catch (Exception exception)
            {
                return new Response<Message>(message: exception.Message);
            }
        }

        public async Task<Response<IEnumerable<FileMessage>>> ListFileMessagesByConversationIdAsync(string conversationId)
        {
            try
            {
                var list = await messageRepository.GetAllFileMessagesByConversationId(conversationId);
                return new Response<IEnumerable<FileMessage>>(list);
            }
            catch (Exception exception)
            {
                return new Response<IEnumerable<FileMessage>>(exception.Message);
            }
        }
    }
}
