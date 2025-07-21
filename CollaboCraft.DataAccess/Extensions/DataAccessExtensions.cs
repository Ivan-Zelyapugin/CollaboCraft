using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Dapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using CollaboCraft.DataAccess.Models.Settings;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.DataAccess.Repositories;

namespace CollaboCraft.DataAccess.Extensions
{
    public static class DataAccessExtensions
    {
        public static IServiceCollection MigrateDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("CollaboCraftDatabase")["ConnectionString"];

            EnsureDatabase.For.PostgresqlDatabase(connectionString);

            var upgrader = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithTransaction()
                .WithVariablesDisabled()
                .LogToConsole()
                .Build();

            if (upgrader.IsUpgradeRequired())
            {
                upgrader.PerformUpgrade();
            }

            return services;
        }
        public static IServiceCollection AddDapper(this IServiceCollection services)
        {
            return services
                .AddSingleton<IDapperSettings, CollaboCraftDatabase>()
                .AddSingleton<IDapperContext<IDapperSettings>, DapperContext<IDapperSettings>>();
        }
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IDocumentRepository, DocumentRepository>()
                .AddScoped<IDocumentParticipantRepository, DocumentParticipantRepository>()
                .AddScoped<IBlockRepository, BlockRepository>()
                .AddScoped<IBlockImageRepository, BlockImageRepository>()
                .AddScoped<IContactRepository, ContactRepository>();
        }
    }
}
