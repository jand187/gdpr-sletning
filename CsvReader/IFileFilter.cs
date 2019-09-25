namespace CsvReader
{
	public interface IFileFilter
	{
		bool IsValid(IFileData file);
	}
}
