using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Database.Models;

namespace Database.Interfaces
{
    public interface IConversationRepository:IRepository<Conversation>
    {
        Task<List<Conversation>> GetAllConversationsByUserId(string id);
    }
}
