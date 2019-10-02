﻿using System;
using System.IO;
using System.Threading.Tasks;
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
		public async Task DeleteFile_should_call_FileHelper_Delete()
		{
			var scannedFile1 = new ScannedFile();

			await this.gdprService.DeleteFiles(
				new[]
				{
					scannedFile1
				});

			this.fileHelper.Received(1).Delete(scannedFile1);
		}

		[Test]
		public async Task DeleteFile_should_catch_and_log_file_not_found_exception()
		{
			var exception = new FileNotFoundException();
			this.fileHelper.When(fh => fh.Delete(Arg.Any<ScannedFile>())).Do(fh => throw exception);

			await this.gdprService.DeleteFiles(
				new[]
				{
					new ScannedFile()
				});

			this.logger.Received(1).LogError(exception.Message, exception);
		}

		[Test]
		public async Task DeleteFile_should_catch_and_log_unauthorized_access_exception()
		{
			var exception = new UnauthorizedAccessException();
			this.fileHelper.When(fh => fh.Delete(Arg.Any<ScannedFile>())).Do(fh => throw exception);

			await this.gdprService.DeleteFiles(
				new[]
				{
					new ScannedFile()
				});

			this.logger.Received(1).LogError(exception.Message, exception);
		}

		[Test]
		public async Task DeleteFile_should_throw_exceptions()
		{
			this.fileHelper.When(fh => fh.Delete(Arg.Any<ScannedFile>())).Do(fh => throw new Exception());

			Assert.ThrowsAsync<Exception>(
				() => this.gdprService.DeleteFiles(
					new[]
					{
						new ScannedFile()
					}));
		}
	}
}
