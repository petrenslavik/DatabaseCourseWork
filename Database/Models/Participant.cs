using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Database.Models
{
    public class Participant
    {
        public string UserId { get; set; }
        public bool IsCreator { get; set; }
    }
}
