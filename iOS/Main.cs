using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using Photos;
using UIKit;

namespace Ta7meel.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{
			Authorize();
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}

		static void Authorize()
		{
			
			PHPhotoLibrary.RequestAuthorization(status =>
{
	switch (status)
	{
		case PHAuthorizationStatus.Authorized:
			break;
		case PHAuthorizationStatus.Denied:
			break;
		case PHAuthorizationStatus.Restricted:
			break;
		default:
			break;
	}
});
		}
	}
}
