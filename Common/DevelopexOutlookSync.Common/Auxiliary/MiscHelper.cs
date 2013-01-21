using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;

namespace DevelopexOutlookSync.Common.Auxiliary
{
	public static class MiscHelper
	{
		public static void WriteEventLog(string message, EventLogEntryType entryType)
		{
			EventLog.WriteEntry("OutlookSync", message, entryType);
		}

		public static MemoryStream DuplicateMemoryStream(MemoryStream stream)
		{
			byte[] buffer = stream.GetBuffer();
			return new MemoryStream(buffer, 0, buffer.Length, stream.CanWrite, true);
		}
	
		public static Attachment DuplicateMemoryStreamAttachment(Attachment attachment)
		{
			return new Attachment(DuplicateMemoryStream((MemoryStream)attachment.ContentStream), attachment.Name, 
			                      attachment.ContentType.MediaType);
		}

		public static bool TryParseDouble(string s, out double res)
		{
			if (string.IsNullOrEmpty(s))
			{
				res = default(double);
				return false;
			}

			if (double.TryParse(s, out res))
				return true;

			s = s.Replace(',', '.');
			if (double.TryParse(s, out res))
				return true;

			s = s.Replace('.', ',');
			if (double.TryParse(s, out res))
				return true;

			var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			s = s.Replace(",", separator).Replace(".", separator);

			return double.TryParse(s, out res);
		}

		public static string ComputeHash(string value)
		{
			byte[] data = System.Text.Encoding.UTF8.GetBytes(value);
			byte[] hash = SHA256.Create().ComputeHash(data);
			return BitConverter.ToString(hash).Replace("-", string.Empty).ToUpper();
		}
	}
}
