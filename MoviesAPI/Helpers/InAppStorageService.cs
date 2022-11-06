namespace MoviesAPI.Helpers
{
    public class InAppStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment env;
        private readonly IHttpContextAccessor httpContextAccessor;

        public InAppStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            this.env = env;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string fileRoute, string continerName)
        {
            if (string.IsNullOrEmpty(fileRoute))
                return Task.CompletedTask;

            var fileName = Path.GetFileName(fileRoute);
            var fileDirectory = Path.Combine(env.WebRootPath, continerName, fileName);

            if(File.Exists(fileDirectory))
                File.Delete(fileDirectory);

            return Task.CompletedTask;
        }

        public async Task<string> EditFile(string continerName, IFormFile file, string fileRoute)
        {
            await DeleteFile(fileRoute, continerName);
            return await SaveFile(continerName, file);
        }

        public async Task<string> SaveFile(string continerName, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";
            string folder = Path.Combine(env.WebRootPath, continerName);

            if(!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string route = Path.Combine(folder, fileName);
            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                var content = ms.ToArray();
                await File.WriteAllBytesAsync(route, content);
            }

            var url = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var routeForDB = Path.Combine(url, continerName, fileName).Replace("\\", "/");

            return routeForDB;
        }
    }
}
