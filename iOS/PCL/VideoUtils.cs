using System;
using System.IO;
using System.Linq;
using Acr.UserDialogs;
using AVFoundation;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Ta7meel.iOS.VideoUtils))]
namespace Ta7meel.iOS
{
	public class VideoUtils:IVideoUtils
	{
		public string GetCopiedLinkFromClipboard()
		{
			UIPasteboard clipboard = UIPasteboard.General;
			return clipboard.String;
		}

		public string GetVideoDuration(string url)
		{
			string timeStr;
			NSUrl videoUrl = NSUrl.CreateFileUrl(url, null);
			var asset = AVAsset.FromUrl(videoUrl);
			var seconds =  asset.Duration.Seconds;
			TimeSpan time = TimeSpan.FromSeconds(seconds);
			if(seconds > 3600)
				timeStr = time.ToString(@"hh\:mm\:ss");
			else
				timeStr = time.ToString(@"mm\:ss");

			return timeStr;
			
		}


		public void OpenActionSheet(string path)
		{
			var window = UIApplication.SharedApplication.KeyWindow;
			var subviews = window.Subviews;
			var view = subviews.Last();

			NSUrl videoUrl = NSUrl.CreateFileUrl(path, null);
			UIDocumentInteractionController openInWindow = UIDocumentInteractionController.FromUrl(videoUrl);
			openInWindow.PresentOptionsMenu(CGRect.Empty, view, true);
			//openInWindow.PresentOpenInMenu(CGRect.Empty, view, true);
		}

		ImageSource IVideoUtils.GetVideoThumbnail(string url)
		{
			NSUrl videoUrl = NSUrl.CreateFileUrl(url, null);

			var asset = AVAsset.FromUrl(videoUrl);
			var imageGenerator = AVAssetImageGenerator.FromAsset(asset);
			imageGenerator.AppliesPreferredTrackTransform = true;
			var seconds = asset.Duration.Seconds;
			CoreMedia.CMTime actualTime;
			CoreMedia.CMTime cmTime = asset.Duration;
			var timeScale = asset.Duration.TimeScale;
			cmTime.Value =(seconds > 5) ? timeScale * 5 : timeScale * 1;
			NSError error;
			var imageRef = imageGenerator.CopyCGImageAtTime(cmTime, out actualTime, out error);

			if (imageRef == null)
				return null;

			var uiImage = UIImage.FromImage(imageRef);



			var img  = Xamarin.Forms.ImageSource.FromStream(() => ((uiImage.AsPNG().AsStream())));


			return img;
		}
	}
}
