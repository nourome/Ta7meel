using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PCLStorage;
using Xamarin.Forms;

namespace Ta7meel
{
	public class VideoHelper
	{
		private static string AppFolder = "Ta7meel";
		IVideoUtils videoUtils = DependencyService.Get<IVideoUtils>();

		public bool ValidateLink(string url)
		{
			if (url.Contains("youtube.com") || url.Contains("youtu.be"))
			{
				return true;
			}
			return false;
		}

		public string MakeValidYoutubeLink(string url)
		{
			string videoId = "";
			if (url.Contains("youtube.com"))
			{
				if (url.Contains("?v="))
				{
					int questioMarkIndex = url.IndexOf('?');
					videoId = url.Substring(questioMarkIndex + 3);
					if (videoId.IndexOf('/') > 0)
						videoId = videoId.Remove('/', ' ');

					videoId = videoId.Trim();
					videoId = "https://www.youtube.com/watch?v=" + videoId;

				}
			}
			else if (url.Contains("youtu.be"))
			{
				int slashIndex = url.LastIndexOf('/');
				if (slashIndex > 10)
				{
					videoId = url.Substring(slashIndex + 1);
					videoId = videoId.Trim();
					videoId = "https://www.youtube.com/watch?v=" + videoId;
				}
			}

			return videoId;
		}

		public string GetLinkFromClipboard()
		{
			string link = videoUtils.GetCopiedLinkFromClipboard();
			string youtubeLink = "";
			if (ValidateLink(link))
			{
				youtubeLink = MakeValidYoutubeLink(link);
			}

			return youtubeLink;
		}

		public async Task<IFile> CreateNewVideoFile(string videoFileNameExt)
		{
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			IFolder folder = await rootFolder.CreateFolderAsync(AppFolder, CreationCollisionOption.OpenIfExists);
			IFile videoFile = await folder.CreateFileAsync(videoFileNameExt, CreationCollisionOption.ReplaceExisting);
			return videoFile;
		}

		public void LoadAllVideos(ObservableCollection<DownloadedVideo> DownloadedList)
		{
			
			var listTask = GetAllFilesInFolder();
			listTask.ContinueWith(task =>
			{
				var list = listTask.Result;
				DownloadedList.Clear();
				
				foreach (var item in list)
				{
					if (item.Name.EndsWith("mp4", StringComparison.CurrentCulture) || item.Name.EndsWith("webM", StringComparison.CurrentCulture))
					{

						DownloadedVideo video = new DownloadedVideo
						{
							VideoName = item.Name,
							VideoPath = item.Path,
							VideoTime = videoUtils.GetVideoDuration(item.Path),
							IsDownloading = false,
							VideoThumbnail = videoUtils.GetVideoThumbnail(item.Path)

						};
						DownloadedList.Add(video);

					}
				}
			});


		}

		public async Task RemoveVideoFile(string filename)
		{

			IFolder rootFolder = FileSystem.Current.LocalStorage;
			IFolder folder = await rootFolder.CreateFolderAsync(AppFolder, CreationCollisionOption.OpenIfExists);
			IFile file = await folder.GetFileAsync(filename);
			await file.DeleteAsync();

		}

		public void OpenActionSheet(string path)
		{
			
			videoUtils.OpenActionSheet(path);

		}



		private async Task<IList<IFile>> GetAllFilesInFolder()
		{
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			IFolder folder = await rootFolder.CreateFolderAsync(AppFolder, CreationCollisionOption.OpenIfExists);
			IList<IFile> list = await folder.GetFilesAsync();
			return list;
		}




	}
}
