using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DevelopexOutlookSync.Common.Dto
{
	[DataContract]
	public class SyncMessagesMethodResultDto
	{
		[DataMember]
		public List<MailItemDto> Items;

		[DataMember]
		public string Message { get; set; }
	}
}
