using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void toRegScene() {
		Application.LoadLevel ("Reg");
	}

	public void toMainScene() {
		Application.LoadLevel ("SocketIOTest");
	}
}
