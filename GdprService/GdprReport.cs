using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GdprService
{
	public interface IGdprReport
	{
		Task RegisterDeleted(ScannedFile file);
		Task RegisterNotDeleted(ScannedFile file, string reason);
		Task RegisterFailed(ScannedFile file, string reason);
		string Results();
	}

	public class GdprReport : IGdprReport
	{
		private readonly List<ActionResult> results;

		public GdprReport()
		{
			this.results = new List<ActionResult>();
		}

		public string Results()
		{
			var builder = new StringBuilder();

			builder.AppendLine("The following files failed: ");
			this.results.Where(r => r.Status == GdprActionStatus.Failed).ToList().ForEach(f => builder.AppendLine(f.Reason));
			builder.AppendLine();

			builder.AppendLine("The following files WAS NOT deleted: ");
			this.results.Where(r => r.Status == GdprActionStatus.NotDeleted).ToList().ForEach(f => builder.AppendLine(f.Reason));
			builder.AppendLine();
			
			builder.AppendLine("The following files was deleted: ");
			this.results.Where(r => r.Status == GdprActionStatus.Deleted).ToList().ForEach(f => builder.AppendLine(f.ScannedFile.Filename));
			builder.AppendLine();

			return builder.ToString();
		}

		public async Task RegisterDeleted(ScannedFile file)
		{
			await Task.Run(
				() =>
				{
					this.results.Add(new ActionResult(file, GdprActionStatus.Deleted));
				});
		}

		public async Task RegisterNotDeleted(ScannedFile file, string reason)
		{
			await Task.Run(
				() =>
				{
					this.results.Add(new ActionResult(file, GdprActionStatus.NotDeleted, reason));
				});
		}

		public async Task RegisterFailed(ScannedFile file, string reason)
		{
			await Task.Run(
				() =>
				{
					this.results.Add(new ActionResult(file, GdprActionStatus.Failed, reason));
				});
		}
	}

	public enum GdprActionStatus
	{
		NotSet,
		Deleted,
		NotDeleted,
		Failed
	}

	public class ActionResult
	{
		public ScannedFile ScannedFile { get; }
		public GdprActionStatus Status { get; }
		public string Reason { get; }

		public ActionResult(ScannedFile scannedFile, GdprActionStatus status, string reason = "")
		{
			Status = status;
			Reason = reason;
			ScannedFile = scannedFile;
		}
	}
}
