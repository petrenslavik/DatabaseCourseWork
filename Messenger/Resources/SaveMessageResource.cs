using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Messenger.Resources
{
    public class SaveMessageResource
    {
        public string ConversationId { get; set; }
        public string Text { get; set; }
        public IFormFile File { get; set; }
        public string UserId { get; set; }
    }
}
