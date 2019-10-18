using System.Security;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using File = System.IO.File;

namespace GdprService
{
	public class SharePointFileHelper : IFileHelper
	{
		public async Task Delete(ScannedFile file)
		{
			var sharePointSiteUrl = "https://sharepoint.forca.com/sites/test";

			using (ClientContext ctx = new ClientContext(sharePointSiteUrl))
			{
				string password = "*****";
				string account = "user@tenant.onmicrosoft.com";
				var secret = new SecureString();
				foreach (char c in password)
				{
					secret.AppendChar(c);
				}
				ctx.Credentials = new SharePointOnlineCredentials(account, secret);

				//var mylibrary = ctx.Web.Lists.GetByTitle("Documents");
				//FileCollection files = mylibrary.RootFolder.Folders.GetByUrl("/sites/dev/shared documents/folder1").Files;

				//ctx.Load(files);
				//ctx.ExecuteQuery();

				var spFile = ctx.Web.GetFileByUrl(file.Filename);
				ctx.Load(spFile);
				spFile.DeleteObject();
				await ctx.ExecuteQueryAsync();

				//foreach (Microsoft.SharePoint.Client.File file in files)
				//{
				//	FileInformation fileinfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(ctx, file.ServerRelativeUrl);

				//	ctx.ExecuteQuery();

				//	using (FileStream filestream = new FileStream("D:" + "\\" + file.Name, FileMode.Create))
				//	{
				//		fileinfo.Stream.CopyTo(filestream);
				//	}

				//}
				//files.ToList().ForEach(file => file.DeleteObject());
			};
		}

		public string ReadAllText(string path)
		{
			return File.ReadAllText(path);
		}
	}
}