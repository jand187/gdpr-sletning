﻿using System.IO;
using System.Threading.Tasks;

namespace GdprService
{
	public interface IFileHelper
	{
		Task Delete(ScannedFile file);
		string ReadAllText(string path);
	}

	public class FileHelper : IFileHelper
	{
		private readonly ILogger logger;

		public FileHelper(ILogger logger)
		{
			this.logger = logger;
		}

		public async Task Delete(ScannedFile file)
		{
			this.logger.Log($"Deleting file '{file.Filename}'.");
			File.Delete(file.Filename);
		}

		public string ReadAllText(string path)
		{
			return File.ReadAllText(path);
		}
	}
}
