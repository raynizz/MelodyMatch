using System;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.Uow;

namespace MelodyMatch.DataSeed;

public class MelodyMatchDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IdentityRoleManager _roleManager;

    public MelodyMatchDataSeederContributor(IdentityRoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await EnsureRoleExistsAsync(RolesConsts.Admin, RolesConsts.AdminDescription);
        await EnsureRoleExistsAsync(RolesConsts.Dater, RolesConsts.DaterDescription);
    }

    private async Task EnsureRoleExistsAsync(string roleName, string description)
    {
        if (await _roleManager.FindByNameAsync(roleName) == null)
        {
            var role = new IdentityRole(SimpleGuidGenerator.Instance.Create(), roleName)
            {
                IsPublic = true,
                IsStatic = true
            };

            if (roleName == RolesConsts.Dater)
            {
                role.IsDefault = true;
            }
            role.SetProperty(RolesConsts.DescriptionFieldName, description);
            await _roleManager.CreateAsync(role);
        }
    }
}