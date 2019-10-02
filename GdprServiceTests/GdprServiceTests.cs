using System;
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
		private ILogger logger;

		[SetUp]
		public void Setup()
		{
			this.fileHelper = Substitute.For<IFileHelper>();
			this.logger = Substitute.For<ILogger>();
			this.gdprService = new GdprService.GdprService(this.fileHelper, this.logger);
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
		public void DeleteFile_should_catch_and_log_file_not_found_exception()
		{
			var exception = new FileNotFoundException();
			this.fileHelper.When(fh => fh.Delete(Arg.Any<ScannedFile>())).Do(fh => throw exception);

			this.gdprService.DeleteFiles(
				new[]
				{
					new ScannedFile()
				});

			this.logger.Received(1).LogError(exception.Message, exception);
		}

		[Test]
		public void DeleteFile_should_catch_and_log_unauthorized_access_exception()
		{
			var exception = new UnauthorizedAccessException();
			this.fileHelper.When(fh => fh.Delete(Arg.Any<ScannedFile>())).Do(fh => throw exception);

			this.gdprService.DeleteFiles(
				new[]
				{
					new ScannedFile()
				});

			this.logger.Received(1).LogError(exception.Message, exception);
		}

		[Test]
		public void DeleteFile_should_throw_exceptions()
		{
			this.fileHelper.When(fh => fh.Delete(Arg.Any<ScannedFile>())).Do(fh => throw new Exception());

			Assert.Throws<Exception>(
				() => this.gdprService.DeleteFiles(
					new[]
					{
						new ScannedFile()
					}));
		}
	}
}


/*
 *

	Assert.Throws<FileNotFoundException>(
				() => this.gdprService.DeleteFiles(
					new[]
					{
						scannedFile1
					}));

 * 
 */
