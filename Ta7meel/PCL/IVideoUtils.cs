using System;
using Xamarin.Forms;

namespace Ta7meel
{
	public interface IVideoUtils
	{
		ImageSource GetVideoThumbnail(string url);
		string GetVideoDuration(string url);
		void OpenActionSheet(string path);
		string GetCopiedLinkFromClipboard();

	}
}
