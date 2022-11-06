namespace MoviesAPI.Helpers
{
    public interface IFileStorageService
    {
        Task DeleteFile(string fileRoute, string continerName);
        Task<string> SaveFile(string continerName, IFormFile file);
        Task<string> EditFile(string continerName, IFormFile file, string fileRoute);
    }
}
