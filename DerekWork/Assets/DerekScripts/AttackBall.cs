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
		directionVector.x = directionVector.x / 65.0f;
		directionVector.y = directionVector.y / 65.0f;
		directionVector.z = directionVector.z / 65.0f;
	}
	
	/*void OnParticleCollision (Collision col) {
		if (col.gameObject.name == "Monster(Clone)" || col.gameObject.name == "Monster") {
			Destroy (col.gameObject);
			//Steve's burning sound of awesomeness
		}
	}*/
	
	void OnParticleCollision(GameObject other) {
		Debug.Log ("LOGGING COLLISION SHIT");
		//if (other.name == "Monster(Clone)" || other.name == "Monster") {
		Destroy (gameObject);
		//Steve's burning sound  
		//}
		
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
}
