using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Application.Services;

namespace MelodyMatch.File.Services;

public interface IAvatarApplicationService : IApplicationService
{
    Task<string> UploadAvatarAsync(IFormFile file);
    
    Task DeleteAvatarAsync(string fileName);
}