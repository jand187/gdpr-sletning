namespace GdprService
{
	public interface IScannedFileMapper
	{
		ScannedFile Map(string line);
	}
}