using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	
	private double time;
	private double LIFESPAN = 0.4;
	
	public GameObject Explosion;
	
	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name == "Monster(Clone)" || col.gameObject.name == "Monster") {
			Destroy (col.gameObject);
			Instantiate(Explosion,transform.position,transform.rotation);
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
