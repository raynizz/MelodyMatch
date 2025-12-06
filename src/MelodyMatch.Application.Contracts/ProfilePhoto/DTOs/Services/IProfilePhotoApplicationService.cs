using System;
using System.Threading.Tasks;
using MelodyMatch.ProfilePhoto.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;

namespace MelodyMatch.ProfilePhoto.DTOs.Services;

public interface IProfilePhotoApplicationService : IApplicationService
{
    Task<ProfilePhotoResponseDto> UploadAsync(IFormFile file, Guid userProfileId);

    Task DeleteAsync(Guid id);
}