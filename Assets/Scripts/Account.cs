using UnityEngine;
using System.Collections;
using SocketIO;
using UnityEngine.UI;

public class Account : MonoBehaviour {
	private SocketIOComponent socket;
	GameObject canvas;
	Text errorText;


	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find("SocketIO");
		canvas = GameObject.Find("Canvas");
		errorText = canvas.transform.FindChild ("Errors").GetComponent<Text>();
		socket = go.GetComponent<SocketIOComponent>();
		socket.On("reg", reg);
		socket.On("auth", auth);

	}
	
	// Update is called once per frame
	void Update () {

	}
	

	public void reg(SocketIOEvent e) {
		errorText.text = e.data.GetField ("answer").str;
	}

	public void auth(SocketIOEvent e) {
		if (e.data.GetField ("answer").str.Equals ("failure")) {
			errorText.text = "�������� ����� ��� ������!";
		} else {
			PlayerPrefs.SetString("playerName", e.data.GetField ("name").str);
			Application.LoadLevel("Lobby");
		}

	}

	public void sendDataToReg() {
		string errorTemp = "��������� ��������� ����:";
		string inputName = canvas.transform.FindChild("NameField").GetComponent<InputField>().text;
		string inputPass = canvas.transform.FindChild("PassField").GetComponent<InputField>().text;

		if (!inputName.Equals ("") && !inputPass.Equals ("")) {
			JSONObject obj = new JSONObject ();
			obj.AddField ("name", inputName);
			obj.AddField ("password", inputPass);
			socket.Emit ("reg", obj);
		} else {
			if (inputName.Equals ("")) {
				errorTemp += " ���";
			}
			if (inputPass.Equals ("")) {
				errorTemp += " ������";
			}
			errorText.text = errorTemp;
			errorTemp = "��������� ��������� ����:";
		}
	}

	public void sendDataToAuth() {
		string errorTemp = "��������� ��������� ����:";
		string inputName = canvas.transform.FindChild("NameField").GetComponent<InputField>().text;
		string inputPass = canvas.transform.FindChild("PassField").GetComponent<InputField>().text;
		
		if (!inputName.Equals ("") && !inputPass.Equals ("")) {
			JSONObject obj = new JSONObject ();
			obj.AddField ("name", inputName);
			obj.AddField ("password", inputPass);
			socket.Emit ("auth", obj);
		} else {
			if (inputName.Equals ("")) {
				errorTemp += " ���";
			}
			if (inputPass.Equals ("")) {
				errorTemp += " ������";
			}
			errorText.text = errorTemp;
			errorTemp = "��������� ��������� ����:";
		}
	}

}
