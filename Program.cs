// See https://aka.ms/new-console-template for more information
using System;
using Newtonsoft.Json;
using blekenbleu;

namespace CarSpecJSON
{
	internal class Program
	{
		
		static void Main(string[] args)
		{
			string version = "version 1.2 ";
			string bname = "blekenbleu";
            string pname = "Haptics";
            string myfile = $"D:/my/SimHub/PluginsData/{pname}.{Environment.UserName}.json";
            if (File.Exists(myfile))
			{
				string text = File.ReadAllText(myfile);
				if (null != text)
				{
					Dictionary<string, List<CarSpec>>? json =
						JsonConvert.DeserializeObject<Dictionary<string, List<CarSpec>>>(text);
					if (null != json)
						Console.WriteLine(version + bname + ".Main():  JsonConvert non-null " + myfile);
				} else Console.WriteLine(bname + ".Main():  null " + myfile);
			}
			else Console.WriteLine(bname + ".Main():  " + myfile + " not found"); 
		}
	}
}