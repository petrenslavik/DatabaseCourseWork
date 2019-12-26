using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Messenger.Util;
using Microsoft.AspNetCore.Http;

namespace Messenger.Interfaces
{
    public interface IMessageService
    {
        Task<Response<IEnumerable<Message>>> ListAllByConversationId(string id);
        Task<Response<Message>> CreateTextMessageAsync(Message message);
        Task<Response<FileMessage>> CreateFileMessageAsync(FileMessage message, IFormFile file);
        Task<Response<string>> DeleteAsync(string messageId);
        Task<Response<string>> UpdateAsync(Message message);
        Task<Response<Message>> GetAsync(string id);
        
        Task<Response<IEnumerable<FileMessage>>> ListFileMessagesByConversationIdAsync(string conversationId);
    }
}
