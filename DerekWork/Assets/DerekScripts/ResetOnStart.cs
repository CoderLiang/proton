using UnityEngine;
using System.Collections;

public class ResetOnStart : MonoBehaviour {
	
	public TextMesh me;
	private bool was_playing = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (MyoTrack.game_started) {
			me.text = "";
			was_playing = true;
		}
		if (!MyoTrack.game_started && was_playing == true) {
			was_playing = false;
			me.text = "Proton";
		}
	}
}
