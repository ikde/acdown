using Kaedei.AcDown.Core;
using Kaedei.AcDown.Interface;
using Kaedei.AcDown.Interface.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Kaedei.AcDown.Downloader
{
	public class JiyingDmDownloader : IDownloader
	{
		public DelegateContainer delegates { get; set; }

		public TaskInfo Info { get; set; }

		//下载参数
		DownloadParameter currentParameter = new DownloadParameter();

		//文件总长度
		public long TotalLength
		{
			get
			{
				if (currentParameter != null)
				{
					return currentParameter.TotalLength;
				}
				else
				{
					return 0;
				}
			}
		}

		//已完成的长度
		public long DoneBytes
		{
			get
			{
				if (currentParameter != null)
				{
					return currentParameter.DoneBytes;
				}
				else
				{
					return 0;
				}
			}
		}

		public bool Download()
		{
			//开始下载
			delegates.TipText(new ParaTipText(this.Info, "正在分析動畫下載位址"));                  

            Regex regChapter = new Regex("http://.+/video-[0-9]-[0-9][0-9]?.html");
            Match linkname = regChapter.Match (Info.Url);

			//要下载的Url列表
			var subUrls = new Collection<string>();

			if (linkname.Success == false) {
                //尼玛是整部

                regChapter = new Regex("http://www.jiyingdm.com/(?<value1>[^/]+)/(?<value2>[^/]+)/");
                linkname = regChapter.Match(Info.Url);

                //取得Url源文件
                string src = Network.GetHtmlSource(Info.Url, Encoding.GetEncoding("GBK"), Info.Proxy);

                //解析
                string chapter = "title='(?<chapter_name>[^']+)' href='/" + linkname.Groups["value1"].Value + "/" + linkname.Groups["value2"].Value +"/" + "(?<link_url>[^']+)'";

                regChapter = new Regex(chapter);
				Match item = regChapter.Match(src);

				//填充字典
				var dict = new Dictionary<string, string>();

                while (item.Success)
                {
                    string url = "http://www.jiyingdm.com/" + linkname.Groups["value1"].Value + "/" + linkname.Groups["value2"].Value + "/"
                        + item.Groups["link_url"].Value;
                    string chapter_name = item.Groups["chapter_name"].Value;                    
                    dict.Add( url.Trim(), chapter_name);

                    // first match is "hello world"!! but this turns out to be an infinite loop
                    item = item.NextMatch();
                }

				//选择下载哪部漫画
				subUrls = ToolForm.CreateMultiSelectForm(dict, Info.AutoAnswer, "jiyingdm");
				//如果用户没有选择任何章节
				if (subUrls.Count == 0)
				{
					return false;
				}
			}

			Info.PartCount = subUrls.Count;
			Info.CurrentPart = 0;
			foreach (string url in subUrls)
			{
				//提示更换新Part
                delegates.NewPart(new ParaNewPart(this.Info, Info.CurrentPart));
			}

			return true;
		}

		public void StopDownload()
		{
			if (currentParameter != null)
			{
				//将停止flag设置为true
				currentParameter.IsStop = true;
			}
		}
		//最后一次Tick时的值
		public long LastTick
		{
			get
			{
				if (currentParameter != null)
				{
					//将tick值更新为当前值
					long tmp = currentParameter.LastTick;
					currentParameter.LastTick = currentParameter.DoneBytes;
					return tmp;
				}
				else
				{
					return 0;
				}
			}
		}

	}
}
