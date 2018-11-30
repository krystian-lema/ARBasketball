using System;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class ARCoreController : MonoBehaviour
{
    [SerializeField] private Camera arCamera;
    [SerializeField] private GameObject[] visualizationObjects;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private Button startARButton;

    private bool surfaceEnabled;
    private TrackableHit currentHit;

    private DateTime lastRaycastTime;
    private double raycastDelay = 0.5;

    private void Start()
    {
        startARButton.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        startARButton.onClick.AddListener(StartAR);
    }

    private void OnDisable()
    {
        startARButton.onClick.RemoveListener(StartAR);
    }

    public void Visualize(bool enable)
    {
        foreach (var visualizationObject in visualizationObjects)
        {
            visualizationObject.SetActive(enable);
        }
    }

    private void Update()
    {
        if ((DateTime.Now - lastRaycastTime).TotalSeconds < raycastDelay)
        {
            return;
        }
        
        lastRaycastTime = DateTime.Now;
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                                          TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(arCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                surfaceEnabled = false;
                startARButton.enabled = false;
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {
                currentHit = hit;
                surfaceEnabled = true;
                startARButton.enabled = true;
            }
        }
        else
        {
            surfaceEnabled = false;
            startARButton.enabled = false;
        }
    }

    public void StartAR()
    {
        if (!surfaceEnabled)
        {
            return;
        }
        
        // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
        // world evolves.
        var anchor = currentHit.Trackable.CreateAnchor(currentHit.Pose);
                
        gameplay.SetActive(true);
        gameplay.transform.parent = anchor.transform;
        gameplay.transform.position = currentHit.Pose.position;
        gameplay.transform.rotation = currentHit.Pose.rotation;

        Visualize(false);
        startARButton.gameObject.SetActive(false);
    }
}