using Microsoft.AspNetCore.Http;

namespace VolleyLeague.Services.Services
{
    public interface IFileService
    {
        Task UploadFiles(List<IFormFile> files);

        bool TeamHasImage(int teamId);

        Task UploadTeamImage(int teamId, IFormFile file);
    }

}

