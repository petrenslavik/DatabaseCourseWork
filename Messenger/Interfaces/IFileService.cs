using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Messenger.Interfaces
{
    public interface IFileService
    {
        Task<string> GetImageData(string imagePath);
        Task<FileInfo> SaveFile(IFormFile file);
        Task<FileStream> GetFile(string path);
    }
}
