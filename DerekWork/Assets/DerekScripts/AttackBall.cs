using UnityEngine;

public class AttackBall : MonoBehaviour {
	Vector3 directionVector;
	// Use this for initialization
	void Start () {
		directionVector = new Vector3 ();
		directionVector.x = 0 - this.transform.position.x;
		directionVector.y = 0 - this.transform.position.y;
		directionVector.z = 0 - 10 - this.transform.position.z;
		directionVector.Normalize ();
		directionVector.x = directionVector.x / 40.0f;
		directionVector.y = directionVector.y / 40.0f;
		directionVector.z = directionVector.z / 40.0f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (directionVector, Space.World);
		if (transform.position.x < 2.0f) {
			Debug.Log ("Print I AM ANGRY");
			if(transform.position.x > -2.0f) {
				Debug.Log ("I am VERY angry");
				if(transform.position.z > -12.0f) {
					Debug.Log ("I am ZZZZ angry");
					if(transform.position.z < -8.0f) {
						Debug.Log ("I am ARGH angry");
						MyoTrack.endGame();
					}
				}
			}
		}
	}
}
