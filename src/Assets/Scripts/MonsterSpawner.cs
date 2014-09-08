using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour {
	private const float SPAWN_DISTANCE = 30f;
	public GameObject Monster;
	private float time;
	private float rate;
	private int spawned;
	// Use this for initialization
	void Start () {
		float x = Random.Range(-1f, 1f);
		float z = Random.Range(0.5f, 1f);
		Vector3 direction = new Vector3(x, 0, z);
		direction = direction.normalized * SPAWN_DISTANCE;
		Instantiate (Monster, direction, new Quaternion (0, 180f, 0, 0));
		time = -3;
		rate = 0;
		spawned = 1;
	}
	
	void Spawn () {
		float x = Random.Range(-1f, 1f);
		float z = Random.Range(-1f, 1f);
		Vector3 direction = new Vector3(x, 0f, z);
		direction = direction.normalized * SPAWN_DISTANCE;
		direction.y = Random.Range (-2f, 20f);
		int level = 0;
		int rand = Random.Range (1,25);
		if (spawned >= 5 && spawned < 10) {
			if (rand > 15) {
				level = 1;
			}
		} else if (spawned >= 10 && spawned < 15) {
			if (rand > 15) {
				level = 2;
			} else if (rand > 5) {
				level = 1;
			}
		} else if (spawned >= 15 && spawned < 20) {
			if (rand > 15) {
				level = 3;
			} else if (rand > 5) {
				level = 2;
			} else {
				level = 1;
			}
		} else if (spawned >= 20) {
			if (rand > 10) {
				level = 3;
			} else {
				level = 2;
			}
		}
		Instantiate (Monster,direction,new Quaternion(level,180f,0,0));
		++spawned;
		Debug.Log (spawned);
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		rate += Time.deltaTime;
		if (time > (18f * Mathf.Pow(rate,-0.35f))) {
			time = 0;
			Spawn ();
		}
	}
}
