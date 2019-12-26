using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database.Models
{
    public class Conversation:BaseModel
    {
        public List<Participant> Users { get; set; }
    }
}
