using UnityEngine;
using System.Collections;

public class ShowScore : MonoBehaviour {
	
	public TextMesh me;
	private bool was_playing = false;
	public float time = 0;
	private string old;
	// Use this for initialization
	void Start () {
		old = me.text;
	}
	
	// Update is called once per frame
	void Update () {
		if (MyoTrack.game_started) {
			me.text = "";
			was_playing = true;
		} else if (!MyoTrack.game_started && was_playing == true) {
			was_playing = false;
			me.text = "Score: " + MyoTrack.score;
			MyoTrack.score = 0;
			time = 0;
		} else {
			time += Time.deltaTime;
			if (time > 15.0f) {
				me.text = old;
			}
		}
	}
}
