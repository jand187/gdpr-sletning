using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GdprDeleteFiles;
using NUnit.Framework;

namespace GdprDeleteFilesTests
{
	public class ConsoleCommandFactoryTests
	{
		[Test]
		public void Create_must_create_ShowFilesCommand()
		{
			var target = new ConsoleCommandFactory();
			Assert.Fail();
			// target.Create(new GdprDeleteFilesArgument{Command = "Show", "some"})
		}

	}
}
