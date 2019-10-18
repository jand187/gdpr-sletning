using System.Linq;
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
		private IFileReader fileReader;

		[SetUp]
		public void Setup()
		{
			this.fileHelper = Substitute.For<IFileHelper>();
			this.scannedFileMapper = Substitute.For<IScannedFileMapper>();
			this.fileReader = Substitute.For<IFileReader>();
			this.target = new CsvReader(this.fileHelper, this.scannedFileMapper, this.fileReader);
		}

		[Test]
		public async Task Parse_should_call_FileHelper()
		{
			var filename = "SomeFile.csv";

			await this.target.Parse(filename);

			this.fileReader.Received(1).ReadAllText(filename);
		}

		[Test]
		public async Task Parse_should__call_ScannedFileMapper_ignoring_headers()
		{
			var csvFile = "SomeFile.csv";
			var headerLine = "Repository;File Name;Status;Comment;";
			var firstLine = "some repo;my file name.txt;all-ok;nothing to see here;";
			var contents = new StringBuilder().AppendLine(headerLine).AppendLine(firstLine).ToString();
			this.fileReader.ReadAllText(csvFile).Returns(contents);

			await this.target.Parse(csvFile);

			Received.InOrder(async () => await this.scannedFileMapper.Map(firstLine));
		}
	}
}
