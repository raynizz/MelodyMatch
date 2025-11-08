using System.Threading.Tasks;
using MelodyMatch.File.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace MelodyMatch.Controllers.Files;

public class AvatarsController : AbpController
{
    private readonly IAvatarApplicationService _avatarApplicationService;
    
    public AvatarsController(IAvatarApplicationService avatarApplicationService)
    {
        _avatarApplicationService = avatarApplicationService;
    }
    
    [HttpPost("upload")]
    public async Task<ActionResult<string>> UploadAvatarAsync(IFormFile file)
    {
        var avatarUrl = await _avatarApplicationService.UploadAvatarAsync(file);
        return Ok(new { AvatarUrl = avatarUrl });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAvatarAsync([FromQuery] string fileName)
    {
        await _avatarApplicationService.DeleteAvatarAsync(fileName);
        return Ok();
    }
}