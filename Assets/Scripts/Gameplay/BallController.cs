using System;
using UnityEngine;
using Random = System.Random;

public class BallController : MonoBehaviour
{
	[SerializeField] private float minInputSpeed = 5f;
	[SerializeField] private float maxInputSpeed = 50f;
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
		startPos = transform.position;
		startRot = transform.rotation;
    }

    private void Start()
	{
	    ballRigidbody.isKinematic = true;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			RestartBall();
		}
		
		if (Input.GetKeyDown(KeyCode.T))
		{
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				ThrowBall(Vector3.up + transform.forward / 2, 10.5f);
			}
			else
			{
				ThrowBall(Vector3.up + transform.forward / 2, 9.8f);	
			}
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
	    var normalizedBallSpeed =
		    NormalizeBallSpeed(ballSpeed, minInputSpeed, maxInputSpeed, minThrowSpeed, maxThrowSpeed);

        ThrowBall(throwVector.normalized + transform.forward / 2, normalizedBallSpeed);
	    Debug.LogFormat("BallOriginSpeed: {0}", normalizedBallSpeed);
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

	// Cast to normalized values (easier to throw)
	private float NormalizeBallSpeed(float ballSpeed, float originMin, float originMax,
		float normalMin, float normalMax)
	{
		var clampedBallSpeed = Mathf.Clamp(ballSpeed, originMin, originMax);
		return normalMin + (clampedBallSpeed - originMin) / (originMax - originMin) * (normalMax - normalMin);
	}
}