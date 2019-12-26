using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models;
using MongoDB.Driver;

namespace Database.Mongo
{
    public class MessageRepository:Repository<Message>, IMessageRepository
    {
        public MessageRepository(DbContext context) : base(context, "messages")
        {

        }

        public Task<List<Message>> GetAllMessagesByConversationId(string conversationId)
        {
            return collection.Find(m => m.ConversationId == conversationId).ToListAsync();
        }

        public Task<List<FileMessage>> GetAllFileMessagesByConversationId(string conversationId)
        {
            return Task.Run(()=>collection.AsQueryable().OfType<FileMessage>().Where(m => m.ConversationId == conversationId).ToList());
        }

        public async Task<int> AmountOfNewMessages(DateTime @from)
        {
            throw new NotImplementedException();
        }
    }
}
