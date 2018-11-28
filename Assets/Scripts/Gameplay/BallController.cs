using System;
using UnityEngine;

public class BallController : MonoBehaviour
{
	[SerializeField] private float minThrowSpeed = 2f;
	[SerializeField] private float maxThrowSpeed = 15f;
	
    private Rigidbody ballRigidbody;
	private Vector3 startPos;
	private Quaternion startRot;

    private Vector3 throwStartPos;
    private DateTime throwStartTime;


    private void Awake()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
	{
		startPos = transform.position;
		startRot = transform.rotation;
	    ballRigidbody.isKinematic = true;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			RestartBall();
		}
	}

    private void OnMouseDown()
    {
        throwStartPos = Input.mousePosition;
        throwStartTime = DateTime.Now;
    }
    
    private void OnMouseUp()
    {
        var throwVector = Input.mousePosition - throwStartPos;
        var throwTime = (DateTime.Now - throwStartTime).TotalMilliseconds;
        var ballSpeed = (throwVector.magnitude / (float)throwTime) * 10f;

        ThrowBall(throwVector.normalized + transform.forward / 2, ballSpeed);
    }

    private void ThrowBall(Vector3 throwDirection, float throwSpeed)
    {
	    throwSpeed = Mathf.Clamp(throwSpeed, minThrowSpeed, maxThrowSpeed);
	    Debug.LogFormat("ThrowPower: {0}", throwSpeed);
	    
        ballRigidbody.isKinematic = false;        
        ballRigidbody.velocity = throwDirection.normalized * throwSpeed;
        ballRigidbody.AddTorque(new Vector3(1.0f, 1.0f, 1.0f) * ballRigidbody.velocity.magnitude, ForceMode.VelocityChange);
    }

	public void RestartBall()
	{
		transform.position = startPos;
		transform.rotation = startRot;
		ballRigidbody.isKinematic = true;
	}
}