using System;
using System.Threading.Tasks;
using MelodyMatch.Complaints;
using MelodyMatch.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreComplaintRepository :  EfCoreRepository<MelodyMatchDbContext, Complaint, Guid>, IComplaintRepository
{
    public EfCoreComplaintRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }
    
    public async Task<Complaint> AddComplaintAsync(Complaint complaintToAdd)
    {
        var dbContext = await GetDbContextAsync();
        
        await dbContext.Complaints.AddAsync(complaintToAdd);
        await dbContext.SaveChangesAsync();
        
        return complaintToAdd;
    }
    
    public async Task<Complaint> GetByIdAsync(Guid id)
    {
        var dbContext = await GetDbContextAsync();
        
        var complaint = await dbContext.Complaints
            .Include(x => x.ReportedUser)
                .ThenInclude(x => x.UserProfile)
                    .ThenInclude(x => x.ProfilePhotos!)
            .Include(x => x.ReportedUser)
                .ThenInclude(x => x.IdentityUser)
            .Include(x => x.Reporter)
                .ThenInclude(x => x.UserProfile)
            .Include(x => x.Reporter)
                .ThenInclude(x => x.IdentityUser)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        
        if (complaint == null)
        {
            throw new Volo.Abp.UserFriendlyException("Complaint not found");
        }
        
        return complaint;
    }
}