using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace GdprService
{
	public class SharePointFileHelper : IFileHelper
	{
		public async Task Delete(ScannedFile file)
		{
			using (var context = new ClientContext(file.Repository))
			{
				var serverRelativeUrl = GetRelativeUrl(file);
				var spFile = context.Web.GetFileByServerRelativeUrl(serverRelativeUrl);
				context.Load(spFile);
				spFile.DeleteObject();
				await context.ExecuteQueryAsync();
			}
		}

		private string GetRelativeUrl(ScannedFile file)
		{
			var path = Regex.Replace(file.Filename, @"^http:\/\/[^\/]+", "");
			return path;
		}
	}
}
