using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using VolleyLeague.Services.Interfaces;
using VolleyLeague.Services.Services;
using VolleyLeague.Shared.Dtos.Teams;

namespace VolleyLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;
        private readonly IFileService _fileService;
        private readonly ITeamService _teamService;
        private readonly IWebHostEnvironment _env;

        public FileController(ILogger<FileController> logger, IWebHostEnvironment env, IFileService fileService, ITeamService teamService)
        {
            _logger = logger;
            _fileService = fileService;
            _env = env;
            _teamService = teamService;
        }

        [HttpPost("UploadFile")]
        public async Task<ActionResult> UploadFile(List<IFormFile> files)
        {
            await _fileService.UploadFiles(files);
            return Ok();
        }

        [HttpGet("DownloadFile/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(filePath), fileName);
        }

        [HttpGet("GetAllTeamsImagesStatus")]
        public async Task<IActionResult> GetAllTeamsImagesStatus()
        {
            var teams = await _teamService.GetAllTeams();
            var teamImageDtos = teams.Select(team => new TeamImageDto
            {
                Id = team.Id,
                Name = team.Name,
                HasImage = _fileService.TeamHasImage(team.Id)
            }).ToList();

            return Ok(teamImageDtos);
        }

        [HttpGet("GetUploadedFiles")]
        public IActionResult GetUploadedFiles()
        {
            try
            {
                var uploadsPath = Path.Combine(_env.ContentRootPath, "uploads");
                // Logowanie œcie¿ki dla sprawdzenia
                _logger.LogInformation($"Uploads path: {uploadsPath}");

                if (!Directory.Exists(uploadsPath))
                {
                    _logger.LogWarning("Uploads directory does not exist.");
                    return NotFound("Uploads directory does not exist.");
                }

                var files = Directory.EnumerateFiles(uploadsPath).Select(Path.GetFileName).ToList();

                if (!files.Any())
                {
                    _logger.LogInformation("No files found in uploads directory.");
                }

                return Ok(files);
            }
            catch (Exception ex)
            {
                // Logowanie b³êdów
                _logger.LogError(ex, "Error occurred while getting uploaded files.");
                return StatusCode(500, "Internal server error");
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
