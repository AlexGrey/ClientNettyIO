using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SocketIO;

public class Game : MonoBehaviour {
	private SocketIOComponent socket;
	GameObject canvas;
	private string playerName;
	private string roomId;

	public Text roomID;
	public Text clientName1;
	public Text clientName2;
	public Text ready;
	public Button readyButton;
	int clicks = 0;
	bool gameStart = false;
	bool gameFinish = false;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		canvas = GameObject.Find ("Canvas");
		playerName = PlayerPrefs.GetString ("playerName");
		roomId = PlayerPrefs.GetString ("roomId");
		socket.On("roomInfo", roomInfo);
		socket.On ("readyToFight", readyToFightListener);
		socket.On ("preGame", preGameListener);
		socket.On ("startGame", startGameListener);
		socket.On ("game", gameListener);
		socket.On ("endGame", endGameListener);
		socket.On ("resultGame", resultGameListener);

		getRoomInfo ();
	}

	void readyToFightListener(SocketIOEvent e) {
		if (e.data.GetField ("answer").str.Equals ("allReady")) {
			ready.text = "игроки готовы!";
		}
	}

	void preGameListener(SocketIOEvent e) {
		ready.text = e.data.GetField ("answer").str;
	}

	void startGameListener(SocketIOEvent e) {
		ready.text = e.data.GetField ("answer").str;
		if ( e.data.GetField ("answer").str.Equals("startGame")) {
			readyButton.transform.FindChild("Text").GetComponent<Text>().text = "Жми!";
			gameStart = true;
		}
	}

	void gameListener(SocketIOEvent e) {
		ready.text = e.data.GetField ("answer").str;
	}

	void endGameListener(SocketIOEvent e) {
		if ( e.data.GetField ("answer").str.Equals("gameOver")) {
			ready.text = "Игра окончена!";
			JSONObject obj = new JSONObject ();
			obj.AddField ("id", roomId);
			obj.AddField ("clicks", clicks);
			socket.Emit ("endGame", obj);
		}
	}

	void resultGameListener(SocketIOEvent e) {
		ready.text = e.data.GetField ("answer").str;
		readyButton.transform.FindChild("Text").GetComponent<Text>().text = "назад в лобби";
		gameFinish = true;
	}

	public void readyToFight() {
		if (!gameStart && !gameFinish) {
			Debug.Log (playerName);
			JSONObject obj = new JSONObject ();
			obj.AddField ("id", roomId);
			obj.AddField ("name", playerName);
			socket.Emit ("readyToFight", obj);
		}
	}

	public void backToLobby() {
		if (gameFinish) {
			Application.LoadLevel ("Lobby");
		}
	}

	void roomInfo(SocketIOEvent e) {
		roomID.text = e.data.GetField ("id").str;
		clientName1.text = e.data.GetField ("clientName1").str;
		clientName2.text = e.data.GetField ("clientName2").str;
	}

	public void getRoomInfo() {
		JSONObject obj = new JSONObject ();
		obj.AddField ("id", roomId);
		socket.Emit ("roomInfo", obj);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void clickUpdate() {
		if (gameStart && !gameFinish) {
			clicks++;
		}

	}
}
