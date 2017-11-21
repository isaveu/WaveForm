using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChanger : MonoBehaviour {
	Text UIText;
	// Use this for initialization
	void Awake () {
		UIText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	public void OnValueChanged (float value) {
		UIText.text = value.ToString("n2");
	}
}
