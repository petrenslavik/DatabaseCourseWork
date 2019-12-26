using Database.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;

namespace Database.Mongo
{
    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        public ConversationRepository(DbContext context) : base(context, "conversations")
        {

        }

        public Task<List<Conversation>> GetAllConversationsByUserId(string id)
        {
            return collection.Find(m => m.Users.Any(p=>p.UserId == id)).ToListAsync();
        }
    }
}
