namespace CsvReader
{
	public interface IFileFilter
	{
		bool IsValid(IFileData file);
		bool IsValid(ScannedFile file);
	}
}
