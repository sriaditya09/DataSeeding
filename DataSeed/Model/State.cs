namespace DataSeed.Model
{
	public class State
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty; // Ensures Name is never null
		public int StateCode { get; set; }
		public int CountryCode { get; set; }
	}
}
