using System;
using Xamarin.Forms;

namespace Ta7meel
{
	public class DownloadedVideo
	{
		private bool isDownloading = true;
		private ImageSource thumbNail;


		public String VideoName
		{
			get;
			set;
		}
		public String VideoPath
		{
			get;
			set;
		}
		public String VideoTime
		{
			get;
			set;
		}
		public ImageSource VideoThumbnail
		{
			get { return thumbNail;}
			set { thumbNail = value; }
		}

		public bool IsDownloading
		{
			get { return isDownloading;}
			set { isDownloading = value;}
		}

	}
}
