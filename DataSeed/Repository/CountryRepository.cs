using DataSeed.Data;
using DataSeed.Model;
using Microsoft.EntityFrameworkCore;

namespace DataSeed.Repository
{
	public class CountryRepository
	{
		private readonly DataDbContext _context;

		public CountryRepository(DataDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		// Get All Countries
		public async Task<List<Country>> GetAllAsync()
		{
			return await _context.Countries.ToListAsync();
		}

		// Get Country by CountryCode
		public async Task<Country?> GetByIdAsync(int countryCode)
		{
			return await _context.Countries.FirstOrDefaultAsync(c => c.CountryCode == countryCode);
		}

		// Add a New Country
		public async Task AddAsync(Country country)
		{
			if (country == null) throw new ArgumentNullException(nameof(country));

			await _context.Countries.AddAsync(country);
			await _context.SaveChangesAsync();
		}

		// Update an Existing Country
		public async Task UpdateAsync(Country country)
		{
			if (country == null) throw new ArgumentNullException(nameof(country));

			_context.Countries.Update(country);
			await _context.SaveChangesAsync();
		}

		// Delete a Country by CountryCode
		public async Task<bool> DeleteAsync(int countryCode)
		{
			var country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryCode == countryCode);
			if (country == null) return false; // Handle null case

			_context.Countries.Remove(country);
			await _context.SaveChangesAsync();
			return true;
		}
	}
}
