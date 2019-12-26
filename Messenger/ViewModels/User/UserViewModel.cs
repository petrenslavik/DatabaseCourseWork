using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.ViewModels.User
{
    public class UserViewModel
    {
        public UserViewModel()
        {

        }

        public UserViewModel(Database.Models.User user)
        {
            FirstName = user.FirstName;
            SecondName = user.SecondName;
            Email = user.Email;
            Username = user.Username;
            Id = user.Id;
        }

        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Id { get; set; }
        public string ImageData { get; set; }
    }
}
