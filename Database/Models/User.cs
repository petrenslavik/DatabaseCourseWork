using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Database.Models
{
    [BsonIgnoreExtraElements]
    public class User:BaseModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        [BsonIgnoreIfNull]
        public string Username { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime RegistrationDate { get; set; }
        [BsonIgnoreIfNull]
        public string ConfirmationToken { get; set; }
        [BsonIgnoreIfNull]
        public string ImagePath { get; set; }

        [BsonIgnoreIfNull]
        public List<Preference> Preferences {get; set; }
    }
}
