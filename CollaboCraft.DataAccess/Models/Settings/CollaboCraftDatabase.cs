using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Dapper.Models;
using Microsoft.Extensions.Configuration;

namespace CollaboCraft.DataAccess.Models.Settings
{
    public class CollaboCraftDatabase(IConfiguration configuration) : IDapperSettings
    {
        public string ConnectionString => configuration.GetSection("CollaboCraftDatabase")["ConnectionString"];
        public Provider Provider => Enum.Parse<Provider>(configuration.GetSection("CollaboCraftDatabase")["Provider"]);
    }
}
