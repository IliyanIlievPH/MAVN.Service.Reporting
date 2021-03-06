using System.Data.Common;
using JetBrains.Annotations;
using Lykke.Common.MsSql;
using MAVN.Service.Reporting.MsSqlRepositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace MAVN.Service.Reporting.MsSqlRepositories
{
    public class ReportContext : MsSqlContext
    {
        private const string Schema = "report"; // TODO put proper schema name here

        public DbSet<TransactionReportEntity> TransactionReports { get; set; }

        // empty constructor needed for EF migrations
        [UsedImplicitly]
        public ReportContext()
            : base(Schema)
        {
        }

        public ReportContext(string connectionString, bool isTraceEnabled)
            : base(Schema, connectionString, isTraceEnabled)
        {
        }

        //Needed constructor for using InMemoryDatabase for tests
        public ReportContext(DbContextOptions options)
            : base(Schema, options)
        {
        }

        public ReportContext(DbConnection dbConnection)
            : base(Schema, dbConnection)
        {
        }

        protected override void OnLykkeModelCreating(ModelBuilder modelBuilder)
        {
            // TODO put db entities models building code here

            modelBuilder.Entity<TransactionReportEntity>()
                .HasIndex(c => c.Timestamp);
        }
    }
}
