using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ARGameplayCameraScaler : MonoBehaviour
{
	private Camera _camera;
	private Camera _arCamera;
	private GameObject _arTargetObject;
	private float _cameraScale;

	private void Awake()
	{
		_camera = GetComponent<Camera>();
	}

	public void Init(Camera arCamera, GameObject arTargetObject, float cameraScale)
	{
		_arCamera = arCamera;
		_arTargetObject = arTargetObject;
		_cameraScale = cameraScale;
	}

	private void Update()
	{
		if (_arCamera != null && _arTargetObject != null && _cameraScale > 0.0001f && _cameraScale < 10000.0f)
		{
			_camera.transform.parent = null;
			float invScale = 1.0f / _cameraScale;
			var arCameraPos = _arCamera.transform.position;
			var vecArTargetToCamera = arCameraPos - _arTargetObject.transform.position;

			_camera.transform.localPosition = _arTargetObject.transform.position + (vecArTargetToCamera * invScale);
			_camera.transform.localRotation = _arCamera.transform.rotation;
			_camera.projectionMatrix = _arCamera.projectionMatrix;
			_camera.transform.parent = _arCamera.transform;
		}
	}
}