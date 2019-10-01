using System.IO;
using GdprService;
using NSubstitute;
using NUnit.Framework;

namespace GdprServiceTests
{
	public class GdprServiceTests
	{
		private IFileHelper fileHelper;
		private GdprService.GdprService gdprService;

		[SetUp]
		public void Setup()
		{
			this.fileHelper = Substitute.For<IFileHelper>();
			this.gdprService = new GdprService.GdprService(this.fileHelper, Substitute.For<ILogger>());
		}

		[Test]
		public void DeleteFile_should_call_FileHelper_Delete()
		{
			var scannedFile1 = new ScannedFile();

			this.gdprService.DeleteFiles(
				new[]
				{
					scannedFile1
				});

			this.fileHelper.Received(1).Delete(scannedFile1);
		}

		[Test]
		public void DeleteFile_should_log_error_when_file_not_found()
		{
			var scannedFile1 = new ScannedFile();

			this.fileHelper.When(fh => fh.Delete(Arg.Any<ScannedFile>())).Do(fh => throw new FileNotFoundException());

			Assert.Throws<FileNotFoundException>(
				() => this.gdprService.DeleteFiles(
					new[]
					{
						scannedFile1
					}));
		}
	}
}
