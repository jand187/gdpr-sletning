using System;
using System.Data.SqlClient;

namespace CsvReader
{
	public class ScannedFile
	{
		public string Action { get; set; }
		public string AppliedLabel { get; set; }
		public string AppliedLabelId { get; set; }
		public string Comment { get; set; }
		public string ConditionName { get; set; }
		public string CurrentLabel { get; set; }
		public string CurrentLabelId { get; set; }
		public string FileName { get; set; }
		public string InformationTypeName { get; set; }
		public DateTime LastModified { get; set; }
		public string LastModifiedBy { get; set; }
		public string MatchedInformationTypesString { get; set; }
		public string MatchedString { get; set; }
		public string ProtectionAfterAction { get; set; }
		public string ProtectionBeforeAction { get; set; }
		public string Repository { get; set; }
		public string Status { get; set; }
		public IFileData FileData { get; set; }

		public static ScannedFile CreateFromCsvLine(string s)
		{
			var columns = s.Split(';');

			return new ScannedFile
			{
				Repository = columns[0],
				FileName= columns[1],
				Status= columns[2],
				Comment= columns[3],
				CurrentLabel= columns[4],
				CurrentLabelId = columns[5],
				AppliedLabel= columns[6],
				AppliedLabelId = columns[7],
				ConditionName= columns[8],
				MatchedString= columns[9],
				InformationTypeName= columns[10],
				MatchedInformationTypesString= columns[11],
				Action= columns[12],
				LastModified = DateTime.Parse(columns[13]),
				LastModifiedBy= columns[14],
				ProtectionBeforeAction= columns[15],
				ProtectionAfterAction= columns[16]
			};
		}
	}
}
