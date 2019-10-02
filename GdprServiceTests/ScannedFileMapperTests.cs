using System.Threading.Tasks;
using GdprService;
using NUnit.Framework;

namespace GdprServiceTests
{
	public class ScannedFileMapperTests
	{
		private ScannedFileMapper target;

		[SetUp]
		public void Setup()
		{
			this.target = new ScannedFileMapper();
		}

		[Test]
		public async Task Map_should_return_ScannedFile()
		{
			var firstFile = "my file name.txt";
			var repository = "some repo";
			var status = "all-ok";
			var comment = "nothing to see here";
			var firstLine = $"{repository};{firstFile};{status};{comment};";

			var file = await target.Map(firstLine);

			Assert.That(file.Repository, Is.EqualTo(repository));
			Assert.That(file.Filename, Is.EqualTo(firstFile));
			Assert.That(file.Status, Is.EqualTo(status));
			Assert.That(file.Comment, Is.EqualTo(comment));
			
		}
	}
}
