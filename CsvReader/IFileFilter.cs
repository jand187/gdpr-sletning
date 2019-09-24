namespace CsvReader
{
	public interface IFileFilter<TEntity>
	{
		bool IsValid(TEntity target);
	}
}