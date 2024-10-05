// See https://aka.ms/new-console-template for more information
using System;
using Newtonsoft.Json;
using blekenbleu;
using System.Net;
using System.Xml.Linq;
using System.Collections.Generic;

namespace CarSpecJSON
{
	internal class Program
	{

		static void Main(string[] args)
		{
			string version = "version 1.3 ";
			string bname = "blekenbleu";
			string pname = "Haptics";
			string myfile = $"D:/my/SimHub/PluginsData/{pname}.{Environment.UserName}.json";

			if (args.Length > 1 )
				myfile = args[1];

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
						Dictionary<string, List<CarSpec>> atlas = json!;	// force non-null
                        Console.WriteLine(bname + $".Main({myfile}): {P.JtoSource(json!).Length} source length");

                    }
				} else Console.WriteLine(bname + ".Main():  null " + myfile);
			}
			else Console.WriteLine(bname + ".Main():  " + myfile + " not found");
		}

        readonly string[] sname = ["name", "category", "config", "order", "loc", "drive"];
        readonly string[] uname = ["idlerpm", "redline", "maxrpm", "cyl", "hp", "ehp", "cc", "nm"];
		string source = "AtlasDict = new()\n {\n\t{\n\t\t";
		void Sadd(int index, string? value)
		{
			if (null == value || 0 == value.Length)
				return;
			source += $",\n\t\t\t\t{sname[index]} = \"{value}\"";
		}

        void Uadd(int index, ushort? value)
        {
			if (null == value || 0 == value)
				return;
            source += $",\n\t\t\t\t{uname[index]} = {value}";
        }

		string JtoSource(Dictionary<string, List<CarSpec>> atlas)
		{
			foreach (var game in atlas)
			{
				source += $"\"{game.Key}\", new() {{\n ";
                foreach (var car in game.Value)
				{
                    source += $"\t\t\t new() {{\n\t\t\t\tid = \"{car.id}\"";
					Sadd(0, car.name);
					Uadd(0, car.idlerpm);
				}
				source += "\n\t\t\t},\n";
			}
			return source;
		}
	}
}
