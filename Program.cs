using Newtonsoft.Json;
using System;
using blekenbleu;
using System.Text;

namespace CarSpecJSON
{
	internal class Program
	{
		static readonly string version = "version 1.10 ";
		static void Main(string[] args)
		{
			
			string bname = "blekenbleu";
			string pname = "Haptics";
			string myfile = $"D:/my/SimHub/PluginsData/{pname}.{Environment.UserName}.json";
//			string myfile = $"D:/my/SimHub/PluginsData/Haptics.demas.json";		// 13 Aug 2024
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
							+ $"\n{P.SortGameLengths(json!)}"
							+ $"\n{P.JtoSource(json!, mysource).Length} Dictionary source length"
						);
					}
				} else Console.WriteLine(bname + ".Main():  null " + myfile);
			}
			else Console.WriteLine(bname + ".Main():  " + myfile + " not found");
		}

		readonly string[] sname = ["name", "category", "config", "order", "loc", "drive"];
		readonly string[] uname = ["idlerpm", "redline", "maxrpm", "cyl", "hp", "ehp", "cc", "nm"];
		string source = "namespace blekenbleu\t// "+version+"\n{\npublic partial class CarSpecAtlas\n{\nreadonly Dictionary<string, List<CarSpec>> AtlasDict = new() {\n";
		ushort[] Sorted = new ushort[1];

		void Sadd(int index, string? value)
		{
			if (null == value || 0 == value.Length || "?" == value)
				return;
			string temp = value.Replace("\"", "\\\"");
			value = temp.Replace("\n", "");
			temp = value.Replace("\r", "");
			source += $",\n\t\t\t{sname[index]} = \"{temp}\"";
		}

		void Uadd(int index, string? value)
		{
			if (null == value || 0 == value.Length || "?" == value)
				return;
			source += $",\n\t\t\t{uname[index]} = {value}";
		}

		string SortGameLengths(Dictionary<string, List<CarSpec>> atlas)
		{
            Sorted = new ushort[atlas.Count];
			ushort[] size = new ushort[atlas.Count];
			string[] gname = new string[atlas.Count];
			ushort i = 0;
			foreach (var game in atlas)
			{
				gname[i] = game.Key;
				Sorted[i] = i;
				size[i++] = (ushort)game.Value.Count;
			}
			Array.Sort(size, Sorted);
			string str = $"{{ {Sorted[0]}, {size[0]}, {gname[0]}";
			for (i = 1; i < Sorted.Length; i++)
				str +=  $",\n  {Sorted[i]}, {size[i]}, {gname[i]}";

			return str + "\n}";
		}

		string JtoSource(Dictionary<string, List<CarSpec>> atlas, string file)
		{
			int n = Sorted.Length - 1;
			string s = "\nreadonly ushort[] Up = [" + Sorted[n].ToString();

			for(int i = 0; i <= n; i++)
			{
				if (0 < i) {
					source += "\n\t\t}\n\t],\n";
					s += ","+Sorted[n- i].ToString();
				}

				var game = atlas.ElementAt(Sorted[n- i]);	// largest first
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
			
			source += "\n\t\t}\n\t]\n};\t//AtlasDict\n" + s + "];\n\n"
					+ "byte[][] cs = " + CarByte(atlas.ElementAt(Sorted[n]).Value[0])
					+";\n}\t//class CarSpecAtlas\n}\t//blekenbleu";
			File.WriteAllText(file, source);
			return source;
		}

		private static byte[] ToBytes(string? c)
		{
			return (null == c || (1 == c.Length && ("?" == c || "0" == c))) ? [0] : Encoding.ASCII.GetBytes(c!);
		}

		private static string ByteString(string? c)
		{
			byte[] spec = ToBytes(c);
			StringBuilder sb = new($"\n\t[{spec[0]}");
			for(int i = 1; i < spec.Length; i++)
				sb.Append($",{spec[i]}");
			sb.Append($"],"); // {c}");
			return sb.ToString();
		}

		private static string LastByteString(string? c)
		{
			byte[] spec = ToBytes(c);
			StringBuilder sb = new($"\n\t[{spec[0]}");
			for(int i = 1; i < spec.Length; i++)
				sb.Append($",{spec[i]}");
			sb.Append("]\n\t}");
			return sb.ToString();
		}

		private static string CarByte(CarSpec car)
		{
			return "{"
				+ ByteString(car.id)
				+ ByteString(car.name)
				+ ByteString(car.category)
				+ ByteString(car.idlerpm)
				+ ByteString(car.redline)
				+ ByteString(car.maxrpm)
				+ ByteString(car.config)
				+ ByteString(car.cyl)
				+ ByteString(car.order)
				+ ByteString(car.loc)
				+ ByteString(car.drive)
				+ ByteString(car.hp)
				+ ByteString(car.ehp)
				+ ByteString(car.cc)
				+ ByteString(car.nm)
				+ LastByteString(car.cc);
		}
	}	// class
}
