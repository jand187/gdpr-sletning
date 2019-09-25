using System;
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
		private IFileDataFactory fileDataFactoryMock;
		private IFileHelper fileHelperMock;
		private IFileFilter matchAllFilesFilterMock;
		private IReaderOptions optionsMock;
		private DefaultReader target;

		[SetUp]
		public void SetUp()
		{
			this.fileHelperMock = Substitute.For<IFileHelper>();
			this.fileDataFactoryMock = Substitute.For<IFileDataFactory>();
			this.matchAllFilesFilterMock = Substitute.For<IFileFilter>();

			this.target = new DefaultReader(this.fileHelperMock, this.fileDataFactoryMock);

			this.matchAllFilesFilterMock.IsValid(Arg.Any<IFileData>()).Returns(true);
		}

		[Test]
		public async Task GetFiles_should_call_all_filters_on_all_files()
		{
			var input = CreateCsvFor3Files();

			var fileFilter1 = Substitute.For<IFileFilter>();
			fileFilter1.IsValid(Arg.Any<IFileData>()).Returns(true);

			var fileFilter2 = Substitute.For<IFileFilter>();
			fileFilter2.IsValid(Arg.Any<IFileData>()).Returns(true);

			var fileFilter3 = Substitute.For<IFileFilter>();
			fileFilter3.IsValid(Arg.Any<IFileData>()).Returns(true);

			var files = await this.target.GetFiles(input, fileFilter1, fileFilter2, fileFilter3);

			fileFilter1.Received(3).IsValid(Arg.Any<IFileData>());
			fileFilter2.Received(3).IsValid(Arg.Any<IFileData>());
			fileFilter3.Received(3).IsValid(Arg.Any<IFileData>());

			Assert.That(files.Count(), Is.EqualTo(3));
		}

		[Test]
		public async Task GetFiles_should_read_csv_contents_and_return_file_names()
		{
			var csvFile = new FileInfo(@"somedir\somefile.csv");
			var input = CreateCsvFor3Files();
			this.fileHelperMock.ReadAllText(csvFile).Returns(input);

			var files = await this.target.GetFiles(csvFile, this.matchAllFilesFilterMock);

			Assert.That(files, Contains.Item(@"C:\AIPScanner\testdoc001.docx"));
			Assert.That(files, Contains.Item(@"C:\AIPScanner\testdoc002.docx"));
			Assert.That(files, Contains.Item(@"C:\AIPScanner\testdoc003.docx"));
		}

		[Test]
		public async Task CreateFileList_should_return_list_with_all_files()
		{
			var fileContents = new StringBuilder()
				.AppendLine(
					@"Repository;File Name;Status;Comment;Current Label;Current Label ID;Applied Label;Applied Label ID;Condition Name;Matched String;Information Type Name;Matched Information Types String;Action;Last Modified;Last Modified By;Protection Before Action;Protection After Action")
				.AppendLine(
					@"\\virinffilpf0001\Afdeling;\\virinffilpf0001\Afdeling\AIA\samsung2.pdf;Success;No conditions match for this file;Not set;;;;CPR-nummer;[CPR-nummer: XXXXXXXXXX,XXXXXX-XXXX];CPR-nummer;[CPR-nummer: XXXXXXXXXX,XXXXXX-XXXX];;2006-08-16 14:02:17Z;;false;");

			var files = await this.target.CreateFileSet(fileContents.ToString());

			Assert.That(files.Single().FileName, Is.EqualTo(@"\\virinffilpf0001\Afdeling\AIA\samsung2.pdf"));
		}

		[Test]
		public async Task ApplyFilter_should_filter_out_unmatched_files()
		{
			const string oldFile_name = "older than five years.pdf";
			const string youngFile_name = "not older than five years.pdf";

			var files = new[]
			{
				new ScannedFile {FileName = oldFile_name},
				new ScannedFile {FileName = youngFile_name}
			};

			var filteredFiles = await this.target.ApplyFilters(
				files,
				new GenericFilter(f => f.FileName == oldFile_name));

			Assert.That(filteredFiles.First().FileName, Is.EqualTo(oldFile_name));
		}


		private static string CreateCsvFor3Files()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Repository;File Name;Status;Comment;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc001.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc002.docx;Success;");
			stringBuilder.AppendLine(@"C:\AIPScanner;C:\AIPScanner\testdoc003.docx;Success;");
			return stringBuilder.ToString();
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
