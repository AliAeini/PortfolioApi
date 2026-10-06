using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Interfaces;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/upload")]
[Tags("Upload")]
[Authorize]
public class UploadsController : ControllerBase
{
    private readonly IFileStorageService _storageService;
    private readonly ICurrentUserService _currentUser;

    public UploadsController(
        IFileStorageService storageService,
        ICurrentUserService currentUser)
    {
        _storageService = storageService;
        _currentUser = currentUser;
    }

    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile file, CancellationToken ct)
    {
        if (_currentUser.UserId is null)
            return Unauthorized(new { success = false, message = "Not authenticated" });

        try
        {
            var uploadContext = new FileUploadContext(_currentUser.UserId.Value, "profile");
            var path = await _storageService.SaveFileAsync(file, uploadContext, ct);
            return Ok(new { success = true, path });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("project-image")]
    public async Task<IActionResult> UploadProjectImage(IFormFile file, CancellationToken ct)
    {
        if (_currentUser.UserId is null)
            return Unauthorized(new { success = false, message = "Not authenticated" });

        try
        {
            var uploadContext = new FileUploadContext(_currentUser.UserId.Value, "projects");
            var path = await _storageService.SaveFileAsync(file, uploadContext, ct);
            return Ok(new { success = true, path });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    [HttpPost("project-images")]
    [DisableRequestSizeLimit]
    public async Task<IActionResult> UploadProjectImages(CancellationToken ct)
    {
        if (_currentUser.UserId is null)
            return Unauthorized(new { success = false, message = "Not authenticated" });

        var files = Request.Form.Files;
        if (files.Count == 0)
            return BadRequest(new { success = false, message = "No files provided" });

        try
        {
            var uploadContext = new FileUploadContext(_currentUser.UserId.Value, "projects");
            var paths = await _storageService.SaveFilesAsync(files, uploadContext, ct);
            return Ok(new { success = true, paths });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}