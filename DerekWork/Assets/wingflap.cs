using UnityEngine;
using System.Collections;

public class wingflap : MonoBehaviour {
	private float time;

	// Use this for initialization
	void Start () {
		time = 0;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (12 * Vector3.forward);
		//time += Time.deltaTime;
		//transform.eulerAngles = new Vector3 (5.0f * System.Convert.ToSingle(Mathf.Sin (time)), 0, 0);
	}
}
