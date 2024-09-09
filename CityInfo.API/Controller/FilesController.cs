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
}