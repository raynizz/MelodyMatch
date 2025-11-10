using System;
using System.Threading.Tasks;

namespace MelodyMatch.Contexts.MelodyMatchUser;

public interface ICurrentMelodyMatchUser
{
    Task<Users.MelodyMatchUser> GetAsync();
    
    Task<Guid> GetIdAsync();
}