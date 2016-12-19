using System;
using Android.Media;
using Java.Nio.Channels;
using Xamarin.Forms;

namespace Ta7meel.Droid
{
	public class VideoSave
	{
		public static void SaveVideoToExternalStorage( string path)
		{
			Java.IO.File root = Android.OS.Environment.ExternalStorageDirectory;
			Java.IO.File appDir = new Java.IO.File(root, "Ta7meelVideos");
			if (!appDir.Exists())
			{
				if (!appDir.Mkdir())
					return;
			}
				Java.IO.File videoDownloadedFile = new Java.IO.File(path);
				Java.IO.File file = new Java.IO.File(appDir, videoDownloadedFile.Name);
				if (!file.Exists())
				{
					try
					{
						Java.IO.FileInputStream videoToCopy = new Java.IO.FileInputStream(videoDownloadedFile);
						Java.IO.FileOutputStream copyOfVideo = new Java.IO.FileOutputStream(file);
						byte[] buf = new byte[1024];
						int len;
						while ((len = videoToCopy.Read(buf)) > 0)
						{
							copyOfVideo.Write(buf, 0, len);
						}

						copyOfVideo.Flush();
						copyOfVideo.Close();
						videoToCopy.Close();
						String[] Pathes = new String[] { file.Path };
						MediaScannerConnection.ScanFile(Forms.Context, Pathes, null, null);
					}
					catch (Exception e)
					{
						e.GetType();
					}

				}

			
		}
	}

}
