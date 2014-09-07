using UnityEngine;
using System.Collections;

public class wingflap : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		transform.Rotate (12 * Vector3.forward);
		//time += Time.deltaTime;
		//transform.eulerAngles = new Vector3 (5.0f * System.Convert.ToSingle(Mathf.Sin (time)), 0, 0);
	}
}
