using UnityEngine;
using System.Collections;

public class DeleteOnStop : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!MyoTrack.game_started) {
			Destroy (gameObject);
		}
	}
}
