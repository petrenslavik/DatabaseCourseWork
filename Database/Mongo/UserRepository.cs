using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models;
using MongoDB.Driver;

namespace Database.Mongo
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context, "users")
        {
        }


        public Task<User> GetByEmail(string email)
        {
            return collection.Find(m => m.Email == email).SingleOrDefaultAsync();
        }

        public Task<User> GetByConfirmationToken(string token)
        {
            return collection.Find(m => m.ConfirmationToken == token).SingleOrDefaultAsync();
        }

        public Task<List<User>> FindBySimilarUsername(string username)
        {
            return collection.Find(m => m.Username.Contains(username)).ToListAsync();
        }

        public Task<List<User>> FindByUsersIds(IEnumerable<string> list)
        {
            return collection.Find(m => list.Contains(m.Id)).ToListAsync();
        }

        public Task<User> GetByUsername(string username)
        {
            return collection.Find(m => m.Username == username).SingleOrDefaultAsync();
        }

        public async Task UpdateConversationPreference(Preference preference, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateConversationPreference(Preference preference, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteConversationPreference(string conversationId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Preference> GetConversationPreference(string conversationId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> AmountOfNewUsers(DateTime @from)
        {
            throw new NotImplementedException();
        }
    }
}
