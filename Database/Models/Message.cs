using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database.Models
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(TextMessage),typeof(FileMessage))]
    public class Message:BaseModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ConversationId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AuthorId { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime SendDateTime { get; set; }
    }
}
