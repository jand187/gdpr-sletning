using System.Threading.Tasks;

namespace GdprService
{
	public interface IScannedFileMapper
	{
		Task<ScannedFile> Map(string line);
	}

	public class ScannedFileMapper : IScannedFileMapper
	{
		public async Task<ScannedFile> Map(string line)
		{
			var fields = line.Split(';');

			return await Task.Run(() => new ScannedFile
			{
				Repository = fields[(int) CsvFields.Repository],
				Filename = fields[(int) CsvFields.FileName],
				Status = fields[(int) CsvFields.Status],
				Comment = fields[(int) CsvFields.Comment]
			});
		}
	}

	public enum CsvFields
	{
		Repository = 0,
		FileName = 1,
		Status = 2,
		Comment = 3,
		CurrentLabel = 4,
		CurrentLabelId = 5,
		AppliedLabel = 6,
		AppliedLabelId = 7,
		ConditionName = 8,
		MatchedString = 9,
		InformationTypeName = 10,
		MatchedInformationTypesString = 11,
		Action = 12,
		LastModified = 13,
		LastModifiedBy = 14,
		ProtectionBeforeAction = 15,
		ProtectionAfterAction = 16
	}
}
