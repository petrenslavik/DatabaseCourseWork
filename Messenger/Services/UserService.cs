using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Interfaces;
using Database.Models;
using Messenger.Interfaces;
using Messenger.Resources;
using Messenger.Util;

namespace Messenger.Services
{
    public class UserService:IUserService
    {
        private IUserRepository userRepository;
        private IFileService fileService;

        public UserService(IUserRepository userRepository, IFileService fileService)
        {
            this.userRepository = userRepository;
            this.fileService = fileService;
        }

        public async Task<Response<User>> ByUsername(string username)
        {
            try
            {
                var user = await userRepository.GetByUsername(username);
                if (user == null)
                {
                    return new Response<User>("Invalid username");
                }
                return new Response<User>(user);
            }
            catch (Exception exception)
            {
                return new Response<User>(exception.Message);
            }
        }
    }
}
