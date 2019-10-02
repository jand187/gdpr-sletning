using System;
using System.Collections.Generic;
using System.Linq;

namespace GdprService
{
	public interface ICsvReader
	{
		IEnumerable<ScannedFile> Parse(string filename);
	}

	public class CsvReader : ICsvReader
	{
		private readonly IFileHelper fileHelper;
		private readonly IScannedFileMapper scannedFileMapper;

		public CsvReader(IFileHelper fileHelper, IScannedFileMapper scannedFileMapper)
		{
			this.fileHelper = fileHelper;
			this.scannedFileMapper = scannedFileMapper;
		}

		public IEnumerable<ScannedFile> Parse(string filename)
		{
			var contents = this.fileHelper.ReadAllText(filename);
			var rawLines = contents.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
			return rawLines.Skip(1).Select(line => this.scannedFileMapper.Map(line)).ToList();
		}
	}
}
