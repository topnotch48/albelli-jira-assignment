using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Albelli.Orders.Persistence
{
	public static class DbContextExtensions
	{
		public static bool AllAvailableMigrationsApplied(this DbContext context)
		{
			var applied = context.GetService<IHistoryRepository>()
				.GetAppliedMigrations()
				.Select(m => m.MigrationId);

			var total = context.GetService<IMigrationsAssembly>()
				.Migrations
				.Select(m => m.Key);

			return !total.Except(applied).Any();
		}
	}
}
