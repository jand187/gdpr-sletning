namespace GdprService
{
	public class FilterProcessResult
	{
		public bool Status { get; }
		public string Reason { get; }

		public FilterProcessResult(bool status, string reason)
		{
			Status = status;
			if (!status)
			{
				Reason = reason;
			}
		}
	}
}