using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VolleyLeague.Entities.Dtos.Discussion;
using VolleyLeague.Entities.Dtos.Files;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;

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

        public async Task UploadFiles(List<IFormFile> files)
        {
            List<UploadResultDto> uploadResults = new List<UploadResultDto>();

            foreach (var file in files)
            {
                var uploadResult = new UploadResultDto();
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay = WebUtility.HtmlEncode(untrustedFileName);

                var uploadsPath = Path.Combine(_env.ContentRootPath, "uploads");
                var filePath = Path.Combine(uploadsPath, untrustedFileName);

                try
                {
                    // Ensure the uploads directory exists
                    if (!Directory.Exists(uploadsPath))
                    {
                        Directory.CreateDirectory(uploadsPath);
                    }

                    // Check if the file already exists and create a unique name if it does
                    int count = 1;
                    string fileNameOnly = Path.GetFileNameWithoutExtension(untrustedFileName);
                    string extension = Path.GetExtension(untrustedFileName);
                    while (File.Exists(filePath))
                    {
                        string tempFileName = $"{fileNameOnly}({count++}){extension}";
                        filePath = Path.Combine(uploadsPath, tempFileName);
                    }

                    // Create the file on the server
                    await using FileStream fs = new(filePath, FileMode.Create);
                    await file.CopyToAsync(fs);

                    uploadResult.StoredFileName = Path.GetFileName(filePath);
                    uploadResults.Add(uploadResult);
                }
                catch (Exception ex)
                {
                    // Log the error (you can replace this with your logging mechanism)
                    Console.WriteLine($"Error uploading file {untrustedFileName}: {ex.Message}");
                    uploadResults.Add(uploadResult);
                }
            }

        }

    }
}

