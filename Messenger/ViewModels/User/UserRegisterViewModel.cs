using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;

namespace Messenger.ViewModels.User
{
    public class UserRegisterViewModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Username { get; set; }

        public Database.Models.User ToUser()
        {
            return new Database.Models.User()
            {
                FirstName = FirstName,
                SecondName = SecondName,
                Email = Email,
                PasswordHash = Password,
                Username = Username,
            };
        }
    }
}
