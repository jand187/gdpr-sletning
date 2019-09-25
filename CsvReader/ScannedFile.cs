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
		public string LastModified { get; set; }
		public string LastModifiedBy { get; set; }
		public string MatchedInformationTypesString { get; set; }
		public string MatchedString { get; set; }
		public string ProtectionAfterAction { get; set; }
		public string ProtectionBeforeAction { get; set; }
		public string Repository { get; set; }
		public string Status { get; set; }

		public static ScannedFile CreateFromCsvLine(string s)
		{
			return new ScannedFile();
		}
	}
}
