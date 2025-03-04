using DataSeed.Data;
using DataSeed.Model;
using DataSeed.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataSeed.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LocationController : ControllerBase
	{
		private readonly DataDbContext _context;

		public LocationController(DataDbContext context)
		{
			_context = context;
		}

		// GET: api/Location/{countryCode}
		[HttpGet("{countryCode}")]
		public async Task<IActionResult> GetStatesAndDistrictsAndCities(int countryCode)
		{
			// Get States for the given CountryCode
			var states = await _context.States
				.Where(s => s.CountryCode == countryCode)
				.ToListAsync();

			if (states == null || !states.Any())
			{
				return NotFound("No states found for the given country code.");
			}

			var statesWithDistrictsAndCities = new List<object>();

			foreach (var state in states)
			{
				// Get Districts for each State
				var districts = await _context.Districts
					.Where(d => d.StateCode == state.StateCode)
					.ToListAsync();

				var districtsWithCities = new List<object>();

				foreach (var district in districts)
				{
					// Get Cities for each District
					var cities = await _context.Cities
						.Where(c => c.district == district.Id)
						.ToListAsync();

					districtsWithCities.Add(new
					{
						District = district.Name,
						Cities = cities.Select(c => new { c.Name, c.Id }).ToList()
					});
				}

				statesWithDistrictsAndCities.Add(new
				{
					State = state.Name,
					Districts = districtsWithCities
				});
			}

			return Ok(statesWithDistrictsAndCities);
		}

		// GET: api/Location/city/{cityCode}
		[HttpGet("city/{cityCode}")]
		public async Task<IActionResult> GetCityLocationByCityCode(int cityCode)
		{
			// Get the city by cityCode
			var city = await _context.Cities
				.Where(c => c.CityCode == cityCode)
				.FirstOrDefaultAsync();

			if (city == null)
			{
				return NotFound("City not found for the given city code.");
			}

			// Get the district, state, and country based on the city
			var district = await _context.Districts
				.Where(d => d.district == city.district)
				.FirstOrDefaultAsync();

			if (district == null)
			{
				return NotFound("District not found for the city.");
			}

			var state = await _context.States
				.Where(s => s.StateCode == district.StateCode)
				.FirstOrDefaultAsync();

			if (state == null)
			{
				return NotFound("State not found for the district.");
			}

			var country = await _context.Countries
				.Where(c => c.CountryCode == state.CountryCode)
				.FirstOrDefaultAsync();

			if (country == null)
			{
				return NotFound("Country not found for the state.");
			}

			// Return the city, district, state, and country
			return Ok(new
			{
				City = city.Name,
				District = district.Name,
				State = state.Name,
				Country = country.Name
			});
		}

		// GET: api/Location/district/{districtCode}
		[HttpGet("district/{districtCode}")]
		public async Task<IActionResult> GetDistrictLocationByDistrictCode(int districtCode)
		{
			// Get the district by districtCode
			var district = await _context.Districts
				.Where(d => d.district == districtCode)
				.FirstOrDefaultAsync();

			if (district == null)
			{
				return NotFound("District not found for the given district code.");
			}

			// Get the state for the district
			var state = await _context.States
				.Where(s => s.StateCode == district.StateCode)
				.FirstOrDefaultAsync();

			if (state == null)
			{
				return NotFound("State not found for the district.");
			}

			// Get the country for the state
			var country = await _context.Countries
				.Where(c => c.CountryCode == state.CountryCode)
				.FirstOrDefaultAsync();

			if (country == null)
			{
				return NotFound("Country not found for the state.");
			}

			// Get the cities in the district
			var cities = await _context.Cities
				.Where(c => c.district == district.Id)
				.ToListAsync();

			// Return the district, cities, state, and country
			return Ok(new
			{
				District = district.Name,
				Cities = cities.Select(c => new { c.Name, c.Id }).ToList(),
				State = state.Name,
				Country = country.Name
			});
		}
	}


}
