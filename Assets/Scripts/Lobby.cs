using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SocketIO;

public class Lobby : MonoBehaviour {
	private SocketIOComponent socket;
	GameObject canvas;
	Text userName;
	Text amountOfPlayer;
	string playerName;
	public Text messageToChat;
	public Text stats;
	public Text chatMsgList; 
	
	void Start () {
		playerName = PlayerPrefs.GetString ("playerName");
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		canvas = GameObject.Find ("Canvas");
		userName = canvas.transform.FindChild ("Name").GetComponent<Text>();
		amountOfPlayer = canvas.transform.FindChild ("AmountOfPlayer").GetComponent<Text>();

		socket.On("msgFromChat", msgFromChat);
		socket.On("amountOfPlayers", amountOfPlayers);
		socket.On("userInfo", userInfo);
		socket.On("findGame", gameReady);

		getUserInfo ();
		getUsersOnline ();

	}


	void msgFromChat(SocketIOEvent e) {
		Debug.Log (e.data);
		chatMsgList.text += e.data.GetField("name").str + ":" + e.data.GetField("message").str + "\n";
	}

	void amountOfPlayers(SocketIOEvent e) {
		Debug.Log (e.data);
		amountOfPlayer.text = e.data.GetField ("answer").str;
	}

	void userInfo(SocketIOEvent e) {
		Debug.Log ("userInfo" + e.data);
		userName.text = e.data.GetField ("name").str + "!";
		stats.text = e.data.GetField ("answer").str;
	}

	public void sendMessage() {
		string toChat = messageToChat.text;
		JSONObject obj = new JSONObject ();
		obj.AddField ("name", playerName);
		obj.AddField ("message", toChat);
		socket.Emit ("sendToChat", obj);
	}

	public void findGame(){
		JSONObject obj = new JSONObject ();
		obj.AddField ("name", playerName);
		socket.Emit ("findGame", obj);
	}

	public void gameReady(SocketIOEvent e){
		if (e.data.GetField("answer").str.Equals("gameIsReady")) {
			Debug.Log ("gameIsReady");
			PlayerPrefs.SetString("roomId", e.data.GetField("uuid").str);
			Application.LoadLevel("Game");
		} else if(e.data.GetField("answer").str.Equals("roomCreate")) {
			Debug.Log ("roomCreate");
		}
	}

	public void getUserInfo() {
		JSONObject obj = new JSONObject ();
		obj.AddField ("name", playerName);
		socket.Emit ("userInfo", obj);
	}

	public void getUsersOnline() {
		JSONObject obj = new JSONObject ();
		obj.AddField ("name", playerName);
		socket.Emit ("amountOfPlayers", obj);
	}

	void Update () {
	
	}
}
