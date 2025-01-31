﻿using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using VolleyLeague.Entities.Models;
using VolleyLeague.Repositories.Interfaces;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Shared.Dtos.Files;

namespace VolleyLeague.Services.Services
{
    public class FileService : IFileService
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ILogService _logService;
        private readonly IBaseRepository<TeamImage> _teamImageRepository;

        public FileService(ILogService logService, IMapper mapper, IWebHostEnvironment env, IBaseRepository<TeamImage> teamImageRepository)
        {
            _mapper = mapper;
            _env = env;
            _logService = logService;
            _teamImageRepository = teamImageRepository;
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

        public async Task<(Stream FileStream, string ContentType, string FileName)> GetLogoAsync()
        {
            var servicesPath = Path.Combine(_env.ContentRootPath);
            if (servicesPath.Contains("VolleyLeague.API"))
            {
                servicesPath = servicesPath.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Teams");
            }
            var filePath = Path.Combine(servicesPath, "LigaSiatkowkiNew.png");

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return (memory, GetContentType(filePath), "LigaSiatkowkiNew.png");
        }

        public string GetLogoPath()
        {
            var servicesPath = Path.Combine(_env.ContentRootPath);
            if (servicesPath.Contains("VolleyLeague.API"))
            {
                servicesPath = servicesPath.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Logos");
            }
            return Path.Combine(servicesPath, "LigaSiatkowkiNew.png");
        }

        public string GetPathForEmail(string fileName)
        {
            var servicesPath = Path.Combine(_env.ContentRootPath);
            if (servicesPath.Contains("VolleyLeague.API"))
            {
                servicesPath = servicesPath.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Logos");
            }
            return Path.Combine(servicesPath, $"{fileName}.png");
        }

        public async Task MigrateTeamImagesToDatabase()
        {
            var servicesPath2 = Path.Combine(_env.ContentRootPath);
            if (servicesPath2.Contains("VolleyLeague.API"))
            {
                servicesPath2 = servicesPath2.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Teams");
            }
            var fileNames = Directory.EnumerateFiles(servicesPath2, "*.jpg");
            
            foreach (var fileName in fileNames)
            {

                var servicesPath1 = Path.Combine(_env.ContentRootPath);
                if (servicesPath1.Contains("VolleyLeague.API"))
                {
                    servicesPath1 = servicesPath1.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Teams");
                }

                var filePath = Path.Combine(servicesPath1, $"{fileName}");
                if (File.Exists(filePath))
                {
                    var teamId = int.Parse(Path.GetFileNameWithoutExtension(filePath));
                    var imageBytes = await File.ReadAllBytesAsync(filePath);

                    var teamImage = new TeamImage
                    {
                        TeamId = teamId,
                        Image = imageBytes,
                        ImageType = "image/jpeg"
                    };

                    await _teamImageRepository.InsertAsync(teamImage);
                }
            }

            await _teamImageRepository.SaveChangesAsync();
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
        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }
}
