using UnityEngine;
using System.Collections;

public class FixForward : MonoBehaviour {
	Quaternion initDirection;
	// Use this for initialization
	void Start () {
		initDirection = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.rotation = initDirection;
	}
}
