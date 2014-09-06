using UnityEngine;
using System.Collections;

public class MonsterSpawner : MonoBehaviour {
	private const float SPAWN_DISTANCE = 20f;
	public GameObject Monster;
	private float time;
	private float rate;
	// Use this for initialization
	void Start () {
		time = 0;
		rate = 0;
	}
	
	void Spawn () {
		float x = Random.Range(-1f, 1f);
		float z = Random.Range(-1f, 1f);
		Vector3 direction = new Vector3(x, 0f, z);
		direction = direction.normalized * SPAWN_DISTANCE;
		direction.y = Random.Range (-2f, 15f);
		Instantiate (Monster,direction,new Quaternion(0,0,0,0));
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		rate += Time.deltaTime;
		if (time > (9f * Mathf.Pow(rate,-0.2f))) {
			time = 0;
			Spawn ();
		}
	}
}
