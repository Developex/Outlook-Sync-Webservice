using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using DevelopexOutlookSync.Common.Auxiliary;
using DevelopexOutlookSync.Common.Enums;

namespace DevelopexOutlookSync.Common.Dto
{
	[DataContract]
	public class MailItemDto
	{
		[DataMember]
		public Int64 CheckSum;

		[DataMember]
		public DateTime SentOn;

		[DataMember]
		public string Id;

		[DataMember]
		public string Subject;
		
		[DataMember]
		public string From;

		[DataMember]
		public byte[] Data;

		[DataMember]
		public string Link;

		[DataMember] 
		public SyncMessagesMethodResultStatus SyncStatus;

		[DataMember]
		public bool IsCRMCategory;

		public Int64 CalculatedCheckSum
		{
			get
			{
				return (Int64)Data.Sum(b => (decimal)b);
			}
		}

		public string AsLogLine
		{
			get
			{
				return string.Format(
					"{0}\t{1}\t{2}\t{3}\t{4}\t{5}", 
					Id.Enc(),
					SentOn.Enc(), 
					From.Enc(), 
					Subject.Enc(),
					SyncStatus.ToString().Enc(), 
					Link.Enc());
			}
		}

		public static MailItemDto FromLogLine(string line)
		{
			MailItemDto result = null;
			List<string> parts = line.SplitCSV().ToList();
			if(parts.Count >= 6)
			{
				result = new MailItemDto
				         	{
				         		Id = parts[0],
				         		SentOn = DateTime.ParseExact(parts[1], StringHelper.DATE_TIME_FORMAT, DateTimeFormatInfo.CurrentInfo),
				         		From = parts[2],
				         		Subject = parts[3],
				         		SyncStatus = (SyncMessagesMethodResultStatus) Enum.Parse(typeof (SyncMessagesMethodResultStatus), parts[4]),
				         		Link = parts[5]
				         	};
			}
			return result;
		}
	}
}
