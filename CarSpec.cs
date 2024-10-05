namespace blekenbleu
{
    public class CarSpec
	{
		public string? game;
		public string? name;
		public required string id;
		public string? config;
		public ushort? cyl;
		public string? loc;
		public string? drive;
		public ushort? hp;
		public ushort? ehp;
		public ushort? cc;
		public ushort? nm;
		public ushort? redline;
		public ushort? maxrpm;
		public ushort? idlerpm;							// CarSpec element
		public string? order;							// firing order added 19 Jun 2024
		public string? category;
		public string? notes;
		public string? defaults;
		public string? properties;
	}   // class CarSpec
}
