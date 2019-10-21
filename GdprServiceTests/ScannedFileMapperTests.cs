using System;
using System.Threading.Tasks;
using GdprService;
using NSubstitute;
using NUnit.Framework;

namespace GdprServiceTests
{
	public class ScannedFileMapperTests
	{
		private ScannedFileMapper target;
		private ILogger logger;

		[SetUp]
		public void Setup()
		{
			this.logger = Substitute.For<ILogger>();
			this.target = new ScannedFileMapper(this.logger);
		}

		[Test]
		public async Task Map_should_return_ScannedFile()
		{
			var firstFile = "my file name.txt";
			var repository = "some repo";
			var status = "all-ok";
			var comment = "nothing to see here";
			var lastModified = new DateTime(2013, 1, 1);
			var firstLine = $"{repository};{firstFile};{status};{comment};;;;;;;;;;{lastModified};";

			var file = await target.Map(firstLine);

			Assert.That(file.Repository, Is.EqualTo(repository));
			Assert.That(file.Filename, Is.EqualTo(firstFile));
			Assert.That(file.Status, Is.EqualTo(status));
			Assert.That(file.Comment, Is.EqualTo(comment));
			Assert.That(file.LastModified, Is.EqualTo(lastModified));
			
		}
	}
}
