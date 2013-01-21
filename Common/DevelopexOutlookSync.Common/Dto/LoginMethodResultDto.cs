using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DevelopexOutlookSync.Common.Dto
{
	[DataContract]
	public class LoginMethodResultDto
	{
		[DataMember]
		public Guid SessionId;

		[DataMember]
		public bool Result;
	}
}
