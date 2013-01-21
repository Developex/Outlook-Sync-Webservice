using System.ServiceModel;
using DevelopexOutlookSync.Common.Dto;

namespace OutlookSyncService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IOutlookSyncService" in both code and config file together.
	[ServiceContract]
	public interface IOutlookSyncService
	{
		[OperationContract]
		LoginMethodResultDto Login(LoginMethodParametersDto pars);

		[OperationContract]
		SyncMessagesMethodResultDto SyncMessages(SyncMessagesMethodParametersDto pars);
	}
}
