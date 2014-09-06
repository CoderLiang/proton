using UnityEngine;
using System.Collections;

public class MyoTrack : MonoBehaviour {

	private GameObject Myo;
	private Vector3 rotation;
	private Vector3 offset;
	private int steps = 0;
	
	// Use this for initialization
	void Start () {
		Myo = GameObject.Find ("Myo");
	}
	
	void Initialize () {
		offset = Myo.transform.eulerAngles;
		offset.x += 90;
	}
	
	// Update is called once per frame
	void Update () {
		if (steps < 5)	{
			if (steps == 4) {
				Initialize();
			}
			++steps;
			return;
		}
		rotation = Myo.transform.eulerAngles - offset;
		rotation.x *= -1;
		rotation.z = 0;
		transform.eulerAngles = rotation;
	}
}
