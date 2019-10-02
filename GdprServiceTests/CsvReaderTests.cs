using System.Text;
using System.Threading.Tasks;
using GdprService;
using NSubstitute;
using NUnit.Framework;

namespace GdprServiceTests
{
	public class CsvReaderTests
	{
		private IFileHelper fileHelper;
		private IScannedFileMapper scannedFileMapper;
		private CsvReader target;

		[SetUp]
		public void Setup()
		{
			this.fileHelper = Substitute.For<IFileHelper>();
			this.scannedFileMapper = Substitute.For<IScannedFileMapper>();
			this.target = new CsvReader(this.fileHelper, this.scannedFileMapper);
		}

		[Test]
		public async Task Parse_should_call_FileHelper()
		{
			var filename = "Somefile.csv";

			await this.target.Parse(filename);

			this.fileHelper.Received(1).ReadAllText(filename);
		}

		[Test]
		public async Task Parse_should_Return_call_ScannedFileMapper_ignoring_headers()
		{
			var filename = "Somefile.csv";
			var headerline = "Repository;File Name;Status;Comment;";
			var firstLine = "some repo;my file name.txt;all-ok;nothing to see here;";
			var contents = new StringBuilder().AppendLine(headerline).AppendLine(firstLine).ToString();
			this.fileHelper.ReadAllText(filename).Returns(contents);
			
			await this.target.Parse(filename);

			this.scannedFileMapper.Received(1).Map(firstLine);
		}
	}
}
