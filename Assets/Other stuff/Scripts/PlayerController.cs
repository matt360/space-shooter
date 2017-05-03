using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Boundary boundary;

	public GameObject shot;
	public Transform[] shotSpawns;
	public float fireRate;
	public SimpleTouchPad touchPad;
	public SimpleTouchAreaButton areaButton;

	private Rigidbody rb;
	private float nextFire;
	private AudioSource audioSource;
	private Quaternion calibrationQuaternion;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		audioSource = GetComponent<AudioSource> ();
		CalibrateAccelerometer ();
	}

	void Update ()
	{
//		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		if (areaButton.CanFire() && Time.time > nextFire)
		{
			/*nextFire = Time.time + fireRate;
			Vector3 spawnPosition = new Vector3(shotSpawn.position.x, 0.0f, shotSpawn.position.z);
			Quaternion spawnRotation = Quaternion.Euler(0.0f, shotSpawn.rotation.eulerAngles.y, 0.0f);
			foreach (var shotSpawn in shotSpaws) {
				Instantiate(shot, spawnPosition, spawnRotation);
			}
			audioSource.Play ();*/

			nextFire = Time.time + fireRate;
			foreach (var shotSpawn in shotSpawns) {
				Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			}
			audioSource.Play ();
		}
	}

	//Used to calibrate the Iput.acceleration input
	void CalibrateAccelerometer () {
		Vector3 accelerationSnapshot = Input.acceleration;
		Quaternion rotateQuaternion = Quaternion.FromToRotation (new Vector3 (0.0f, 0.0f, -1.0f), accelerationSnapshot);
		calibrationQuaternion = Quaternion.Inverse (rotateQuaternion);
	}
	
	//Get the 'calibrated' value from the Input
	Vector3 FixAcceleration (Vector3 acceleration) {
		Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
		return fixedAcceleration;
	}

	void FixedUpdate ()
	{
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");

//		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
//		Vector3 accelerationRaw = Input.acceleration;
//		Vector3 acceleration = FixAcceleration (accelerationRaw);
//		Vector3 movement = new Vector3 (acceleration.x, 0.0f, acceleration.y);

		Vector2 direction = touchPad.GetDirection ();
		Vector3 movement = new Vector3 (direction.x, 0.0f, direction.y);
		rb.velocity = movement * speed;

		rb.position = new Vector3
			(
				Mathf.Clamp (rb.position.x, boundary.xMin, boundary.xMax), 
				0.0f, 
				Mathf.Clamp (rb.position.z, boundary.zMin, boundary.zMax)
			);

		//rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
