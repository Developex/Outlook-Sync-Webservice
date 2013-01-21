using DevelopexOutlookSync.Common.Dto;
using OutlookSyncService.Library;

namespace OutlookSyncService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OutlookSyncService" in code, svc and config file together.
	public class OutlookSyncService : IOutlookSyncService
	{
		public LoginMethodResultDto Login(LoginMethodParametersDto pars)
		{
			return Helper.MailsController.Login(pars);
		}

		public SyncMessagesMethodResultDto SyncMessages(SyncMessagesMethodParametersDto pars)
		{
			return Helper.MailsController.SyncMessages(pars);
		}
	}
}
