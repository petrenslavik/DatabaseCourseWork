using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

namespace Messenger.Resources
{
    public class ConversationResource
    {
        public string ConversationId { get; set; }
        public IEnumerable<UserResource> Users { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
