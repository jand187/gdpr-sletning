using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GdprDeleteFiles;
using NUnit.Framework;

namespace GdprDeleteFilesTests
{
	public class GdprDeleteFilesArgumentParserTests
	{
		[Test]
		public void Parse_should_return_GdprDeleteFilesArgument_show()
		{
			var target = new GdprDeleteFilesArgumentParser();
			GdprDeleteFilesArgument arg = target.Parse(new string[] {"show", "-f", "someFile.txt"});

			Assert.That(arg.Command, Is.EqualTo("Show"));
			Assert.That(arg.Filename, Is.EqualTo("someFile.txt"));
		}

		[Test]
		public void Parse_should_return_GdprDeleteFilesArgument_delete()
		{
			var target = new GdprDeleteFilesArgumentParser();
			GdprDeleteFilesArgument arg = target.Parse(new string[] {"delete", "-f", "someFile.txt"});

			Assert.That(arg.Command, Is.EqualTo("Delete"));
			Assert.That(arg.Filename, Is.EqualTo("someFile.txt"));
		}

	}
}
