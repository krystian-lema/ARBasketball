using System;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
	public event Action OnScored;
	
	private DateTime triggerEnterTime;
	private Vector3 ballEnterPosition;

	private void Start()
	{
		// test
		OnScored += () =>
		{
			Debug.LogFormat("Scored!");
		};
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("Ball"))
		{
			return;
		}

		Debug.LogFormat("Ball enters.");
		triggerEnterTime = DateTime.Now;
		ballEnterPosition = other.transform.position;
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("Ball"))
		{
			return;
		}
		
		Debug.LogFormat("Ball exits.");
		var exitTime = (DateTime.Now - triggerEnterTime).TotalMilliseconds;
		Debug.LogFormat("ExitTime: {0}", exitTime);
		var ballDirection = other.transform.position - ballEnterPosition;
		Debug.LogFormat("BallDirection: {0}", ballDirection);
		if (exitTime < 1000 && ballDirection.y < 0)
		{
			if (OnScored != null)
			{
				OnScored();
			}
		}
	}
}