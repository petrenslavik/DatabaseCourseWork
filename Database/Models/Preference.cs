using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database.Models
{
    public class Preference
    {
        public bool IsMuted { get; set; }
        public bool IsPinned { get; set; }
        public string ConversationId { get; set; }
    }
}
