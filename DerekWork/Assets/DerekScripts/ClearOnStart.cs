using UnityEngine;
using System.Collections;

public class ClearOnStart : MonoBehaviour {

	public TextMesh me;
	private string originalText;
	// Use this for initialization
	void Start () {
		originalText = me.text;
	}
	
	// Update is called once per frame
	void Update () {
		if (MyoTrack.game_started) {
			me.text = "";
		} else {
			me.text = originalText;
		}
	}
}
