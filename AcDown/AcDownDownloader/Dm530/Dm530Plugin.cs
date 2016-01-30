using System;
using System.Collections.Generic;
using System.Text;
using Kaedei.AcDown.Interface;
using System.Text.RegularExpressions;

namespace Kaedei.AcDown.Downloader
{
	[AcDownPluginInformation("Dm530Downloader", "風車動漫網下载插件", "", "1.1.0.1", "Dm530下载插件", "")]
	public class Dm530Plugin : IPlugin
	{
    	public static string RegexPatternWeb = @"www.dm530.com/.+";

		public Dm530Plugin()
		{
			Feature = new Dictionary<string, object>();
			//ExampleUrl
			Feature.Add("ExampleUrl", new string[] { 
				"Dm530Dm530下载插件:",
				"支持简写形式",
				"",
				"http://www.dm530.com"
			});
			//AutoAnswer(不支持)
			//ConfigurationForm(不支持)
		}

		public IDownloader CreateDownloader()
		{
			return new Dm530Downloader();
		}

		public bool CheckUrl(string url)
		{
			if (Regex.IsMatch(url, RegexPatternWeb, RegexOptions.IgnoreCase))
				return true;

			return false;
		}

		/// <summary>
		/// 规则为 sm+数字。例如sm1234567
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public string GetHash(string url)
		{
            if (CheckUrl(url))
            {
                return "dm530" + Guid.NewGuid().ToString();
            }
            else
            {
                return null;
            }
        }

		public Dictionary<string, object> Feature { get; private set; }
		public SerializableDictionary<string, string> Configuration { get; set; }


	}
}
