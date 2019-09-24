namespace CsvReader
{
	public interface IReaderOptions
	{
		bool FirstRowIsHeader { get; set; }
	}

	class DefaultReaderOptions : IReaderOptions
	{
		public bool FirstRowIsHeader { get; set; } = true;
	}
}