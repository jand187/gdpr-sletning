using System;

namespace GdprService
{
	public class ScannedFile
	{
		public string Repository { get; set; }
		public string Filename { get; set; }
		public string Status { get; set; }
		public string Comment { get; set; }
		public DateTime LastModified { get; set; }
		public string AppliedLabel { get; set; }
		public string InformationTypeName { get; set; }
	}
}