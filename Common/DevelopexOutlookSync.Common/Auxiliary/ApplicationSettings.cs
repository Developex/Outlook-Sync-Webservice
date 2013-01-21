using System.Configuration;

namespace DevelopexOutlookSync.Common.Auxiliary
{
	public class ApplicationSettings
	{
		private static string GetPathFromConfig(string appSettingKey)
		{
			string path = ConfigurationManager.AppSettings[appSettingKey];

			return path;
		}

		/// <summary>
		/// Check if current compile mode is DEBUG
		/// </summary>
		/// <remarks>For web sites, where debug mode is taken from web.config</remarks>
		public static bool IsDebug
		{
			get
			{
#if DEBUG
				return true;
#else
                return false;
#endif
			}
		}

		public static string HtmlDocPath
		{
			get
			{
				return GetPathFromConfig("HtmlDocPath");
			}
		}

		public static string ImagesPath
		{
			get
			{
				return GetPathFromConfig("ImagesPath");
			}
		}

		public static string GlobalResourcesPath
		{
			get
			{
				return GetPathFromConfig("GlobalResourcesPath");
			}
		}

		public static string SiteUrl
		{
			get
			{
				return GetPathFromConfig("SiteUrl");
			}
		}
	}
}
