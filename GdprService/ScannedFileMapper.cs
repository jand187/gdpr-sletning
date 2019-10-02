namespace GdprService
{
	public interface IScannedFileMapper
	{
		ScannedFile Map(string line);
	}

	internal class ScannedFileMapper : IScannedFileMapper
	{
		public ScannedFile Map(string line)
		{
			return new ScannedFile();
		}
	}
}
