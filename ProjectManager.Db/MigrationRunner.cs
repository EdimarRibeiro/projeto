using FluentMigrator;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors.Postgres;
using System.Reflection;

namespace ProjectManager.Db
{
    public class MigrationRunner
    {
        public static void Up(string connectionString)
        {
            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer);

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = new PostgresProcessorFactory();

            using (var processor = factory.Create(connectionString, announcer, options))
            {
                var runner = new FluentMigrator.Runner.MigrationRunner(assembly, migrationContext, processor);
                runner.MigrateUp(true);
            }
        }

        public static void Down(string connectionString, long version = 0)
        {
            var announcer = new TextWriterAnnouncer(s => System.Diagnostics.Debug.WriteLine(s));
            var assembly = Assembly.GetExecutingAssembly();

            var migrationContext = new RunnerContext(announcer);

            var options = new MigrationOptions { PreviewOnly = false, Timeout = 60 };
            var factory = new PostgresProcessorFactory();

            using (var processor = factory.Create(connectionString, announcer, options))
            {
                var runner = new FluentMigrator.Runner.MigrationRunner(assembly, migrationContext, processor);

                runner.MigrateDown(version);
            }
        }
    }

    public class MigrationOptions : IMigrationProcessorOptions
    {
        public bool PreviewOnly { get; set; }
        public string ProviderSwitches { get; set; }
        public int Timeout { get; set; }
    }
}