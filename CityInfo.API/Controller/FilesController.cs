using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controller;

[Route("api/files")]
[ApiController]
public class FilesController(FileExtensionContentTypeProvider contentTypeProvider) : ControllerBase
{
    [HttpGet("{fileId}")]
    public ActionResult GetFile(string fileId)
    {
        // look up the actual file, depending on fileId...
        string path = "getting-started-with-rest-slides.pdf";

        if (!System.IO.File.Exists(path))
        {
            return NotFound();
        }
        
        var bytes = System.IO.File.ReadAllBytes(path);

        if (!contentTypeProvider.TryGetContentType(path, out string? contentType))
        {
            contentType = MediaTypeNames.Application.Octet;
        }
        
        return File(bytes, contentType, Path.GetFileName(path));
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateFile(IFormFile file)
    {
        if (file.Length == 0 || !file.FileName.EndsWith(".pdf") ||
            file.ContentType != MediaTypeNames.Application.Pdf || file.Length > 20971520)
        {
            return BadRequest("Please upload a valid file.");
        }
        
        var path = Path.Combine(Directory.GetCurrentDirectory(), $"uploaded_file_{Guid.NewGuid()}.pdf");

        await using FileStream stream = new(path, FileMode.Create);
        await file.CopyToAsync(stream);
        
        return Ok("Your file was uploaded successfully.");
    }
}