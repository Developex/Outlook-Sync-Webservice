using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace DevelopexOutlookSync.Common.Dto
{
	[DataContract]
	public class SyncMessagesMethodParametersDto
	{
		[DataMember]
		public Guid SessionId;

		[DataMember]
		public string ClientId;

		[DataMember]
		public List<MailItemDto> Items;
	}
}
