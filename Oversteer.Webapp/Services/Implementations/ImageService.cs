using System;

namespace Oversteer.Webapp.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment environment;

        public ImageService(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public Task RemoveImage(string folder, string name)
        {
            string path = Path.Combine(environment.WebRootPath, folder, name);
            
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return Task.CompletedTask;
        }

        public Task SaveImage(byte[] image, string folder)
        {
            string path = Path.Combine(environment.WebRootPath, folder);
            using var writer = new BinaryWriter(File.OpenWrite(path));
            writer.Write(image);

            return Task.CompletedTask;
        }
    }
}
