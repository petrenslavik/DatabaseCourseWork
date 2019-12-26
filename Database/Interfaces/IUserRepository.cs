using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models;
using MongoDB.Bson;

namespace Database.Interfaces
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByConfirmationToken(string token);
        Task<List<User>> FindBySimilarUsername(string username);
        Task<List<User>> FindByUsersIds(IEnumerable<string> list);
        Task<User> GetByUsername(string username);

        Task UpdateConversationPreference(Preference preference,string userId);
        Task CreateConversationPreference(Preference preference, string userId);
        Task DeleteConversationPreference(string conversationId, string userId);
        Task<Preference> GetConversationPreference(string conversationId, string userId);

        Task<int> AmountOfNewUsers(DateTime from);
    }
}
