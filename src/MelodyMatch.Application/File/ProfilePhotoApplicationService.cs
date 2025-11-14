using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.Exceptions;
using MelodyMatch.ProfilePhoto.DTOs.Responses;
using MelodyMatch.ProfilePhoto.DTOs.Services;
using MelodyMatch.ProfilePhotos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace MelodyMatch.ProfilePhoto;

[RemoteService(false)]
public class ProfilePhotoApplicationService : ApplicationService, IProfilePhotoApplicationService
{
    private readonly IWebHostEnvironment _env;
    private readonly IProfilePhotoRepository _repository;
    private readonly IConfiguration _configuration;

    public ProfilePhotoApplicationService(
        IWebHostEnvironment env,
        IConfiguration configuration,
        IProfilePhotoRepository repository)
    {
        _env = env;
        _repository = repository;
        _configuration = configuration;
    }

    public async Task<ProfilePhotoResponseDto> UploadAsync(IFormFile file, Guid userProfileId)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!FileConsts.Profile.AllowedExtensions.Contains(extension))
        {
            throw new InvalidFileExtensionException(MelodyMatchDomainErrorCodes.File.InvalidFileExtension).WithData("allowedExtensions", string.Join(", ", FileConsts.Avatar.AllowedExtensions));
        }

        var existingCount = await _repository.CountByUserProfileIdAsync(userProfileId);
        if (existingCount > FileConsts.Profile.MaxCountPerUserProfile)
        {
            throw new FileCountException(MelodyMatchDomainErrorCodes.UserProfile.ProfilePhotoLimitExceeded).WithData("maxCount", FileConsts.Profile.MaxCountPerUserProfile);
        }

        var folder = Path.Combine(_env.WebRootPath, FileConsts.Profile.ProfileFolderPath);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(folder, fileName);
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        var baseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/');

        var photo = new ProfilePhotos.ProfilePhoto
        {
            UserProfileId = userProfileId,
            FileName = fileName,
            Url = $"{baseUrl}/uploads/profiles/{fileName}",
            IsConfirmed = true 
        };

        await _repository.InsertAsync(photo, autoSave: true);
        return ObjectMapper.Map<ProfilePhotos.ProfilePhoto, ProfilePhotoResponseDto>(photo);
    }

    [Authorize(Roles = RolesConsts.Admin)]
    public async Task DeleteAsync(Guid id)
    {
        var photo = await _repository.GetAsync(id);
        var folder = Path.Combine(_env.WebRootPath, FileConsts.Profile.ProfileFolderPath);
        var filePath = Path.Combine(folder, photo.FileName);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        await _repository.DeleteAsync(photo);
    }
}