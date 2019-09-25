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

		public DefaultReader(IReaderOptions options, IFileHelper fileHelper, IFileDataFactory fileDataFactoryMock)
		{
			this.options = options;
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
	}
}
