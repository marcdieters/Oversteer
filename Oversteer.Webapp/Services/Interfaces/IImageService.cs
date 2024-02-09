namespace Oversteer.Webapp.Services
{
    public interface IImageService
    {
        Task SaveImage(byte[] image, string folder);
        Task RemoveImage(string folder, string name);
    }
}
