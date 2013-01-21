using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;

//using DIngvar.CommonWeb;

namespace DevelopexOutlookSync.Common.Auxiliary
{
	public class ResXHelper
	{
		private static readonly Dictionary<string, Dictionary<string, string>> resourcesByFiles = new Dictionary<string, Dictionary<string, string>>();

		private static Dictionary<string, string> LoadResources(string fileName)
		{
			Dictionary<string, string> resources = new Dictionary<string, string>();

			using (ResXResourceReader reader = new ResXResourceReader(fileName))
			{
				foreach (DictionaryEntry de in reader)
					resources.Add(de.Key.ToString(), de.Value.ToString());
			}

			return resources;
		}

		public static string GetResource(string fileName, string key)
		{
			Dictionary<string, string> resources;
			if (!resourcesByFiles.TryGetValue(fileName, out resources))
			{
				lock (resourcesByFiles)
				{
					if (!resourcesByFiles.TryGetValue(fileName, out resources))
					{
						resources = LoadResources(fileName);
						resourcesByFiles.Add(fileName, resources);
					}
				}
			}

			string value;
			if (resources.TryGetValue(key, out value))
				return value;
			else
				return null;
		}

		public static string GetGlobalResource(string className, string key)
		{
			string path = null;

			if (path == null)
				path = ApplicationSettings.GlobalResourcesPath;

			return GetResource(Path.Combine(path, className + ".resx"), key);
		}

		public static string GetGlobalResourcePlainText(string className, string key)
		{
			var res = GetGlobalResource(className, key);
			var replacement = Environment.NewLine;
			if (res != null)
				foreach (var s in new[] { "<br/>", "<br>", "<br />" })
				{
					res = res.Replace(s, replacement);
				}

			return res;
		}

		public static string LocalizeEnum(Enum value)
		{
			return GetGlobalResource("Enum", string.Format("{0}_{1}", value.GetType().Name, value));
		}

		public static string LocalizeType(Type value)
		{
			return GetGlobalResource("Enum", string.Format("{0}", value.Name));
		}
	}
}
