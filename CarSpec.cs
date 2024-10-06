namespace blekenbleu
{
	public class CarSpec
	{
		public required string id;
		public string? game;	// redundant
		public string? name;
		public string? category;
		public ushort? idlerpm;	// CarSpec element
		public ushort? redline;
		public ushort? maxrpm;
		public string? config;
		public ushort? cyl;
		public string? order;	// firing order
		public string? loc;
		public string? drive;
		public ushort? hp;
		public ushort? ehp;
		public ushort? cc;
		public ushort? nm;
	}   // class CarSpec
}

/* Atlas sample JSON, C#
{
  "AC": [
	{
	  "id": "ks_abarth500_assetto_corse",
	  "game": "AC",
	  "name": "Abarth 500 Assetto Corse",
	  "category": "Kunos",
	  "idlerpm": "1250",
	  "redline": "6000",
	  "maxrpm": "6500",
	  "config": "I",
	  "cyl": "4",
	  "order": "1-3-4-2",
	  "loc": "F",
	  "drive": "F",
	  "hp": "197",
	  "cc": "1368",
	  "nm": "302"
	}
  ]
}

Dictionary<string, List<CarSpec>> AtlasDict = new() {
	["foo"] = [
        new() {
            id = "ks_abarth500_assetto_corse",
            name = "Abarth 500 Assetto Corse",
            config = "I",
            cyl = 4,
            loc = "F",
            drive = "F",
            hp = 197,
            cc = 1368,
            nm = 302
        },
        new() {
            id = "ks_abarth500_assetto_corse",
            name = "Abarth 500 Assetto Corse",
            config = "I",
            cyl = 4,
            loc = "F",
            drive = "F",
            hp = 197,
            cc = 1368,
            nm = 302
        }
    ]
}
 */
