using System.Threading.Tasks;

namespace MelodyMatch.Data;

public interface IMelodyMatchDbSchemaMigrator
{
    Task MigrateAsync();
}
