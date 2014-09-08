using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	private float time;
	private const float MAX_TIME = 0.5f;

	// Use this for initialization
	void Start () {
		time = 0;
		transform.localScale = Vector3Util.Vector3(0,0,0);
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > MAX_TIME) {
			Object.Destroy(gameObject);
		}
		transform.localScale = Vector3Util.Vector3(3*time,3*time,3*time);
	}
}
