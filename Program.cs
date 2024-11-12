using Newtonsoft.Json;
using blekenbleu;
using sierses.Sim;

namespace CarSpecJSON
{
	internal class Program
	{
		static readonly string version = "version 1.20 ";
		string[] gname = [""];
		ushort[] Sorted = new ushort[1];

		static void Main(string[] args)
		{
			string bname = "blekenbleu";
			string myfile = $"D:/my/SimHub/PluginsData/Haptics.demas.json";		// 13 Aug 2024
			//string myfile = $"D:/my/SimHub/PluginsData/Haptics.Atlas.json";	// missing some order; test missing id
			string mysource = "R:/Temp/New.cs";

			if (args.Length > 1)
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
				}
				else Console.WriteLine(bname + ".Main():  null " + myfile);
			}
			else Console.WriteLine(bname + ".Main():  " + myfile + " not found");
		}

		readonly string[] sname = ["name", "category", "config", "order", "loc", "drive"];
		readonly string[] uname = ["idlerpm", "redline", "maxrpm", "cyl", "hp", "ehp", "cc", "nm"];
		string source = "namespace Atlas\t// " + version + "\n{\npublic partial class CarSpecAtlas\n{\n";

		internal void Sadd(int index, string? value)
		{
			if (null == value || 0 == value.Length || "?" == value)
				return;
			string temp = value.Replace("\"", "\\\"");
			value = temp.Replace("\n", "");
			temp = value.Replace("\r", "");
			source += $",\n\t\t\t{sname[index]} = \"{temp}\"";
		}

		internal void Uadd(int index, string? value)
		{
			if (null == value || 0 == value.Length || "?" == value)
				return;
			source += $",\n\t\t\t{uname[index]} = {value}";
		}

		string SortGameLengths(Dictionary<string, List<CarSpec>> atlas)
		{
			Sorted = new ushort[atlas.Count];
			ushort[] size = new ushort[atlas.Count];
			gname = new string[atlas.Count];
			ushort i = 0;
			foreach (var game in atlas)
			{
				gname[i] = game.Key;
				Sorted[i] = i;
				size[i++] = (ushort)game.Value.Count;
			}
			Array.Sort(size, Sorted);
			string str = $"{{ {Sorted[0]}, {size[0]}, {gname[Sorted[0]]}";
			for (i = 1; i < Sorted.Length; i++)
				str += $",\n  {Sorted[i]}, {size[i]}, {gname[Sorted[i]]}";

			return str + "\n}";
		}

		string JtoSource(Dictionary<string, List<CarSpec>> atlas, string file)
		{
			int n = Sorted.Length - 1;
			string up = "internal readonly byte[] Up = [" + Sorted[0].ToString();

			for (int i = 0; i <= n; i++)
			{
				if (0 < i)
					up += "," + Sorted[i].ToString();
			}

			source += up + "];\n"
					+ GameNames(gname)
					+ "internal readonly byte[][][][] cs =\n\t"
					+ Barray.Ag(atlas, Sorted)
					+ ";\n}\t//class CarSpecAtlas\n}\t//Atlas";
			File.WriteAllText(file, source);
			return source;
		}

		private static string GameNames(string[] gn)
		{
			string s = "internal readonly string[] gname = [";
			int n = gn.Length - 1;
			for (int i = 0; i < n; i++)
				s += "\"" + gn[i] + "\",";
			return s + "\"" + gn[n] + "\"];\n\n";
		}
	}
}
