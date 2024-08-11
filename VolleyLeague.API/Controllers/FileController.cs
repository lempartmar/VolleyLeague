using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("MigrateTeamImages")]
        public async Task<ActionResult> MigrateTeamImages()
        {
            try
            {
                await _fileService.MigrateTeamImagesToDatabase();
                return Ok("Zdjêcia dru¿yn zmigrowane prawid³owo.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
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

        [HttpPost("UploadTeamImage/{teamId}")]
        public async Task<ActionResult> UploadTeamImage(int teamId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Brak updatu plików.");

            await _fileService.UploadTeamImage(teamId, file);
            return Ok();
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
                _logger.LogError(ex, "Error occurred while getting uploaded files.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteTeamImage/{teamId}")]
        public async Task<ActionResult> DeleteTeamImage(int teamId)
        {
            var servicesPath = Path.Combine(_env.ContentRootPath);
            if (servicesPath.Contains("VolleyLeague.API"))
            {
                servicesPath = servicesPath.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Teams");
            }
            var filePath = Path.Combine(servicesPath, $"{teamId}.jpg");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            try
            {
                System.IO.File.Delete(filePath);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, $"Access denied while deleting image for team {teamId}");
                return StatusCode(403, "Access denied. Check file permissions.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting image for team {teamId}");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpGet("DownloadTeamImage/{teamId}")]
        public async Task<IActionResult> DownloadTeamImage(int teamId)
        {
            var servicesPath = Path.Combine(_env.ContentRootPath);
            if (servicesPath.Contains("VolleyLeague.API"))
            {
                servicesPath = servicesPath.Replace("VolleyLeague.API", "VolleyLeague.Shared/Images/Teams");
            }
            var filePath = Path.Combine(servicesPath, $"{teamId}.jpg");

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
            var file = File(memory, GetContentType(filePath), $"{teamId}.jpg");
            return file;
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
