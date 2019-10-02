namespace GdprService
{
	public interface IFileHelper
	{
		void Delete(ScannedFile file);
		string ReadAllText(string path);
	}

	public class FileHelper : IFileHelper
	{
		public void Delete(ScannedFile file)
		{
			throw new System.NotImplementedException();
		}

		public string ReadAllText(string path)
		{
			throw new System.NotImplementedException();
		}
	}
}