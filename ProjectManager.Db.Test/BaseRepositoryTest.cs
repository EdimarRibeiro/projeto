using ProjectManager.Db.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectManager.Db.Test
{

    public abstract class BaseRepositoryTest
    {
        private DbProjectManagerContext? _dbContext;

        protected DbProjectManagerContext GetDbProjectManagerContext()
        {
            if (_dbContext != null) return _dbContext;

            var config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            var option = new DbContextOptionsBuilder<DbProjectManagerContext>()
                .UseNpgsql(config["ConnectionStrings:ProjectManagerWeb"], opt => opt.CommandTimeout(10000))
                .EnableSensitiveDataLogging();

            _dbContext = new DbProjectManagerContext(option.Options, null);
            return _dbContext;
        }
    }
}
