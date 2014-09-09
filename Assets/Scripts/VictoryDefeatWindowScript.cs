using UnityEngine;
using System.Collections;

public class VictoryDefeatWindowScript : MonoBehaviour {
	UILabel label;
	void Awake(){
		label = GetComponentInChildren<UILabel> ();
	}
	// Use this for initialization
	void Start () {
	
	}
	public void Show(bool victory){
		transform.GetChild (0).gameObject.SetActive (true);
		label.text = victory ? "Victory" : "Defeat";
	}

	public void Hide(){
		transform.GetChild (0).gameObject.SetActive (false);
		label.text = "";
	}
	// Update is called once per frame
	void Update () {
	
	}
}
