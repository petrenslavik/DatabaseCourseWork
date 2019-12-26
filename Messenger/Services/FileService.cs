using Messenger.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Messenger.Services
{
    public class FileService : IFileService
    {
        private IHostingEnvironment environment;

        public FileService(IHostingEnvironment environment)
        {
            this.environment = environment;
        }

        public Task<string> GetImageData(string imagePath)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(imagePath))
                {
                    imagePath = "/images/default.png";
                }

                byte[] bytes;
                var file = environment.ContentRootFileProvider.GetFileInfo(imagePath);
                if (!file.Exists)
                {
                    return null;
                }

                using (var stream = environment.ContentRootFileProvider.GetFileInfo(imagePath).CreateReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        stream.CopyTo(memoryStream);
                        bytes = memoryStream.ToArray();
                    }
                }

                string base64 =
                    $"data:image/{Path.GetExtension(imagePath).Remove(0, 1)};base64,{Convert.ToBase64String(bytes)}";
                return base64;
            });
        }

        public Task<FileInfo> SaveFile(IFormFile file)
        {
            return Task.Run(() =>
            {
                var path = Path.Combine(environment.ContentRootPath, "uploads");

                var filePath = Path.Combine(path, GetUniqueFilename(file.FileName));
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return new FileInfo(filePath);
            });
        }

        public Task<FileStream> GetFile(string path)
        {
            return Task.Run(() =>
            {
                return new FileStream(path, FileMode.Open);
            });
        }

        private static string GetUniqueFilename(string filename)
        {
            var guid = Guid.NewGuid();
            filename = $"{Path.GetFileNameWithoutExtension(filename)}-{guid}{Path.GetExtension(filename)}";
            return filename;
        }
    }
}
