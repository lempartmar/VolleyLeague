using Microsoft.AspNetCore.Http;
using VolleyLeague.Entities.Dtos.Discussion;

namespace VolleyLeague.Services.Services
{
    public interface IFileService
    {
        Task UploadFiles(List<IFormFile> files);
    }

}

