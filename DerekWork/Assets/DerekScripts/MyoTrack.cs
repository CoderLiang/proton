using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using VibrationType = Thalmic.Myo.VibrationType;

[RequireComponent(typeof(AudioSource))]

public class MyoTrack : MonoBehaviour {
	public OVRCameraController cameraController;
	public OVRPlayerController playerController;
	public static bool game_started = false;
	private const double ROCKET_FIRE_TIME = 0.2;
	private const double ROCKET_RECHARGE_TIME = 0.6;
	private const double ROCKET_SPEED = 100.0;
	private double RocketCD;
	private const double FLAMETHROWER_FIRE_TIME = 6.0;
	private const double FLAMETHROWER_RECHARGE_TIME = 30.0;
	private double FlameCD;
	
	private float BaseX;
	private float BaseY;
	private float BaseZ;

	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	public GameObject myo = null;
	private GameObject Myo;
	public ParticleSystem Flamethrower;
	public ParticleSystem ProtonBurst;
	private Vector3 rotation;
	private Vector3 offset;

	
	// Materials to change to when poses are made.
	public Material waveInMaterial;
	public Material waveOutMaterial;
	public Material thumbToPinkyMaterial;
	
	// The pose from the last update. This is used to determine if the pose has changed
	// so that actions are only performed upon making them rather than every frame during
	// which they are active.
	private Pose _lastPose = Pose.Unknown;
	
	private enum States
	{
		Ready, RocketLauncher, Flamethrower
	}
	private int State = (int)States.Ready;
	
	// Require the rocket to be a rigidbody.
	// This way we the user can not assign a prefab without rigidbody
	public Rigidbody Rocket;	
    public AudioClip impact;

	// Use this for initialization
	void Start () {
		Myo = GameObject.Find ("Myo");
		BaseX = transform.localScale.x;
		BaseY = transform.localScale.y;
		BaseZ = transform.localScale.z;
		RocketCD = 0;
		FlameCD = 0;
	}
	
	void PoseCommand () {
		if (Input.GetKey("p") && RocketCD <= 0) {
			RocketCD = ROCKET_FIRE_TIME;
			State = (int)States.RocketLauncher;
			transform.localScale = Vector3Util.Vector3(1.25*BaseX,0.75*BaseY,1.25*BaseZ);
			return;
		} else if (Input.GetKey("o") && FlameCD <= 0) {
			FlameCD = FLAMETHROWER_FIRE_TIME;
			State = (int)States.Flamethrower;
			//audio.PlayOneShot(STEVE'S FIRE');
			Flamethrower.emissionRate = 100;
			return;
		}

		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		
		// Check if the pose has changed since last update.
		// The ThalmicMyo component of a Myo game object has a pose property that is set to the
		// currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
		// detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
		// is not on a user's arm, pose will be set to Pose.Unknown.
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
			
			// Vibrate the Myo armband when a fist is made.
			if ((thalmicMyo.pose == Pose.Fist || thalmicMyo.pose == Pose.WaveIn || thalmicMyo.pose == Pose.WaveOut)
			&& RocketCD <= 0) {
				thalmicMyo.Vibrate (VibrationType.Medium);                
				RocketCD = ROCKET_FIRE_TIME;
				State = (int)States.RocketLauncher;
				transform.localScale = Vector3Util.Vector3(1.25*BaseX,0.75*BaseY,1.25*BaseZ);
			} else if (thalmicMyo.pose == Pose.ThumbToPinky && FlameCD <= 0) {
				thalmicMyo.Vibrate (VibrationType.Long);  
				FlameCD = FLAMETHROWER_FIRE_TIME;
				State = (int)States.Flamethrower;
				//auido.PlayOneShot(STEVE'S FIRE');
				Flamethrower.emissionRate = 100;
			}
		}
	}
	
	void start_game() {
		//cameraController.EnableOrientation = true;
		//cameraController.EnablePosition = true;
		//cameraController.TrackerRotatesY = true;
		//Debug.Log (cameraController.transform.rotation.ToString ());
		//Quaternion inverseQuat = new Quaternion (-cameraController.transform.rotation.x,
		//                                         -cameraController.transform.rotation.y,
		//                                         -cameraController.transform.rotation.z,
		//                                         -cameraController.transform.rotation.w);
		
		//cameraController.SetOrientationOffset (inverseQuat);
		game_started = true;
		Initialize ();
	}
	
	void Initialize () {
		offset = Myo.transform.eulerAngles;
		offset.x += 90;
		State = (int)States.Ready;
	}
	
	void RotationCommand () {
		rotation = Myo.transform.eulerAngles - offset;
		rotation.z = 0;
		transform.eulerAngles = rotation;
		if (Input.GetKey("w")) {
			offset.x -= 1;
		}
		if (Input.GetKey("a")) {
			offset.y += 1;
		}
		if (Input.GetKey("s")) {
			offset.x += 1;
		}
		if (Input.GetKey("d")) {
			offset.y -= 1;
		}
	}
	
	void RocketPrep () {
		RocketCD -= Time.deltaTime;
		if (RocketCD <= 0) {
			FireRocket();
		}
	}

	void FireRocket () {
		State = (int)States.Ready;
		RocketCD = ROCKET_RECHARGE_TIME;
		transform.localScale = Vector3Util.Vector3(BaseX,BaseY,BaseZ);
		audio.PlayOneShot(impact, 0.7F);
		ProtonBurst.Clear();
		ProtonBurst.Play ();
		Debug.Log (cameraController.transform.rotation.ToString ());
		Rigidbody rocketClone = (Rigidbody) Instantiate(Rocket, transform.position, transform.rotation);
		Physics.IgnoreCollision(rocketClone.collider, collider);
		rocketClone.velocity = transform.up * System.Convert.ToSingle(-ROCKET_SPEED);
	}
	
	void FireFlamethrower () {
		FlameCD -= Time.deltaTime;
		if (FlameCD <= 0) {
			FlameCD = FLAMETHROWER_RECHARGE_TIME;
			State = (int)States.Ready;
			Flamethrower.emissionRate = 0;
		}
	}
	
	void CooldownReduction () {
		if (State != (int)States.RocketLauncher && RocketCD > 0) {
			RocketCD -= Time.deltaTime;
		}
		if (State != (int)States.Flamethrower && FlameCD > 0) {
			FlameCD -= Time.deltaTime;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (game_started) {
			RotationCommand ();
			CooldownReduction ();
		}
		if (State == (int)States.Ready) {
			Debug.Log ("States.Ready");
			PoseCommand ();
		} else if (State == (int)States.RocketLauncher) {
			Debug.Log("States.RocketLauncher");
			if (game_started) {
				RocketPrep ();
			} else {
				start_game();
			}
		} else if (State == (int)States.Flamethrower) {
			Debug.Log("States.Flamethrower");
			FireFlamethrower ();
		}		
	}
}
