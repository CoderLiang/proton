using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	
	private double time;
	private double LIFESPAN = 3;
	
	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name == "Sphere") {
			Destroy (col.gameObject);
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > LIFESPAN) {
			Object.Destroy (gameObject);
		}
	}
}
