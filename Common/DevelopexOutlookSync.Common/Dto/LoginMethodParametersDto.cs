using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DevelopexOutlookSync.Common.Dto
{
	[DataContract]
	public class LoginMethodParametersDto
	{
		[DataMember]
		public string UserName;

		[DataMember]
		public string Password;
	}
}
