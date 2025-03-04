using DataSeed.Data;
using DataSeed.Model;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace DataSeed.Seed
{
	public class DataSeed
	{
		public sealed class StaticSeed
		{
			private readonly DataDbContext context;
			private bool pending = false;

			public StaticSeed(DataDbContext context)
			{
				this.context = context;
			}

			public async Task Run()
			{
				pending = true;
				await AddCountries();
				await AddDistricts();
				await AddStates();
				await AddCities(); // Add Cities to be seeded
				if (pending)
					await context.SaveChangesAsync();
				return;
			}

			private static readonly string currentDirectory = Directory.GetCurrentDirectory();
			private static readonly string folder = "Json";

			public async Task AddCountries()
			{
				var existing = await context.Set<Country>()
					.Select(x => x.CountryCode)
					.ToArrayAsync();

				var filtered = Countries.Where(x => !existing.Contains(x.CountryCode))
						.ToArray();
				if (filtered.Length > 0)
				{
					await context.AddRangeAsync(filtered);
					await context.SaveChangesAsync();
					pending = true;
				}
			}

			public static Country[] Countries
			{
				get
				{
					var jdoc = GetJsonString("Country.json");

					var list = jdoc.RootElement.ValueKind == JsonValueKind.Array
					? jdoc.RootElement.Deserialize<Country[]>()
					: jdoc.RootElement.GetProperty("Country").Deserialize<Country[]>();

					return list ?? [];
				}
			}

			private static JsonDocument GetJsonString(string filename)
			{
				string fullpath = Path.Combine(currentDirectory, folder, filename);
				using StreamReader reader = new(fullpath);
				string json = reader.ReadToEnd();
				var jdoc = JsonDocument.Parse(json);
				return jdoc;
			}

			public async Task AddDistricts()
			{
				var existing = await context.Set<District>()
					.Select(x => x.StateCode)
					.ToArrayAsync();
				var filtered = Districts.Where(x => !existing.Contains(x.StateCode))
					.ToArray();
				if (filtered.Length > 0)
				{
					await context.AddRangeAsync(filtered);
					pending = true;
				}
			}

			public static District[] Districts
			{
				get
				{
					var jdoc = GetJsonString("District.json");
					var districts = jdoc.RootElement.Deserialize<District[]>();
					return districts ?? [];
				}
			}

			public async Task AddStates()
			{
				var existing = await context.Set<State>()
					.Select(x => x.StateCode)
					.ToArrayAsync();

				var filtered = States.Where(x => !existing.Contains(x.StateCode))
					.ToArray();
				if (filtered.Length > 0)
				{
					await context.AddRangeAsync(filtered);
					pending = true;
				}
			}

			public static State[] States
			{
				get
				{
					var jdoc = GetJsonString("State.json");
					var states = jdoc.RootElement.Deserialize<State[]>();
					return states ?? [];
				}
			}

			public async Task AddCities() // New method for adding cities
			{
				var existing = await context.Set<City>()
					.Select(x => x.Id)
					.ToArrayAsync();

				var filtered = Cities.Where(x => !existing.Contains(x.Id))
					.ToArray();
				if (filtered.Length > 0)
				{
					await context.AddRangeAsync(filtered);
					pending = true;
				}
			}

			public static City[] Cities
			{
				get
				{
					var jdoc = GetJsonString("City.json");
					var cities = jdoc.RootElement.Deserialize<City[]>();
					return cities ?? [];
				}
			}
		}
	}
}


