using System.IO;

namespace GdprService
{
	internal class FileReader : IFileReader
	{
		public string ReadAllText(string path)
		{
			return File.ReadAllText(path);
		}
	}
}
