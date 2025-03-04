using DataSeed.Data;
using DataSeed.Model;
using Microsoft.EntityFrameworkCore;

namespace DataSeed.Repository
{
	public class DistrictRepository
	{
		private readonly DataDbContext _context;

		public DistrictRepository(DataDbContext context)
		{
			_context = context;
		}

		// Get all districts
		public async Task<List<District>> GetAllAsync()
		{
			return await _context.Districts.ToListAsync();
		}

		// Get district by ID
		public async Task<District?> GetByIdAsync(int StateCode)
		{
			return await _context.Districts.FindAsync(StateCode);
		}

		// Add a new district
		public async Task AddAsync(District district)
		{
			await _context.Districts.AddAsync(district);
			await _context.SaveChangesAsync();
		}

		// Update an existing district
		public async Task UpdateAsync(District district)
		{
			_context.Districts.Update(district);
			await _context.SaveChangesAsync();
		}

		// Delete a district
		public async Task DeleteAsync(int StateCode)
		{
			var district = await _context.Districts.FindAsync(StateCode);
			if (district != null)
			{
				_context.Districts.Remove(district);
				await _context.SaveChangesAsync();
			}
		}
	}
}
