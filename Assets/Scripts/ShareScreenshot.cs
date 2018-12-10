using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ShareScreenshot : MonoBehaviour
{
	private string screenshotName = "screenshot.png";

	private void ShareScreenshotWithText(string text)
	{
		Debug.LogFormat("[ShareScreenshot] ShareScreenshotWithText");
		
		string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
		if (File.Exists(screenShotPath))
		{
			File.Delete(screenShotPath);
		}

		ScreenCapture.CaptureScreenshot(screenshotName);

		StartCoroutine(DelayedShareCoroutine(screenShotPath, text));
	}

	private IEnumerator DelayedShareCoroutine(string screenShotPath, string text)
	{
		while(!File.Exists(screenShotPath)) {
			yield return new WaitForSeconds(.05f);
		}

		ShareController.Share(text, screenShotPath, "", "", "image/png", true, "");
	}

	public void ShareCurrentScreen()
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		Debug.LogFormat("[ShareScreenshot] ShareCurrentScreen");
		ShareScreenshotWithText("I just played AR Basketball. It's awesome!");
		#endif
	}

	private void OnDestroy()
	{
		string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
		if (File.Exists(screenShotPath))
		{
			File.Delete(screenShotPath);
		}
	}
}