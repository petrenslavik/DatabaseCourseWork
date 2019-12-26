using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Models;

namespace Database.Interfaces
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<List<Message>> GetAllMessagesByConversationId(string conversationId);
        Task<List<FileMessage>> GetAllFileMessagesByConversationId(string conversationId);

        Task<int> AmountOfNewMessages(DateTime from);
    }
}
