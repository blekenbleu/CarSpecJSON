using Newtonsoft.Json;
using blekenbleu;

namespace CarSpecJSON
{
	internal class Program
	{

		static void Main(string[] args)
		{
			string version = "version 1.4 ";
			string bname = "blekenbleu";
			string pname = "Haptics";
			string myfile = $"D:/my/SimHub/PluginsData/{pname}.{Environment.UserName}.json";
			string mysource = "R:/Temp/New.cs";

			if (args.Length > 1 )
			{
				myfile = args[1];
				if (2 < args.Length)
				mysource = args[2];
			}
			if (File.Exists(myfile))
			{
				string text = File.ReadAllText(myfile);
				if (null != text)
				{
					Program P = new();
					Dictionary<string, List<CarSpec>>? json =
						JsonConvert.DeserializeObject<Dictionary<string, List<CarSpec>>>(text);
					if (null != json)
					{
						Console.WriteLine(version + bname + ".Main():  JsonConvert non-null " + myfile);
                        Console.WriteLine(bname + $".Main({myfile}): "
										// force non-null
							+ $"{P.JtoSource(json!, mysource).Length} source length");
                    }
				} else Console.WriteLine(bname + ".Main():  null " + myfile);
			}
			else Console.WriteLine(bname + ".Main():  " + myfile + " not found");
		}

        readonly string[] sname = ["name", "category", "config", "order", "loc", "drive"];
        readonly string[] uname = ["idlerpm", "redline", "maxrpm", "cyl", "hp", "ehp", "cc", "nm"];
		string source = "AtlasDict = new()\n{";
		void Sadd(int index, string? value)
		{
			if (null == value || 0 == value.Length)
				return;
			source += $",\n\t\t\t{sname[index]} = \"{value}\"";
		}

        void Uadd(int index, ushort? value)
        {
			if (null == value || 0 == value)
				return;
            source += $",\n\t\t\t{uname[index]} = {value}";
        }

		string JtoSource(Dictionary<string, List<CarSpec>> atlas, string file)
		{
			bool firstgame = true;
			foreach (var game in atlas)
			{
				if (!firstgame)
					source += "\n\t\t}\n\t},";
                firstgame = false;
				source += $"\n\t\"{game.Key}\", new() {{\n ";
				bool firstcar = true;
                foreach (var car in game.Value)
				{
					if (!firstcar)
						source += "\n\t\t},\n";
                    source += $"\t\tnew() {{\n\t\t\tid = \"{car.id}\"";
					Sadd(0, car.name);
					Uadd(0, car.idlerpm);
                    firstcar = false;
                }
//				source += "\n\t\t}";
			}
			source += "\n\t\t}\n\t}\n}";
			File.WriteAllText(file, source);
			return source;
		}
	}
}
