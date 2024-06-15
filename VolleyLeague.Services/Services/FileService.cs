using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using VolleyLeague.Entities.Dtos.Files;
using VolleyLeague.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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

                var servicesPath = Path.Combine(_env.ContentRootPath, "uploads");
                if (servicesPath.Contains("VolleyLeague.API"))
                {
                    servicesPath = servicesPath.Replace("VolleyLeague.API", "VolleyLeague.Services");
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
