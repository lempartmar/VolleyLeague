using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Files;

namespace VolleyLeague.Services.Services
{
    public class FileService : IFileService
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ILogService _logService;

        public FileService(ILogService logService, IMapper mapper, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _env = env;
            _logService = logService;
        }

        public bool TeamHasImage(int teamId)
        {
            var servicesPath = Path.Combine(_env.ContentRootPath);
            if (servicesPath.Contains("VolleyLeague.API"))
            {
                servicesPath = servicesPath.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Teams");
            }

            var filePath = Path.Combine(servicesPath, $"{teamId}.jpg");

            return File.Exists(filePath);
        }

        public async Task UploadTeamImage(int teamId, IFormFile file)
        {
            var untrustedFileName = $"{teamId}.jpg";
            var servicesPath = Path.Combine(_env.ContentRootPath);
            if (servicesPath.Contains("VolleyLeague.API"))
            {
                servicesPath = servicesPath.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Teams");
            }

            var filePath = Path.Combine(servicesPath, untrustedFileName);

            try
            {
                if (!Directory.Exists(servicesPath))
                {
                    Directory.CreateDirectory(servicesPath);
                }

                await using FileStream fs = new(filePath, FileMode.Create);
                await file.CopyToAsync(fs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file {untrustedFileName}: {ex.Message}");
            }
        }


        public async Task UploadFiles(List<IFormFile> files)
        {
            List<UploadResultDto> uploadResults = new List<UploadResultDto>();

            foreach (var file in files)
            {
                var uploadResult = new UploadResultDto();
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

                var servicesPath = Path.Combine(_env.ContentRootPath);
                if (servicesPath.Contains("VolleyLeague.API"))
                {
                    servicesPath = servicesPath.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Teams");
                }

                var filePath = Path.Combine(servicesPath, untrustedFileName);

                try
                {
                    if (!Directory.Exists(servicesPath))
                    {
                        Directory.CreateDirectory(servicesPath);
                    }

                    int count = 1;
                    string fileNameOnly = Path.GetFileNameWithoutExtension(untrustedFileName);
                    string extension = Path.GetExtension(untrustedFileName);
                    while (File.Exists(filePath))
                    {
                        string tempFileName = $"{fileNameOnly}({count++}){extension}";
                        filePath = Path.Combine(servicesPath, tempFileName);
                    }

                    await using FileStream fs = new(filePath, FileMode.Create);
                    await file.CopyToAsync(fs);

                    uploadResult.StoredFileName = Path.GetFileName(filePath);
                    uploadResults.Add(uploadResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error uploading file {untrustedFileName}: {ex.Message}");
                    uploadResults.Add(uploadResult);
                }
            }

        }

    }
}
