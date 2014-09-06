﻿using UnityEngine;
using System.Collections;

using Pose = Thalmic.Myo.Pose;
using VibrationType = Thalmic.Myo.VibrationType;

[RequireComponent(typeof(AudioSource))]

public class MyoTrack : MonoBehaviour {
	private const double FIRE_TIME = 0.2;
	private const double RECHARGE_TIME = 0.6;
	public float ROCKET_SPEED = 100f;

	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	public GameObject myo = null;
	
	private GameObject Myo;
	private Vector3 rotation;
	private Vector3 offset;
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
		Preinitial, Initial, Ready, Firing, Recharging
	}
	private int State = (int)States.Preinitial;
	
	// Require the rocket to be a rigidbody.
	// This way we the user can not assign a prefab without rigidbody
	public Rigidbody Rocket;
	
    public AudioClip impact;

	// Use this for initialization
	void Start () {
		Myo = GameObject.Find ("Myo");
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
			if (thalmicMyo.pose == Pose.Fist) {
                audio.PlayOneShot(impact, 0.7F);
				thalmicMyo.Vibrate (VibrationType.Medium);
                
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
			offset.x += 1;
		}
		if (Input.GetKey("a")) {
			offset.y+= 1;
		}
		if (Input.GetKey("s")) {
			offset.x -= 1;
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
			transform.localScale = Vector3Util.Vector3(0.25,2,0.25);
			FireRocket();			
			return;
		}
		transform.localScale = Vector3Util.Vector3(0.4,1.6,0.4);
	}
	
	void FireRocket () {
		Rigidbody rocketClone = (Rigidbody) Instantiate(Rocket, transform.position, transform.rotation);
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
		if (State == (int)States.Preinitial)	{
			State = (int)States.Initial;
			return;
		}
		if (State == (int)States.Initial)	{
			Initialize ();
			return;
		}
		RotationCommand ();
		if (State == (int)States.Ready) {
			PoseCommand ();
		} else if (State == (int)States.Firing) {
			FireCommand ();
		} else if (State == (int)States.Recharging) {
			Recharge ();
		}
	}
}
