using Microsoft.AspNetCore.Http;

namespace VolleyLeague.Services.Services
{
    public interface IFileService
    {
        Task UploadFiles(List<IFormFile> files);
    }

}

