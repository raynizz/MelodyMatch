using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.Complaints;

public interface IComplaintRepository : IRepository<Complaint, Guid>
{
    Task<Complaint> AddComplaintAsync(Complaint complaintToAdd);
    
    Task<Complaint> GetByIdAsync(Guid id);
}