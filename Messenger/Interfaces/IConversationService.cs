using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Database.Models;
using Messenger.Resources;
using Messenger.Util;

namespace Messenger.Interfaces
{
    public interface IConversationService
    {
        Task<Response<IEnumerable<ConversationResource>>> ListByUserIdAsync(string userId);
        Task<Response<Conversation>> CreateAsync(Conversation conversation);
        Task<Response<string>> DeleteAsync(string conversationId);
        Task<Response<string>> UpdateAsync(Conversation conversation);
        Task<Response<ConversationResource>> ById(string id);

        Task<Response<IEnumerable<PossibleChatResource>>> FindPossibleChatsAsync(string username);
        Task<Response<string>> UpdatePreferenceAsync(string conversationId, Preference preference);
    }
}
