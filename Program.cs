using Newtonsoft.Json;
using blekenbleu;

namespace CarSpecJSON
{
	internal class Program
	{
        static string version = "version 1.6 ";
        static void Main(string[] args)
		{
			
			string bname = "blekenbleu";
			string pname = "Haptics";
//			string myfile = $"D:/my/SimHub/PluginsData/{pname}.{Environment.UserName}.json";
			string myfile = $"D:/my/SimHub/PluginsData/{pname}.Atlas_engine_data.json";
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
						Console.WriteLine(Program.version + bname + ".Main():  JsonConvert non-null " + myfile);
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
		string source = "namespace blekenbleu\t// "+version+"\n{\npublic partial class CarSpecAtlas\n{\nreadonly Dictionary<string, List<CarSpec>> AtlasDict = new() {\n";
		void Sadd(int index, string? value)
		{
			if (null == value || 0 == value.Length || "?" == value)
				return;
			string temp = value.Replace("\"", "\\\"");
			value = temp.Replace("\n", "");
			temp = value.Replace("\r", "");
			source += $",\n\t\t\t{sname[index]} = \"{temp}\"";
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
					source += "\n\t\t}\n\t],\n";
                firstgame = false;
				source += $"\t[\"{game.Key}\"] = [\n";
				bool firstcar = true;
                foreach (var car in game.Value)
				{
					if (!firstcar)
						source += "\n\t\t},\n";
                    source += $"\t\tnew() {{\n\t\t\tid = \"{car.id}\"";
					Sadd(0, car.name);
					Sadd(1, car.category);
					Uadd(0, car.idlerpm);
					Uadd(1, car.redline);
					Uadd(2, car.maxrpm);
					Sadd(2, car.config);
					Uadd(3, car.cyl);
					Sadd(3, car.order);
					Sadd(4, car.loc);
					Sadd(5, car.drive);
					Uadd(4, car.hp);
					Uadd(5, car.ehp);
					Uadd(6, car.cc);
					Uadd(7, car.nm);
                    firstcar = false;
                }
			}
			source += "\n\t\t}\n\t]\n};\t//AtlasDict\n}\t//class CarSpecAtlas\n}\t//blekenbleu";
			File.WriteAllText(file, source);
			return source;
		}
	}
}
