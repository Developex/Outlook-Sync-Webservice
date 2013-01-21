using System.Web.Hosting;
using OutlookSyncService;
using OutlookSyncService.Library;

public class MailsControllerImpl : MailsController
{
	public const string MESSAGES_FOLDER = "Messages";

	public override string MailsPath
	{
		get
		{
			return HostingEnvironment.MapPath(@"~/" + MESSAGES_FOLDER);
		}
	}
}