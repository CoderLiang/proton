using UnityEngine;
using System.Collections;

public class AttackBall : MonoBehaviour {
	Vector3 directionVector;
	// Use this for initialization
	void Start () {
		directionVector = new Vector3 ();
		directionVector.x = 0 - this.transform.position.x;
		directionVector.y = 0 - this.transform.position.y;
		directionVector.z = 0 - 10 - this.transform.position.z;
		directionVector.Normalize ();
		directionVector.x = directionVector.x / 10.0f;
		directionVector.y = directionVector.y / 10.0f;
		directionVector.z = directionVector.z / 10.0f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (directionVector, Space.World);
	}
}
