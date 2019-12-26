using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Database.Models;
using Messenger.Resources;
using Messenger.Util;

namespace Messenger.Interfaces
{
    public interface IUserService
    {
        Task<Response<User>> ByUsername(string username);
    }
}
