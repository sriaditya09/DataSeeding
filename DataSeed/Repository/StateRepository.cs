using DataSeed.Data;
using DataSeed.Model;
using Microsoft.EntityFrameworkCore;

namespace DataSeed.Repository
{
	public class StateRepository
	{
		private readonly DataDbContext _context;

		public StateRepository(DataDbContext context)
		{
			_context = context;
		}

		// Get all states
		public async Task<List<State>> GetAllAsync()
		{
			return await _context.States.ToListAsync(); // Removed incorrect Include
		}

		// Get state by StateCode (int)
		public async Task<State?> GetByIdAsync(int stateCode) // Changed string to int
		{
			return await _context.States.FirstOrDefaultAsync(s => s.StateCode == stateCode);
		}

		// Add a new state
		public async Task AddAsync(State state)
		{
			await _context.States.AddAsync(state);
			await _context.SaveChangesAsync();
		}

		// Update an existing state
		public async Task UpdateAsync(State state)
		{
			_context.States.Update(state);
			await _context.SaveChangesAsync();
		}

		// Delete a state by StateCode
		public async Task DeleteAsync(int stateCode) // Changed string to int
		{
			var state = await _context.States.FirstOrDefaultAsync(s => s.StateCode == stateCode); // Fixed FindAsync usage
			if (state != null)
			{
				_context.States.Remove(state);
				await _context.SaveChangesAsync();
			}
		}
	}
}

