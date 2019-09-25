namespace CsvReader
{
	public interface IFileDataFactory
	{
		IFileData Create(string filename);
	}
}