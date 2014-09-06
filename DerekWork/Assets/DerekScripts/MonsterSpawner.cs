using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour {
	private const float SPAWN_DISTANCE = 20f;
	public GameObject Monster;
	private float time;
	private float rate;
	private int spawned;
	// Use this for initialization
	void Start () {
		time = 0;
		rate = 0;
		spawned = 1;
	}
	
	void Spawn () {
		float x = Random.Range(-1f, 1f);
		float z = Random.Range(-1f, 1f);
		Vector3 direction = new Vector3(x, 0f, z);
		direction = direction.normalized * SPAWN_DISTANCE;
		direction.y = Random.Range (-2f, 15f);
		int level = 0;
		if (spawned % 12 == 0) {
			level = 3;
		} else if (spawned % 7 == 0) {
			level = 2;
		} else if (spawned % 5 == 0) {
			level = 1;
		}
		Instantiate (Monster,direction,new Quaternion(level,0,0,0));
		++spawned;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		rate += Time.deltaTime;
		if (time > (10f * Mathf.Pow(rate,-0.2f))) {
			time = 0;
			Spawn ();
		}
	}
}
