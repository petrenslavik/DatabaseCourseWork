using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Database.Models;
using Messenger.Resources;
using Messenger.ViewModels.User;
using MongoDB.Driver.Core.Operations;

namespace Messenger.Mapping
{
    public class ModelToResourceProfile: Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<UpdateTextMessageResource, TextMessage>();
            CreateMap<User, UserResource>();
        }
    }
}
