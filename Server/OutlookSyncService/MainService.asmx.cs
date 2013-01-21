using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using AutomationInterfaces;
using DevelopexOutlookSync.Common;
using DevelopexOutlookSync.Common.Dto;
using DevelopexOutlookSync.Common.Enums;
using OutlookSyncService.Library;

namespace OutlookSyncService
{
	/// <summary>
	/// Summary description for MainService
	/// </summary>
	[WebService(Namespace = "http://tempuri.org/")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[System.ComponentModel.ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	// [System.Web.Script.Services.ScriptService]
	public class MainService : System.Web.Services.WebService
	{

		[WebMethod]
		public LoginMethodResultDto Login(LoginMethodParametersDto pars)
		{
			return Helper.MailsController.Login(pars);
		}

		[WebMethod]
		public SyncMessagesMethodResultDto SyncMessages(SyncMessagesMethodParametersDto pars)
		{
			return Helper.MailsController.SyncMessages(pars);
		}
	}
}
