using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvReader;
using NSubstitute;
using NUnit.Framework;

namespace CsvReaderTests
{
	public class DefaultReaderTests
	{
		private IFileHelper fileHelperMock;
		private IReaderOptions optionsMock;

		[SetUp]
		public void SetUp()
		{
			this.optionsMock = Substitute.For<IReaderOptions>();
			this.fileHelperMock = Substitute.For<IFileHelper>();
		}

		[Test]
		public async Task Parse_should_read_headlines()
		{
			var target = new DefaultReader(this.optionsMock, this.fileHelperMock);

			//IDataReader reader = target.Parse(new StringBuilder().ToString());

			var dt = new DataTable();
		}

		[Test]
		public async Task GetFiles_should_returns_6_entries()
		{
			var target = new DefaultReader(this.optionsMock, this.fileHelperMock);

			this.optionsMock.FirstRowIsHeader.Returns(true);

			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Repository;File Name;Status;Comment;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc001.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc002.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc003.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc004.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc005.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc006.docx;Success;");

			var files = await target.GetFiles(stringBuilder.ToString());

			Assert.That(files.Count(), Is.EqualTo(6));
		}

		[Test]
		public async Task GetFiles_should_returns_testdoc001_and_testdoc006()
		{
			var target = new DefaultReader(this.optionsMock, this.fileHelperMock);

			this.optionsMock.FirstRowIsHeader.Returns(true);

			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Repository;File Name;Status;Comment;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc001.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc002.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc003.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc004.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc005.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc006.docx;Success;");

			var files = await target.GetFiles(stringBuilder.ToString());

			Assert.That(files, Contains.Item(@"C:\AIPScanner\testdoc001.docx"));
			Assert.That(files, Contains.Item(@"C:\AIPScanner\testdoc006.docx"));
		}

		[Test]
		public async Task GetFiles_should_return_filtered_files()
		{
			this.optionsMock.FirstRowIsHeader.Returns(true);
			var target = new DefaultReader(this.optionsMock, this.fileHelperMock);

			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Repository;File Name;Status;Comment;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc001.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc002.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc003.docx;Success;");

			this.fileHelperMock.GetCreateDate(Arg.Is<FileInfo>( fi => fi.FullName == @"C:\AIPScanner\testdoc001.docx"))
				.Returns(DateTime.Now.FiveYearsAgo().TwoDaysBefore());

			this.fileHelperMock.GetCreateDate(Arg.Is<FileInfo>( fi => fi.FullName == @"C:\AIPScanner\testdoc002.docx"))
				.Returns(DateTime.Now.FiveYearsAgo());

			this.fileHelperMock.GetCreateDate(Arg.Is<FileInfo>( fi => fi.FullName == @"C:\AIPScanner\testdoc003.docx"))
				.Returns(DateTime.Now.FiveYearsAgo().TwoDaysAfter());

			var files = await target.GetFiles(
				stringBuilder.ToString(),
				new FileLastModifiedDateIsBeforeFilter(DateTime.Now.FiveYearsAgo()));

			Assert.That(files, Contains.Item(@"C:\AIPScanner\testdoc001.docx"));
			Assert.That(files.Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task GetFiles_should_read_csv_contents_and_return_file_names()
		{
			var csvFile = new FileInfo(@"DetailedReport_2019-09-20_12_42_38.csv");

			this.optionsMock.FirstRowIsHeader.Returns(true);
			var target = new DefaultReader(this.optionsMock, this.fileHelperMock);

			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Repository;File Name;Status;Comment;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc001.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc002.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc003.docx;Success;");

			this.fileHelperMock.ReadAllText(csvFile).Returns(stringBuilder.ToString());

			var files = await target.GetFiles(csvFile);

			Assert.That(files, Contains.Item(@"C:\AIPScanner\testdoc001.docx"));
			Assert.That(files, Contains.Item(@"C:\AIPScanner\testdoc002.docx"));
			Assert.That(files, Contains.Item(@"C:\AIPScanner\testdoc003.docx"));
		}
	}


	public static class DateTimeExtensions
	{
		public static DateTime FiveYearsAgo(this DateTime @this)
		{
			return @this.AddYears(-5);
		}

		public static DateTime TwoDaysBefore(this DateTime @this)
		{
			return @this.AddDays(-2);
		}

		public static DateTime TwoDaysAfter(this DateTime @this)
		{
			return @this.AddDays(2);
		}
	}
}
