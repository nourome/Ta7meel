using System;
using System.IO;
using Acr.UserDialogs;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.Provider;
using Android.Support.V4.Content;
using Java.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Ta7meel.Droid.VideoUtils))]
namespace Ta7meel.Droid
{
	public class VideoUtils : IVideoUtils
	{
		public string GetCopiedLinkFromClipboard()
		{
			var clipboardManager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
			if (clipboardManager.HasText)
				return clipboardManager.Text;
			else
				return "";
		}

		public string GetVideoDuration(string url)
		{
			string timeStr;

			MediaMetadataRetriever retriever = new MediaMetadataRetriever();
			retriever.SetDataSource(url);
			var duration = retriever.ExtractMetadata(MetadataKey.Duration);
			int playbackTime = Convert.ToInt32(duration);
			TimeSpan time = TimeSpan.FromMilliseconds(playbackTime);
			if (time.Hours > 0)
				timeStr = time.ToString(@"hh\:mm\:ss");
			else
				timeStr = time.ToString(@"mm\:ss");

			return timeStr;

		}

		public ImageSource GetVideoThumbnail(string url)
		{
			Bitmap thumb = ThumbnailUtils.CreateVideoThumbnail(url, ThumbnailKind.MiniKind);
			var imgsrc = ImageSource.FromStream(() =>
		  {
			  MemoryStream ms = new MemoryStream();
			  thumb.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
			  ms.Seek(0L, SeekOrigin.Begin);
			  return ms;
		  });

			return imgsrc;
		}



		public void OpenActionSheet(string path)
		{
			ActionSheetConfig actionSheetConfig = new ActionSheetConfig();
			actionSheetConfig.Title = "Choose Action";
			actionSheetConfig.Add("Share", () => OpenShareActionSheet(path));
			actionSheetConfig.Add("Play", () => OpenPlayActionSheet(path));
			actionSheetConfig.Add("Save To Gallery", () => VideoSave.SaveVideoToExternalStorage(path));
			UserDialogs.Instance.ActionSheet(actionSheetConfig);
		}

		public void OpenShareActionSheet(string path)
		{

			Java.IO.File newFile = new Java.IO.File(path);
			Android.Net.Uri contentUri = FileProvider.GetUriForFile(Forms.Context, "com.companyname.ta7meel.fileprovider", newFile);

			if (contentUri != null)
			{

				Intent shareIntent = new Intent();
				shareIntent.SetAction(Intent.ActionSend);
				shareIntent.AddFlags(ActivityFlags.GrantReadUriPermission); // temp permission for receiving app to read this file
				string type = Forms.Context.ContentResolver.GetType(contentUri);
				shareIntent.SetDataAndType(contentUri, type);
				shareIntent.PutExtra(Intent.ExtraStream, contentUri);
				Forms.Context.StartActivity(Intent.CreateChooser(shareIntent, "Open With"));
				//VideoSave.SaveImageToExternalStorage("hello.mp4");
			}

		}

		public void OpenPlayActionSheet(string path)
		{

			Java.IO.File newFile = new Java.IO.File(path);
			Android.Net.Uri contentUri = FileProvider.GetUriForFile(Forms.Context, "com.companyname.ta7meel.fileprovider", newFile);

			if (contentUri != null)
			{

				Intent shareIntent = new Intent();
				shareIntent.SetAction(Intent.ActionView);
				shareIntent.AddFlags(ActivityFlags.GrantReadUriPermission); // temp permission for receiving app to read this file
				string type = Forms.Context.ContentResolver.GetType(contentUri);
				shareIntent.SetDataAndType(contentUri, type);
				shareIntent.PutExtra(Intent.ExtraStream, contentUri);
				Forms.Context.StartActivity(Intent.CreateChooser(shareIntent, "Open With"));
				//VideoSave.SaveImageToExternalStorage("hello.mp4");
			}

		}
	}


}
