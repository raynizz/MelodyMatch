using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.Exceptions;
using MelodyMatch.File.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace MelodyMatch.File;

[RemoteService(false)]
public class AvatarApplicationService : ApplicationService, IAvatarApplicationService
{
    private readonly IWebHostEnvironment _environment;
    private readonly IConfiguration _config;

    public AvatarApplicationService(IWebHostEnvironment environment, IConfiguration config)
    {
        _environment = environment;
        _config = config;
    }

    public async Task<string> UploadAvatarAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new EmptyFileExceptions(MelodyMatchDomainErrorCodes.File.EmptyFileName);
        }

        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (!FileConsts.Avatar.AllowedExtensions.Contains(fileExtension))
        {
            throw new InvalidFileExtensionException(MelodyMatchDomainErrorCodes.File.InvalidFileExtension).WithData("allowedExtensions", string.Join(", ", FileConsts.Avatar.AllowedExtensions));
        }

        var folder = Path.Combine(_environment.WebRootPath, FileConsts.Avatar.AvatarFolderPath);
        Directory.CreateDirectory(folder);

        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(folder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var tempLogPath = Path.Combine(_environment.WebRootPath, "uploads", FileConsts.Avatar.LogAvatarUploadsFileName);
        await System.IO.File.AppendAllTextAsync(tempLogPath, $"{filePath}{Environment.NewLine}");

        var baseUrl = _config["App:BaseUrl"]?.TrimEnd('/');
        
        return $"{baseUrl}/{FileConsts.Avatar.AvatarFolderPath}/{fileName}";
    }

    public Task DeleteAvatarAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new EmptyFileNameException(MelodyMatchDomainErrorCodes.File.EmptyFileName);
        }

        var filePath = Path.Combine(_environment.WebRootPath, FileConsts.Avatar.AvatarFolderPath, fileName);
        
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}