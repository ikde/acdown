using System;
using System.Collections.Generic;
using System.Text;
using Kaedei.AcDown.Interface;
using System.Text.RegularExpressions;

namespace Kaedei.AcDown.Downloader
{
	[AcDownPluginInformation("JiyingDmDownloader", "極影動漫網下载插件", "", "1.1.0.1", "JiyingDmJiyingDm下载插件", "")]
	public class JiyingDmPlugin : IPlugin
	{
    	public static string RegexPatternWeb = @"www.jiyingdm.com/(?<title>\S[^\/]+)/(?<var>\S[^\/]+/?)";

		public JiyingDmPlugin()
		{
			Feature = new Dictionary<string, object>();
			//ExampleUrl
			Feature.Add("ExampleUrl", new string[] { 
				"JiyingDmJiyingDm下载插件:",
				"支持简写形式",
				"",
				"http://www.jiyingdm.com"
			});
			//AutoAnswer(不支持)
			//ConfigurationForm(不支持)
		}

		public IDownloader CreateDownloader()
		{
			return new JiyingDmDownloader();
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
                return "jiyingdm" + Guid.NewGuid().ToString();
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
