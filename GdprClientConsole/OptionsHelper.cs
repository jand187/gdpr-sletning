using System.Linq;

namespace GdprClientConsole
{
	public static class OptionsHelper
	{
		public static string GetOptionParameter(string[] args, string option)
		{
			var optionFIndex = args.ToList().IndexOf(option);
			var filenameArg = args.Skip(optionFIndex + 1).Take(1).Single();
			return filenameArg;
		}

		public static bool GetSwitch(string[] args, string @switch)
		{
			return args.Any(a => a == @switch);
		}
	}
}