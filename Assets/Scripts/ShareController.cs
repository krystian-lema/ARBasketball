using UnityEngine;

public class ShareController : MonoBehaviour
{
	public static void Share(string body, string filePath = null, string url = null, string subject = "", string mimeType = "text/html", bool chooser = false, string chooserText = "Select sharing app")
	{
		Debug.LogFormat("[ShareController] Share");
		ShareAndroid(body, subject, url, new string[] { filePath }, mimeType, chooser, chooserText);
	}

	private static void ShareAndroid(string body, string subject, string url, string[] filePaths, string mimeType, bool chooser, string chooserText)
	{
		Debug.LogFormat("[ShareController] ShareAndroid");
		using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent"))
		using (AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent"))
		{
			using (intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND")))
			{ }
			using (intentObject.Call<AndroidJavaObject>("setType", mimeType))
			{ }
			using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject))
			{ }
			using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body))
			{ }

			if (!string.IsNullOrEmpty(url))
			{
				// attach url
				using (AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri"))
				using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", url))
				using (intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject))
				{ }
			}
			else if (filePaths != null)
			{
				// attach extra files (pictures, pdf, etc.)
				using (AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri"))
				using (AndroidJavaObject uris = new AndroidJavaObject("java.util.ArrayList"))
				{
					for (int i = 0; i < filePaths.Length; i++)
					{
						//instantiate the object Uri with the parse of the url's file
						using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + filePaths[i]))
						{
							uris.Call<bool>("add", uriObject);
						}
					}

					using (intentObject.Call<AndroidJavaObject>("putParcelableArrayListExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uris))
					{ }
				}
			}

			// finally start application
			using (AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			using (AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				if (chooser)
                {
                    AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, chooserText);
                    currentActivity.Call("startActivity", jChooser);
                }
                else
                {
                    currentActivity.Call("startActivity", intentObject);
                }
			}
		}
	}    
}
