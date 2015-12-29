using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {

	public static Transform io;

	void Awake(){
		if (io != null) {
			Destroy (gameObject);
			return;
		}
		DontDestroyOnLoad (this);
		io = transform;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
