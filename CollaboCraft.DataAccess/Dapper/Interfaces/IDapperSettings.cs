using CollaboCraft.DataAccess.Dapper.Models;

namespace CollaboCraft.DataAccess.Dapper.Interfaces
{
    public interface IDapperSettings
    {
        string ConnectionString { get; }

        Provider Provider { get; }
    }
}
