using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary {
	
	public float xMin, xMax, zMin, zMax;

}

public class PlayerController : MonoBehaviour {

	public int speed;
	public int speedMobile;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	private float nextFire;

	private Rigidbody rb;
	private AudioSource audioSource;
	private Quaternion calibrationQuaternion;

	public SimpleTouchPad touchPad;
	public SimpleTouchAreaButton areaButton;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource > ();
		#if UNITY_IPHONE || UNITY_ANDROID
			CalibrateAccelerometer ();
		#endif
	}

	void Update () {

		#if UNITY_IPHONE || UNITY_ANDROID
			if (areaButton.CanFire() && Time.time > nextFire) 
		#else
			if (Input.GetButton("Fire1") && Time.time > nextFire) 
		#endif
		{
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			audioSource.Play ();
		}
	}

	void FixedUpdate () {

		#if UNITY_IPHONE || UNITY_ANDROID
			//Vector3 acceleration = Input.acceleration;
			//rb.velocity = FixAcceleration(new Vector3 (acceleration.x, 0.0f, acceleration.y)) * speedMobile;
			Vector2 direction = touchPad.GetDirection();
			rb.velocity = new Vector3 (direction.x, 0.0f, direction.y) * speedMobile;
		#else
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			rb.velocity = new Vector3 (moveHorizontal, 0.0f, moveVertical) * speed;
		#endif

		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}

	void CalibrateAccelerometer () {
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}

	Vector3 FixAcceleration (Vector3 acceleration) {
		Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
		return fixedAcceleration;
	}
}
