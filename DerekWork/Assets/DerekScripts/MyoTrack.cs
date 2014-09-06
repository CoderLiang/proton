using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using VibrationType = Thalmic.Myo.VibrationType;

[RequireComponent(typeof(AudioSource))]

public class MyoTrack : MonoBehaviour {
	public OVRCameraController cameraController;
	public OVRPlayerController playerController;
	public static bool game_started = false;
	public static int score = 0;
	private static Quaternion initialOrientation;
	private const double FIRE_TIME = 0.2;
	private const double RECHARGE_TIME = 0.6;
	public float ROCKET_SPEED = 100f;
	public GameObject monsterSpawner;
	private float BaseX;
	private float BaseY;
	private float BaseZ;
	public TextMesh titleText;
	public TextMesh subTitleText1;
	public TextMesh subTitleText2;
	public TextMesh subTitleText3;
	public bool wasStrongGesture;
	public static MyoTrack me;


	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	public GameObject myo = null;
	
	private static GameObject Myo;
	private Vector3 rotation;
	private static Vector3 offset;
	private float time;
	
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
		Ready, Firing, Recharging
	}
	private static int State = (int)States.Ready;
	
	// Require the rocket to be a rigidbody.
	// This way we the user can not assign a prefab without rigidbody
	public Rigidbody Rocket;
	
    public AudioClip impact;

	// Use this for initialization
	void Start () {
		Myo = GameObject.Find ("Myo");
		me = this;
		BaseX = transform.localScale.x;
		BaseY = transform.localScale.y;
		BaseZ = transform.localScale.z;
		RenderTitle ();
		//initialOrientation = transform.rotation;
		}

	void RenderTitle() {
		//initialize title text
		titleText.text = "Proton";
		subTitleText1.text = "Look directly at this text,";
		subTitleText2.text = "Raise your arm in front of you,";
		subTitleText3.text = "And make a fist to begin.";
	}

	static void RenderScore(TextMesh titleTextIn, TextMesh subTitleTextIn) {
		titleTextIn.text = "Proton";
		subTitleTextIn.text = "Score: " + score;
	}

	public static void endGame() {
		Debug.Log (MyoTrack.score.ToString ());
		game_started = false;

		Initialize ();

	}
	
	void PoseCommand () {
		if (Input.GetKey("p")) {
			time = 0;
			State = (int)States.Firing;
			return;
		}
	
		// Access the ThalmicMyo component attached to the Myo game object.
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		
		// Check if the pose has changed since last update.
		// The ThalmicMyo component of a Myo game object has a pose property that is set to the
		// currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
		// detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
		// is not on a user's arm, pose will be set to Pose.Unknown.
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
			
			// Vibrate the Myo armband when a fist is made.
			if (thalmicMyo.pose != Pose.Rest && thalmicMyo.pose != Pose.WaveIn && thalmicMyo.pose != Pose.WaveOut) {
				thalmicMyo.Vibrate (VibrationType.Medium); 
				if(thalmicMyo.pose == Pose.ThumbToPinky || thalmicMyo.pose == Pose.Fist) {
					wasStrongGesture = true;
				} else {
					wasStrongGesture = false;
				}
				time = 0;
				State = (int)States.Firing;
				
				// Change material when wave in, wave out or thumb to pinky poses are made.
			} else if (thalmicMyo.pose == Pose.WaveIn) {
				renderer.material = waveInMaterial;
			} else if (thalmicMyo.pose == Pose.WaveOut) {
				renderer.material = waveOutMaterial;
			} else if (thalmicMyo.pose == Pose.ThumbToPinky) {
				renderer.material = thumbToPinkyMaterial;
			}
		}
	}
	
	static void Initialize () {
		me.transform.rotation = Quaternion.Euler (270f, 0f, 0f);
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
	
	void FireCommand () {
		time += Time.deltaTime;
		if (time > FIRE_TIME) {
			State = (int)States.Recharging;
			time = 0;
			transform.localScale = Vector3Util.Vector3(BaseX,BaseY,BaseZ);
			FireRocket();			
			return;
		}
		transform.localScale = Vector3Util.Vector3(1.25*BaseX,0.75*BaseY,1.25*BaseZ);
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
		Instantiate (monsterSpawner, Vector3.zero, Quaternion.identity);
		Initialize ();
	}
	
	void FireRocket () {
		audio.PlayOneShot(impact, 0.7F);
		Debug.Log (cameraController.transform.rotation.ToString ());
		Rigidbody rocketClone = (Rigidbody) Instantiate(Rocket, transform.position + transform.up, transform.rotation);
		Physics.IgnoreCollision(rocketClone.collider, collider);
		rocketClone.velocity = -transform.up * ROCKET_SPEED;		
		
		// You can also acccess other components / scripts of the clone
		//rocketClone.GetComponent<MyRocketScript>().DoSomething();
	}
	
	void Recharge () {
		time += Time.deltaTime;
		if (time > RECHARGE_TIME) {
			State = (int)States.Ready;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (game_started) {
				RotationCommand ();
		}
		if (State == (int)States.Ready) {
			Debug.Log ("States.Ready");
			PoseCommand ();
		} else if (State == (int)States.Firing) {
			Debug.Log("States.Firing");
			if (game_started) {
				FireCommand ();
			} else {
				if (wasStrongGesture == true) {
					start_game();
				}
			}
		} else if (State == (int)States.Recharging) {
			Recharge ();
		}
	}
}
