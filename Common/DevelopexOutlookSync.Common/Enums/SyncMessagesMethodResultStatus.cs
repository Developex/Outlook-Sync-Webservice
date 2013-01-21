using System;
using System.Runtime.Serialization;

namespace DevelopexOutlookSync.Common.Enums
{
	[DataContract]
	public enum SyncMessagesMethodResultStatus
	{
		[EnumMember]
		Ok = 0,
		[EnumMember]
		Failed = 1
	}
}
