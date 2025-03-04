using DataSeed.Data;
using DataSeed.Model;
using Microsoft.EntityFrameworkCore;

namespace DataSeed.Repository
{
	public class CityRepository
	{
		private readonly DataDbContext _context;

		public CityRepository(DataDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		// Get all cities
		public async Task<List<City>> GetAllAsync()
		{
			return await _context.Cities.ToListAsync();
		}

		// Get city by ID
		public async Task<City?> GetByIdAsync(int id)
		{
			return await _context.Cities.FindAsync(id);
		}

		// Get city by DistrictCode
		public async Task<List<City>> GetByDistrictCodeAsync(int districtCode)
		{
			return await _context.Cities.Where(c => c.district == districtCode).ToListAsync();
		}

		// Add a new city
		public async Task AddAsync(City city)
		{
			if (city == null) throw new ArgumentNullException(nameof(city));

			await _context.Cities.AddAsync(city);
			await _context.SaveChangesAsync();
		}

		// Update an existing city
		public async Task UpdateAsync(City city)
		{
			if (city == null) throw new ArgumentNullException(nameof(city));

			_context.Cities.Update(city);
			await _context.SaveChangesAsync();
		}

		// Delete a city
		public async Task DeleteAsync(int id)
		{
			var city = await _context.Cities.FindAsync(id);
			if (city != null)
			{
				_context.Cities.Remove(city);
				await _context.SaveChangesAsync();
			}
		}
	}
}
