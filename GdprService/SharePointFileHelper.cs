using System.Security;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace GdprService
{
	public class SharePointFileHelper : IFileHelper
	{
		private string sharePointSiteUrl;

		public SharePointFileHelper()
		{
			sharePointSiteUrl = "http://sharepoint/gdprsletningtest";
		}

		public async Task Delete(ScannedFile file)
		{
			using (var context = new ClientContext(this.sharePointSiteUrl))
			{
				var spFile = context.Web.GetFileByServerRelativeUrl(file.Filename);
				context.Load(spFile);
				spFile.DeleteObject();
				await context.ExecuteQueryAsync();
			}
		}
	}
}
