using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using DevelopexOutlookSync.Common.Auxiliary;
using DevelopexOutlookSync.Common.Dto;
using DevelopexOutlookSync.Common.Enums;

namespace OutlookSyncService.Library
{
	public interface IMailsController
	{
		LoginMethodResultDto Login(LoginMethodParametersDto pars);

		SyncMessagesMethodResultDto SyncMessages(SyncMessagesMethodParametersDto pars);
	}

	public abstract class MailsController : IMailsController
	{
		public const string MESSAGES_FOLDER = "Messages";

		public abstract string MailsPath { get; }

		/*private const int BUFFER_SIZE = 10;

		public static int ReadAllBytesFromStream(Stream stream, byte[] buffer)
		{
			// Use this method is used to read all bytes from a stream.
			int offset = 0;
			int totalCount = 0;
			while (true)
			{
				int bytesRead = stream.Read(buffer, offset, BUFFER_SIZE);
				if (bytesRead == 0)
				{
					break;
				}
				offset += bytesRead;
				totalCount += bytesRead;
			}
			return totalCount;
		}*/

		internal class CheckSumException : Exception {}

		protected void SaveMail(string clientId, MailItemDto message)
		{
			var path = MailsPath;
			if(!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			var dateFolder = message.SentOn.ToString("yyyyMMdd");
			path = Path.Combine(path, dateFolder);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			string id = string.IsNullOrEmpty(message.Id) ? Guid.NewGuid().ToString() : message.Id;
			string fileName = string.Format("{0}_{1}.msg", clientId, id);
			path = Path.Combine(path, fileName);


			
			using (var ms = new MemoryStream())
			{
				ms.Write(message.Data, 0, message.Data.Length);
				ms.Position = 0;
				using (MemoryStream outStream = new MemoryStream(message.Data.Length*3))
				{
					using (var decompress = new GZipStream(ms, CompressionMode.Decompress))
					{
						decompress.CopyTo(outStream);
						decompress.Close();
					}
					outStream.Position = 0;
					message.Data = outStream.GetBuffer();
					var realSum = message.CalculatedCheckSum;
					if (realSum != message.CheckSum)
					{
						throw new CheckSumException();
					}
					File.WriteAllBytes(path, message.Data);
					message.Data = null;
				}

				/*using (GZipStream gZipStream = new GZipStream(ms, CompressionMode.Decompress))
				{
					byte[] decompressedBuffer = new byte[10*1024*1024];
					// Use the ReadAllBytesFromStream to read the stream.
					int totalCount = ReadAllBytesFromStream(gZipStream, decompressedBuffer);

					using (var fs = File.OpenWrite(path))
					{
						fs.Write(decompressedBuffer, 0, totalCount);
					}
				}*/
				message.Link = string.Join("/", ApplicationSettings.SiteUrl, MESSAGES_FOLDER, dateFolder, fileName);
			}
		}

		private bool TestSession(Guid sessionId)
		{
			return _clients.ContainsKey(sessionId);
		}

		public SyncMessagesMethodResultDto SyncMessages(SyncMessagesMethodParametersDto pars)
		{
			var result = new SyncMessagesMethodResultDto { Items = new List<MailItemDto>() };

			if (!TestSession(pars.SessionId))
			{
				result.Message = "Session is expired or invalid";
				return result;
			}

			if (pars.Items != null)
			{
				foreach (MailItemDto mailItem in pars.Items)
				{
					try
					{
						SaveMail(pars.ClientId, mailItem);
					}
					catch (Exception ex)
					{
						mailItem.SyncStatus = SyncMessagesMethodResultStatus.Failed;
						result.Message += 
							ex is CheckSumException ? 
								Environment.NewLine + "Check sum doesn't match for " + mailItem.Id :
								string.Format(
									"{0}Error saving of message({1}) {2}",
									Environment.NewLine,
									ex.Message,
									mailItem.Id);
					}
					mailItem.Data = null;
					result.Items.Add(mailItem);
				}
			}

			System.Threading.Thread.Sleep(500);

			return result;
		}

		private static Dictionary<Guid, DateTime> _clients = new Dictionary<Guid, DateTime>();

		public LoginMethodResultDto Login(LoginMethodParametersDto pars)
		{
			var result = pars.UserName == "admin" && pars.Password == "1";
			var sessionId = result ? Guid.NewGuid() : Guid.Empty;
			if (result)
			{
				_clients.Add(sessionId, DateTime.UtcNow);
			}
			return new LoginMethodResultDto { Result = result, SessionId = sessionId };
		}
	}
}