using System;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using System.Threading.Tasks;
using System.Diagnostics;
using VideoLibrary;
using PCLStorage;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Collections.Specialized;

namespace Ta7meel
{
	public class MainPageViewModel: ViewModelBase
	{
		VideoHelper videoHelper = new VideoHelper();
		private int videoCount = 1;
		private DownloadedVideo selectedItem = null;

		public ObservableCollection<DownloadedVideo> DownloadedList { get; set; }
		public int VideoCount
		{
			get{ return videoCount;}
			set{ SetProperty(ref videoCount, value);}
		}
		public DownloadedVideo ListItemSelected
		{
			get { return selectedItem; }
			set { 
				SetProperty(ref selectedItem, value);
				ShareVideo(selectedItem);

			}
		}




		public MainPageViewModel()
		{
			AddCommand = new Command(async () => await AddNewVideo());

			RemoveCommand = new Command<DownloadedVideo>(RemoveVideo);
			//ShareCommand = new Command(ShareVideo);
			DownloadedList = new ObservableCollection<DownloadedVideo>();
			DownloadedList.CollectionChanged += OnCollectionChanged;
			videoHelper.LoadAllVideos(DownloadedList);

		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
				VideoCount = DownloadedList.Count;
		}

		async Task AddNewVideo()
		{
			string copiedLink = videoHelper.GetLinkFromClipboard();

			PromptResult result = await UserDialogs.Instance.PromptAsync(new PromptConfig
			{
				Message = "Insert youtube link here:",
				Text = copiedLink,
				OnTextChanged = args =>
				{
					args.IsValid = true; // setting this to false will disable the OK/Positive button
					
				}
			});

			if (result.Ok)
			{
				if (videoHelper.ValidateLink(result.Value))
				{
					string youtubeVideoUrl = videoHelper.MakeValidYoutubeLink(result.Value);
					if (youtubeVideoUrl.Length > 1)
					{
						await DownloadNewVideo(videoHelper.MakeValidYoutubeLink(youtubeVideoUrl));
					}
					else {
						ShowErrorMsg("Youtube link is invalid");
					}
				}
				else {
					ShowErrorMsg("Youtube link is invalid");

				}
				    
			}

		}

		private async Task DownloadNewVideo(string url)
		{
					//var allVideos = await YouTube.Default.GetAllVideosAsync("https://www.youtube.com/watch?v=8KyIj7Kbpew");
					

			var allVideos = await YouTube.Default.GetAllVideosAsync(url);
					var video = allVideos.First(_video => _video.Format == VideoFormat.Mp4);
					if (video == null) 
					{
						video = YouTube.Default.GetVideo(url);
						if (video == null)
						{
							ShowErrorMsg("Failed to download the video!");
							return;
						}
							
					}
					
					string videoFileNameExt = video.Title + video.FileExtension;
					CreateNewDownloadedVideo(videoFileNameExt);
					IFile videoFile = await videoHelper.CreateNewVideoFile(videoFileNameExt);

			if (videoFile != null)
			{
				using (var fileHandler = await videoFile.OpenAsync(FileAccess.ReadAndWrite))
				{
					var content = video.GetBytesAsync();
					var downloadedBytes = content.Result;
					await fileHandler.WriteAsync(downloadedBytes, 0, downloadedBytes.Length);
					videoHelper.LoadAllVideos(DownloadedList);

				}
			}
			else {
				ShowErrorMsg("Cannot save the video to the storage!");
			}
			
		}

	
		void RemoveVideo(DownloadedVideo videoItem)
		{
			string path = videoItem.VideoPath;
			int index = DownloadedList.IndexOf(videoItem);
			System.Diagnostics.Debug.WriteLine("Menu item delete click and its index in collection is: " + index);
			DownloadedList.RemoveAt(index);
			videoHelper.RemoveVideoFile(videoItem.VideoName);
		}

		void ShareVideo(DownloadedVideo item)
		{
			if (item != null)
			{
				videoHelper.OpenActionSheet(item.VideoPath);
				ListItemSelected = null;


			}
		}


		private void CreateNewDownloadedVideo(string name )
		{
			DownloadedVideo video = new DownloadedVideo
			{
				VideoName = name,
				VideoPath = "",
				VideoTime = "Downloading..." ,
				IsDownloading = true,
				VideoThumbnail = ImageSource.FromFile("downloading.png")
			};
			DownloadedList.Add(video);
		}

		void ShowErrorMsg(string message)
		{
			UserDialogs.Instance.Alert("Error", message, "OK");
		}

		public ICommand AddCommand { private  set; get; }
		public ICommand DownloadCommand { private set; get; }
		public ICommand RemoveCommand { private set; get; }
		//public ICommand ShareCommand { private set; get; }

		//source github TheRealAdamKemp/Xamarin.Forms-Test

	}
}
