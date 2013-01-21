using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DevelopexOutlookSync.Common.Auxiliary
{
	public static class StringHelper
	{
		private const char ExportDelimiterChar = ';';
		public static readonly string ExportDelimiter = ExportDelimiterChar.ToString();
		private const string QuoteDelimiterReplace = "\"\"";

		public static string ToSafeString<T>(this T sorce, string defaultValue = "")
			where T : class
		{
			return sorce == null ? defaultValue : sorce.ToString();
		}

		public static string Replace(string source, string oldValue, string newValue, bool ignoreCase)
		{
			if (source == null || newValue == null)
				throw new ArgumentNullException();
			if (string.IsNullOrEmpty(oldValue))
				throw new ArgumentException("Value must be non-null and non-empty", "oldValue");

			StringBuilder sb = new StringBuilder(source.Length);

			int ind = 0;
			while (ind < source.Length)
			{
				int newInd = source.IndexOf(oldValue, ind, ignoreCase ? StringComparison.InvariantCultureIgnoreCase :
				                                                                                                    	StringComparison.InvariantCulture);

				if (newInd == -1)
				{
					sb.Append(source, ind, source.Length - ind);
					break;
				}

				sb.Append(source, ind, newInd - ind);
				sb.Append(newValue);

				ind = newInd + oldValue.Length;
			}

			return sb.ToString();
		}

		public static string ProcessTemplate(string template, Dictionary<string, string> properties, bool ignoreCase)
		{
			if (string.IsNullOrEmpty(template))
			{
				StackTrace trace = new StackTrace();
				return "Unknown template in " + (trace.FrameCount > 1 ? trace.GetFrame(1).GetMethod().ToString() : string.Empty );
			}

			foreach (KeyValuePair<string, string> kvp in properties)
				template = Replace(template, string.Format("{{{0}}}", kvp.Key), kvp.Value, ignoreCase);

			return template;
		}

		public static string WrapExtGridHeaderInDiv(string text)
		{
			return string.Format("<div style=\"margin-top:-5px; line-height: 12px\">{0}</div>", text);
		}
	
		public static string ProcessJSString(string text, bool singleQuote)
		{
			if (singleQuote)
				return text.Replace("\\", "\\\\").Replace("'", "\\'");
			else
				return text.Replace("\\", "\\\\").Replace("\"", "\\\"");
		}
	
		public static string GetPrice(double price)
		{
			return price.ToString("#,0.00", CultureInfo.InvariantCulture);
		}

		public static string GetMonthPrice(double price)
		{
			return price.ToString("#,0.000", CultureInfo.InvariantCulture);
		}

		public static string IsNull(object obj, string sValue, string nullValue)
		{
			if (obj is string)
				return !string.IsNullOrEmpty(obj.ToString()) ? sValue : nullValue;
			return obj != null ? sValue : nullValue;
		}

		public static string IsNull(object obj, string sValue)
		{
			return IsNull(obj, sValue, "-");
		}

		public static string IsNull(string sValue)
		{
			return (sValue != null && sValue.Trim().Length > 0) ? sValue : "-";
		}

		public static string Truncate(string source, int length)
		{
			if (source.Length > length)
				return source.Substring(0, length);
			return source;
		}

		public static string Enc(this string s)
		{
			return s != null ? "\"" + s.Trim().Replace("\"", QuoteDelimiterReplace) + "\"" : "\"\"";
		}

		public const string DATE_TIME_FORMAT = "yyyyMMdd";

		public static string Enc(this DateTime dateTime)
		{
			return "\"" + dateTime.ToString(DATE_TIME_FORMAT) + "\"";
		}

		public static string Enc(this bool val)
		{
			return val ? "\"Y\"" : "\"N\"";
		}

		public static string Enc(this double? val)
		{
			return val.HasValue ? Enc(val.Value) : "\"\"";
		}

		public static string Enc(this double val)
		{
			return "\"" + val.ToString("F2") + "\"";
		}

		public static IEnumerable<string> SplitCSV(this string line)
		{
			IEnumerable<string> result = line.SplitCSVImpl(',');
			if(result.Count() == 1)
			{
				result = line.SplitCSVImpl(';');
			}
			if (result.Count() == 1)
			{
				result = line.SplitCSVImpl('\t');
			}
			return result;
		}

		private static IEnumerable<string> SplitCSVImpl(this string line, char delimiter)
		{
			int index = 0;
			int start = 0;
			bool inString = false;

			Func<string, string> Norm =
				delegate(string arg)
					{
						var cell = arg.Trim();
						if (cell.StartsWith("\""))
							cell = cell.Substring(1);
						if (cell.EndsWith("\""))
							cell = cell.Substring(0, cell.Length - 1);
						return cell;
					};

			foreach (char c in line)
			{
				if (c == '"')
				{
					inString = !inString;
				}
				else if (c == delimiter)
				{
					if (!inString)
					{
						var cell = Norm(line.Substring(start, index - start));
						yield return cell;
						start = index + 1;
					}
				}
				index++;
			}

			if (start < index)
			{
				var cell = Norm(line.Substring(start, index - start));
				yield return cell;
			}
		}

		public class CsvHelper
		{
			private readonly string _separator;
			private readonly StringBuilder _csv = new StringBuilder(Int16.MaxValue);

			public CsvHelper(string separator)
			{
				_separator = separator;
			}

			public int Length
			{
				get
				{
					return _csv.Length;
				}
			}

			public byte[] ToBytes()
			{
				return Encoding.UTF8.GetBytes(ToString());
			}

			public override string ToString()
			{
				return _csv.ToString();
			}

			public void Reset()
			{
				_csv.Length = 0;
			}

			public void Add(params object[] lines)
			{
				if(lines == null || lines.Length == 0)
					return;
				List<string> result = new List<string>();
				foreach (var line in lines)
				{
					if(line is string)
					{
						result.Add(((string)line).Enc());
					} else if (line is double)
					{
						result.Add(((double)line).Enc());
					}
					else if (line is double?)
					{
						result.Add(((double?)line).Enc());
					}
					else if (line is bool)
					{
						result.Add(((bool)line).Enc());
					}
					else if (line == null)
					{
						result.Add(string.Empty.Enc());
					}
					else
					{
						result.Add(line.ToString());
					}
				}
				_csv.AppendLine(string.Join(_separator, result.ToArray()));
			}
		}
	}
}
