using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GdprService
{
	public interface ICsvReader
	{
		Task<IEnumerable<ScannedFile>> Parse(string filename);
	}

	public class CsvReader : ICsvReader
	{
		private readonly IFileHelper fileHelper;
		private readonly IScannedFileMapper scannedFileMapper;
		private readonly IFileReader fileReader;

		public CsvReader(IFileHelper fileHelper, IScannedFileMapper scannedFileMapper, IFileReader fileReader)
		{
			this.fileHelper = fileHelper;
			this.scannedFileMapper = scannedFileMapper;
			this.fileReader = fileReader;
		}

		public async Task<IEnumerable<ScannedFile>> Parse(string filename)
		{
			var contents = this.fileReader.ReadAllText(filename);
			var rawLines = contents.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries);
			return await Task.WhenAll(rawLines.Skip(1).Select(line => this.scannedFileMapper.Map(line)));
		}
	}
}
