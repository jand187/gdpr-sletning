using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CsvReader
{
	public class DefaultReader
	{
		private readonly IFileDataFactory fileDataFactoryMock;
		private readonly IFileHelper fileHelper;
		private readonly IReaderOptions options;

		public DefaultReader(IFileHelper fileHelper,
			IFileDataFactory fileDataFactoryMock,
			IReaderOptions options = null)
		{
			this.options = options ?? new DefaultReaderOptions();
			this.fileHelper = fileHelper;
			this.fileDataFactoryMock = fileDataFactoryMock;
		}

		public async Task<IEnumerable<string>> GetFiles(string input, params IFileFilter[] filters)
		{
			IEnumerable<string> rawLines =
				input.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToList();
			var table = new DataTable();

			if (this.options.FirstRowIsHeader)
			{
				var columnNames = rawLines.First().Split(new[] {",", ";"}, StringSplitOptions.RemoveEmptyEntries);
				table.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());
				rawLines = rawLines.Skip(1);
			}

			rawLines.ToList().ForEach(l => table.Rows.Add(l.Split(new[] {",", ";"}, StringSplitOptions.None)));

			var files = table.AsEnumerable().Select(r => r.Field<string>("File Name")).ToList();

			return files.Where(file => filters.All(filter => filter.IsValid(this.fileDataFactoryMock.Create(file))))
				.ToList();
		}

		public async Task<IEnumerable<string>> GetFiles(FileInfo file, params IFileFilter[] filters)
		{
			return await GetFiles(this.fileHelper.ReadAllText(file), filters);
		}

		public async Task<IEnumerable<ScannedFile>> CreateFileSet(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new FileNotFoundException($"The file {file.FullName} was not found");
			}
			var contents = File.ReadAllText(file.FullName);
			return await CreateFileSet(contents);
		}

		public async Task<IEnumerable<ScannedFile>> CreateFileSet(string fileContents)
		{
			var rawLines = fileContents.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries).Skip(1);

			return rawLines.Select(r => ScannedFile.CreateFromCsvLine(r));
		}

		public async Task<IEnumerable<ScannedFile>> ApplyFilters(IEnumerable<ScannedFile> files, params IFileFilter[] filters)
		{
			return files.Where(file => filters.All(filter => filter.IsValid(file)));
		}
	}
}
