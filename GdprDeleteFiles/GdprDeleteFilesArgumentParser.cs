using System.Linq;
using System.Net.NetworkInformation;

namespace GdprDeleteFiles
{
	public class GdprDeleteFilesArgumentParser
	{
		public GdprDeleteFilesArgument Parse(string[] args)
		{
			return new GdprDeleteFilesArgument
			{
				Command = args[0].Capitalize(),
				Filename = GetOption("-f", args),
			};
		}

		private string GetOption(string s,string[] args)
		{
			var index = args.ToList().IndexOf("-f");
			return args.Skip(index + 1).Take(1).Single();
		}
	}

	internal static class StringExtensions
	{
		public static string Capitalize(this string @this)
		{
			return char.ToUpper(@this[0]) + @this.Substring(1);
		}
	}

	public class GdprDeleteFilesArgument
	{
		public string Filename { get; set; }
		public string Command { get; set; }
	}
}
