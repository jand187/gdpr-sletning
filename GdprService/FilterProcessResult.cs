namespace GdprService
{
	public class FilterProcessResult
	{
		public bool Status { get; }
		public ScannedFile File { get; }
		public string Reason { get; }

		public FilterProcessResult(bool status, string reason, ScannedFile file)
		{
			Status = status;
			File = file;
			if (!status)
			{
				Reason = reason;
			}
		}
	}
}