using System;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.ProfilePhoto.DTOs.Requests;
using MelodyMatch.ProfilePhoto.DTOs.Responses;
using MelodyMatch.ProfilePhoto.DTOs.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MelodyMatch.Controllers.Files;

[Authorize(Roles = RolesConsts.Dater)]
[Route("api/profile-photos")]
[ApiController]
public class ProfilePhotoController : ControllerBase
{
    private readonly IProfilePhotoApplicationService _profilePhotoApplicationService;

    public ProfilePhotoController(IProfilePhotoApplicationService profilePhotoApplicationService)
    {
        _profilePhotoApplicationService = profilePhotoApplicationService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ProfilePhotoResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProfilePhotoResponseDto>> Upload([FromForm] UploadProfilePhotoRequestDto form)
    {
        var result = await _profilePhotoApplicationService.UploadAsync(form.File, form.UserProfileId);
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _profilePhotoApplicationService.DeleteAsync(id);
        return NoContent();
    }
}