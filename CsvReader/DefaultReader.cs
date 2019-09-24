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
		private readonly IReaderOptions options;
		private readonly IFileHelper fileHelper;

		public DefaultReader(IReaderOptions options, IFileHelper fileHelper)
		{
			this.options = options;
			this.fileHelper = fileHelper;
		}

		public async Task<IEnumerable<string>> GetFiles(string input, params IFileFilter<DateTime>[] filters)
		{
			IEnumerable<string> rawLines = input.Split(new[] {"\n", "\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToList();
			var table = new DataTable();
			
			if (this.options.FirstRowIsHeader)
			{
				var columnNames = rawLines.First().Split(new []{",", ";"}, StringSplitOptions.RemoveEmptyEntries);
				table.Columns.AddRange(columnNames.Select(c=> new DataColumn(c)).ToArray());
				rawLines = rawLines.Skip(1);
			}

			rawLines.Select(l => table.Rows.Add(l.Split(new[] {",", ";"}, StringSplitOptions.None))).ToList();

			var files = table.AsEnumerable().Select(r => r.Field<string>("File Name"));

			return files.Where(f => filters.All(filter => filter.IsValid(this.fileHelper.GetCreateDate(new FileInfo(f)))));
		}

		public async Task<IEnumerable<string>> GetFiles(FileInfo file, params IFileFilter<DateTime>[] filters)
		{
			return await GetFiles(this.fileHelper.ReadAllText(file), filters);
		}
	}

	public interface IReaderOptions
	{
		bool FirstRowIsHeader { get; set; }
	}

	public class FileIsOlderThanFilter : IFileFilter<DateTime>
	{
		private readonly DateTime createdDate;

		public FileIsOlderThanFilter(DateTime createdDate)
		{
			this.createdDate = createdDate;
		}

		public bool IsValid(DateTime target)
		{
			return this.createdDate > target;
		}
	}

	public interface IFileFilter<TEntity>
	{
		bool IsValid(TEntity target);
	}

	public interface IFileHelper
	{
		DateTime GetCreateDate(FileInfo file);
		string ReadAllText(FileInfo file);
	}
}
