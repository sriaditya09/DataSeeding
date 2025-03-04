using DataSeed.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace DataSeed.Data
{
	public class DataDbContext : DbContext
	{
		
			public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

			public DbSet<Country> Countries { get; set; } = default!;
			public DbSet<State> States { get; set; } = default!;
			public DbSet<District> Districts { get; set; } = default!;
			public DbSet<City> Cities { get; set; } =default!;

		

	}
}
