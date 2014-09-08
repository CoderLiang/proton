using UnityEngine;

public class AttackBall : MonoBehaviour {
	Vector3 directionVector;
	public int level;
	private float time;
	// Use this for initialization
	void Start () {
		level = (int)transform.rotation.x;
		time = 0;
		directionVector = new Vector3 ();
		directionVector.x = 0 - this.transform.position.x;
		directionVector.y = 0 - this.transform.position.y;
		directionVector.z = 0 - 10 - this.transform.position.z;
		directionVector.Normalize ();
		directionVector.x = directionVector.x / (50.0f - (9f * level));
		directionVector.y = directionVector.y / (50.0f - (9f * level));
		directionVector.z = directionVector.z / (50.0f - (9f * level));
	}
	
	void OnParticleCollision(GameObject other) {
		Debug.Log ("Monster collided with flamethrower particle; destroying it.");
		Destroy (gameObject);
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
		time += Time.deltaTime;
		Vector3 modificationVector = Vector3.zero;
		if (level == 3) {
			modificationVector.z = 0.09f * Mathf.Sin (3.1f * time + 1f);
		} if (level >= 2) {
			modificationVector.y = 0.06f * Mathf.Cos (1.5f * time);
		} if (level >= 1) {
			modificationVector.x = 0.04f * Mathf.Sin (time);
		}
		transform.Translate (directionVector + modificationVector, Space.World);
	}

	void OnDestroy() {
		MyoTrack.score += 100;
	}
}
