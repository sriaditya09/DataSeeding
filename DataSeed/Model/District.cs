namespace DataSeed.Model
{
	public class District
	{
		public int Id { get; set; }
		public required string Name { get; set; } // Forces assignment during object creation
		public int StateCode { get; set; }
		public int district { get; set; }
	}
}
