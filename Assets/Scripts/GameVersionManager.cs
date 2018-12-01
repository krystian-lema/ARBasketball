using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVersionManager : MonoBehaviour
{
	[SerializeField] private GameObject[] nonArVersionElements;
	[SerializeField] private GameObject[] arVersionElements;
	
	private void Start()
	{
		#if UNITY_EDITOR
		foreach (var element in nonArVersionElements)
		{
			element.SetActive(true);
		}
		foreach (var element in arVersionElements)
		{
			element.SetActive(false);
		}
		#elif UNITY_ANDROID
		foreach (var element in nonArVersionElements)
		{
			element.SetActive(false);
		}
		foreach (var element in arVersionElements)
		{
			element.SetActive(true);
		}
		#endif
	}
}