using Microsoft.Practices.Unity;

namespace OutlookSyncService.Library
{
	public class Helper
	{
		private static IUnityContainer _container;
		public static IUnityContainer Container
		{
			get
			{
				if (_container == null)
				{
					_container = new UnityContainer();
				}
				return _container;
			}
		}

		public static IMailsController MailsController
		{
			get
			{
				return Container.Resolve<IMailsController>();
			}
		}
	}
}